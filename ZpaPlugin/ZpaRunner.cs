using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ZpaPlugin.Models;

namespace ZpaPlugin
{
    public class ZpaRunner
    {
        private string zpaCli;
        private readonly IPlsqlDevApi plsqlDevApi;

        public ZpaRunner(IPlsqlDevApi plsqlDevApi)
        {
            this.plsqlDevApi = plsqlDevApi;
            var zpaCliFolder = Directory.GetDirectories(ZpaPlugin.dependenciesPath).Where(x => Path.GetFileName(x).StartsWith("zpa-cli")).OrderByDescending(x => x).First();
            zpaCli = Path.Combine(ZpaPlugin.dependenciesPath, zpaCliFolder, "bin", "zpa-cli.bat");
        }

        public void Analyze(string contents)
        {
            var version = 0;
            var fullVersion = "not found";

            var javaExe = "java.exe";
            var javaHome = Environment.GetEnvironmentVariable("JAVA_HOME").Trim('"');
            if (javaHome != null)
            {
                javaExe = Path.Combine(javaHome, "bin", javaExe);
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = javaExe,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,

                    Arguments = "-version"
                };
                var p = Process.Start(psi);
                string strOutput = p.StandardError.ReadToEnd();
                p.WaitForExit();

                var exp = new Regex(@"\(build ((\d+)\..*)\)");
                var match = exp.Match(strOutput);
                version = Convert.ToInt32(match.Groups[2].Value);
                fullVersion = match.Groups[1].Value;
            }
            catch 
            { 
                // ignored
            }

            if (version < 11)
            {
                MessageBox.Show($"The ZPA plugin requires Java 11 or newer to run. You are currently using Java {fullVersion} from {javaExe}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var tempDir = Path.Combine(Path.GetTempPath(), "zpa-plsql-developer");
            Directory.CreateDirectory(tempDir);

            var path = Path.Combine(tempDir, "output.sql");
            var output = Path.Combine(tempDir, "zpa-issues.json");
            try
            {
                File.WriteAllText(path, contents);

                var processInfo = new ProcessStartInfo()
                {
                    FileName = zpaCli,
                    Arguments = $"--sources \"{tempDir}\" --output-file \"{output}\" --output-format sq-generic-issue-import",
                    CreateNoWindow = true,
                    UseShellExecute = false

                };
                Process.Start(processInfo).WaitForExit();
            }
            finally
            {
                File.Delete(path);
            }

            var json = File.ReadAllText(output);
            var issueData = JsonConvert.DeserializeObject<GenericIssueData>(json);

            new ResultWindow(plsqlDevApi, issueData.Issues).Show();
        }
    }
}
