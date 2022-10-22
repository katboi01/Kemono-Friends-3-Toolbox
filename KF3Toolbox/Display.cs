using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static StolenStuff.CharaDef;

public partial class KF3Parse
{
    public StatsBrief DispStats(CharaData friend, bool wiki = false)
    {
        if (friend.PromotePresetDatas.Count < 4 || friend.ParamAlphaBase == null)
        {
            throw new Exception();
        }

        string outputString = "";
        int hpCosBonus = 0, atkCosBonus = 0, defCosBonus = 0;

        foreach (CharaClothesData clothe in friend.ClothesDatas)
        {
            if (friend.rankHigh < clothe.getRank) { continue; }
            hpCosBonus += clothe.hpBonus;
            atkCosBonus += clothe.atkBonus;
            defCosBonus += clothe.defBonus;
        }

        int level = friend.rankHigh == 6 ? 99 : 80;
        float starBoost = 1 + (friend.rankHigh - 1) * 0.02f;
        int wr = (friend.PromotePresetDatas[4].promoteStepDatetime != "1917860400000") ? 5 : 4;

        int maxHp = 0, maxAtk = 0, maxDef = 0;

        maxHp = StolenStuff.CalcParamInternal(level, 99, friend.ParamAlphaBase.hpParamLv1, friend.ParamAlphaBase.hpParamLv99, friend.ParamAlphaBase.hpParamLvMiddle, friend.ParamAlphaBase.hpLvMiddleNum);
        maxAtk = StolenStuff.CalcParamInternal(level, 99, friend.ParamAlphaBase.atkParamLv1, friend.ParamAlphaBase.atkParamLv99, friend.ParamAlphaBase.atkParamLvMiddle, friend.ParamAlphaBase.atkLvMiddleNum);
        maxDef = StolenStuff.CalcParamInternal(level, 99, friend.ParamAlphaBase.defParamLv1, friend.ParamAlphaBase.defParamLv99, friend.ParamAlphaBase.defParamLvMiddle, friend.ParamAlphaBase.defLvMiddleNum);

        maxHp = (int)Math.Ceiling((maxHp + friend.GetPromoteStat(wr, "hp")) * starBoost) + hpCosBonus;
        maxAtk = (int)Math.Ceiling((maxAtk + friend.GetPromoteStat(wr, "atk")) * starBoost) + atkCosBonus;
        maxDef = (int)Math.Ceiling((maxDef + friend.GetPromoteStat(wr, "def")) * starBoost) + defCosBonus;

        float evd = 10 * friend.ParamAlphaBase.avoidRatio + friend.GetPromoteStat(wr, "evd");
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
            $"{friend.ParamAlphaBase.orderCardType00} {friend.ParamAlphaBase.orderCardValue00}\n" +
            $"{friend.ParamAlphaBase.orderCardType01} {friend.ParamAlphaBase.orderCardValue01}\n" +
            $"{friend.ParamAlphaBase.orderCardType02} {friend.ParamAlphaBase.orderCardValue02}\n" +
            $"{friend.ParamAlphaBase.orderCardType03} {friend.ParamAlphaBase.orderCardValue03}\n" +
            $"{friend.ParamAlphaBase.orderCardType04} {friend.ParamAlphaBase.orderCardValue04}\n";


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
        string photoSource = photo.id switch
        {
            < 5000 => "Gacha",
            5000 => "Event/Shop",
            > 5000 and < 6000 => "Gacha",
            6000 => "Event/Shop",
            > 6000 and < 7000 => "Friend Story",
            > 7000 and < 7500 => "Main Story",
            7501 or 7502 => "Labyrinth",
            7503 or 7504 => "Event/Shop",
            _ => "Missing"
        };

        string categorySymbol = photo.type switch
        {
            1 => "{{KF3PhotoPaw}}",
            2 => "{{KF3PhotoStar}}",
            _ => ""
        };

        string categoryName = photo.type switch
        {
            1 => "Passive",
            2 => "Conditional",
            _ => "Missing"
        };

        string limited = photo.id switch
        {
            <= 6000 => "Yes",
            > 6000 => "No",
        };

        string rarity = photo.rarity switch
        {
            1 => "One",
            2 => "Two",
            3 => "Three",
            4 => "Four",
            _ => "Missing"
        };

        string outputString = string.Join("\n",
            "{{PhotoBox",
            $"|photoimage= KF3.png",
            $"|photoupgrade= KF3+.png",
            $"|rarity= {{{{KF3{photo.rarity}Star}}}}",
            photo.type <= 2 ? $"|type= {categorySymbol} {categoryName}" : null,
            $"|from= {photoSource}",
            $"|illus= {photo.illustratorName}",
            $"|limited= {limited}",
            $"|jpcardname= {photo.name}",
            $"|ID= {photo.id}",
            $"|stamina= {photo.hpParamLv1}",
            $"|maxstamina= {photo.hpParamLvMax}",
            $"|attack= {photo.atkParamLv1}",
            $"|maxattack= {photo.atkParamLvMax}",
            $"|defense= {photo.defParamLv1}",
            $"|maxdefense= {photo.defParamLvMax}",
            photo.preEffect != null? $"|basetrait= {GeneratePhotoEffect(photo.preEffect)}" : null,
            photo.postEffect != null ? $"|upgradetrait= {GeneratePhotoEffect(photo.postEffect)}" : null,
            $"|story= {photo.flavorTextBefore}",
            $"|upgradestory= {photo.flavorTextAfter}",
            $"|update= {EpochToKF3Time(long.Parse(photo.startTime)).ToString("D")}: Added to [[Kemono Friends 3]].",
            //$"|strategy= -",
            "}}",
            $"[[Category:{rarity} Star Photos]]{(photo.type <= 2? $" [[Category:{categoryName} Photos]] " : " ")}[[Category:Photos (KF3)]] [[Category:Illustrated by {photo.illustratorName}]]"
            );

        if (!wiki)
        {
            Console.WriteLine(outputString);
            Console.WriteLine();
            Console.WriteLine(GeneratePhotoEffect(photo.preEffect));
        }
        else
        {
            File.WriteAllText(Path.Combine(SharedSettings.exportPath, "photos", $"Photo_{photo.id}.txt"), outputString);
        }
    }

