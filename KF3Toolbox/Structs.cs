using KF3Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF3Toolbox
{
    public partial class KF3Parse
    {
        #region variable declarations
        public List<CharaData> charaData; //parsed chara_data
        public List<CharaClothesData> charaClothesData; //parsed chara_clothes_data
        public List<EventData> eventData; //parsed event_data
        public List<PhotoData> photoData; //parsed photo_data
        public List<PromoteData> promoteData; //parsed Promote_Data
        public List<PromotePresetData> promotePresetData; //parsed Promote_Data
        public List<ItemCommon> itemCommon; //parsed Item_Common
        List<ShopItemData> shopItemData;
        #region droplist
        public List<QuestEnemiesData> questEnemiesData;
        public List<QuestEnemyData> questEnemyData;
        public List<QuestEnemyFriendsData> questEnemyFriendsData;
        public List<QuestWaveData> questWaveData;
        public List<QuestDrawItemData> questDrawItemData;
        public List<QuestMapData> questMapData;
        public List<QuestQuestOneData> questQuestOneData;
        public List<QuestQuestQuestgroupData> questQuestQuestgroupData;
        #endregion
        #endregion

        public class CharaData
        {
            public string animalDistributionArea;
            public string animalFlavorText;
            public string animalImagePath;
            public string animalImageProvider;
            public string animalRedList;
            public string animalScientificName;
            public string animalTabitat;
            public int artsId;
            public string attribute;
            public string castName;
            public string charaScenarioId;
            public int clothesPresetId;
            public string duplicateItemId;
            public string endTime;
            public string eponymName;
            public string expTableId;
            public string flavorText;
            public string greetingText;
            public string homeTypeActTime;
            public string homeTypeFlying;
            public string homeTypeTree;
            public string hometouchSleep01;
            public string hometouchSleep02;
            public string hometouchSleep03;
            public string hometouchStand01;
            public string hometouchStand02;
            public string hometouchStand03;
            public string hometouchStand04;
            public string hometouchWalk01;
            public string hometouchWalk02;
            public string hometouchWalk03;
            public string hometouchWalk04;
            public string hometouchWalk05;
            public int id;
            public string kizunaLevelId;
            public string kizunaPhotoId;
            public string kizunaupText;
            public string loginSubText;
            public string loginText;
            public string modelFileName;
            public string name;
            public string nameEn;
            public string nickname;
            public string notOpenDispFlg;
            public string originalId;
            public string paramFileName;
            public string picnicGettimeId;
            public string ppId;
            public string ppItemId;
            public string promoteItemId;
            public int promotePresetId;
            public int rankHigh;
            public string rankId;
            public string rankItemId;
            public int rankLow;
            public string rarity;
            public string spAbilityRelPp;
            public string stackMax;
            public long startTime;

            public List<CharaClothesData> clothesDatas;
            public ParamAlphaBase paramAlphaBase;
            public ParamAbility paramAbility;
            public ParamAbility paramAbility1;
            public ParamArts paramArts;
            public ParamSpecialAttack paramSpecialAttack;
            public ParamWait paramWaitAction;
            public List<PromotePresetData> promotePresetDatas;

            public void SetMultiple(KF3Parse p)
            {
                clothesDatas = p.charaClothesData.Where(c => c.clothesPresetId == clothesPresetId).ToList();
                promotePresetDatas = p.promotePresetData.Where(pd => pd.promotePresetId == promotePresetId).ToList();

                string friendNumber = id.ToString().PadLeft(4, '0');

                string openFile = SharedSettings.paramsPath + "ParamAlphaBase_" + friendNumber + ".json";
                ReadAlphaBase(openFile);

                openFile = SharedSettings.paramsPath + "ParamAbility_" + friendNumber + ".json";
                ReadAbility(openFile);
                openFile = SharedSettings.paramsPath + "ParamAbility_" + friendNumber + "_1.json";
                ReadAbility1(openFile);

                openFile = SharedSettings.paramsPath + "ParamArts_" + friendNumber + ".json";
                ReadArts(openFile);

                openFile = SharedSettings.paramsPath + "ParamWaitAction_" + friendNumber + ".json";
                ReadWait(openFile);

                openFile = SharedSettings.paramsPath + "ParamSpecialAttack_" + friendNumber + ".json";
                ReadSpecialAttack(openFile);
            }

            /// <summary>
            /// Parse ParamAlphaBase files
            /// </summary>
            void ReadAlphaBase(string openFile)
            {
                if (!File.Exists(openFile)) { Console.WriteLine("file does not exist IMPORANT"); return; }
                paramAlphaBase = JObject.Parse(File.ReadAllText(openFile)).ToObject<ParamAlphaBase>();
            }

            /// <summary>
            /// Parse ParamAbility files
            /// </summary>
            void ReadAbility(string openFile)
            {
                if (!File.Exists(openFile)) { Console.WriteLine("file does not exist"); return; }
                paramAbility = JObject.Parse(File.ReadAllText(openFile)).ToObject<ParamAbility>();
            }

            /// <summary>
            /// Parse ParamAbility_1 files
            /// </summary>
            void ReadAbility1(string openFile)
            {
                if (!File.Exists(openFile)) { Console.WriteLine("file does not exist"); return; }
                paramAbility1 = JObject.Parse(File.ReadAllText(openFile)).ToObject<ParamAbility>();
            }

            /// <summary>
            /// Parse Special Attack files
            /// </summary>
            void ReadSpecialAttack(string openFile)
            {
                if (!File.Exists(openFile)) { Console.WriteLine("file does not exist"); return; }
                paramSpecialAttack = JObject.Parse(File.ReadAllText(openFile)).ToObject<ParamSpecialAttack>();
            }

            /// <summary>
            /// Parse ParamWaitAction files
            /// </summary>
            void ReadWait(string openFile)
            {
                if (!File.Exists(openFile)) { Console.WriteLine("file does not exist"); return; }
                paramWaitAction = JObject.Parse(File.ReadAllText(openFile)).ToObject<ParamWait>();
            }

            /// <summary>
            /// Parse ParamArts files
            /// </summary>
            void ReadArts(string openFile)
            {
                if (!File.Exists(openFile)) { Console.WriteLine("file does not exist"); return; }
                paramArts = JObject.Parse(File.ReadAllText(openFile)).ToObject<ParamArts>();
            }

            public float GetPromoteStat(int wildRelease, string stat)
            {
                float totalBonus = 0;

                for (int i = 0; i < wildRelease; i++)
                {
                    if (promotePresetDatas.Count == 0) { break; }
                    foreach (PromoteData pd in promotePresetDatas[i].promoteDatas)
                    {
                        switch (stat)
                        {
                            case "hp":
                                totalBonus += pd.promoteHp;
                                break;
                            case "atk":
                                totalBonus += pd.promoteAtk;
                                break;
                            case "def":
                                totalBonus += pd.promoteDef;
                                break;
                            case "evd":
                                totalBonus += pd.promoteAvoid;
                                break;
                            case "beat":
                                totalBonus += pd.promoteBeatDamageRatio;
                                break;
                            case "act":
                                totalBonus += pd.promoteActionDamageRatio;
                                break;
                            case "try":
                                totalBonus += pd.promoteTryDamageRatio;
                                break;
                        }
                    }
                }

                return totalBonus;
            }
        }
        public class CharaClothesData
        {
            public int atkBonus;
            public string bgTextureName;
            public int clothesPresetId;
            public int defBonus;
            public string flavorText;
            public int getRank;
            public int hpBonus;
            public string iconName;
            public int id;
            public string imgId;
            public string longskirt;
            public string name;
            public string playMotionType;
            public int rarity;
            public string replaceItemNum;
            public int stackMax;
        }
        public class EventData
        {
            public string bgFilename;
            public string bonusPhotoId1;
            public string bonusPhotoId2;
            public string bonusPhotoId3;
            public string bonusRatio1;
            public string bonusRatio1Max;
            public string bonusRatio2;
            public string bonusRatio2Max;
            public string bonusRatio3;
            public string bonusRatio3Max;
            public string coopStartLevel;
            public string dispCharaBodyMotion;
            public string dispCharaFaceMotion;
            public string dispCharaId;
            public string endDatetime;
            public string eventBannerId;
            public string eventCategory;
            public string eventChapterId;
            public string eventCoinId;
            public string eventCoinId2;
            public string eventGachaId;
            public string eventId;
            public string eventMissionGroupId;
            public string eventName;
            public string eventShopId;
            public string eventShopId2;
            public string extraopenQuestid;
            public string growthPresetId;
            public string growthSelcharaRatio;
            public string hardopenQuestid;
            public string homeDispFlg;
            public string missionBannerFilename;
            public string missionIconFilename;
            public string modeUiType;
            public string paidItemId;
            public string resetTime;
            public string startDatetime;
            public string storyFilename;
        }
        public class ParamAbility
        {
            public MGameObject m_GameObject;
            public int m_Enabled;
            public MScript m_Script;
            public string m_Name;
            public string abilityName;
            public string abilityEffect;
            public List<BuffList> buffList;
            public List<object> gutsList;

            public class MGameObject
            {
                public int m_FileID;
                public int m_PathID;
            }

            public class MScript
            {
                public int m_FileID;
                public long m_PathID;
            }

            public class BuffList
            {
                public string infoMessage;
                public int infoTime;
                public int condition;
                public int conditionHpRate;
                public int attributeMask;
                public int waveEnemyMask;
                public int healthMask;
                public int traitsTerrain;
                public int traitsTimezone;
                public int targetType;
                public int breakElement;
                public int buffType;
                public int abnormalType;
                public int spAttributeMask;
                public int spHealthMask;
                public int spEnemyMask;
                public int waveReuseNum;
                public int turn;
                public double coefficient;
                public int increment;
            }
        }
        public class ParamAlphaBase
        {
            public string m_Name;
            public int plasmPoint;
            public int hpParamLv1;
            public int hpParamLvMiddle;
            public int hpLvMiddleNum;
            public int hpParamLv99;
            public int atkParamLv1;
            public int atkParamLvMiddle;
            public int atkLvMiddleNum;
            public int atkParamLv99;
            public int defParamLv1;
            public int defParamLvMiddle;
            public int defLvMiddleNum;
            public int defParamLv99;
            public int maxStockMp;
            public int avoidRatio;
            public double width;
            public double height;
            public double AbnormalEffectHeadScale;
            public double AbnormalEffectHeadY;
            public double AbnormalEffectHeadZ;
            public double AbnormalEffectRootScale;
            public double AbnormalEffectRootY;
            public double AbnormalEffectRootZ;
            public int orderCardType00;
            public int orderCardType01;
            public int orderCardType02;
            public int orderCardType03;
            public int orderCardType04;
            public int orderCardValue00;
            public int orderCardValue01;
            public int orderCardValue02;
            public int orderCardValue03;
            public int orderCardValue04;
            public List<object> modelEffect;
            public double cameraOffsetX;
            public double cameraOffsetY;
            public double cameraOffsetZ;
            public double cameraRotationX;
            public double cameraRotationY;
            public double cameraRotationZ;
            public int entryLpNum;
            public string entryEffect;
        }
        public class ParamArts
        {
            public string m_Name { get; set; }
            public string actionName { get; set; }
            public string actionEffect { get; set; }
            public int actionInfoTime { get; set; }
            public AuthParam authParam { get; set; }
            public ActionParam actionParam { get; set; }
            public MotionParam motionParam { get; set; }
            public List<DamageList> damageList { get; set; }
            public List<BuffList> buffList { get; set; }

            public class AuthParam
            {
                public string authName { get; set; }
                public int SynergyFlag { get; set; }
            }

            public class ActionParam
            {
                public int attackType { get; set; }
                public int noLook { get; set; }
                public double quakeStartTime { get; set; }
                public double quakeEndTime { get; set; }
                public double quakeWidth { get; set; }
                public int quakeNum { get; set; }
                public int quakeType { get; set; }
                public double voiceDelay { get; set; }
                public int cameraType { get; set; }
            }
            public class CharaEffectOffsetPosition
            {
                public double x { get; set; }
                public double y { get; set; }
                public double z { get; set; }
            }
            public class MotionParam
            {
                public double advanceTime { get; set; }
                public double heightDifference { get; set; }
                public double motionStartTime { get; set; }
                public string charaEffectName { get; set; }
                public int jointMove { get; set; }
                public double charaEffectStartOffsetTime { get; set; }
                public double retreatTime { get; set; }
                public double nextActionTime { get; set; }
                public int motionActKey { get; set; }
            }
            public class ActionEffect
            {
                public string effectName { get; set; }
                public double startOffsetTime { get; set; }
                public int dispType { get; set; }
                public int dispModelNodeName { get; set; }
                public double flightTime { get; set; }
                public double hitHeight { get; set; }
                public string hitEffectName { get; set; }
                public double hitStartOffsetTime { get; set; }
                public double effectStartTime { get; set; }
            }
            public class DamageList
            {
                public int attributeMask { get; set; }
                public int waveEnemyMask { get; set; }
                public int healthMask { get; set; }
                public int targetType { get; set; }
                public int breakElement { get; set; }
                public double damageRate { get; set; }
                public int hitNum { get; set; }
                public double hitInterval { get; set; }
                public int dispObjectOnce { get; set; }
                public int blowType { get; set; }
                public double blowSpeed { get; set; }
                public double blowTime { get; set; }
                public int isGrow { get; set; }
                public double growthRate { get; set; }
                public ActionEffect actionEffect { get; set; }
            }
            public class ActionEffect2
            {
                public string effectName { get; set; }
                public double startOffsetTime { get; set; }
                public int dispType { get; set; }
                public int dispModelNodeName { get; set; }
                public double flightTime { get; set; }
                public double hitHeight { get; set; }
                public string hitEffectName { get; set; }
                public double hitStartOffsetTime { get; set; }
                public double effectStartTime { get; set; }
            }
            public class BuffList
            {
                public int attributeMask { get; set; }
                public int waveEnemyMask { get; set; }
                public int healthMask { get; set; }
                public int targetType { get; set; }
                public int breakElement { get; set; }
                public int buffType { get; set; }
                public int abnormalType { get; set; }
                public int spAttributeMask { get; set; }
                public int spHealthMask { get; set; }
                public int spEnemyMask { get; set; }
                public int timingType { get; set; }
                public int successRate { get; set; }
                public int turn { get; set; }
                public double coefficient { get; set; }
                public int increment { get; set; }
                public int isGrow { get; set; }
                public double growthRate { get; set; }
                public ActionEffect2 actionEffect { get; set; }
            }
        }
        public class ParamEnemyBase
        {
            public int m_Enabled { get; set; }
            public string m_Name { get; set; }
            public int charaType { get; set; }
            public int rare { get; set; }
            public string charaName { get; set; }
            public string eponymName { get; set; }
            public int attribute { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public double AbnormalEffectHeadScale { get; set; }
            public double AbnormalEffectHeadY { get; set; }
            public double AbnormalEffectHeadZ { get; set; }
            public double AbnormalEffectRootScale { get; set; }
            public double AbnormalEffectRootY { get; set; }
            public double AbnormalEffectRootZ { get; set; }
            public double modelDispOfsset { get; set; }
            public double nearAttackPosition { get; set; }
            public int actionNum { get; set; }
            public int increaseKpByAttack { get; set; }
            public int increaseKpByDamage { get; set; }
            public string artsParamId { get; set; }
            public string artsName { get; set; }
            public string artsInfo { get; set; }
            public int actionPattern { get; set; }
            public int hpParamLv1 { get; set; }
            public int hpParamLvMiddle { get; set; }
            public int hpLvMiddleNum { get; set; }
            public int hpParamLv99 { get; set; }
            public int atkParamLv1 { get; set; }
            public int atkParamLvMiddle { get; set; }
            public int atkLvMiddleNum { get; set; }
            public int atkParamLv99 { get; set; }
            public int defParamLv1 { get; set; }
            public int defParamLvMiddle { get; set; }
            public int defLvMiddleNum { get; set; }
            public int defParamLv99 { get; set; }
            public int maxStockMp { get; set; }
            public int avoidRatio { get; set; }
            public List<object> modelEffect { get; set; }
            public string abilityFileName { get; set; }
            public int modelId { get; set; }
            public string modelNodeName { get; set; }
            public string deathEffect { get; set; }
        }
        public class ParamSpecialAttack
        {
            public class ActionParam
            {
                public int attackType { get; set; }
                public int noLook { get; set; }
                public double quakeStartTime { get; set; }
                public double quakeEndTime { get; set; }
                public double quakeWidth { get; set; }
                public int quakeNum { get; set; }
                public int quakeType { get; set; }
                public double voiceDelay { get; set; }
                public int cameraType { get; set; }
            }
            public class ActionEffect
            {
                public string effectName { get; set; }
                public double startOffsetTime { get; set; }
                public int dispType { get; set; }
                public int dispModelNodeName { get; set; }
                public double flightTime { get; set; }
                public double hitHeight { get; set; }
                public string hitEffectName { get; set; }
                public double hitStartOffsetTime { get; set; }
                public double effectStartTime { get; set; }
            }

            public class DamageList
            {
                public int attributeMask { get; set; }
                public int waveEnemyMask { get; set; }
                public int healthMask { get; set; }
                public int targetType { get; set; }
                public int breakElement { get; set; }
                public double damageRate { get; set; }
                public int hitNum { get; set; }
                public double hitInterval { get; set; }
                public int dispObjectOnce { get; set; }
                public int blowType { get; set; }
                public double blowSpeed { get; set; }
                public double blowTime { get; set; }
                public int isGrow { get; set; }
                public double growthRate { get; set; }
                public ActionEffect actionEffect { get; set; }
            }
            public class ActionEffect2
            {
                public string effectName { get; set; }
                public double startOffsetTime { get; set; }
                public int dispType { get; set; }
                public int dispModelNodeName { get; set; }
                public double flightTime { get; set; }
                public double hitHeight { get; set; }
                public string hitEffectName { get; set; }
                public double hitStartOffsetTime { get; set; }
                public double effectStartTime { get; set; }
            }

            public class BuffList
            {
                public int attributeMask { get; set; }
                public int waveEnemyMask { get; set; }
                public int healthMask { get; set; }
                public int targetType { get; set; }
                public int breakElement { get; set; }
                public int buffType { get; set; }
                public int abnormalType { get; set; }
                public int spAttributeMask { get; set; }
                public int spHealthMask { get; set; }
                public int spEnemyMask { get; set; }
                public int timingType { get; set; }
                public int successRate { get; set; }
                public int turn { get; set; }
                public double coefficient { get; set; }
                public int increment { get; set; }
                public int isGrow { get; set; }
                public double growthRate { get; set; }
                public ActionEffect2 actionEffect { get; set; }
            }

            public string m_Name { get; set; }
            public string actionName { get; set; }
            public string actionEffect { get; set; }
            public int actionInfoTime { get; set; }
            public ActionParam actionParam { get; set; }
            public List<DamageList> damageList { get; set; }
            public List<BuffList> buffList { get; set; }

        }
        public class ParamWait
        {
            public int m_Enabled { get; set; }
            public string m_Name { get; set; }
            public string skillName { get; set; }
            public string skillEffect { get; set; }
            public int inBack { get; set; }
            public int atttackJoin { get; set; }
            public int activationRate { get; set; }
            public int activationNum { get; set; }
            public string charaEffectName { get; set; }
            public double charaEffectStartOffsetTime { get; set; }
            public List<BuffList> buffList { get; set; }
            public class ActionEffect
            {
                public string effectName { get; set; }
                public double startOffsetTime { get; set; }
                public int dispType { get; set; }
                public int dispModelNodeName { get; set; }
                public double flightTime { get; set; }
                public double hitHeight { get; set; }
                public string hitEffectName { get; set; }
                public double hitStartOffsetTime { get; set; }
                public double effectStartTime { get; set; }
            }

            public class BuffList
            {
                public int attributeMask { get; set; }
                public int waveEnemyMask { get; set; }
                public int healthMask { get; set; }
                public int targetType { get; set; }
                public int breakElement { get; set; }
                public int buffType { get; set; }
                public int abnormalType { get; set; }
                public int spAttributeMask { get; set; }
                public int spHealthMask { get; set; }
                public int spEnemyMask { get; set; }
                public int timingType { get; set; }
                public int successRate { get; set; }
                public int turn { get; set; }
                public double coefficient { get; set; }
                public int increment { get; set; }
                public int isGrow { get; set; }
                public double growthRate { get; set; }
                public ActionEffect actionEffect { get; set; }
            }
        }
        public class PhotoData
        {
            public string abilityEffectChangeFlg;
            public string atkLvMiddleNum;
            public string atkParamLv1;
            public string atkParamLvMax;
            public string atkParamLvMiddle;
            public string defLvMiddleNum;
            public string defParamLv1;
            public string defParamLvMax;
            public string defParamLvMiddle;
            public string expPhotoFlg;
            public string flavorTextAfter;
            public string flavorTextBefore;
            public string hpLvMiddleNum;
            public string hpParamLv1;
            public string hpParamLvMax;
            public string hpParamLvMiddle;
            public string id;
            public string illustratorName;
            public string imgFlg;
            public string kizunaPhotoFlg;
            public string levelTableId;
            public string limitOverFlg;
            public string name;
            public string noDestoryFlg;
            public string qeDispDatetime;
            public string rarity;
            public string reading;
            public string stackMax;
            public string startTime;
            public string type;

            public ParamAbility preEffect;
            public ParamAbility postEffect;
        }
        public class PromoteData
        {
            public float promoteActionDamageRatio;
            public int promoteAtk;
            public float promoteAvoid;
            public float promoteBeatDamageRatio;
            public int promoteCostItemNum;
            public int promoteCostSubItemNum;
            public int promoteDef;
            public int promoteHp;
            public int promoteId;
            public string promoteReleaseResult;
            public float promoteTryDamageRatio;
            public int promoteUseItemId00;
            public int promoteUseItemId01;
            public int promoteUseItemId02;
            public int promoteUseItemId03;
            public int promoteUseItemId04;
            public int promoteUseItemNum00;
            public int promoteUseItemNum01;
            public int promoteUseItemNum02;
            public int promoteUseItemNum03;
            public int promoteUseItemNum04;
        }
        public class PromotePresetData
        {
            public int promoteId00;
            public int promoteId01;
            public int promoteId02;
            public int promoteId03;
            public int promoteId04;
            public int promoteId05;
            public int promotePresetId;
            public int promoteStep;
            public string promoteStepDatetime;

            public List<PromoteData> promoteDatas;

            public void SetPromoteDatas(KF3Parse p)
            {
                promoteDatas = new List<PromoteData>();
                promoteDatas.Add(p.promoteData.Where(d => d.promoteId == promoteId00).FirstOrDefault());
                promoteDatas.Add(p.promoteData.Where(d => d.promoteId == promoteId01).FirstOrDefault());
                promoteDatas.Add(p.promoteData.Where(d => d.promoteId == promoteId02).FirstOrDefault());
                promoteDatas.Add(p.promoteData.Where(d => d.promoteId == promoteId03).FirstOrDefault());
                promoteDatas.Add(p.promoteData.Where(d => d.promoteId == promoteId04).FirstOrDefault());
                promoteDatas.Add(p.promoteData.Where(d => d.promoteId == promoteId05).FirstOrDefault());
            }
        }
        public class ItemCommon
        {
            public string flavorText;
            public string iconName;
            public int id;
            public string name;
            public int noSaleFlg;
            public int rarity;
            public int salePrice;
            public int stackMax;
            public int tabCategory;
            public string unitName;
        }
        public class QuestEnemiesData
        {
            public int enemiesId;
            public int enemyId00;
            public int enemyId01;
            public int enemyId02;
            public int enemyId03;
            public int enemyId04;
            public string escapeEnemyHpratio00;
            public string escapeEnemyHpratio01;
            public string escapeEnemyHpratio02;
            public string escapeEnemyHpratio03;
            public string escapeEnemyHpratio04;

            public bool vsFriends = false;
            public List<QuestEnemyData> questEnemyDatas = new List<QuestEnemyData>();
            public List<QuestEnemyFriendsData> questEnemyFriends = new List<QuestEnemyFriendsData>();

            public void SetEnemyDatas(KF3Parse p)
            {
                if (p.questEnemyData.Where(e => e.enemyId == enemyId00).FirstOrDefault() != null)
                {
                    questEnemyDatas.AddIfNotNull(p.questEnemyData.Where(e => e.enemyId == enemyId00).FirstOrDefault());
                    questEnemyDatas.AddIfNotNull(p.questEnemyData.Where(e => e.enemyId == enemyId01).FirstOrDefault());
                    questEnemyDatas.AddIfNotNull(p.questEnemyData.Where(e => e.enemyId == enemyId02).FirstOrDefault());
                    questEnemyDatas.AddIfNotNull(p.questEnemyData.Where(e => e.enemyId == enemyId03).FirstOrDefault());
                    questEnemyDatas.AddIfNotNull(p.questEnemyData.Where(e => e.enemyId == enemyId04).FirstOrDefault());
                }
                else
                {
                    questEnemyFriends.AddIfNotNull(p.questEnemyFriendsData.Where(e => e.enemyId == enemyId00).FirstOrDefault());
                    questEnemyFriends.AddIfNotNull(p.questEnemyFriendsData.Where(e => e.enemyId == enemyId01).FirstOrDefault());
                    questEnemyFriends.AddIfNotNull(p.questEnemyFriendsData.Where(e => e.enemyId == enemyId02).FirstOrDefault());
                    questEnemyFriends.AddIfNotNull(p.questEnemyFriendsData.Where(e => e.enemyId == enemyId03).FirstOrDefault());
                    questEnemyFriends.AddIfNotNull(p.questEnemyFriendsData.Where(e => e.enemyId == enemyId04).FirstOrDefault());
                }
            }
        }
        public class QuestEnemyData
        {
            public int drawId;
            public int enemyCharaId;
            public int enemyId;
            public int level;

            public ParamEnemyBase paramEnemyBase;
            public List<QuestDrawItemData> questDrawItemDatas;

            public void SetEnemyData(KF3Parse p)
            {
                string openFile = SharedSettings.paramsPath + "ParamEnemyBase_" + enemyCharaId + ".json";

                if (!File.Exists(openFile)) { Console.WriteLine("file " + openFile + " does not exist"); return; }
                paramEnemyBase = JObject.Parse(File.ReadAllText(openFile)).ToObject<ParamEnemyBase>();
            }
            public void SetDrawItemDatas(KF3Parse p)
            {
                questDrawItemDatas = p.questDrawItemData.Where(d => d.drawId == drawId).ToList();
            }
        }
        public class QuestEnemyFriendsData
        {
            public int charaId;
            public int clothId;
            public int drawId;
            public int enemyId;
            public int kizunaLevel;
            public int level;
            public int miracleLevel;
            public int miracleMax;
            public int photoId1;
            public int photoId2;
            public int photoId3;
            public int photoId4;
            public int photoLimit1;
            public int photoLimit2;
            public int photoLimit3;
            public int photoLimit4;
            public int photoLv1;
            public int photoLv2;
            public int photoLv3;
            public int photoLv4;
            public int rank;
            public int yasei;

            public List<QuestDrawItemData> questDrawItemDatas;

            public void SetDrawItemDatas(KF3Parse p)
            {
                questDrawItemDatas = p.questDrawItemData.Where(d => d.drawId == drawId).ToList();
            }
        }
        public class QuestWaveData
        {
            public string bossAuth;
            public int enemiesId;
            public int questOneId;
            public float rate;
            public int textinfoId;
            public int vsFriendsSetId;
            public int waveId;
            public string waveStartBgmId;

            public QuestEnemiesData questEnemiesData;
            public QuestQuestOneData questOneData;
            public QuestEnemyFriendsData questEnemyFriendsData;

            public void SetMultiple(KF3Parse p)
            {
                if(enemiesId != 0)
                    questEnemiesData = p.questEnemiesData.Where(e => e.enemiesId == enemiesId).FirstOrDefault();
                if(vsFriendsSetId != 0)
                    questEnemiesData = p.questEnemiesData.Where(e => e.enemiesId == vsFriendsSetId).FirstOrDefault();
                questOneData = p.questQuestOneData.Where(q => q.questOneId == questOneId).FirstOrDefault();
            }
        }
        public class QuestDrawItemData
        {
            public int drawId;
            public int itemId;
            public int itemNum;
            public float rate;

            public ItemCommon item;

            public void SetItem(KF3Parse p)
            {
                item = p.itemCommon.Where(i => i.id == itemId).FirstOrDefault();
            }
        }
        public class QuestMapData
        {
            public int chapterId { get; set; }
            public int dispItemIconId00 { get; set; }
            public int dispItemIconId01 { get; set; }
            public int dispItemIconId02 { get; set; }
            public int dispItemIconId03 { get; set; }
            public int dispItemIconId04 { get; set; }
            public int dispItemIconId05 { get; set; }
            public int mapId { get; set; }
            public string mapName { get; set; }
            public string mapObjName { get; set; }
            public int mapPosX { get; set; }
            public int mapPosY { get; set; }
            public int questMapCategory { get; set; }
            public int questRelWeek { get; set; }
            public object startDatetime { get; set; }
            public int startHideFlag { get; set; }
            public int weatherType { get; set; }
        }
        public class QuestQuestOneData
        {
            public string attkMask;
            public int charaExp;
            public string clearPerformance;
            public int compItemId;
            public int continueImpossible;
            public int difficulty;
            public string dispPriority;
            public string dummyHelperId01;
            public string dummyHelperId02;
            public string dummyHelperId03;
            public string evalSetId;
            public int goldNum;
            public int kizunaExp;
            public int kizunabonusCharaId;
            public float kizunabonusRatio;
            public string limitClearNum;
            public string memoryCharaId01;
            public string memoryCharaId02;
            public string memoryText01;
            public string memoryText02;
            public string memoryTextType;
            public string notUserhelper;
            public string openKeyItemId;
            public string openKeyItemNum;
            public int questGroupId;
            public int questOneId;
            public string questOneName;
            public string recoveryKeyItemId;
            public string recoveryKeyItemNum;
            public string recoveryMaxNum;
            public int relQuestId1;
            public int relQuestId2;
            public int relQuestId3;
            public int relQuestId4;
            public int relQuestId5;
            public string relQuestType1;
            public string relQuestType2;
            public string relQuestType3;
            public string relQuestType4;
            public string relQuestType5;
            public int rewardGroupId;
            public string rewardPhoto;
            public string scenarioAfterId;
            public string scenarioBeforeId;
            public string scenarioMiddleId;
            public string stagePresetId;
            public int stamina;
            public int storyOnly;
            public int useItemId;
            public int useItemNum;
            public int userExp;

            public QuestMapData questMap;
            public QuestQuestQuestgroupData questGroup;

            public void SetQuestGroup(KF3Parse p)
            {
                questGroup = p.questQuestQuestgroupData.Where(g => g.questGroupId == questGroupId).FirstOrDefault();
                questMap = p.questMapData.Where(m => m.mapId == questGroup.mapId).FirstOrDefault();
            }
        }
        public class QuestQuestQuestgroupData
        {
            public int autoModeEnable;
            public string dispChara2BodyMotion;
            public string dispChara2FaceMotion;
            public string dispCharaBodyMotion;
            public string dispCharaComment;
            public string dispCharaFaceMotion;
            public string dispCharaId;
            public string dispCharaId2;
            public string dispPriority;
            public long endDatetime;
            public int growthEventId;
            public string limitClearNum;
            public int mapId;
            public string questGroupCategory;
            public int questGroupId;
            public string relCharaId;
            public long startDatetime;
            public string storyName;
            public string timeDispFlg;
            public string titleCategory;
            public string titleName;
        }
        public class ShopItemData
        {
            public int dispType;
            public int disptypeTargetitemId;
            public int disptypeTargetitemId2;
            public long endTime;
            public int goodsId;
            public int itemId;
            public int itemNum;
            public int maxChangeNum;
            public int notOpenDispFlg;
            public int openMissionId;
            public int openQuestOneId;
            public string openQuestOneText;
            public int priceItemId;
            public int priceItemNum;
            public int priority;
            public int resetType;
            public int shopId;
            public long startTime;
        }
        public class Scenario
        {
            public class CharaData
            {
                public string model { get; set; }
                public string name { get; set; }
            }

            public class RowData
            {
                public int mType { get; set; }
                public int mSerifCharaID { get; set; }
                public string mSerifCharaName { get; set; }
                public int arrayNum { get; set; }
                public List<int> ID { get; set; }
                public List<int> mCharaPosition { get; set; }
                public List<int> mCharaMove { get; set; }
                public List<int> mModelMotion { get; set; }
                public List<int> mMotionFade { get; set; }
                public List<string> mModelFaceId { get; set; }
                public List<string> mIdleFaceId { get; set; }
                public List<int> mCharaEffect { get; set; }
                public List<int> mCharaFaceRot { get; set; }
                public List<string> mStrParams { get; set; }
                public List<int> mIntParams { get; set; }
                public List<double> mFloatParams { get; set; }
            }

            public int m_Enabled { get; set; }
            public string m_Name { get; set; }
            public string mTitleName { get; set; }
            public int ImpossibleSkip { get; set; }
            public List<object> cueSheetList { get; set; }
            public List<object> seSheetList { get; set; }
            public List<string> effectSheetList { get; set; }
            public List<CharaData> charaDatas { get; set; }
            public List<object> miraiDatas { get; set; }
            public List<RowData> rowDatas { get; set; }
        }
        public class StatsBrief
        {
            public string attribute;
            public string voice;
            public int level;
            public int wr;
            public int status;
            public int hp;
            public int atk;
            public int def;
            public string evd;
            public string beatBonus, actBonus, tryBonus;
        }
        public class SkillsBrief
        {
            public string MiracleName;
            public string MiracleDesc;
            public int MiracleType;
            public string MiracleMax;
            public string BeatName;
            public string BeatDesc;
            public string WaitName;
            public string WaitDesc;
            public string AbilityName;
            public string AbilityDesc;
            public string Ability1Name;
            public string Ability1Desc;
        }
        public class StagesBrief
        {
            public int id;
            public string drop;
            public float percentage;
        }
    }
}
