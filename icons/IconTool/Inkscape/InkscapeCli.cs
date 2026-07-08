using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconTool.Inkscape
{
    public static class InkscapeCli
    {
        static void ExportFile(string svgfile, string exportFilename, int exportWidth, int exportHeight, string sizefolder, ExportFileType exportFileType)
        {
            // 1. Configure the process settings
            var startInfo = new ProcessStartInfo
            {
                FileName = "inkscape",
                Arguments = $"-z -w {exportWidth} -h {exportHeight} -d 300 {sizefolder}/{svgfile} --export-filename tmp/{exportFilename}_{exportWidth}.{exportFileType.ToString().ToLower()} --export-type {exportFileType.ToString().ToLower()}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // 2. Run the process
            using (Process process = Process.Start(startInfo))
            {
                // 3. Read the output logs
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit(); // Block thread until finished

                // 4. Handle results
                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Success Output:");
                    Console.WriteLine(output);
                }
                else
                {
                    throw new Exception($"Error executing Inkscape CLI. Exit Code: {process.ExitCode}. Error Output: {error}");
                }
            }
        }
    }
}