    public SkillsBrief DispSkills(CharaData friend, bool wiki = false)
    {
        if (friend.ParamAlphaBase == null)
        {
            throw new Exception();
        }

        SkillsBrief sb = new SkillsBrief();
        string output = "Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n\n";

        sb.MiracleName = friend.ParamArts.actionName;
        sb.MiracleDesc = GenerateCharaDamageEffect(friend.ParamArts.damageList) + "\n" + GenerateCharaBuffEffect(friend.ParamArts.buffList);
        sb.MiracleType = friend.ParamArts.authParam.SynergyFlag;

        output += $"Miracle Name: {sb.MiracleName}\n{sb.MiracleDesc}\n\n";

        List<string> miracleValues = FillMiracleNumbers(friend);
        for (int i = 0; i < miracleValues.Count(); i++)
        {
            output += $"Lvl {i + 1}: {miracleValues[i]}\n";
        }

        output += "\n";

        sb.MiracleMax = miracleValues[4];

        //abilities
        sb.AbilityName = friend.ParamAbility.abilityName;
        sb.AbilityDesc = GenerateAbilityBuffEffects(friend.ParamAbility.buffList);
        output += $"Unique Trait: {sb.AbilityName}\n{sb.AbilityDesc}\n\n";

        if (friend.ParamAbility2 != null)
        {
            sb.Ability1Name = friend.ParamAbility2.abilityName;
            sb.Ability1Desc = GenerateAbilityBuffEffects(friend.ParamAbility2.buffList);
            output += $"Miracle Trait: {sb.Ability1Name}\n{sb.Ability1Desc}\n\n";
        }

        //wait
        sb.WaitName = friend.ParamWaitAction.skillName;
        sb.WaitDesc = $"{GenerateCharaBuffEffect(friend.ParamWaitAction.buffList)}\n{friend.ParamWaitAction.activationRate}% chance, {friend.ParamWaitAction.activationNum} times";
        output += $"Standby Skill: {sb.WaitName}\n{sb.WaitDesc}\n\n";

        //special
        sb.BeatName = friend.ParamSpecialAttack.actionName;
        sb.BeatDesc = GenerateCharaDamageEffect(friend.ParamSpecialAttack.damageList) + "\n" + GenerateCharaBuffEffect(friend.ParamSpecialAttack.buffList);
        output += $"Special Attack: {sb.BeatName}\n{sb.BeatDesc}";
        Console.WriteLine(output);
        if (wiki)
        {
            Directory.CreateDirectory(SharedSettings.exportPath + "generated/");
            File.WriteAllText(SharedSettings.exportPath + "generated/" + friend.nameEn.Trim() + "_" + friend.id + ".txt", output);
        }
        return sb;
    }

