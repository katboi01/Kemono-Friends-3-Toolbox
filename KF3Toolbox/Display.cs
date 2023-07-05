using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static StolenStuff.CharaDef;

public partial class KF3Parse
{
    public StatsBrief DispStats(CharaData friend, bool wiki = false)
    {
        if (friend.PromotePresetDatas.Count < 4 || friend.ParamAlphaBase == null)
        {
            throw new Exception();
        }

        StringBuilder output = new StringBuilder();
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

        int maxHp = StolenStuff.CalcParamInternal(level, 99, friend.ParamAlphaBase.hpParamLv1, friend.ParamAlphaBase.hpParamLv99, friend.ParamAlphaBase.hpParamLvMiddle, friend.ParamAlphaBase.hpLvMiddleNum);
        int maxAtk = StolenStuff.CalcParamInternal(level, 99, friend.ParamAlphaBase.atkParamLv1, friend.ParamAlphaBase.atkParamLv99, friend.ParamAlphaBase.atkParamLvMiddle, friend.ParamAlphaBase.atkLvMiddleNum);
        int maxDef = StolenStuff.CalcParamInternal(level, 99, friend.ParamAlphaBase.defParamLv1, friend.ParamAlphaBase.defParamLv99, friend.ParamAlphaBase.defParamLvMiddle, friend.ParamAlphaBase.defLvMiddleNum);

        maxHp = (int)Math.Ceiling((maxHp + friend.GetPromoteStat(wr, "hp")) * starBoost) + hpCosBonus;
        maxAtk = (int)Math.Ceiling((maxAtk + friend.GetPromoteStat(wr, "atk")) * starBoost) + atkCosBonus;
        maxDef = (int)Math.Ceiling((maxDef + friend.GetPromoteStat(wr, "def")) * starBoost) + defCosBonus;

        float evd = 10 * friend.ParamAlphaBase.avoidRatio + friend.GetPromoteStat(wr, "evd");
        float beatBonus = friend.GetPromoteStat(wr, "beat");
        float actBonus = friend.GetPromoteStat(wr, "act");
        float tryBonus = friend.GetPromoteStat(wr, "try");

        output.AppendLine($"Level: {level}");
        output.AppendLine($"WR: {wr}");
        output.AppendLine($"max STATUS: {CalcStatus(maxHp, maxAtk, maxDef)}");
        output.AppendLine($"max HP: {maxHp}");
        output.AppendLine($"max ATK: {maxAtk}");
        output.AppendLine($"max DEF: {maxDef}");
        output.AppendLine($"evd: {(int)evd / 10}.{evd % 10}%");
        output.AppendLine($"Beat bonus: {(int)beatBonus / 10}.{beatBonus % 10}%");
        output.AppendLine($"Act bonus: {(int)actBonus / 10}.{actBonus % 10}%");
        output.AppendLine($"Try bonus: {(int)tryBonus / 10}.{tryBonus % 10}%");

        output.AppendLine("\nCards:");
        output.AppendLine($"{friend.ParamAlphaBase.orderCardType00} {friend.ParamAlphaBase.orderCardValue00}");
        output.AppendLine($"{friend.ParamAlphaBase.orderCardType01} {friend.ParamAlphaBase.orderCardValue01}");
        output.AppendLine($"{friend.ParamAlphaBase.orderCardType02} {friend.ParamAlphaBase.orderCardValue02}");
        output.AppendLine($"{friend.ParamAlphaBase.orderCardType03} {friend.ParamAlphaBase.orderCardValue03}");
        output.AppendLine($"{friend.ParamAlphaBase.orderCardType04} {friend.ParamAlphaBase.orderCardValue04}");

        if (!wiki)
        {
            Console.WriteLine(output.ToString());
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

    public void DispPhoto(PhotoData photo, bool wiki = false)
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

    public SkillsBrief DispSkills2(CharaData friend, bool wiki = false)
    {
        if (friend.ParamAlphaBase == null)
        {
            throw new Exception();
        }

        StringBuilder output = new StringBuilder();
        output.AppendLine("Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n");
        output.AppendLine($"Miracle: {friend.ParamArts.actionName}\n{friend.ParamArts.actionEffect}\n");
        output.AppendLine($"Miracle + card: {friend.ParamArts.authParam.SynergyFlag}");
        output.AppendFormat("|{0}|{1}|{2}|{3}|{4}|{5}|{6}\n", CenteredString("Miracle Variable", 20), CenteredString("lvl1", 10), CenteredString("lvl2", 10), CenteredString("lvl3", 10), CenteredString("lvl4", 10), CenteredString("lvl5", 10), CenteredString("Miracle+", 10));
        for (int i = 0; i < friend.ParamArts.damageList.Count; i++)
        {
            ParamArts.CharaDamageParam damage = friend.ParamArts.damageList[i];
            output.AppendFormat("|{0}", CenteredString($"DAMAGE{i}", 20));
            for (int lvl = 0; lvl < 6; lvl++)
            {
                output.AppendFormat("|{0}", CenteredString(Math.Round(100 * (damage.damageRate * (1 + lvl * damage.growthRate))).ToString(), 10));
            }
            output.AppendLine();
        }

        for (int i = 0; i < friend.ParamArts.buffList.Count; i++)
        {
            ParamArts.CharaBuffParam buff = friend.ParamArts.buffList[i];
            output.AppendFormat("|{0}", CenteredString($"Buff{i}", 20));
            for (int lvl = 0; lvl < 6; lvl++)
            {
                output.AppendFormat("|{0}", CenteredString(Math.Round(100 - 100 * (buff.coefficient * (1 + lvl * buff.growthRate))).ToString(), 10));
            }
            output.AppendLine($"\nTurns: {buff.turn} Activation rate: {buff.successRate}%");
        }

        output.AppendLine($"\nUnique Trait: {friend.ParamAbility.abilityName}\n{friend.ParamAbility.abilityEffect}");

        if (friend.ParamAbility1 != null)
        {
            output.AppendLine($"\nMiracle Trait: {friend.ParamAbility1.abilityName}\n{friend.ParamAbility1.abilityEffect}");
        }

        if (friend.ParamAbility2 != null)
        {
            output.AppendLine($"\nRainbow Trait: {friend.ParamAbility2.abilityName}\n{friend.ParamAbility2.abilityEffect}");
        }

        output.AppendLine($"\nStandby Skill: {friend.ParamWaitAction.skillName}\n{friend.ParamWaitAction.skillEffect}");

        output.AppendLine($"\nSpecial Attack: {friend.ParamSpecialAttack.actionName}\n{friend.ParamSpecialAttack.actionEffect}");

        foreach (ParamArts.CharaBuffParam buff in friend.ParamSpecialAttack.buffList)
        {
            output.AppendLine($"\nTurns: {buff.turn} Activation rate: {buff.successRate}%");
        }

        if (!wiki)
        {
            Console.WriteLine(output.ToString());
        }

        return new SkillsBrief()
        {
            MiracleName = friend.ParamArts.actionName,
            MiracleDesc = friend.ParamArts.actionEffect,
            MiracleType = friend.ParamArts.authParam.SynergyFlag,
            MiracleMax = FillMiracleNumbers(friend)[4],

            BeatName = friend.ParamSpecialAttack.actionName,
            BeatDesc = friend.ParamSpecialAttack.actionEffect,

            WaitName = friend.ParamWaitAction.skillName,
            WaitDesc = friend.ParamWaitAction.skillEffect,

            Ability1Name = friend.ParamAbility1?.abilityName,
            Ability1Desc = friend.ParamAbility1?.abilityEffect,
            Ability2Name = friend.ParamAbility2?.abilityName,
            Ability2Desc = friend.ParamAbility2?.abilityEffect,
            AbilityName = friend.ParamAbility.abilityName,
            AbilityDesc = friend.ParamAbility.abilityEffect
        };
    }

    void DispFacts(ref CharaData friend, bool wiki)
    {
        StringBuilder output = new StringBuilder();
        output.AppendLine($"Release Date: {EpochToKF3Time(friend.startTime).ToLongDateString()}");
        output.AppendLine($"Animal name: {friend.animalScientificName}, eponym: {friend.eponymName}");
        output.AppendLine($"Japanese name: {friend.name}, English name: {friend.nameEn}");
        output.AppendLine($"voice actress: {friend.castName}");
        output.AppendLine($"Animal's distribution areas: {friend.animalDistributionArea}, habitat: {friend.animalTabitat}");
        output.AppendLine($"Animal flavor text: {friend.animalFlavorText}");
        output.AppendLine($"Friend's flavor text: {friend.flavorText}");

        if (wiki)
        {
            File.WriteAllText(SharedSettings.exportPath + friend.nameEn + "_" + friend.id + ".txt", output.ToString());
        }
        else
        {
            Console.WriteLine("Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n");
            Console.Write(output.ToString());
        }
    }

    void DispPromoteItems(ref CharaData friend, bool wiki)
    {
        StringBuilder output = new StringBuilder();

        foreach (PromotePresetData ppd in friend.PromotePresetDatas)
        {
            output.Append((ppd.promoteStep + 1) + ": ");
            if (ppd.promoteStepDatetime == "1917860400000")
            {
                output.AppendLine("Unavailable");
            }
            else
            {
                foreach (PromoteData pd in ppd.promoteDatas)
                {
                    output.Append(GetItemName(pd.promoteUseItemId00) + " ");
                }
            }
            output.AppendLine();
        }

        if (wiki)
        {
            File.WriteAllText($"{SharedSettings.exportPath}{friend.nameEn}_{friend.id}_UpgradeItems.txt", output.ToString());
        }
        else
        {
            Console.WriteLine("Loaded Friend: " + friend.id + "_" + friend.nameEn + "\n");
            Console.Write(output.ToString());
        }
    }

    bool DispWiki(CharaData friend, bool wiki = false)
    {
        StringBuilder output = new StringBuilder();

        StatsBrief sb = null;
        try { sb = DispStats(friend, true); }
        catch { Console.WriteLine("failed"); return false; }

        var friendClothes = CharaClothesDatas.Where(c => c.clothesPresetId == friend.id).ToList();
        bool hasPartyDress = friendClothes.Where(f => f.clothesId == 8).Any();
        bool hasRainbowTrait = friend.ParamAbility2 != null;
        bool isWR5 = sb.wr == 5;
        string starsWord = "";
        switch (friend.rankLow)
        {
            case 2: starsWord = "Two"; break;
            case 3: starsWord = "Three"; break;
            case 4: starsWord = "Four"; break;
            case 5: starsWord = "Five"; break;
        }

        List<string> categories = new List<string>()
        {
            $"[[Category:{friend.attribute} KF3 Friends]]",
            $"[[Category:{starsWord} Star KF3 Friends]]",
            $"[[Category:Missing Content]]",
            $"[[Category:Needs Audio]]",
            $"{{{{#vardefine:id|{friend.id.ToString().PadLeft(4, '0')}}}}}"
        };

        if (hasPartyDress)
        {
            categories.Insert(categories.Count - 2, "[[Category:Party Dress KF3 Friends]]");
        }
        if (isWR5)
        {
            categories.Insert(categories.Count - 2, "[[Category:Wild Release 5 KF3 Friends]]");
        }
        if (hasRainbowTrait)
        {
            categories.Insert(categories.Count - 2, "[[Category:Rainbow Trait KF3 Friends]]");
        }

        output.AppendLine($"{string.Join(" ", categories)}\n");
        output.AppendLine("{{FriendBox/KF3");
        output.AppendLine($"|name={friend.nameEn.Replace("_", " ")}");
        output.AppendLine($"|apppic={friend.nameEn.Replace("_", " ")}KF3.png");
        output.AppendLine($"|apprarity={{{{KF3{friend.rankLow}Star}}}}");
        output.AppendLine($"|seiyuu={friend.castName}");
        output.AppendLine($"|attribute={friend.attribute} {{{{KF3{friend.attribute}}}}}");
        output.AppendLine($"|implemented={EpochToKF3Time(friend.startTime).ToLongDateString()} (App)");
        output.AppendLine($"|id={friend.id}");
        output.AppendLine($"|wr5={(isWR5? "Yes" : "No")}");
        output.AppendLine($"|rainbowtrait={(hasRainbowTrait ? "Yes" : "No")}");
        output.AppendLine($"|partydress={(hasPartyDress ? "Yes" : "No")}");
        output.AppendLine("}}");

        output.AppendLine("{{FriendBuilder/KF3");
        output.AppendLine($"|introduction = '''{friend.nameEn.Replace("_", " ")}''' is a Friend that appears in the app version of[[Kemono Friends 3]].");

        float starBoostLow = 1 + (friend.rankLow - 1) * 0.02f;
        double hp = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.ParamAlphaBase.hpParamLv1, friend.ParamAlphaBase.hpParamLv99, friend.ParamAlphaBase.hpParamLvMiddle, friend.ParamAlphaBase.hpLvMiddleNum) * starBoostLow);
        double atk = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.ParamAlphaBase.atkParamLv1, friend.ParamAlphaBase.atkParamLv99, friend.ParamAlphaBase.atkParamLvMiddle, friend.ParamAlphaBase.atkLvMiddleNum) * starBoostLow);
        double def = Math.Ceiling(StolenStuff.CalcParamInternal(1, 99, friend.ParamAlphaBase.defParamLv1, friend.ParamAlphaBase.defParamLv99, friend.ParamAlphaBase.defParamLvMiddle, friend.ParamAlphaBase.defLvMiddleNum) * starBoostLow);

        output.AppendLine($"|status={CalcStatus((int)hp, (int)atk, (int)def)}");
        output.AppendLine($"|hp={hp}");
        output.AppendLine($"|atk={atk}");
        output.AppendLine($"|def={def}");
        output.AppendLine($"|evd={friend.ParamAlphaBase.avoidRatio}%");
        output.AppendLine($"|beat=+{friend.GetPromoteStat(0, "beat")}%");
        output.AppendLine($"|action=+{friend.GetPromoteStat(0, "act")}%");
        output.AppendLine($"|try=+{friend.GetPromoteStat(0, "try")}%");
        output.AppendLine($"|plasm={friend.ParamAlphaBase.plasmPoint}");

        output.AppendLine($"|maxstatus={sb.status}");
        output.AppendLine($"|maxhp={sb.hp}");
        output.AppendLine($"|maxatk={sb.atk}");
        output.AppendLine($"|maxdef={sb.def}");
        output.AppendLine($"|maxevd={sb.evd}");
        output.AppendLine($"|maxbeat={sb.beatBonus}");
        output.AppendLine($"|maxaction={sb.actBonus}");
        output.AppendLine($"|maxtry={sb.tryBonus}");

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

        output.AppendLine(cards);
        output.AppendLine($"|miracleplus={{{cardType}}}");
        output.AppendLine($"|miracle={friend.ParamArts.actionName}");

        List<string> miracleValues = FillMiracleNumbers(friend);
        for (int i = 0; i < miracleValues.Count(); i++)
        {
            output.Append("\n|miracle" + (i + 1) + "=" + miracleValues[i].Replace(@"\r\n", "\n").Replace(@"\n", "\n").Replace(@"\", " "));
        }

        output.AppendLine($"\n|beatname={friend.ParamSpecialAttack.actionName}");
        output.AppendLine($"|beatskill={friend.ParamSpecialAttack.actionEffect}");
        output.AppendLine($"|standby={friend.ParamWaitAction.skillName}");
        output.AppendLine($"|standbyskill={friend.ParamWaitAction.skillEffect}");
        output.AppendLine($"|unique={friend.ParamAbility.abilityName}");
        output.AppendLine($"|uniqueskill={friend.ParamAbility.abilityEffect}");
        output.AppendLine($"|miracletrait={friend.ParamAbility1?.abilityName}");
        output.AppendLine($"|miracletraitskill={friend.ParamAbility1?.abilityEffect}");
        output.AppendLine($"|rainbowtrait={(friend.ParamAbility2 != null? friend.ParamAbility2.abilityName : "N/A")}");
        output.AppendLine($"|rainbowtraitskill={(friend.ParamAbility2 != null ? friend.ParamAbility2.abilityEffect : "Not implemented yet.")}");


        string cos = "\n|cos = ";
        string cosobt = "\n|cosobt = ";
        string cosname = "\n|cosname = ";

        var costumes = GetCostumesForWiki(friendClothes, friend.rankLow);
        cos = cos + string.Join(",", costumes.Select(c => c.ImageName)) + ",end";
        cosobt = cosobt + string.Join(",", costumes.Select(c => c.Obtain));
        cosname = cosname + string.Join(",", costumes.Select(c => c.Name));

        output.Append(cos);
        output.Append(cosname); 
        output.Append(cosobt);
        output.AppendLine("}}");

        if (wiki)
        {
            File.WriteAllText(SharedSettings.exportPath + "wiki/" + friend.nameEn.Trim() + "_" + friend.id + "_wiki.txt", output.ToString());
        }
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
