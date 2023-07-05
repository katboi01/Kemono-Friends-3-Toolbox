using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Net;
using System.Web;
using static KF3Parse.ParamArts;
using System.Diagnostics;

public partial class KF3Parse
{
    public void Animation(string text, int frame)
    {
        char[] textArray = text.ToCharArray();

        textArray[frame % (text.Length)] = textArray[frame % (text.Length)].ToString().ToUpper().ToCharArray()[0];
        text = new string(textArray);
        Console.Write("\n" + text + "...");

        Console.SetCursorPosition(0, 0);
    }

    public static int CalcStatus(int hp, int atk, int def)
    {
        return (int)(0 + hp * 8 / 10 + ((hp * 8 % 10 > 0) ? 1 : 0) + atk * 3 + def * 2);
    }

    string DeUnicode(string x)
    {
        x = Regex.Unescape(x);
        return x;
    }

    List<string> FillMiracleNumbers(CharaData friend)
    {
        List<string> output = new List<string>();
        Dictionary<string, CharaDamageParam> damages = new Dictionary<string, CharaDamageParam>();
        Dictionary<string, CharaBuffParam> buffs = new Dictionary<string, CharaBuffParam>();
        for (int i = 0; i < friend.ParamArts.damageList.Count; i++)
        {
            damages.Add($"[DAMAGE{i}]", friend.ParamArts.damageList[i]);
        }
        for (int i = 0; i < friend.ParamArts.buffList.Count; i++)
        {
            buffs.Add($"[BUFF{i}]", friend.ParamArts.buffList[i]);
            buffs.Add($"[HEAL{i}]", friend.ParamArts.buffList[i]);
            buffs.Add($"[INCREMENT{i}]", friend.ParamArts.buffList[i]);
        }

        for (int i = 1; i < 7; i++)
        {
            string baseText = Regex.Escape(friend.ParamArts.actionEffect);
            foreach (KeyValuePair<string, CharaDamageParam> damage in damages)
            {
                string newValue = ((int)(damage.Value.damageRate * (1 + damage.Value.growthRate * (float)(i - 1)) * 100f + 0.01f)).ToString();
                baseText = baseText.Replace(damage.Key, newValue);
            }
            foreach (KeyValuePair<string, CharaBuffParam> buff in buffs)
            {
                if (buff.Key[1] == 'B')
                {
                    string newValue = ((int)(Math.Abs(buff.Value.coefficient * (1f + buff.Value.growthRate * (float)(i - 1)) - 1f) * 100f + 0.01f)).ToString();
                    baseText = baseText.Replace(buff.Key, newValue);
                }
                else if (buff.Key[1] == 'H')
                {
                    string newValue = ((int)(buff.Value.coefficient * (1f + buff.Value.growthRate * (float)(i - 1)) * 100f + 0.01f)).ToString();
                    baseText = baseText.Replace(buff.Key, newValue);

                }
                else if (buff.Key[1] == 'I')
                {
                    string newValue = ((int)(Math.Abs((float)buff.Value.increment) * (1f + buff.Value.growthRate * (float)(i - 1)) + 0.01f)).ToString();
                    baseText = baseText.Replace(buff.Key, newValue);
                }
            }
            output.Add(baseText);
        }

        return output;
    }

    static string CenteredString(string s, int width)
    {
        if (s.Length >= width)
        {
            return s;
        }

        int leftPadding = (width - s.Length) / 2;
        int rightPadding = width - s.Length - leftPadding;

        return new string(' ', leftPadding) + s + new string(' ', rightPadding);
    }

    public string LoadGzipFile(string fileName)
    {
        string openFile = SharedSettings.cachePath + fileName + ".d";
        if (!File.Exists(openFile)) { Console.WriteLine("file does not exist"); return "error"; }
        using (Stream decompress = new GZipStream(File.OpenRead(openFile), CompressionMode.Decompress))
        using (StreamReader stringWriter = new StreamReader(decompress))
        {
            return stringWriter.ReadToEnd();
        }
    }

