﻿using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

public partial class KF3Parse
{
    public static KF3Parse Instance;

    void ReadFriendList()
    {
        Console.Clear();
        foreach (CharaData friend in CharaDatas)
        {
            string output = friend.id + ": ";

            if (string.IsNullOrEmpty(friend.nameEn))
            {
                output += friend.name;
            }
            else if (!string.IsNullOrEmpty(friend.nickname))
            {
                output += friend.nameEn + " " + friend.nickname;
            }
            else output += friend.nameEn;

            Console.WriteLine(output);
        }
        Console.Write("\n");
    }

    void ReadFriendListDebug()
    {
        Console.Clear();
        foreach (CharaData friend in CharaDatas)
        {
            string output = $"case {friend.id}:\n";

            if (string.IsNullOrEmpty(friend.nameEn))
            {
                output += $"return \"{friend.name}";
            }
            else if (!string.IsNullOrEmpty(friend.nickname))
            {
                output += $"return \"{friend.nameEn} {friend.nickname}";
            }
            else output += $"return \"{friend.nameEn}";
            output += "\";";

            Console.WriteLine(output);
        }
        Console.Write("\n");
    }

    void ReadEnemyListDebug()
    {
        List<int> cellienIds = new List<int>();
        List<QuestEnemyData> cellienDatas = new List<QuestEnemyData>();
        foreach (QuestEnemyData enemy in QuestEnemyDatas)
        {
            if (!cellienIds.Contains(enemy.enemyCharaId))
            {
                cellienIds.Add(enemy.enemyCharaId);
                cellienDatas.Add(enemy);
            }
        }
        cellienDatas = cellienDatas.OrderBy(cd => cd.enemyCharaId).ToList();

        int i = 0;

        while (i < cellienDatas.Count)
        {
            if (cellienDatas[i].ParamEnemyBase != null && cellienDatas[i - 1].ParamEnemyBase != null)
            {
                if (cellienDatas[i].ParamEnemyBase.charaName != cellienDatas[i - 1].ParamEnemyBase.charaName)
                {
                    Console.WriteLine($"{{ x => x < {cellienDatas[i].enemyCharaId}, \"{cellienDatas[i - 1].ParamEnemyBase.charaName}\" }},");
                }
            }
            i++;
        }
    }


    static void Main()
    {
        CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        SharedSettings.LocalDirectoriesManager(false);

        bool stop = false;
        while (!stop)
        {
            Console.WriteLine("0 - manual\n" +
                /*"1 - run GUI (unfinished, has interactive stat calculator)\n" +*/
                "2 - run Console\n" +
                "3 - run Console (fastboot)\n"); //+
                                                 //"4 - parse scenarios");
            switch (Console.ReadLine())
            {
                case "0":
                    {
                        Console.Clear();
                        Console.WriteLine("For this tool to work you have to:\n" +
                            $"-Switch Terminal's font to one that supports japanese characters (e.g. MS Gothic)\n" +
                            $"-have KF3 installed and ran at least once, verify there are cache files (*.d) in:\n" +
                            $"  {SharedSettings.cachePath}\n" +
                            $"-verify that parameter.asset file exists in\n" +
                            $"  {SharedSettings.assetsPath + "parameter.asset"}\n" +
                            $"That's all\n");
                        break;
                    }
                case "1": Application.Run(new KFParseUI()); break;
                case "2":
                    {
                        KF3Parse p = new KF3Parse();
                        p.ParseKf3(true, false);
                        stop = true;
                        break;
                    }
                case "3":
                    {
                        KF3Parse p = new KF3Parse();
                        p.ParseKf3(true, true);
                        stop = true;
                        break;
                    }
                case "4":
                    {
                        KF3Parse p = new KF3Parse();
                        p.ExportScenarios();
                        break;
                    }
                case "5":
                    {
                        KF3Parse p = new KF3Parse();
                        p.Test();
                        break;
                    }
                default: break;
            }
        }
    }

    public void Test()
    {
        Console.WriteLine(EpochToPrefix("1621609200000"));
    }

