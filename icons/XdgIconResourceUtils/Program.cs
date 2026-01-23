using System;
using XdgIconResourceUtils.Models;

namespace XdgIconResourceUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            var iconTheme = new InstalledIconTheme(args[0]);
            Console.Out.WriteLine(iconTheme);
        }
    }
}
