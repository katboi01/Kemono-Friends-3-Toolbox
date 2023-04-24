using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public partial class KF3Parse
{
    public void ReadDropData()
    {
        StringBuilder output = new StringBuilder();
        foreach (QuestQuestOneData quest in QuestQuestOneDatas)
        {
            //output.Append(GetStageDrops(quest));
            output.Append(GetBGM(quest));
        }
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(SharedSettings.exportPath, "droplist.txt")))
        {
            outputFile.WriteLine(output.ToString());
        }
    }

    public string GetBGM(QuestQuestOneData quest)
    {
        StringBuilder output = new StringBuilder();
        List<List<QuestDrawItemData>> drawIds = new List<List<QuestDrawItemData>>();

        output.Append($"Quest: {quest.questOneId}\n{GetStageName(quest.questMap.chapterId)} --- {quest.questGroup.storyName.Replace("\"", "")} / {quest.questOneName.Replace("\"", "")}\n lvl {quest.difficulty} stamina {quest.stamina}\n");
        foreach (QuestWaveData wave in QuestWaveDatas.Where(w => w.questOneId == quest.questOneId))
        {
            output.Append($"    wave {wave.waveId}");
            if (wave.rate != 1000) output.Append($" chance {wave.rate / 10}%\n");
            else output.AppendLine();
            output.Append($"        {wave.waveStartBgmId}\n");
        }
        //output.Append(CompileDropList(drawIds));
        output.AppendLine();
        return output.ToString();
    }

    public string GetStageDrops(QuestQuestOneData quest)
    {
        StringBuilder output = new StringBuilder();
        List<List<QuestDrawItemData>> drawIds = new List<List<QuestDrawItemData>>();

        output.Append($"Quest: {quest.questOneId}\n{GetStageName(quest.questMap.chapterId)} --- {quest.questGroup.storyName.Replace("\"", "")} / {quest.questOneName.Replace("\"", "")}\n lvl {quest.difficulty} stamina {quest.stamina}\n");
        foreach (QuestWaveData wave in QuestWaveDatas.Where(w => w.questOneId == quest.questOneId))
        {
            output.Append($"    wave {wave.waveId}");
            if (wave.rate != 1000) output.Append($" chance {wave.rate / 10}%\n");
            else output.AppendLine();
            if (wave.questEnemiesData != null)
            {
                foreach (QuestEnemyData enemy in wave.questEnemiesData.questEnemyDatas)
                {
                    if (enemy != null)
                    {
                        output.Append($"        {GetEnemyName(enemy.enemyCharaId)} ({enemy.enemyCharaId}) level: {enemy.level}\n");
                        if (enemy.ParamEnemyBase != null)
                        {
                            double hp = Math.Ceiling(KF3Parse.Interpolate(
                                enemy.ParamEnemyBase.hpParamLv1,
                                enemy.ParamEnemyBase.hpParamLvMiddle,
                                enemy.level,
                                enemy.ParamEnemyBase.hpLvMiddleNum));
                            double atk = Math.Ceiling(KF3Parse.Interpolate(
                                enemy.ParamEnemyBase.atkParamLv1,
                                enemy.ParamEnemyBase.atkParamLvMiddle,
                                enemy.level,
                                enemy.ParamEnemyBase.atkLvMiddleNum));
                            double def = Math.Ceiling(KF3Parse.Interpolate(
                                enemy.ParamEnemyBase.defParamLv1,
                                enemy.ParamEnemyBase.defParamLvMiddle,
                                enemy.level,
                                enemy.ParamEnemyBase.defLvMiddleNum));

                            output.Append($"        hp: {hp} atk: {atk} def: {def}\n");
                        }


                        if (enemy.questDrawItemDatas != null)
                        {
                            if (enemy.questDrawItemDatas.Count > 0) drawIds.Add(enemy.questDrawItemDatas);
                        }
                    }
                }
                output.Append(CompileDropList(drawIds)); //
                drawIds = new List<List<QuestDrawItemData>>(); //
                foreach (QuestEnemyFriendsData enemyFriend in wave.questEnemiesData.questEnemyFriends)
                {
                    if (enemyFriend != null)
                    {
                        CharaData friend = CharaDatas.Where(c => c.id == enemyFriend.charaId).FirstOrDefault();
                        if (friend != null)
                        {
                            List<int> photoIds = new List<int>
                                {
                                    enemyFriend.photoId1,
                                    enemyFriend.photoId2,
                                    enemyFriend.photoId3,
                                    enemyFriend.photoId4
                                };
                            string photos = "";
                            foreach (int photo in photoIds)
                            {
                                if (photo != 0)
                                {
                                    photos += " " + photo;
                                }
                            }
                            string friendName = friend.nameEn + " " + friend.nickname;
                            output.Append($"        {friendName} ({enemyFriend.enemyId}) level: {enemyFriend.level}\n" +
                                $"           Miracle: {enemyFriend.miracleLevel} WR: {enemyFriend.yasei} Rank(?): {enemyFriend.rank}\n" +
                                $"              Photos:{photos}\n");
                            if (enemyFriend.questDrawItemDatas != null)
                            {
                                if (enemyFriend.questDrawItemDatas.Count > 0) drawIds.Add(enemyFriend.questDrawItemDatas);
                            }
                        }
                        else output.Append($"        unknown enemy friend");
                    }
                }
            }

        }
        //output.Append(CompileDropList(drawIds));
        output.AppendLine();
        return output.ToString();
    }

    public string CompileDropList(List<List<QuestDrawItemData>> drawIds)
    {
        StringBuilder sb = new StringBuilder();
        foreach (List<QuestDrawItemData> drawId in drawIds)
        {
            if (drawId.Count == 0) continue;
            List<int> itemIds = new List<int>();
            List<int> itemNums = new List<int>();
            List<float> itemRates = new List<float>();

            sb.Append("     " + drawId[0].drawId + "\n");

            foreach (QuestDrawItemData item in drawId)
            {
                itemIds.Add(item.itemId);
                itemRates.Add(item.rate);
                itemNums.Add(item.itemNum);
            }

            float totalRate = 0, nullRate = 0;
            foreach (float rate in itemRates)
            {
                totalRate += rate;
            }
            if (totalRate < 100)
            {
                nullRate = 100 - totalRate;
                itemIds.Add(0);
                itemNums.Add(0);
                itemRates.Add(nullRate);
                totalRate = 100;
            }
            for (int i = 0; i < itemRates.Count; i++)
            {
                itemRates[i] = itemRates[i] / totalRate * 100;
            }
            for (int i = 0; i < itemIds.Count; i++)
            {
                if (itemIds[i] != 0)
                { sb.Append($"      {itemNums[i]} x ({itemIds[i]}) {GetItemName(itemIds[i])} : {Math.Round(itemRates[i], 2)}%"); }
                else sb.Append($"       no item : {itemRates[i]}%");
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    public float GetDropChance(QuestQuestOneData quest, int itemId)
    {
        List<QuestDrawItemData> drawDatas = new List<QuestDrawItemData>();
        foreach (QuestWaveData wave in QuestWaveDatas.Where(w => w.questOneData == quest))
        {
            foreach (QuestEnemyData enemy in wave.questEnemiesData.questEnemyDatas)
            {
                foreach (QuestDrawItemData drawItemData in enemy.questDrawItemDatas)
                {
                    drawDatas.Add(drawItemData);
                }
            }
        }

        List<int> itemIds = new List<int>();
        List<int> itemNums = new List<int>();
        List<float> itemRates = new List<float>();

        foreach (QuestDrawItemData item in drawDatas)
        {
            itemIds.Add(item.itemId);
            itemRates.Add(item.rate);
            itemNums.Add(item.itemNum);
        }

        float totalRate = 0, nullRate = 0;
        foreach (float rate in itemRates)
        {
            totalRate += rate;
        }
        if (totalRate < 100)
        {
            nullRate = 100 - totalRate;
            itemIds.Add(0);
            itemNums.Add(0);
            itemRates.Add(nullRate);
            totalRate = 100;
        }
        for (int i = 0; i < itemRates.Count; i++)
        {
            if (itemIds[i] == itemId)
            {
                return itemRates[i] / totalRate * 100;
            }
        }
        return 0;
    }

    public List<StagesBrief> GetDropLocations(int itemId)
    {
        List<StagesBrief> stagesBrief = new List<StagesBrief>();

        Console.WriteLine("3.");
        List<QuestDrawItemData> drawItems = QuestDrawItemDatas.Where(di => di.itemId == itemId).ToList();
        Console.WriteLine(drawItems.Count());
        List<QuestEnemyData> enemyDatas = QuestEnemyDatas.Where(e => drawItems.Any(di => di.drawId == e.drawId)).ToList();
        Console.WriteLine(enemyDatas.Count());
        List<QuestEnemiesData> enemiesDatas = QuestEnemiesDatas.Where(ed => ed.questEnemyDatas.Intersect(enemyDatas).Any()).ToList();
        Console.WriteLine(enemiesDatas.Count());
        List<QuestWaveData> waveDatas = QuestWaveDatas.Where(w => enemiesDatas.Contains(w.questEnemiesData)).ToList();
        Console.WriteLine(waveDatas.Count());
        foreach (QuestWaveData waveData in waveDatas)
        {
            StagesBrief stageBrief = new StagesBrief();
            stageBrief.id = waveData.questOneData.questOneId;
            List<List<QuestDrawItemData>> drawids = new List<List<QuestDrawItemData>>();
            foreach (QuestEnemyData enemyData in waveData.questEnemiesData.questEnemyDatas)
            {
                drawids.Add(enemyData.questDrawItemDatas);
            }
            Console.WriteLine(CompileDropList(drawids));

            stageBrief.drop = $"{GetStageName(waveData.questOneData.questMap.chapterId)} - {waveData.questOneData.questGroup.storyName.Replace("\"", "")} / {waveData.questOneData.questOneName.Replace("\"", "")} lvl {waveData.questOneData.difficulty} stamina {waveData.questOneData.stamina}";
            stageBrief.percentage = GetDropChance(waveData.questOneData, itemId);
            stagesBrief.Add(stageBrief);
        }
        return stagesBrief;
    }
}
