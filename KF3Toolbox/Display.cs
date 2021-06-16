using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KF3Shared;

namespace KF3Toolbox
{
    public partial class KF3Parse
    {
        public StatsBrief DispStats(CharaData friend, bool wiki = false)
        {
            string outputString = "";
            int hpCosBonus = 0, atkCosBonus = 0, defCosBonus = 0;

            foreach (CharaClothesData clothe in friend.clothesDatas)
            {
                if (friend.rankHigh < clothe.getRank) { continue; }
                hpCosBonus += clothe.hpBonus;
                atkCosBonus += clothe.atkBonus;
                defCosBonus += clothe.defBonus;
            }

            int level = friend.rankHigh == 6 ? 99 : 80;
            float starBoost = 1 + (friend.rankHigh - 1) * 0.02f;
            int wr = (friend.promotePresetDatas[4].promoteStepDatetime != "1917860400000") ? 5 : 4;

            int maxHp = 0, maxAtk = 0, maxDef = 0;

            maxHp = StolenStuff.CalcParamInternal(level, 99, friend.paramAlphaBase.hpParamLv1, friend.paramAlphaBase.hpParamLv99, friend.paramAlphaBase.hpParamLvMiddle, friend.paramAlphaBase.hpLvMiddleNum);
            maxAtk = StolenStuff.CalcParamInternal(level, 99, friend.paramAlphaBase.atkParamLv1, friend.paramAlphaBase.atkParamLv99, friend.paramAlphaBase.atkParamLvMiddle, friend.paramAlphaBase.atkLvMiddleNum);
            maxDef = StolenStuff.CalcParamInternal(level, 99, friend.paramAlphaBase.defParamLv1, friend.paramAlphaBase.defParamLv99, friend.paramAlphaBase.defParamLvMiddle, friend.paramAlphaBase.defLvMiddleNum);

            maxHp = (int)Math.Ceiling((maxHp + friend.GetPromoteStat(wr, "hp")) * starBoost)  + hpCosBonus;
            maxAtk = (int)Math.Ceiling((maxAtk + friend.GetPromoteStat(wr, "atk")) * starBoost) + atkCosBonus;
            maxDef = (int)Math.Ceiling((maxDef + friend.GetPromoteStat(wr, "def")) * starBoost) + defCosBonus;

            float evd = 10 * friend.paramAlphaBase.avoidRatio + friend.GetPromoteStat(wr, "evd");
            float beatBonus = friend.GetPromoteStat(wr, "beat");
            float actBonus = friend.GetPromoteStat(wr, "act");
            float tryBonus = friend.GetPromoteStat(wr, "try");

            outputString += $"Level: {level}\n" +
                $"WR: {wr}\n" +
                $"max STATUS: {CalcStatus(maxHp, maxAtk, maxDef)}\n" +
                $"max HP: {maxHp}\n" +
                $"max ATK: {maxAtk}\n" +
                $"max DEF: {maxDef}\n" +
                $"evd: {(int)evd / 10}.{evd % 10}%\n" +
                $"Beat bonus: {(int)beatBonus / 10}.{beatBonus % 10}%\n" +
                $"Act bonus: {(int)actBonus / 10}.{actBonus % 10}%\n" +
                $"Try bonus: {(int)tryBonus / 10}.{tryBonus % 10}%\n";

            outputString += "\nCards:\n" +
                $"{friend.paramAlphaBase.orderCardType00} {friend.paramAlphaBase.orderCardValue00}\n" +
                $"{friend.paramAlphaBase.orderCardType01} {friend.paramAlphaBase.orderCardValue01}\n" +
                $"{friend.paramAlphaBase.orderCardType02} {friend.paramAlphaBase.orderCardValue02}\n" +
                $"{friend.paramAlphaBase.orderCardType03} {friend.paramAlphaBase.orderCardValue03}\n" +
                $"{friend.paramAlphaBase.orderCardType04} {friend.paramAlphaBase.orderCardValue04}\n";


            if (!wiki)
            {
                Console.WriteLine(outputString);
            }
            return new StatsBrief()
            {
                voice = friend.castName,
                attribute = friend.attribute,
                level = level,
                wr = wr,
                hp = maxHp,
                atk = maxAtk,
                def = maxDef,
                status = CalcStatus(maxHp, maxAtk, maxDef),
                evd = $"{(int)evd / 10}.{evd % 10}%",
                beatBonus = $"+{(int)beatBonus / 10}.{beatBonus % 10}%",
                actBonus = $"+{(int)actBonus / 10}.{actBonus % 10}%",
                tryBonus = $"+{(int)tryBonus / 10}.{tryBonus % 10}%"
            };
        }

