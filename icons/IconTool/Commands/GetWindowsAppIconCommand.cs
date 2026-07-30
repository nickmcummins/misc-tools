using IconTool.Models;

namespace IconTool.Commands
{
    public class GetWindowsAppIconCommand : IIconToolCommand<string>
    {
        public void Handle(string windowsAppFolder)
        {
            var assetsFolder = Path.Combine(windowsAppFolder, "Assets");
            var iconFiles = Directory.GetFiles(assetsFolder, "*.png", SearchOption.TopDirectoryOnly)
                .Where(file => WindowsAppIcon.FilenamePattern.IsMatch(file))
                .Select(file => new WindowsAppIcon(file));
            foreach (var windowsAppIcon in iconFiles) {
                Console.WriteLine(windowsAppIcon);
            }
        }
    }
}
