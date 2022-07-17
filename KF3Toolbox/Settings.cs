using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public partial class KF3Parse
{
    #region variable declarations
    public string lastVersion;
    public StreamWriter sw;
    public CharaData loadedFriend;
    public bool friendLoaded = false;
    #endregion

    public void ReloadData(bool fastboot = false)
    {
        LoadCache();
        ReadParameter();
        Console.WriteLine("promotePresetData, presetData =>");
        Parallel.ForEach(PromotePresetDatas, presetData =>
        {
            presetData.SetPromoteDatas(this);
        });

        Console.WriteLine("charaData, friend =>");
        Parallel.ForEach(CharaDatas, friend =>
        {
            friend.SetMultiple(this);
            switch (friend.attribute)
            {
                case "1": friend.attribute = "Funny"; break;
                case "2": friend.attribute = "Friendly"; break;
                case "3": friend.attribute = "Relaxed"; break;
                case "4": friend.attribute = "Lovely"; break;
                case "5": friend.attribute = "Active"; break;
                case "6": friend.attribute = "Carefree"; break;
                default: friend.attribute = "unknown"; break;
            }
        });

        if (!fastboot)
        {
            Console.WriteLine("questEnemiesData, enemies =>");
            Parallel.ForEach(QuestEnemiesDatas, enemies =>
            {
                enemies.SetEnemyDatas(this);
            });
            Console.WriteLine("questEnemyData, enemy =>");
            Parallel.ForEach(QuestEnemyDatas, enemy =>
            {
                enemy.SetDrawItemDatas(this);
            });
            Console.WriteLine("questEnemyFriendsData, enemyFriends =>");
            Parallel.ForEach(QuestEnemyFriendsDatas, enemyFriends =>
            {
                enemyFriends.SetDrawItemDatas(this);
            });
            Console.WriteLine("questWaveData, wave =>");
            Parallel.ForEach(QuestWaveDatas, wave =>
            {
                wave.SetMultiple(this);
            });
            Console.WriteLine("questDrawItemData, drawItem =>");
            Parallel.ForEach(QuestDrawItemDatas, drawItem =>
            {
                drawItem.SetItem(this);
            });
            Console.WriteLine("questQuestOneData, questOne =>");
            Parallel.ForEach(QuestQuestOneDatas, questOne =>
            {
                questOne.SetQuestGroup(this);
            });
        }
    }

    public void ReadParameter()
    {
        new AssetTool().ReadParameterAsset(SharedSettings.assetsPath + "parameter.asset");
    }

    public void LoadCache()
    {
        CharaDatas = JsonConvert.DeserializeObject<List<CharaData>>(LoadGzipFile("CHARA_DATA"));
        CharaClothesDatas = JsonConvert.DeserializeObject<List<CharaClothesData>>(LoadGzipFile("CHARA_CLOTHES_DATA"));
        EventDatas = JsonConvert.DeserializeObject<List<EventData>>(LoadGzipFile("EVENT_DATA"));
        ItemCommons = JsonConvert.DeserializeObject<List<ItemCommon>>(LoadGzipFile("ITEM_COMMON"));
        PhotoDatas = JsonConvert.DeserializeObject<List<PhotoData>>(LoadGzipFile("PHOTO_DATA"));
        PromoteDatas = JsonConvert.DeserializeObject<List<PromoteData>>(LoadGzipFile("CHARA_PROMOTE_DATA"));
        PromotePresetDatas = JsonConvert.DeserializeObject<List<PromotePresetData>>(LoadGzipFile("CHARA_PROMOTE_PRESET_DATA"));
        QuestEnemiesDatas = JsonConvert.DeserializeObject<List<QuestEnemiesData>>(LoadGzipFile("QUEST_ENEMIES_DATA"));
        QuestEnemyDatas = JsonConvert.DeserializeObject<List<QuestEnemyData>>(LoadGzipFile("QUEST_ENEMY_DATA"));
        QuestEnemyFriendsDatas = JsonConvert.DeserializeObject<List<QuestEnemyFriendsData>>(LoadGzipFile("QUEST_ENEMYFRIENDS_DATA"));
        QuestMapDatas = JsonConvert.DeserializeObject<List<QuestMapData>>(LoadGzipFile("QUEST_MAP_DATA"));
        QuestWaveDatas = JsonConvert.DeserializeObject<List<QuestWaveData>>(LoadGzipFile("QUEST_WAVE_DATA"));
        QuestDrawItemDatas = JsonConvert.DeserializeObject<List<QuestDrawItemData>>(LoadGzipFile("QUEST_DRAW_ITEM_DATA"));
        QuestQuestOneDatas = JsonConvert.DeserializeObject<List<QuestQuestOneData>>(LoadGzipFile("QUEST_QUESTONE_DATA"));
        QuestQuestQuestgroupDatas = JsonConvert.DeserializeObject<List<QuestQuestQuestgroupData>>(LoadGzipFile("QUEST_QUEST_QUESTGROUP_DATA"));
    }
}