    public SkillsBrief DispSkills2(CharaData friend)
    {
        if (friend.ParamAlphaBase == null)
        {
            throw new Exception();
        }

        Console.WriteLine("Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n");
        string output = "";
        //miracle
        output += $"Miracle: {friend.ParamArts.actionName}\n{friend.ParamArts.actionEffect}\n" +
            $"Miracle + card: {friend.ParamArts.authParam.SynergyFlag}";
        Console.WriteLine(output);
        Console.WriteLine("Miracle Variable    | lvl1  |  lvl2  |  lvl3  |  lvl4  |  lvl5  |  Miracle+");
        for (int i = 0; i < friend.ParamArts.damageList.Count; i++)
        {
            ParamArts.CharaDamageParam damage = friend.ParamArts.damageList[i];
            Console.WriteLine($"DAMAGE{i}:          | " + Math.Round(100 * damage.damageRate)
                + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 1 * damage.growthRate)))
                + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 2 * damage.growthRate)))
                + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 3 * damage.growthRate)))
                + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 4 * damage.growthRate)))
                + "  |  " + Math.Round(100 * (damage.damageRate * (1 + 5 * damage.growthRate))) + " |");
        }
        for (int i = 0; i < friend.ParamArts.buffList.Count; i++)
        {
            ParamArts.CharaBuffParam buff = friend.ParamArts.buffList[i];
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
            Console.WriteLine($"Lvl {i + 1}:");
            Console.WriteLine(miracleValues[i]);
        }
        //abilities
        Console.WriteLine("\n" + "Unique Trait: " + friend.ParamAbility.abilityName + "\n" + friend.ParamAbility.abilityEffect);
        if (friend.ParamAbility2 != null)
        {
            Console.WriteLine("\n" + "Miracle Trait: " + friend.ParamAbility2.abilityName + "\n" + friend.ParamAbility2.abilityEffect);
        }
        //wait
        Console.WriteLine("\n" + "Standby Skill: " + friend.ParamWaitAction.skillName + "\n" + friend.ParamWaitAction.skillEffect);
        //special
        Console.WriteLine("\n" + "Special Attack: " + friend.ParamSpecialAttack.actionName + "\n" + friend.ParamSpecialAttack.actionEffect);
        foreach (ParamArts.CharaBuffParam buff in friend.ParamSpecialAttack.buffList)
        {
            Console.WriteLine($"\n Turns: {buff.turn} Activation rate: {buff.successRate}%");
        }

        return new SkillsBrief()
        {
            MiracleName = friend.ParamArts.actionName,
            MiracleDesc = friend.ParamArts.actionEffect,
            MiracleType = friend.ParamArts.authParam.SynergyFlag,
            MiracleMax = miracleValues[4],

            BeatName = friend.ParamSpecialAttack.actionName,
            BeatDesc = friend.ParamSpecialAttack.actionEffect,

            WaitName = friend.ParamWaitAction.skillName,
            WaitDesc = friend.ParamWaitAction.skillEffect,

            Ability1Name = friend.ParamAbility2?.abilityName,
            Ability1Desc = friend.ParamAbility2?.abilityEffect,
            AbilityName = friend.ParamAbility.abilityName,
            AbilityDesc = friend.ParamAbility.abilityEffect
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

        foreach (PromotePresetData ppd in friend.PromotePresetDatas)
        {
            Console.Write((ppd.promoteStep + 1) + ": ");
            if (ppd.promoteStepDatetime == "1917860400000")
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
    bool DispWiki(CharaData friend, bool wiki = false)
    {
        string outputString = "";

        StatsBrief sb = null;
        try { sb = DispStats(friend, true); } catch { Console.WriteLine("failed"); return false; }

        string starsWord = "";
        switch (friend.rankLow)
        {
            case 2: starsWord = "Two"; break;
            case 3: starsWord = "Three"; break;
            case 4: starsWord = "Four"; break;
            case 5: starsWord = "Five"; break;
        }

        outputString += $"[[Category:{friend.attribute} KF3 Friends]] [[Category:{starsWord} Star KF3 Friends]] [[Category:Missing Content]] [[Category:Needs Audio]] {{{{#vardefine:id|{friend.id.ToString().PadLeft(4, '0')}}}}}\n\n" +
            "{{FriendBox/KF3\n" +
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
        double hp = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.ParamAlphaBase.hpParamLv1, friend.ParamAlphaBase.hpParamLv99, friend.ParamAlphaBase.hpParamLvMiddle, friend.ParamAlphaBase.hpLvMiddleNum) * starBoostLow);
        double atk = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.ParamAlphaBase.atkParamLv1, friend.ParamAlphaBase.atkParamLv99, friend.ParamAlphaBase.atkParamLvMiddle, friend.ParamAlphaBase.atkLvMiddleNum) * starBoostLow);
        double def = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.ParamAlphaBase.defParamLv1, friend.ParamAlphaBase.defParamLv99, friend.ParamAlphaBase.defParamLvMiddle, friend.ParamAlphaBase.defLvMiddleNum) * starBoostLow);

        outputString +=
            $"|status={CalcStatus((int)hp, (int)atk, (int)def)}\n" +
            $"|hp={hp}\n" +
            $"|atk={atk}\n" +
            $"|def={def}\n" +
            $"|evd={friend.ParamAlphaBase.avoidRatio}%\n" +
            $"|beat=+{friend.GetPromoteStat(0, "beat")}%\n" +
            $"|action=+{friend.GetPromoteStat(0, "act")}%\n" +
            $"|try=+{friend.GetPromoteStat(0, "try")}%\n" +
            $"|plasm={friend.ParamAlphaBase.plasmPoint}\n";

        outputString +=
            $"|maxstatus={sb.status}\n" +
            $"|maxhp={sb.hp}\n" +
            $"|maxatk={sb.atk}\n" +
            $"|maxdef={sb.def}\n" +
            $"|maxevd={sb.evd}\n" +
            $"|maxbeat={sb.beatBonus}\n" +
            $"|maxaction={sb.actBonus}\n" +
            $"|maxtry={sb.tryBonus}\n";

        int[] cardArray = new int[] { friend.ParamAlphaBase.orderCardType00, friend.ParamAlphaBase.orderCardType01, friend.ParamAlphaBase.orderCardType02, friend.ParamAlphaBase.orderCardType03, friend.ParamAlphaBase.orderCardType04 };
        int[] cardValueArray = new int[] { friend.ParamAlphaBase.orderCardValue00, friend.ParamAlphaBase.orderCardValue01, friend.ParamAlphaBase.orderCardValue02, friend.ParamAlphaBase.orderCardValue03, friend.ParamAlphaBase.orderCardValue04 };
        string cards = "|flags=";

        for (int i = 0; i < 5; i++)
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
        switch (friend.ParamArts.authParam.SynergyFlag)
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
            "|miracle=" + friend.ParamArts.actionName;

        List<string> miracleValues = FillMiracleNumbers(friend);
        for (int i = 0; i < miracleValues.Count(); i++)
        {
            outputString += ("\n|miracle" + (i + 1) + "=" + miracleValues[i].Replace(@"\r\n", "\n").Replace(@"\n", "\n").Replace(@"\", " "));
        }

        outputString += "\n|beatname=" + friend.ParamSpecialAttack.actionName + "\n" +
        "|beatskill=" + friend.ParamSpecialAttack.actionEffect + "\n" +
        "|standby=" + friend.ParamWaitAction.skillName + "\n" +
        "|standbyskill=" + friend.ParamWaitAction.skillEffect + "\n" +
        "|unique=" + friend.ParamAbility.abilityName + "\n" +
        "|uniqueskill=" + friend.ParamAbility.abilityEffect + "\n" +
        "|miracletrait=" + friend.ParamAbility2?.abilityName + "\n" +
        "|miracletraitskill=" + friend.ParamAbility2?.abilityEffect + "\n";


        string cos = "\n|cos = ";
        string cosname = "\n|cosname = ";
        string cosobt = "\n|cosobt = ";

        List<CharaClothesData> costumes = CharaClothesDatas.Where(c => c.clothesPresetId == friend.id).ToList();
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
                        if (friend.rankHigh == 6)
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

        if (wiki) { File.WriteAllText(SharedSettings.exportPath + "wiki/" + friend.nameEn.Trim() + "_" + friend.id + "_wiki.txt", outputString); }
        return true;
    }

    public static string GeneratePhotoEffect(ParamAbility abilityData)
    {
        string outputString = GenerateAbilityBuffEffects(abilityData.buffList);
        foreach (ParamAbility.GutsList guts in abilityData.gutsList)
        {
            outputString += $"Revives you when you are defeated {guts.numOfTimes} times.";
        }
        return outputString;
    }

    public static string GenerateAbilityBuffEffects(List<ParamAbility.BuffList> buffs)
    {
        List<string[]> outputArr = new List<string[]>();
        for(int i = 0; i < buffs.Count; i++)
        {
            string[] entry = new string[3];
            var buff = buffs[i];
            string subString = "";
            int conditionCount = 0;
            if (!(buff.condition == ConditionType.BELOW && buff.conditionHpRate == 100) && !(buff.condition == ConditionType.ABOVE && buff.conditionHpRate == 0))
            {
                string healthDesc = buff.condition switch
                {
                    ConditionType.ABOVE => "above",
                    ConditionType.BELOW => "below",
                    ConditionType.EQUAL => "at",
                    _ => ""
                };
                subString += $"{(conditionCount > 0 ? ", and" : "When")} health is {healthDesc} {buff.conditionHpRate}%";
                conditionCount++;
            }
            if ((int)buff.traitsTerrain != -1)
            {
                subString += $"{(conditionCount > 0 ? ", and" : "When")} arena is {buff.traitsTerrain}";
                conditionCount++;
            }
            if (buff.enemyNum > 0)
            {
                string cond = buff.conditionEnemyNum switch
                {
                    ConditionType.ABOVE => "or more",
                    ConditionType.BELOW => "or less",
                    ConditionType.EQUAL => "",
                    _ => ""
                };
                subString += $"{(conditionCount > 0 ? ", and" : "When")} {buff.enemyNum} {cond} enemies are alive";
                conditionCount++;
            }
            if (buff.mysideNum > 0)
            {
                string cond = buff.conditionMysideNum switch
                {
                    ConditionType.ABOVE => "or more",
                    ConditionType.BELOW => "or less",
                    ConditionType.EQUAL => "",
                    _ => ""
                };
                subString += $"{(conditionCount > 0 ? ", and" : "When")} {buff.mysideNum} {cond} Friends are alive";
                conditionCount++;
            }
            if ((int)buff.traitsTimezone != -1)
            {
                subString += $"{(conditionCount > 0 ? ", and" : "When")} it's nighttime";
                conditionCount++;
            }
            if ((int)buff.waveEnemyMask != -1) 
                subString += $"{(conditionCount > 0 ? ", and" : "When")} enemy is {GetEnemyMaskEn(buff.waveEnemyMask)}";

            entry[0] = subString; subString = "";

            string buffDescription = $"{GetBuffNameEn(buff.buffType)}";
            double buffCoefficient = Math.Round(buff.coefficient * 100, 2);

            if (buff.abnormalType != 0)
            {
                buffDescription += $" {GetAbnormalTypeEn(buff.abnormalType)}";
            }
            if (buff.abnormalType2 != 0)
            {
                buffDescription += $" {GetAbnormalTypeEn(buff.abnormalType2)}";
            }

            if (buff.increment != 0)
            {
                buffDescription += $" {buff.increment}";
            }
            if (buff.coefficient != 0)
            {
                buffDescription += $" {buffCoefficient}%";
            }

            buffDescription = $"{buffDescription} to {GetTargetTypeEn(buff.targetType)}";

            entry[1] = buffDescription;

            string targetDescription = "";

            conditionCount = 0;
            if ((int)buff.attributeMask != -1)
            {
                targetDescription += $", when attribute is {GetAttributeNameEn(buff.attributeMask)}";
                conditionCount++;
            }

            if (buff.spAttributeMask != 0 || buff.spEnemyMask != 0 || buff.spHealthMask != 0)
            {
                if (conditionCount > 0) targetDescription += ",";
                targetDescription += " against";
                if (buff.spAttributeMask != 0)
                {
                    targetDescription += $" {GetAttributeNameEn(buff.spAttributeMask)}";
                }
                if (buff.spEnemyMask != 0)
                {
                    targetDescription += $" {GetEnemyMaskEn(buff.spEnemyMask)}";
                }
                targetDescription += " enemies";
                if (buff.spHealthMask != 0)
                {
                    targetDescription += $" affected by ({GetHealthMaskEn(buff.spHealthMask)})";
                }
            }
            if (buff.giveupReuseNum > 0)
            {
                targetDescription += $", when ally is defeated ({buff.giveupReuseNum} times)";
            }
            if (buff.waveReuseNum > 0)
            {
                targetDescription += $" at the start of each wave ({buff.waveReuseNum+1} times)";
            }
            else
            {
                //targetDescription += $" (single activation)";
            }

            entry[2] = targetDescription;

            outputArr.Add(entry);
        }

        string outputString = "";
        for (int i = 0; i < outputArr.Count; i++)
        {
            bool conditionsMatchPrev = i > 0 && outputArr[i][0] == outputArr[i - 1][0] && outputArr[i][2] == outputArr[i - 1][2];
            bool conditionsMatchNext = i < outputArr.Count-1 && outputArr[i][0] == outputArr[i + 1][0] && outputArr[i][2] == outputArr[i + 1][2];

            bool combined = false;

            if(conditionsMatchPrev)
            {
                outputString += $" and ";
                combined = true;
            }
            else
            {
                outputString += $"{outputArr[i][0]}";
            }

            if (string.IsNullOrEmpty(outputArr[i][0]) && !combined)
            {
                outputString += $"Applies {outputArr[i][1]}";
            }
            else
            {
                if (combined)
                {
                    outputString += $"{outputArr[i][1]}";
                }
                else
                {
                    outputString += $", applies {outputArr[i][1]}";
                }
            }

            if (!conditionsMatchNext)
            {
                if (i == buffs.Count - 1)
                {
                    outputString += $"{outputArr[i][2]}.";
                }
                else
                {
                    outputString += $"{outputArr[i][2]}.\n";
                }
            }
        }

        return outputString;
    }

    public static string DecideVariable(ParamArts.CharaBuffParam buff, int id)
    {
        if (!buff.isGrow)
        {
            return "";
        }
        else
        {
            if(buff.coefficient == 0)
            {
                return $"[INCREMENT{id}]";
            }
            else
            {
                return $"[BUFF{id}]";
            }
        }
    }

    public static string DecideVariable(ParamArts.CharaDamageParam buff, int id)
    {
        if (!buff.isGrow)
        {
            return "";
        }
        else
        {
            return $"[DAMAGE{id}]";
        }
    }

    public static string GenerateCharaBuffEffect(List<ParamArts.CharaBuffParam> buffs)
    {
        List<string[]> outputArr = new List<string[]>();
        for (int i = 0; i < buffs.Count; i++)
        {
            string[] entry = new string[3];
            var buff = buffs[i];
            string subString = "";
            int conditionCount = 0;
            if ((int)buff.attributeMask != -1)
            {
                subString += $"When attribute is {GetAttributeNameEn(buff.attributeMask)}";
                conditionCount++;
            }
            if ((int)buff.traitsTerrain != -1)
            {
                subString += $"{(conditionCount > 0 ? ", and" : "When")} arena is {buff.traitsTerrain}";
                conditionCount++;
            }
            if (buff.enemyNum > 0)
            {
                string cond = buff.conditionEnemyNum switch
                {
                    ConditionType.ABOVE => "or more",
                    ConditionType.BELOW => "or less",
                    ConditionType.EQUAL => "",
                    _ => ""
                };
                subString += $"{(conditionCount > 0 ? ", and" : "When")} {buff.enemyNum} {cond} enemies are alive";
                conditionCount++;
            }
            if (buff.mysideNum > 0)
            {
                string cond = buff.conditionMysideNum switch
                {
                    ConditionType.ABOVE => "or more",
                    ConditionType.BELOW => "or less",
                    ConditionType.EQUAL => "",
                    _ => ""
                };
                subString += $"{(conditionCount > 0 ? ", and" : "When")} {buff.mysideNum} {cond} Friends are alive";
                conditionCount++;
            }
            if ((int)buff.waveEnemyMask != -1)
                subString += $"{(conditionCount > 0 ? ", and" : "When")} enemy is {GetEnemyMaskEn(buff.waveEnemyMask)}";

            entry[0] = subString; subString = "";

            string buffDescription = $"{GetBuffNameEn(buff.buffType)}";

            if (buff.abnormalType != 0)
            {
                buffDescription += $" {GetAbnormalTypeEn(buff.abnormalType)}";
            }
            if (buff.abnormalType2 != 0)
            {
                buffDescription += $" {GetAbnormalTypeEn(buff.abnormalType2)}";
            }

            string variableType = DecideVariable(buff, i);
            if (string.IsNullOrEmpty(variableType))
            {
                if (buff.increment != 0)
                    variableType = $" ({buff.increment})";
                else if (buff.coefficient != 0)
                    variableType = $" ({Math.Round(buff.coefficient * 100)})";
            }
            else
                variableType = " " + variableType;

            string successRate = buff.successRate == 100 ? "" : $" ({buff.successRate}% chance)";

            buffDescription = $"{buffDescription}{variableType}{successRate} to {GetTargetTypeEn(buff.targetType)}";
            if (buff.turn > 1)
                buffDescription += $" for {buff.turn} turns";

            entry[1] = buffDescription;

            string targetDescription = "";
            if (buff.spAttributeMask != 0 || buff.spEnemyMask != 0 || buff.spHealthMask != 0)
            {
                targetDescription = " against";
                if (buff.spAttributeMask != 0)
                {
                    targetDescription += $" {GetAttributeNameEn(buff.spAttributeMask)}";
                }
                if (buff.spEnemyMask != 0)
                {
                    targetDescription += $" {GetEnemyMaskEn(buff.spEnemyMask)}";
                }
                targetDescription += " enemies";
                if (buff.spHealthMask != 0)
                {
                    targetDescription += $" affected by ({GetHealthMaskEn(buff.spHealthMask)})";
                }
            }
            if (buff.giveupReuseNum > 0)
            {
                targetDescription += $", when ally is defeated ({buff.giveupReuseNum} times)";
            }

            entry[2] = targetDescription;

            outputArr.Add(entry);
        }

        string outputString = "";
        for (int i = 0; i < outputArr.Count; i++)
        {
            bool conditionsMatchPrev = i > 0 && outputArr[i][0] == outputArr[i - 1][0] && outputArr[i][2] == outputArr[i - 1][2];
            bool conditionsMatchNext = i < outputArr.Count - 1 && outputArr[i][0] == outputArr[i + 1][0] && outputArr[i][2] == outputArr[i + 1][2];

            bool combined = false;

            if (conditionsMatchPrev && (i == outputArr.Count - 1 || conditionsMatchNext))
            {
                outputString += $" and ";
                combined = true;
            }
            else if (i == 0)
            {
                outputString += $"{outputArr[i][0]}";
            }
            else
            {
                outputString += $"\n{outputArr[i][0]}";
            }

            if (string.IsNullOrEmpty(outputArr[i][0]) && !combined)
            {
                outputString += $"Applies {outputArr[i][1]}";
            }
            else
            {
                if (combined)
                {
                    outputString += $"{outputArr[i][1]}";
                }
                else
                {
                    outputString += $", applies {outputArr[i][1]}";
                }
            }

            if (!conditionsMatchNext)
            {
                outputString += $"{outputArr[i][2]}.";
            }
        }

        return outputString;
    }

    public static string GenerateCharaDamageEffect(List<ParamArts.CharaDamageParam> buffs)
    {
        List<string[]> outputArr = new List<string[]>();
        for (int i = 0; i < buffs.Count; i++)
        {
            string[] entry = new string[3];
            var buff = buffs[i];
            string subString = "";
            int conditionCount = 0;
            if (buff.enemyNum > 0)
            {
                string cond = buff.conditionEnemyNum switch
                {
                    ConditionType.ABOVE => "or more",
                    ConditionType.BELOW => "or less",
                    ConditionType.EQUAL => "",
                    _ => ""
                };
                subString += $"{(conditionCount > 0 ? ", and" : "When")} {buff.enemyNum} {cond} enemies are alive";
                conditionCount++;
            }
            if (buff.mysideNum > 0)
            {
                string cond = buff.conditionMysideNum switch
                {
                    ConditionType.ABOVE => "or more",
                    ConditionType.BELOW => "or less",
                    ConditionType.EQUAL => "",
                    _ => ""
                };
                subString += $"{(conditionCount > 0 ? ", and" : "When")} {buff.mysideNum} {cond} Friends are alive";
                conditionCount++;
            }

            entry[0] = subString; subString = "";

            string variableType = DecideVariable(buff, i);
            if (string.IsNullOrEmpty(variableType))
            {
                variableType = $"{Math.Round(buff.damageRate * 100)}";
            }

            string buffDescription = $"{buff.hitNum} * {variableType} damage to {GetTargetTypeEn(buff.targetType)}";

            entry[1] = buffDescription;

            string targetDescription = "";

            if((int)buff.attributeMask != -1 || (int)buff.waveEnemyMask != -1 || (int)buff.targetMask != -1 || (int)buff.healthMask != -1)
            {
                targetDescription = " against";
                if ((int)buff.attributeMask != -1)
                {
                    targetDescription += $" {GetAttributeNameEn(buff.attributeMask)}";
                }
                if ((int)buff.waveEnemyMask != -1)
                {
                    targetDescription += $" {GetEnemyMaskEn(buff.waveEnemyMask)}";
                }
                if ((int)buff.targetMask != -1)
                {
                    targetDescription += $" {GetEnemyMaskEn(buff.targetMask)}";
                }
                targetDescription += " enemies";
                if ((int)buff.healthMask != -1)
                {
                    targetDescription += $" affected by ({GetHealthMaskEn(buff.healthMask)})";
                }
            }

            entry[2] = targetDescription;

            outputArr.Add(entry);
        }

        string outputString = "";
        for (int i = 0; i < outputArr.Count; i++)
        {
            bool conditionsMatchPrev = i > 0 && outputArr[i][0] == outputArr[i - 1][0] && outputArr[i][2] == outputArr[i - 1][2];
            bool conditionsMatchNext = i < outputArr.Count - 1 && outputArr[i][0] == outputArr[i + 1][0] && outputArr[i][2] == outputArr[i + 1][2];

            bool combined = false;

            if (conditionsMatchPrev && (i == outputArr.Count - 1 || conditionsMatchNext))
            {
                outputString += $" and ";
                combined = true;
            }
            else if (i == 0)
            {
                outputString += $"{outputArr[i][0]}";
            }
            else
            {
                outputString += $"\n{outputArr[i][0]}";
            }

            if (string.IsNullOrEmpty(outputArr[i][0]) && !combined)
            {
                outputString += $"Deal {outputArr[i][1]}";
            }
            else
            {
                if (combined)
                {
                    outputString += $"{outputArr[i][1]}";
                }
                else
                {
                    outputString += $", deal {outputArr[i][1]}";
                }
            }

            if (!conditionsMatchNext)
            {
                outputString += $"{outputArr[i][2]}.";
            }
        }

        return outputString;
    }
}