    public void ParseKf3(bool runConsole, bool fastboot)
    {
        Instance = this;
        ReloadData(fastboot);
        Console.WriteLine();

        sw = new StreamWriter(Console.OpenStandardOutput())
        {
            AutoFlush = true
        };
        Console.SetOut(sw);
        bool running = runConsole;
        string shortNumber;
        while (running)
        {
            if (friendLoaded)
            {
                Console.WriteLine("\n 1 - show stats \n 2 - show animal data \n 3 - show special attacks " +
                    "\n 4 - show upgrade items \n 5 - change friend \n 6 - export friend data " +
                    "\n 0 - quit");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": Console.Clear(); try { DispStats(loadedFriend, false); } catch { Console.WriteLine("failed"); } break;
                    case "2": Console.Clear(); DispFacts(ref loadedFriend, false); break;
                    case "3": Console.Clear(); try { DispSkills2(loadedFriend); } catch { Console.WriteLine("failed"); } break;
                    case "4": Console.Clear(); DispPromoteItems(ref loadedFriend, false); break;
                    case "5": Console.Clear(); friendLoaded = false; break;
                    case "6": Console.Clear(); ExportFriend(loadedFriend, true); break;
                    case "7": GetDropLocations(14019); break;
                    case "0": return;
                    default: break;
                }
            }
            else
            {
                if (fastboot)
                {
                    Console.WriteLine(" Input Friend ID - single friend load \n list - friend list" +
                        "\n export - rewrite cache to txt \n export wiki - export all friends to wiki formatting " +
                        "\n export cards - exports photos to txt" +
                        "\n listmax - generate sorted list of friends based on stats \n 0 - exit");
                }
                else
                {
                    Console.WriteLine(" Input Friend ID - single friend load \n list enemies - cellien list \n list - friend list" +
                        "\n export - rewrite cache to txt \n export wiki - export all friends to wiki formatting " +
                        "\n export cards - exports photos to txt \n droplist - generate list of stages and their loot" +
                        "\n listmax - generate sorted list of friends based on stats \n 0 - exit");
                }
                shortNumber = Console.ReadLine();
                switch (shortNumber)
                {
                    case "test": break;
                    case "list": Console.Clear(); ReadFriendList(); break;
                    case "list enemies": Console.Clear(); ReadEnemyListDebug(); break;
                    case "listmax": Console.Clear(); ListMaxStats(); break;
                    case "droplist": Console.Clear(); ReadDropData(); break;
                    case "tracklist": Console.Clear(); ExportBGM(); break;
                    case "export":
                        {
                            Console.Clear();
                            string[] files = Directory.GetFiles(SharedSettings.cachePath);
                            foreach (string file in files)
                            {
                                if (file.Remove(0, file.Length - 1) == "d")
                                {
                                    Console.WriteLine(file.Remove(file.Length - 2, 2).Replace(SharedSettings.cachePath, ""));
                                    ExportCacheFile(file.Remove(file.Length - 2, 2).Replace(SharedSettings.cachePath, ""));
                                }
                            };
                            Console.WriteLine();
                            break;
                        }
                    case "export csv":
                        ExportCSV(); break;
                    case "export wiki": Console.Clear(); ExportForWiki(); break;
                    case "export cards": Console.Clear(); ExportPhotos(); break;
                    case "0": return;
                    default:
                        {
                            if (shortNumber == "0" || shortNumber.Length > 4) { Console.Clear(); Console.WriteLine("Incorrect ID"); goto BadNumber; }
                            if (int.Parse(shortNumber) > 1000)
                            {
                                DispPhoto(PhotoDatas.First(p => p.id.ToString() == shortNumber), false);
                            }
                            else
                            {
                                loadedFriend = FriendFromNumber(shortNumber);
                                if (loadedFriend != null)
                                {
                                    Console.WriteLine("Loaded Friend: " + loadedFriend.id + "_" + loadedFriend.nameEn);
                                    friendLoaded = true;
                                }
                            }
                            break;
                        }
                }
            BadNumber: { }
            }
        }
    }

    public CharaData FriendFromNumber(string shortNumber)
    {
        CharaData friend = CharaDatas.Where(f => f.id.ToString() == shortNumber).FirstOrDefault();
        return friend;
    }

    public CharaData FriendFromNumber(int shortNumber)
    {
        loadedFriend = CharaDatas.FirstOrDefault(f => f.id == shortNumber);
        if (loadedFriend != null)
        {
            Console.WriteLine("Loaded Friend: " + loadedFriend.id + "_" + loadedFriend.nameEn);
            return loadedFriend;
        }
        return null;
    }

    /// <summary>
    /// Gets friend by name and returns it
    /// </summary>
    public CharaData FriendFromName(string name)
    {
        loadedFriend = CharaDatas.FirstOrDefault(f => f.nameEn == name);
        if (loadedFriend != null)
        {
            Console.WriteLine("Loaded Friend: " + loadedFriend.id + "_" + loadedFriend.nameEn);
            return loadedFriend;
        }
        else
        {
            loadedFriend = CharaDatas.FirstOrDefault(f => f.nameEn.Contains(name));
            return loadedFriend;
        }
    }
}