        void DispPhoto(PhotoData photo, bool wiki)
        {
            if (wiki) { Console.Out.Close(); sw = new StreamWriter(SharedSettings.exportPath + "photos\\" + "Photo_" + photo.id + ".txt", false); Console.SetOut(sw); }
            else
            {
                Console.Clear();
                Console.WriteLine("Loaded Photo: " + photo.id + "_" + photo.name + "\n");
            }
            Console.Write("name: " + photo.name + "\nphoto effect: " + photo.preEffect.abilityEffect + "\nupgraded effect: " + photo.postEffect.abilityEffect + "\n\n" + photo.flavorTextBefore + "\n\n" + photo.flavorTextAfter + "\n\n");
            //stats
            Console.WriteLine("\n" + "Base stats:");
            Console.WriteLine($"Hp: {photo.hpParamLv1}");
            Console.WriteLine($"Atk: {photo.atkParamLv1}");
            Console.WriteLine($"Def: {photo.defParamLv1}");
            Console.WriteLine("\n" + "Max stats:");
            Console.WriteLine($"Hp: {photo.hpParamLvMax}");
            Console.WriteLine($"Atk: {photo.atkParamLvMax}");
            Console.WriteLine($"Def: {photo.defParamLvMax}");
            if (wiki)
            {
                Console.Out.Close(); sw = new StreamWriter(Console.OpenStandardOutput())
                {
                    AutoFlush = true
                }; Console.SetOut(sw);
            }

        }

