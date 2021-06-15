using KF3Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KF3Toolbox
{
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
            charaData = JsonConvert.DeserializeObject<List<CharaData>>(LoadGzipFile("CHARA_DATA"));
            charaClothesData = JsonConvert.DeserializeObject<List<CharaClothesData>>(LoadGzipFile("CHARA_CLOTHES_DATA"));
            eventData = JsonConvert.DeserializeObject<List<EventData>>(LoadGzipFile("EVENT_DATA"));
            itemCommon = JsonConvert.DeserializeObject<List<ItemCommon>>(LoadGzipFile("ITEM_COMMON"));
            photoData = JsonConvert.DeserializeObject<List<PhotoData>>(LoadGzipFile("PHOTO_DATA"));
            promoteData = JsonConvert.DeserializeObject<List<PromoteData>>(LoadGzipFile("CHARA_PROMOTE_DATA"));
            promotePresetData = JsonConvert.DeserializeObject<List<PromotePresetData>>(LoadGzipFile("CHARA_PROMOTE_PRESET_DATA"));
            questEnemiesData = JsonConvert.DeserializeObject<List<QuestEnemiesData>>(LoadGzipFile("QUEST_ENEMIES_DATA"));
            questEnemyData = JsonConvert.DeserializeObject<List<QuestEnemyData>>(LoadGzipFile("QUEST_ENEMY_DATA"));
            questEnemyFriendsData = JsonConvert.DeserializeObject<List<QuestEnemyFriendsData>>(LoadGzipFile("QUEST_ENEMYFRIENDS_DATA"));
            questMapData = JsonConvert.DeserializeObject<List<QuestMapData>>(LoadGzipFile("QUEST_MAP_DATA"));
            questWaveData = JsonConvert.DeserializeObject<List<QuestWaveData>>(LoadGzipFile("QUEST_WAVE_DATA"));
            questDrawItemData = JsonConvert.DeserializeObject<List<QuestDrawItemData>>(LoadGzipFile("QUEST_DRAW_ITEM_DATA"));
            questQuestOneData = JsonConvert.DeserializeObject<List<QuestQuestOneData>>(LoadGzipFile("QUEST_QUESTONE_DATA"));
            questQuestQuestgroupData = JsonConvert.DeserializeObject<List<QuestQuestQuestgroupData>>(LoadGzipFile("QUEST_QUEST_QUESTGROUP_DATA"));

            Console.WriteLine("promotePresetData, presetData =>");
            Parallel.ForEach(promotePresetData, presetData =>
            {
                presetData.SetPromoteDatas(this);
            });

            Console.WriteLine("charaData, friend =>");
            Parallel.ForEach(charaData, friend =>
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
            Console.WriteLine("^ those missing files are fine, they are 2 photo slot skills ^");

            if (!fastboot)
            {
                Console.WriteLine("questEnemiesData, enemies =>");
                Parallel.ForEach(questEnemiesData, enemies =>
                {
                    enemies.SetEnemyDatas(this);
                });
                Console.WriteLine("questEnemyData, enemy =>");
                Parallel.ForEach(questEnemyData, enemy =>
                {
                    enemy.SetEnemyData(this);
                    enemy.SetDrawItemDatas(this);
                });
                Console.WriteLine("questEnemyFriendsData, enemyFriends =>");
                Parallel.ForEach(questEnemyFriendsData, enemyFriends =>
                {
                    enemyFriends.SetDrawItemDatas(this);
                });
                Console.WriteLine("questWaveData, wave =>");
                Parallel.ForEach(questWaveData, wave =>
                {
                    wave.SetMultiple(this);
                });
                Console.WriteLine("questDrawItemData, drawItem =>");
                Parallel.ForEach(questDrawItemData, drawItem =>
                {
                    drawItem.SetItem(this);
                });
                Console.WriteLine("questQuestOneData, questOne =>");
                Parallel.ForEach(questQuestOneData, questOne =>
                {
                    questOne.SetQuestGroup(this);
                });
            }
        }
    }
}