    void SwitchToFileOutput(bool on, string fileName)
    {
        if (on)
        {
            Console.Out.Close(); sw = new StreamWriter(fileName); Console.SetOut(sw);
        }
        else
        {
            Console.Out.Close(); sw = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            }; Console.SetOut(sw);
        }
    }

    public static double Interpolate(double min, double max, int lvl, int maxLvl)
    { return (min + (max - min) * (lvl - 1) / (maxLvl - 1)); }

    string GetItemName(int id)
    { return ItemCommons.Where(i => i.id == id).Select(i => i.name).DefaultIfEmpty("no item").First(); }

    string GetStageName(int group)
    {
        switch (group)
        {
            case 1: return "Tutorial";
            case 2: return "Chika-ra kurabe";
            case 3: return "Friend";
            case 4: return "Daily";
            case 5: return "Shiserval Dojo";
            case 1001: return "Chapter 1 (easy)";
            case 1002: return "Chapter 2 (easy)";
            case 1003: return "Chapter 3 (easy)";
            case 1004: return "Chapter 4 (easy)";
            case 1005: return "Chapter 5 (easy)";
            case 1006: return "Chapter 6 (easy)";
            case 1007: return "Chapter 7 (easy)";
            case 1008: return "Chapter 8 (easy)";
            case 2001: return "Chapter 1 (hard)";
            case 2002: return "Chapter 2 (hard)";
            case 2003: return "Chapter 3 (hard)";
            case 2004: return "Chapter 4 (hard)";
            case 2005: return "Chapter 5 (hard)";
            case 2006: return "Chapter 6 (hard)";
            case 2007: return "Chapter 7 (hard)";
            case 2008: return "Chapter 8 (hard)";
            case 6001: return "Arai's Diary 1";
            case 6002: return "Arai's Diary 2";
            case 6003: return "Arai's Diary 3";
            case 6004: return "Arai's Diary 4";
            case 6005: return "Arai's Diary 5";
            case 7001: return "Black Jaguar Park, Under Construction!";
            case 7002: return "Fitness Test, Brown Bear";
            case 7003: return "Cellien Cleanup";
            case 7004: return "Kemo Detective: The Movie";
            case 7005: return "Fitness Test, Monkey";
            case 7006: return "Cellien Cleanup";
            case 7007: return "Fitness Test, AyeAye";
            case 7008: return "Let's Search for the Star! A Sparkling Christmas";
            case 7009: return "Cellien Cleanup";
            case 7010: return "Chameleon Event";
            case 7011: return "Fitness Test, Degu";
            case 7012: return "Cellien Cleanup";
            case 7013: return "Delicious ♡ Mouth-Watering Valentines";
            case 7014: return "Fitness Test, White Lion";
            case 7015: return "Cellien Cleanup";
            case 7016: return "Dance with the Darkness! Jet-Black Darkness Hinamatsuri";
            case 7017: return "Fitness Test, Hippo";
            case 7018: return "Penguins Performance Project";
            case 7019: return "School Event";
            case 7020: return "Fitness Test, Giraffe";
            case 7021: return "Cellien Cleanup";
            case 7022: return "Japari Park off-limits area 1";
            case 7023: return "Hard run!";
            case 7024: return "Fitness Test, Roadrunner";
            case 7025: return "Cellien Cleanup";
            case 7026: return "Japari Park off-limits area 2";
            case 7027: return "Japari Cafe de Salad Bowl";
            case 7028: return "Fitness Test, Tamandua";
            case 7029: return "Japari Park off-limits area 3";
            case 7030: return "Wild Release Challenge";
            case 7031: return "Cellien Cleanup";
            case 7032: return "Japari Park off-limits area 4";
            case 7033: return "Beach saver";
            case 7034: return "Fitness Test, Manul";
            case 7035: return "Wild Release Challenge";
            case 7036: return "Cellien Cleanup";
            case 7037: return "Harvest Gaden Treasure Hunt";
            default: return "unknown";
        }
    }

    public static string GetEnemyName(int id)
    {
        var cases = new Dictionary<Func<int, bool>, string>
            {
                { x => x < 20010 , "Sibire" },
                { x => x < 20020 , "Fangcell" }, //Fangcell
                { x => x < 20030 , "Paramecium" },
                { x => x < 20040 , "Mikazuki" },
                { x => x < 20050 , "Volbox" },
                { x => x < 20060 , "Tilcell" },
                { x => x < 20080 , "Denwa" },
                { x => x < 20090 , "Screw" }, //Bolt
                { x => x < 20100 , "Helmet" }, //Met
                { x => x < 20110 , "Pegasus" }, //Huvarin
                { x => x < 20120 , "Koonin" },
                { x => x < 20130 , "Octopus" },
                { x => x < 20140 , "LocoRoco" },
                { x => x < 20160 , "Boat" },
                { x => x < 20170 , "Santa Hat" },
                { x => x < 20180 , "New Year Bolt" },
                { x => x < 20200 , "Santa Fangcell" },
                { x => x < 21010 , "Aogau" },
                { x => x < 21020 , "Gotsun" },
                { x => x < 21040 , "Outlet" },
                { x => x < 21050 , "Icarin" },
                { x => x < 21060 , "Raincoat" },
                { x => x < 21070 , "Builder Shovel" },
                { x => x < 21080 , "Alex" },
                { x => x < 21100 , "Black Gau" },
                { x => x < 21110 , "Black Gotsun" },
                { x => x < 21120 , "Christmas squid" },
                { x => x < 21130 , "Kagamimochi Gotzen" },
                { x => x < 21140 , "Sachiko" },
                { x => x == 21220 , "Sachiko" },
                { x => x == 21420 , "RulerCell" },
                { x => x != -1 , "No Data" }
            };
        return cases.First(kvp => kvp.Key(id)).Value;
    }

    public static List<CostumeDataBrief> GetCostumesForWiki(List<CharaClothesData> data, int defaultRarity)
    {
        List<CostumeDataBrief> output = new List<CostumeDataBrief>();
        foreach(var costume in data)
        {
            var cos = new CostumeDataBrief()
            {
                ImageName = "icon dressup " + costume.id + ".png",
                Obtain = "Limited"
            };
            switch (costume.clothesId)
            {
                case 1:
                    cos.Name = "Default";
                    cos.Obtain = "Default";
                    break;
                case 3:
                    cos.Name = "Tracksuit";
                    cos.Obtain = defaultRarity < costume.getRank ? $"Upgrade to {costume.getRank} Stars" : "Default";
                    break;
                case 4:
                    cos.Name = "Park Staff";
                    cos.Obtain = defaultRarity < costume.getRank? $"Upgrade to {costume.getRank} Stars" : "Default";
                    break;
                case 5:
                    cos.Name = "Café Uniform";
                    cos.Obtain = defaultRarity < costume.getRank ? $"Upgrade to {costume.getRank} Stars" : "Default";
                    break;
                case 6:
                    cos.Name = "Special Outfit 1";
                    break;
                case 2:
                    cos.Name = "Personal Fashion";
                    cos.Obtain = defaultRarity < costume.getRank ? $"Upgrade to {costume.getRank} Stars" : "Default";
                    break;
                case 8:
                    cos.Name = "Party Dress";
                    cos.Obtain = "Increase Photo Pocket level to 12";
                    break;
                case 9:
                    cos.Name = "School Uniform";
                    break;
                case 10:
                    cos.Name = "Gym Clothes";
                    break;
                case 11:
                    cos.Name = "Raincoat";
                    break;
                case 12:
                    cos.Name = "Hanamaru Animal Swimsuit";
                    break;
                case 13:
                    cos.Name = "Cafe Uniform (Green)";
                    cos.Obtain = "Dojo reward";
                    break;
                case 14:
                    cos.Name = "Cafe Uniform (Blue)";
                    cos.Obtain = "Dojo reward";
                    break;
                case 15:
                    cos.Name = "Cafe Uniform (Pink)";
                    cos.Obtain = "Dojo reward";
                    break;
                case 16:
                    cos.Name = "Orihime Costume";
                    break;
                case 17:
                    cos.Name = "Arle's Outfit";
                    break;
                case 18:
                    cos.Name = "Marine Nanoda! Swimsuit";
                    break;
                case 19:
                    cos.Name = "Batten Japari Dan Swimsuit";
                    break;
                case 20:
                    cos.Name = "Lifeguard Swimsuit";
                    break;
                case 21:
                    cos.Name = "Harvest Festival Outfit";
                    break;
                case 22:
                    cos.Name = "Halloween Costume";
                    break;
                case 23:
                    cos.Name = "Classic Maid Uniform";
                    break;
                case 24:
                    cos.Name = "Arle's Outfit";
                    break;
                case 25:
                    cos.Name = "Swimsuit";
                    break;
                case 26:
                    cos.Name = "Swimsuit";
                    break;
                case 27:
                    cos.Name = "Swimsuit";
                    break;
                case 28:
                    cos.Name = "Matching Clothes";
                    break;
                case 29:
                    cos.Name = "Frilly Frilly PPP Bikini";
                    break;
                case 30:
                    cos.Name = "Venus Flower Bikini";
                    break;
                case 31:
                    cos.Name = "Christmas Dress";
                    break;
                case 32:
                    cos.Name = "Chinese Shrine Maiden";
                    break;
                case 33:
                    cos.Name = "Chinese Dress";
                    break;
                case 34:
                    cos.Name = "Hanamaru Stage Costume";
                    break;
                case 35:
                    cos.Name = "Camo Outfit";
                    break;
                case 36:
                    cos.Name = "でびでび・ふりる";
                    break;
                case 37:
                    cos.Name = "夜に紛れし闇の衣";
                    break;
                case 38:
                    cos.Name = "Santa";
                    break;
                case 39:
                    cos.Name = "Fluffy Pajamas";
                    break;
                case 40:
                    cos.Name = "Japanese Armor";
                    break;
                case 41:
                    cos.Name = "Miko Outfit";
                    break;
                case 42:
                    cos.Name = "PPP Hoodie";
                    break;
                case 43:
                    cos.Name = "Blue Tracksuit";
                    break;
                case 44:
                    cos.Name = "Park Staff (beige)";
                    break;
                case 45:
                    cos.Name = "Sailor School Uniform";
                    break;
                case 46:
                    cos.Name = "Snow Knight Dress";
                    break;
                case 47:
                    cos.Name = "Sparkling Tree Dress";
                    break;
                case 48:
                    cos.Name = "Retro Detective";
                    break;
                case 49:
                    cos.Name = "Steampunk Detective";
                    break;
                case 50:
                    cos.Name = "鞠と花びらのはんなり和風服";
                    break;
                case 51:
                    cos.Name = "Numazu Deep Sea Aquarium Clothes";
                    break;
                case 52:
                    cos.Name = "たのしいことをお届け！";
                    break;
                default:
                    Console.WriteLine($"{costume.clothesId}\t{costume.id}\t{costume.name}");
                    break;
            }
            output.Add(cos);
        }
        return output;
    }

    void PrintArray(string text)
    {
        JArray jArray = JArray.Parse(text);
        foreach (JObject o in jArray.Children<JObject>())
        {
            //Console.WriteLine("nameEn" + ": " + o.GetValue("nameEn"));
            foreach (JProperty p in o.Properties())
            {
                string name = p.Name;
                //string value = (string)p.Value;
                //Console.WriteLine(name + " -- " + value);
                //Console.WriteLine($"[JsonProperty(\"{name}\")]");
                Console.WriteLine($"public string {name};");
            }
            break;
        }
    }

    void PrintObject(string text)
    {
        JObject jObject = JObject.Parse(text);
        PrintArray(jObject["buffList"].ToString());
        foreach (var p in jObject)
        {
            Console.WriteLine($"[JsonProperty(\"{p.Key}\")]");
            Console.WriteLine($"public string {p.Value};");
        }
    }

    public DateTime EpochToKF3Time(long epochMilliseconds)
    {
        DateTime time = DateTimeOffset.FromUnixTimeMilliseconds(epochMilliseconds).DateTime;
        if (time < new DateTime(2019, 09, 24))
            return new DateTime(2019, 09, 24);
        else
            return time;
    }

    public string EpochToPrefix(string epoch)
    {
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(epoch)).DateTime;
        return dateTime.ToString("yyyy_MM_dd_HH_mm");
    }
}
public static class MyExtensions
{
    public static void AddIfNotNull<TValue>(this List<TValue> list, TValue value)
    {
        if ((object)value != null)
            list.Add(value);
    }
}