        public SkillsBrief DispSkills(CharaData friend, bool wiki = false)
        {
            if (wiki) { Console.Out.Close(); sw = new StreamWriter(SharedSettings.exportPath + friend.nameEn + "_" + friend.id + ".txt", true); Console.SetOut(sw); Console.WriteLine("\n S K I L L S\n"); }
            else
            {
                Console.Clear();
                Console.WriteLine("Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n");
            }
            string output = "";
            //miracle
            output += $"Miracle: {friend.paramArts.actionName}\n{friend.paramArts.actionEffect}\n" +
                $"Miracle + card: {friend.paramArts.authParam.SynergyFlag}";
            Console.WriteLine(output);
            Console.WriteLine("Miracle Variable    | lvl1  |  lvl2  |  lvl3  |  lvl4  |  lvl5  |  Miracle+");
            for (int i = 0; i < friend.paramArts.damageList.Count; i++)
            {
                ParamArts.DamageList damage = friend.paramArts.damageList[i];
                Console.WriteLine($"DAMAGE{i}:          | " + Math.Round(100 * damage.damageRate)
                    + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 1 * damage.growthRate)))
                    + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 2 * damage.growthRate)))
                    + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 3 * damage.growthRate)))
                    + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 4 * damage.growthRate)))
                    + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 5 * damage.growthRate))) + " |");
            }
            for (int i = 0; i < friend.paramArts.buffList.Count; i++)
            {
                ParamArts.BuffList buff = friend.paramArts.buffList[i];
                Console.WriteLine($"BUFF{i}:             | " + Math.Round(100 - 100 * buff.coefficient)
                    + "  |  " + Math.Round(100 - 100 * (buff.coefficient * (1 + 1 * buff.growthRate)))
                    + "  |  " + Math.Round(100 - 100 * (buff.coefficient * (1 + 2 * buff.growthRate)))
                    + "  |  " + Math.Round(100 - 100 * (buff.coefficient * (1 + 3 * buff.growthRate)))
                    + "  |  " + Math.Round(100 - 100 * (buff.coefficient * (1 + 4 * buff.growthRate)))
                    + "  |  " + Math.Round(100 - 100 * (buff.coefficient * (1 + 5 * buff.growthRate))) + " |");
                Console.WriteLine($"\nTurns: {buff.turn} Activation rate: {buff.successRate}%");
            }
            List<string> miracleValues = FillMiracleNumbers(friend);
            for (int i = 0; i < miracleValues.Count(); i++)
            {
                Console.WriteLine($"Lvl {i+1}:");
                Console.WriteLine(miracleValues[i]);
            }
            //abilities
            Console.WriteLine("\n" + "Unique Trait: " + friend.paramAbility.abilityName + "\n" + friend.paramAbility.abilityEffect);
            if (friend.paramAbility1 != null)
            {
                Console.WriteLine("\n" + "Miracle Trait: " + friend.paramAbility1.abilityName + "\n" + friend.paramAbility1.abilityEffect);
            }
            //wait
            Console.WriteLine("\n" + "Standby Skill: " + friend.paramWaitAction.skillName + "\n" + friend.paramWaitAction.skillEffect);
            //special
            Console.WriteLine("\n" + "Special Attack: " + friend.paramSpecialAttack.actionName + "\n" + friend.paramSpecialAttack.actionEffect);
            foreach(ParamSpecialAttack.BuffList buff in friend.paramSpecialAttack.buffList)
            {
                Console.WriteLine($"\n Turns: {buff.turn} Activation rate: {buff.successRate}%");
            }

            if (wiki)
            {
                Console.Out.Close(); sw = new StreamWriter(Console.OpenStandardOutput())
                {
                    AutoFlush = true
                }; Console.SetOut(sw);
            }
            return new SkillsBrief()
            {
                MiracleName = friend.paramArts.actionName,
                MiracleDesc = friend.paramArts.actionEffect,
                MiracleType = friend.paramArts.authParam.SynergyFlag,
                MiracleMax = miracleValues[4],

                BeatName = friend.paramSpecialAttack.actionName,
                BeatDesc = friend.paramSpecialAttack.actionEffect,

                WaitName = friend.paramWaitAction.skillName,
                WaitDesc = friend.paramWaitAction.skillEffect,

                Ability1Name = friend.paramAbility1.abilityName,
                Ability1Desc = friend.paramAbility1.abilityEffect,
                AbilityName = friend.paramAbility.abilityName,
                AbilityDesc = friend.paramAbility.abilityEffect
            };
        }
        void DispFacts(ref CharaData friend, bool wiki)
        {
            if (wiki) { Console.Out.Close(); sw = new StreamWriter(SharedSettings.exportPath + friend.nameEn + "_" + friend.id + ".txt", true); Console.SetOut(sw); Console.WriteLine("\n I N F O\n"); }
            else
            {
                Console.Clear();
                Console.WriteLine("Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n");
            }
            Console.WriteLine("Release Date: " + EpochToKF3Time(friend.startTime).ToLongDateString() + "\n");

            Console.WriteLine("Animal name: " + friend.animalScientificName + ", eponym: " + friend.eponymName + "\n" + "Japanese name: " + friend.name + ", English name: " + friend.nameEn
                + "\n" + "voice actress: " + friend.castName + "\n" + "Animal's distribution areas: " + friend.animalDistributionArea + ", habitat: " + friend.animalTabitat + "\n" + "Animal flavor text: " + friend.animalFlavorText
                + "\n" + "Friend's flavor text: " + friend.flavorText);
            if (wiki)
            {
                Console.Out.Close(); sw = new StreamWriter(Console.OpenStandardOutput())
                {
                    AutoFlush = true
                }; Console.SetOut(sw);
            }
        }
        void DispPromoteItems(ref CharaData friend, bool wiki)
        {
            if (wiki) { Console.Out.Close(); sw = new StreamWriter(SharedSettings.exportPath + friend.nameEn + "_" + friend.id + ".txt", true); Console.SetOut(sw); Console.WriteLine("\n U P G R A D E\n I T E M S\n"); }
            else
            {
                Console.Clear();
                Console.WriteLine("Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n");
            }

            foreach(PromotePresetData ppd in friend.promotePresetDatas)
            {
                Console.Write((ppd.promoteStep+1) + ": ");
                if(ppd.promoteStepDatetime == "1917860400000")
                {
                    Console.WriteLine("Unavailable");
                }
                else
                {
                    foreach (PromoteData pd in ppd.promoteDatas)
                    {
                        Console.Write(GetItemName(pd.promoteUseItemId00) + " ");
                    }
                }
                Console.Write("\n");
            }

            if (wiki)
            {
                Console.Out.Close(); sw = new StreamWriter(Console.OpenStandardOutput())
                {
                    AutoFlush = true
                }; Console.SetOut(sw);
            }
        }
        void DispWiki(CharaData friend, bool wiki = false)
        {
            string outputString = "";

            string starsWord = "";
            switch (friend.rankLow)
            {
                case 2: starsWord = "Two"; break;
                case 3: starsWord = "Three"; break;
                case 4: starsWord = "Four"; break;
                case 5: starsWord = "Five"; break;
            }

            outputString += $"[[Category:{friend.attribute} KF3 Friends]] [[Category:{starsWord} Star KF3 Friends]] [[Category:Missing Content]] [[Category:Needs Audio]] {{{{#vardefine:id|{friend.id.ToString().PadLeft(4, '0')}}}}}\n\n" +
                "{{FriendBox/KF3\n"+
                $"|name={friend.nameEn.Replace("_", " ")}\n" +
                $"|apppic={friend.nameEn.Replace("_", " ")}KF3.png\n" +
                $"|apprarity={{{{KF3{friend.rankLow}Star}}}}\n" +
                $"|seiyuu={friend.castName}\n" +
                $"|attribute={friend.attribute} {{{{KF3{friend.attribute}}}}}\n" +
                $"|implemented={EpochToKF3Time(friend.startTime).ToLongDateString()} (App)\n" +
                $"|id={friend.id}\n" +
                "}}\n";

            outputString += "{{FriendBuilder/KF3\n" +
                $"|introduction = '''{friend.nameEn.Replace("_", " ")}''' is a Friend that appears in the app version of[[Kemono Friends 3]].\n";

            float starBoostLow = 1 + (friend.rankLow - 1) * 0.02f;
            double hp = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.paramAlphaBase.hpParamLv1, friend.paramAlphaBase.hpParamLv99, friend.paramAlphaBase.hpParamLvMiddle, friend.paramAlphaBase.hpLvMiddleNum) * starBoostLow);
            double atk = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.paramAlphaBase.atkParamLv1, friend.paramAlphaBase.atkParamLv99, friend.paramAlphaBase.atkParamLvMiddle, friend.paramAlphaBase.atkLvMiddleNum) * starBoostLow);
            double def = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.paramAlphaBase.defParamLv1, friend.paramAlphaBase.defParamLv99, friend.paramAlphaBase.defParamLvMiddle, friend.paramAlphaBase.defLvMiddleNum) * starBoostLow);

            outputString +=
                $"|status={CalcStatus((int)hp, (int)atk, (int)def)}\n" +
                $"|hp={hp}\n" +
                $"|atk={atk}\n" +
                $"|def={def}\n" +
                $"|evd={friend.paramAlphaBase.avoidRatio}%\n" +
                $"|beat=+{friend.GetPromoteStat(0, "beat")}%\n" +
                $"|action=+{friend.GetPromoteStat(0, "act")}%\n" +
                $"|try=+{friend.GetPromoteStat(0, "try")}%\n" +
                $"|plasm={friend.paramAlphaBase.plasmPoint}\n";

            StatsBrief sb = DispStats(friend, true);

            outputString += 
                $"|maxstatus={sb.status}\n" +
                $"|maxhp={sb.hp}\n" +
                $"|maxatk={sb.atk}\n" +
                $"|maxdef={sb.def}\n" +
                $"|maxevd={sb.evd}\n" +
                $"|maxbeat={sb.beatBonus}\n" +
                $"|maxaction={sb.actBonus}\n" +
                $"|maxtry={sb.tryBonus}\n";

            int[] cardArray = new int[] { friend.paramAlphaBase.orderCardType00, friend.paramAlphaBase.orderCardType01, friend.paramAlphaBase.orderCardType02, friend.paramAlphaBase.orderCardType03, friend.paramAlphaBase.orderCardType04 };
            int[] cardValueArray = new int[]{ friend.paramAlphaBase.orderCardValue00, friend.paramAlphaBase.orderCardValue01, friend.paramAlphaBase.orderCardValue02, friend.paramAlphaBase.orderCardValue03, friend.paramAlphaBase.orderCardValue04};
            string cards = "|flags=";

            for(int i = 0; i<5; i++)
            {
                cards += " {{";
                if (cardArray[i] == 1) { cards += "Beat}}"; }
                else if (cardArray[i] == 2) { cards += "Action" + cardValueArray[i] + "}}"; }
                else if (cardArray[i] == 3)
                { //20, 30, 40
                    if (cardValueArray[i] == 20) cards += "TryLow}}";
                    if (cardValueArray[i] == 30) cards += "TryMiddle}}";
                    if (cardValueArray[i] == 40) cards += "TryHigh}}";
                }
            }

            string cardType = "";
            switch (friend.paramArts.authParam.SynergyFlag)
            {
                case 1:
                    cardType = "Beat";
                    break;
                case 2:
                    cardType = "Action20";
                    break;
                case 3:
                    cardType = "TryHigh";
                    break;
            }

            outputString += cards + "\n" +
                "|miracleplus={{" + cardType + "}}\n" +
                "|miracle=" + friend.paramArts.actionName;

            List<string> miracleValues = FillMiracleNumbers(friend);
            for (int i = 0; i < miracleValues.Count(); i++)
            {
                outputString += ("\n|miracle" + (i + 1) + "=" + miracleValues[i].Replace(@"\r\n", "\n").Replace(@"\n", "\n").Replace(@"\", " "));
            }

            outputString += "\n|beatname=" + friend.paramSpecialAttack.actionName + "\n" +
            "|beatskill=" + friend.paramSpecialAttack.actionEffect + "\n" +
            "|standby=" + friend.paramWaitAction.skillName + "\n" +
            "|standbyskill=" + friend.paramWaitAction.skillEffect + "\n" +
            "|unique=" + friend.paramAbility.abilityName + "\n" +
            "|uniqueskill=" + friend.paramAbility.abilityEffect + "\n" +
            "|miracletrait=" + friend.paramAbility1.abilityName + "\n" +
            "|miracletraitskill=" + friend.paramAbility1.abilityEffect + "\n";


            string cos = "\n|cos = ";
            string cosname = "\n|cosname = ";
            string cosobt = "\n|cosobt = ";

            List<CharaClothesData> costumes = charaClothesData.Where(c => c.clothesPresetId == friend.id).ToList();
            for (int i = 0; i < costumes.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            cos += "icon dressup " + costumes[i].id + ".png,";
                            cosname += "Default";
                            cosobt += "Default";
                            break;
                        }
                    case 1:
                        {
                            cos += "icon dressup " + costumes[i].id + ".png,";
                            cosname += ",Tracksuit";
                            cosobt += friend.rankLow >= 3 ? ",Default" : ",Upgrade to 3 Stars";
                            break;
                        }
                    case 2:
                        {
                            cos += "icon dressup " + costumes[i].id + ".png,";
                            cosname += ",Park Staff";
                            cosobt += friend.rankLow >= 4 ? ",Default" : ",Upgrade to 4 Stars";
                            break;
                        }
                    case 3:
                        {
                            cos += "icon dressup " + costumes[i].id + ".png,";
                            cosname += ",Café Uniform";
                            cosobt += friend.rankLow >= 5 ? ",Default" : ",Upgrade to 5 Stars";
                            break;
                        }
                    case 4:
                        {
                            if(friend.rankHigh == 6)
                            {
                                cos += "icon dressup " + costumes[i].id + ".png,";
                                cosname += ",Personal Fashion";
                                cosobt += friend.rankLow >= 6 ? ",Default" : ",Upgrade to 6 Stars";
                                break;
                            }
                            else
                            {
                                cos += "icon dressup " + costumes[i].id + ".png,";
                                cosname += "," + costumes[i].name;
                                cosobt += ", ";
                                break;
                            }
                        }
                    default:
                        {
                            cos += "icon dressup " + costumes[i].id + ".png,";
                            cosname += "," + costumes[i].name;
                            cosobt += ", ";
                            break;
                        }

                }
                
            }

            cos += "end";

            outputString += cos + cosname + cosobt + "\n}}";

            if (wiki) { File.WriteAllText(SharedSettings.exportPath + "wiki/" + friend.nameEn + "_" + friend.id + "_wiki.txt", outputString); }
        }
    }
}
