using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SharedSettings
{
    public static string lastVersion, assetsPath, cachePath, exportPath, guiPath;

    public static void LocalDirectoriesManager(bool update)
    {
        assetsPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low"}\\Appirits\\けもフレ３\\StreamingAssets\\assets\\";
        cachePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Temp\\Appirits\\けもフレ３\\";
        exportPath = Directory.GetCurrentDirectory() + @"\export\";
        guiPath = Directory.GetCurrentDirectory() + @"\icons\";

        Directory.CreateDirectory(guiPath);
        Directory.CreateDirectory(exportPath);
        Directory.CreateDirectory(exportPath + @"wiki\");
        Directory.CreateDirectory(exportPath + @"photos\");
        Directory.CreateDirectory(exportPath + @"gachaBanner\");
        Directory.CreateDirectory(exportPath + @"homeBanner\");
        Directory.CreateDirectory(exportPath + @"cache\");
    }
}
