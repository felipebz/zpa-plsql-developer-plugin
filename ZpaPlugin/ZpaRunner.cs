using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using ZpaPlugin.Models;

namespace ZpaPlugin
{
    public class ZpaRunner
    {
        private static readonly string zpaCli = Path.Combine(ZpaPlugin.dependenciesPath, "zpa-cli", "bin", "zpa-cli.bat");
        private readonly IPlsqlDevApi plsqlDevApi;

        public ZpaRunner(IPlsqlDevApi plsqlDevApi)
        {
            this.plsqlDevApi = plsqlDevApi;
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
                    Arguments = $"--sources \"{tempDir}\" --output \"{output}\"",
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
