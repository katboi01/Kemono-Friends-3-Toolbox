using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF3Shared
{
    public class SharedSettings
    {
        public static string lastVersion, assetsPath, paramsPath, cachePath, exportPath, filesPath, guiPath;

        public static void LocalDirectoriesManager(bool update)
        {
            //assetsPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low"}\\SEGA\\けもフレ３\\StreamingAssets\\assets\\";
            assetsPath = Directory.GetCurrentDirectory() + "/Parameters/MonoBehaviour/";
            paramsPath = Directory.GetCurrentDirectory() + "/Parameters/MonoBehaviour/";
            cachePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Temp\\SEGA\\けもフレ３\\";
            exportPath = Directory.GetCurrentDirectory() + @"\export\";
            guiPath = Directory.GetCurrentDirectory() + @"\icons\";

            Directory.CreateDirectory(guiPath);
            Directory.CreateDirectory(exportPath);
            Directory.CreateDirectory(exportPath + @"wiki\");
            Directory.CreateDirectory(exportPath + @"photos\");
            Directory.CreateDirectory(exportPath + @"gachaBanner\");
            Directory.CreateDirectory(exportPath + @"homeBanner\");
        }
    }
}
