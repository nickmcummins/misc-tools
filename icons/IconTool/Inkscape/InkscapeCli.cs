using System.Diagnostics;
using System.Reflection;

namespace IconTool.Inkscape
{
    public static class InkscapeCli
    {
        public static void ExportFile(string svgfile, string exportFilename, int exportWidth, int exportHeight, string tempDirectory, ExportFileType exportFileType)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "inkscape",
                Arguments = $"-z -w {exportWidth} -h {exportHeight} -d 300 {svgfile} --export-filename {tempDirectory}/{Path.GetFileName(exportFilename)}_{exportWidth}.{exportFileType.ToString().ToLower()} --export-type {exportFileType.ToString().ToLower()}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Console.Out.WriteLine($"Executing {startInfo.FileName} {startInfo.Arguments}");

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

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

        public static ObjectGeometry QueryObjectGeometry(string svgfile, IList<QueryObjectGeometryProperty> queryProperties)
        {
            queryProperties.Sort(QueryObjectGeometryPropertyOrderComparer.Instance);
            var startInfo = new ProcessStartInfo
            {
                FileName = "inkscape",
                Arguments = string.Concat(string.Join(' ', queryProperties.Select(p => $"--query-{p.ToString().ToLower()}")), " ", svgfile),
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
                    var currentLine = 0;
                    var objectGeometry = new ObjectGeometry();
                    var outputLines = output.Trim().Split("\n");
                    while (currentLine < outputLines.Length)
                    {
                        var property = ObjectGeometry.Properties[queryProperties[currentLine]];
                        var value = outputLines[currentLine];
                        if (queryProperties[currentLine] == QueryObjectGeometryProperty.All)
                        {
                            
                        }
                        else
                        {
                            property.SetValue(objectGeometry, Parse(property, value));
                        }
                        currentLine++;
                    }

                    return objectGeometry;
                }
                else
                {
                    throw new Exception($"Error executing Inkscape CLI. Exit Code: {process.ExitCode}. Error Output: {error}");
                }
            }
        }

        public class QueryObjectGeometryPropertyOrderComparer : IComparer<QueryObjectGeometryProperty>
        {
            public static readonly IComparer<QueryObjectGeometryProperty> Instance = new QueryObjectGeometryPropertyOrderComparer();
            public int Compare(QueryObjectGeometryProperty x, QueryObjectGeometryProperty y)
            {
                return x.CompareTo(y);
            }
        }

        private static object Parse(PropertyInfo property, string value)
        {
            if (property.PropertyType == typeof(int?))
            {
                return int.Parse(value);
            }
            else
            {
                throw new ArgumentException($"Unsupported property type {property.PropertyType}.");
            }
        }
    }
}
