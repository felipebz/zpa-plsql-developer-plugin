using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ZpaPlugin
{
    class ZpaRunner
    {
        private static readonly string zpaCli = Path.Combine(ZpaPlugin.dependenciesPath, "zpa-cli", "bin", "zpa-cli.bat");

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

            var obj = JObject.Parse(File.ReadAllText(output));
            JArray issues = (JArray)obj["issues"];
            MessageBox.Show($"Found {issues.Count} issues.", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
