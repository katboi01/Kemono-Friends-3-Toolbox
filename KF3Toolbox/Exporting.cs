﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.DirectoryServices.ActiveDirectory;

public partial class KF3Parse
{
    void ExportCacheFile(string openFile)
    {
        string gzipExport = LoadGzipFile(openFile);
        if (gzipExport == "error") return;
        string fileName = openFile + "_" + System.DateTime.Today.Year + "_" + System.DateTime.Today.Month + "_" + System.DateTime.Today.Day + ".txt";
        JArray jArray = JArray.Parse(gzipExport);
        bool first = true;

        using (StreamWriter sw = new StreamWriter(SharedSettings.exportPath + "cache\\" + fileName))
        {
            foreach (JObject o in jArray.Children<JObject>())
            {
                if (first)
                {
                    foreach (JProperty p in o.Properties())
                    {
                        sw.Write($"{p.Name};");
                    }
                    sw.WriteLine();
                    first = false;
                }

                foreach (JProperty p in o.Properties())
                {
                    if (p.Name == "bannerName")
                    {
                        if (p.Value.ToString() != "")
                        {
                            if (openFile == "BANNER_DATA")
                            {
                                if (EpochToKF3Time((long)o.Property("endTime").Value) >= DateTime.Now && !File.Exists(SharedSettings.exportPath + "homeBanner\\" + EpochToPrefix(o.Property("startTime").Value.ToString()) + "_" + p.Value + "_" + EpochToPrefix(o.Property("endTime").Value.ToString()) + ".png"))
                                {
                                    string link = SharedServers.cdn + "Texture2D/HomeBanner/home_banner_" + p.Value + ".png";
                                    using (WebClient client = new WebClient())
                                    {
                                        try
                                        {
                                            client.DownloadFile(link, SharedSettings.exportPath + "homeBanner\\" + EpochToPrefix(o.Property("startTime").Value.ToString()) + "_" + p.Value + ".png");
                                        }
                                        catch
                                        {
                                            Console.WriteLine(SharedServers.cdn + "Texture2D/HomeBanner/home_banner_" + p.Value + ".png");
                                            Console.WriteLine("banner not found");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (p.Name == "banner")
                    {
                        if (p.Value.ToString() != "")
                        {
                            if (openFile == "GACHA_DATA")
                            {
                                if (EpochToKF3Time((long)o.Property("endDatetime").Value) >= DateTime.Now && !File.Exists(SharedSettings.exportPath + "gachaBanner\\" + EpochToPrefix(o.Property("startDatetime").Value.ToString()) + "_" + p.Value + "_" + EpochToPrefix(o.Property("endDatetime").Value.ToString()) + ".png"))
                                {
                                    string link = SharedServers.cdn + "Texture2D/GachaTop/" + p.Value + ".png";
                                    using (WebClient client = new WebClient())
                                    {
                                        try
                                        {
                                            client.DownloadFile(link, SharedSettings.exportPath + "gachaBanner\\" + EpochToPrefix(o.Property("startDatetime").Value.ToString()) + "_" + p.Value + ".png");
                                        }
                                        catch
                                        {
                                            Console.WriteLine(SharedServers.cdn + "Texture2D/GachaTop/" + p.Value + ".png");
                                            Console.WriteLine("banner not found");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    sw.Write($"{p.Value.ToString().Replace("\n", " ")};");
                }
                sw.WriteLine();
            }
        }
        Console.WriteLine("File exported to: " + SharedSettings.exportPath + fileName);
    }

    void ExportBGM()
    {
        var masterRoomFurniture = JArray.Parse(LoadGzipFile("MASTER_ROOM_FURNITURE_DATA"));
        var list = new List<string>();
        foreach(var track in masterRoomFurniture)
        {
            if (!string.IsNullOrEmpty(track["bgmFilepath"].ToString()))
            {
                list.Add(track["bgmFilepath"].ToString() + ": " + track["bgmName"].ToString());
            }
        }
        list.Sort();
        foreach (var d in list)
        {
            Console.WriteLine(d);
        }
    }

    void ExportPhotos()
    {
        int frame = 0;
        foreach (PhotoData photo in PhotoDatas)
        {
            Animation("exporting please wait", frame++);
            DispPhoto(photo, true);
        }

        Console.Clear();
        Console.WriteLine("Files successfully exported to " + SharedSettings.exportPath + "photos\\ \n");
    }

    void ExportForWiki()
    {
        foreach (CharaData data in CharaDatas)
        {
            if (!DispWiki(data, true))
            {
                Console.WriteLine("export failed: " + data.id + " " + data.name);
            }
            //DispSkills(data, true);
        }
    }

    void ExportCSV()
    {
        StringBuilder sb = new StringBuilder();
        var newLine = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}",
                    "Name",
                    "Miracle Name",
                    "Miracle 1",
                    "Miracle 2",
                    "Miracle 3",
                    "Miracle 4",
                    "Miracle 5",
                    "Beat Name",
                    "Beat Effect",
                    "Wait Name",
                    "Wait Effect",
                    "Ability Name",
                    "Ability Effect",
                    "Ability2 Name",
                    "Ability2 Effect"
                    );
        sb.AppendLine(newLine);

        foreach (CharaData friend in CharaDatas)
        {
            try
            {
                List<string> miracleValues = FillMiracleNumbers(friend);

                newLine = string.Format("\"{0}\"\t\"{1}\"\t\"{2}\"\t\"{3}\"\t\"{4}\"\t\"{5}\"\t\"{6}\"\t\"{7}\"\t\"{8}\"\t\"{9}\"\t\"{10}\"\t\"{11}\"\t\"{12}\"\t\"{13}\"\t\"{14}\"\t\"{15}\"\t\"{16}\"",
                    friend.GetName(),
                    friend.ParamArts.actionName,
                    miracleValues[0].Replace("\r\n",@"\r\n").Replace("\n", @"\n"),
                    miracleValues[1].Replace("\r\n",@"\r\n").Replace("\n", @"\n"),
                    miracleValues[2].Replace("\r\n",@"\r\n").Replace("\n", @"\n"),
                    miracleValues[3].Replace("\r\n",@"\r\n").Replace("\n", @"\n"),
                    miracleValues[4].Replace("\r\n",@"\r\n").Replace("\n", @"\n"),
                    friend.ParamSpecialAttack.actionName,
                    friend.ParamSpecialAttack.actionEffect,
                    friend.ParamWaitAction.skillName,
                    friend.ParamWaitAction.skillEffect,
                    friend.ParamAbility.abilityName,
                    friend.ParamAbility.abilityEffect,
                    friend.ParamAbility1?.abilityName,
                    friend.ParamAbility1?.abilityEffect,
                    friend.ParamAbility2?.abilityName,
                    friend.ParamAbility2?.abilityEffect
                    );
                sb.AppendLine(newLine);
            }
            catch
            {
                Console.WriteLine("fail");
            }
            //DispSkills(data, true);
        }
        File.WriteAllText("test.txt", sb.ToString());
    }

    void ExportFriend(CharaData friend, bool wiki)
    {
        if (DispWiki(friend, true))
        {
            Console.WriteLine("Exported friend data to: " + SharedSettings.exportPath + "wiki/" + friend.nameEn + "_" + friend.id + ".txt");
        }
        else Console.WriteLine("Export failed");
    }

    void ExportScenarios()
    {
        List<Scenario> scenarios = new List<Scenario>();
        foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Scenarios")))
        {
            if (file.EndsWith(".json"))
            {
                string json = File.ReadAllText(file);
                Scenario scenario = JsonConvert.DeserializeObject<Scenario>(json);

                StringBuilder output = new StringBuilder();

                output.Append("Friends:\n");
                foreach (Scenario.CharaData chara in scenario.charaDatas)
                {
                    output.AppendLine($"\tmodel: {chara.model} name: {chara.name}");
                }
                output.AppendLine("Dialogue:");

                int linenum = 1;
                bool previousAlsoNull = false;
                for (int i = 0; i < scenario.rowDatas.Count; i++)
                {
                    Scenario.RowData row = scenario.rowDatas[i];
                    //if (string.IsNullOrEmpty(row.mSerifCharaName)) { continue; }
                    if (row.mType == 2 || !string.IsNullOrEmpty(row.mSerifCharaName))
                    {
                        previousAlsoNull = false;
                        output.AppendLine($"{linenum++}. {row.mSerifCharaName} :");
                        foreach (string str in row.mStrParams)
                        {
                            if (string.IsNullOrEmpty(str) || str == "none") { continue; }

                            string strParam = RemoveBetween(str.Replace("\n", " "), '<', '>');

                            output.AppendLine($"\t{strParam}");
                        }
                    }
                    else if (row.mType == 18)
                    {
                        if (!previousAlsoNull) { output.AppendLine($"{linenum++}:"); previousAlsoNull = true; }
                        foreach (string str in row.mStrParams)
                        {
                            if (string.IsNullOrEmpty(str) || str == "none") { continue; }

                            string strParam = RemoveBetween(str.Replace("\n", " "), '<', '>');

                            output.AppendLine($"\t{strParam}");
                        }
                    }
                }
                File.WriteAllText($"Scenarios/{Path.GetFileName(file)}.txt", output.ToString());
            }
        }
    }

    string RemoveBetween(string s, char begin, char end)
    {
        Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
        return regex.Replace(s, string.Empty);
    }

    void ListMaxStats()
    {
        Dictionary<int, int[]> topList = new Dictionary<int, int[]>();

        foreach (CharaData friend in CharaDatas)
        {
            try
            {
                StatsBrief sb = DispStats(friend, true);
                topList.Add(friend.id, new int[4] { sb.status, sb.hp, sb.atk, sb.def });
            }
            catch
            {

            }
        }

        string timeStamp = System.DateTime.Today.Year + "_" + System.DateTime.Today.Month + "_" + System.DateTime.Today.Day;
        string[] fileNames = new string[4] { "Status", "Hp", "Atk", "Def" };
        for (int i = 0; i < 4; i++)
        {
            int rank = 1;
            string outputString = "";

            foreach (KeyValuePair<int, int[]> pair in topList.OrderByDescending(key => key.Value[i]))
            {
                CharaData friend = CharaDatas.Where(f => f.id == pair.Key).First();
                if (!string.IsNullOrEmpty(friend.nickname))
                {
                    outputString += $"{rank++};{friend.nameEn + " " + friend.nickname};{pair.Value[i]}\n";
                }
                else
                {
                    outputString += $"{rank++};{friend.nameEn};{pair.Value[i]}\n";
                }
            }
            File.WriteAllText(SharedSettings.exportPath + $"TopList_{fileNames[i]}_{timeStamp}.txt", outputString);
            Console.WriteLine($"Exported to {SharedSettings.exportPath + $"TopList_{fileNames[i]}_{timeStamp}.txt"}");
        }
        Console.WriteLine();
    }
}
