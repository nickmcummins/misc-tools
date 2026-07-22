using System.Diagnostics;

namespace IconTool.ImageMagick;

public static class ImageMagickCli
{
    public static void Convert(IEnumerable<string> inputFiles, string outputFile)
    {
        RunCommand($"convert {string.Join(' ', inputFiles)} {outputFile}");
    }
    
    private static string RunCommand(string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "magick",
            Arguments = arguments,
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

            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                Console.WriteLine("Success Output:");
                Console.WriteLine(output);
                return output.Trim();
            }
            else
            {
                throw new Exception($"Error executing Inkscape CLI. Exit Code: {process.ExitCode}. Error Output: {error}");
            }
        }
    }
}