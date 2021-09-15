using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
