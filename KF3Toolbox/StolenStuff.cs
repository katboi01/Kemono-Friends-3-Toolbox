using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StolenStuff
{
	public static int CalcParamInternal(int lvNow, int lvMax, int prmMin, int prmMax, int prmMid, int lvMid)
	{
		int result;
		if (lvNow < lvMid)
		{
			float t = (lvNow == lvMid) ? 1f : (((float)lvNow - 1f) / ((float)lvMid - 1f));
			result = (int)Lerp((float)prmMin, (float)prmMid, t);
		}
		else
		{
			float t2 = (lvNow == lvMax) ? 1f : (((float)lvNow - (float)lvMid) / ((float)lvMax - (float)lvMid));
			result = (int)Lerp((float)prmMid, (float)prmMax, t2);
		}
		return result;
	}

	public static float Lerp(float a, float b, float t)
	{
		return a * (1 - t) + b * t;
	}

	public class CharaDef
	{
		public static string GetAttributeName(CharaDef.AttributeType type)
		{
			switch (type)
			{
				case CharaDef.AttributeType.ALL:
					return "オール";
				case CharaDef.AttributeType.RED:
					return "ファニー";
				case CharaDef.AttributeType.GREEN:
					return "フレンドリー";
				case CharaDef.AttributeType.BLUE:
					return "リラックス";
				case CharaDef.AttributeType.PINK:
					return "ラブリー";
				case CharaDef.AttributeType.LIME:
					return "アクティブ";
				case CharaDef.AttributeType.AQUA:
					return "マイペース";
				default:
					return "";
			}
		}

		public static CharaDef.AttributeType AttributeMask2Type(CharaDef.AttributeMask msk)
		{
			CharaDef.AttributeType result = CharaDef.AttributeType.ALL;
			if ((msk & CharaDef.AttributeMask.RED) != (CharaDef.AttributeMask)0)
			{
				result = CharaDef.AttributeType.RED;
			}
			else if ((msk & CharaDef.AttributeMask.GREEN) != (CharaDef.AttributeMask)0)
			{
				result = CharaDef.AttributeType.GREEN;
			}
			else if ((msk & CharaDef.AttributeMask.BLUE) != (CharaDef.AttributeMask)0)
			{
				result = CharaDef.AttributeType.BLUE;
			}
			else if ((msk & CharaDef.AttributeMask.PINK) != (CharaDef.AttributeMask)0)
			{
				result = CharaDef.AttributeType.PINK;
			}
			else if ((msk & CharaDef.AttributeMask.LIME) != (CharaDef.AttributeMask)0)
			{
				result = CharaDef.AttributeType.LIME;
			}
			else if ((msk & CharaDef.AttributeMask.AQUA) != (CharaDef.AttributeMask)0)
			{
				result = CharaDef.AttributeType.AQUA;
			}
			return result;
		}

		public static CharaDef.EnemyMask Type2EnemyMask(CharaDef.Type typ)
		{
			if (typ == CharaDef.Type.BOSS)
			{
				return CharaDef.EnemyMask.BOSS;
			}
			if (typ == CharaDef.Type.ENEMY)
			{
				return CharaDef.EnemyMask.MOB;
			}
			if (typ != CharaDef.Type.PARTS)
			{
				return CharaDef.EnemyMask.FRIENDS;
			}
			return CharaDef.EnemyMask.PARTS;
		}

		public static string GetEnemyName(CharaDef.EnemyMask mask)
		{
			if ((mask & CharaDef.EnemyMask.BOSS) != (CharaDef.EnemyMask)0)
			{
				return ("強敵");
			}
			if ((mask & CharaDef.EnemyMask.PARTS) != (CharaDef.EnemyMask)0)
			{
				return ("部位");
			}
			if ((mask & CharaDef.EnemyMask.MOB) != (CharaDef.EnemyMask)0)
			{
				return ("強敵以外");
			}
			if ((mask & CharaDef.EnemyMask.FRIENDS) != (CharaDef.EnemyMask)0)
			{
				return ("フレンズ");
			}
			return "";
		}

		public static string GetAbilityTraitsName(CharaDef.AbilityTraits type)
		{
			if (type <= CharaDef.AbilityTraits.desert)
			{
				if (type <= CharaDef.AbilityTraits.jungle)
				{
					if (type == CharaDef.AbilityTraits.savanna)
					{
						return ("サバンナ");
					}
					if (type == CharaDef.AbilityTraits.jungle)
					{
						return ("ジャングル");
					}
				}
				else
				{
					if (type == CharaDef.AbilityTraits.cave)
					{
						return ("どうくつ");
					}
					if (type == CharaDef.AbilityTraits.desert)
					{
						return ("さばく");
					}
				}
			}
			else if (type <= CharaDef.AbilityTraits.cold_district)
			{
				if (type == CharaDef.AbilityTraits.waterside)
				{
					return ("みずべ");
				}
				if (type == CharaDef.AbilityTraits.cold_district)
				{
					return ("寒冷地");
				}
			}
			else
			{
				if (type == CharaDef.AbilityTraits.mountain)
				{
					return ("やま");
				}
				if (type == CharaDef.AbilityTraits.city)
				{
					return ("まち");
				}
				if (type == CharaDef.AbilityTraits.stadium)
				{
					return ("運動場");
				}
			}
			return ("-");
		}

		public static string GetAttributeNameEn(AttributeMask mask, bool shortName = false)
		{
			string output = "";
            if (shortName)
			{
				if (mask.HasFlag(AttributeMask.RED)) output += "Fu, ";
				if (mask.HasFlag(AttributeMask.GREEN)) output += "Fr, ";
				if (mask.HasFlag(AttributeMask.BLUE)) output += "Re, ";
				if (mask.HasFlag(AttributeMask.PINK)) output += "Lo, ";
				if (mask.HasFlag(AttributeMask.LIME)) output += "Ac, ";
				if (mask.HasFlag(AttributeMask.AQUA)) output += "Ca, ";
			}
            else
			{
				if (mask.HasFlag(AttributeMask.RED)) output += "Funny (red), ";
				if (mask.HasFlag(AttributeMask.GREEN)) output += "Friendly (green), ";
				if (mask.HasFlag(AttributeMask.BLUE)) output += "Relaxed (blue), ";
				if (mask.HasFlag(AttributeMask.PINK)) output += "Lovely (pink), ";
				if (mask.HasFlag(AttributeMask.LIME)) output += "Active (lime), ";
				if (mask.HasFlag(AttributeMask.AQUA)) output += "Carefree (aqua), ";
			}
			if(output.EndsWith(", "))
            {
				output = output.Substring(0,output.Length - 2);
            }
			return output;
		}

		public static string GetRedListInformationSource(int redListId)
		{
			if (!CharaDef.IsExistRedListId(redListId))
			{
				return "";
			}
			if (redListId <= 10)
			{
				return "IUCNによる保全状況(Ver.3.1)";
			}
			if (redListId <= 110)
			{
				return "環境省RDBによる保全状況";
			}
			if (redListId <= 210)
			{
				return "環境省レッドリスト2019";
			}
			if (redListId <= 310)
			{
				return "環境省レッドリスト2020";
			}
			if (redListId <= 410)
			{
				return "";
			}
			int uma_ID = CharaDef.UMA_ID;
			return "";
		}

		public static List<string> GetIconArrangement(int redListId)
		{
			List<string> list = new List<string>();
			if (CharaDef.IsExistRedListId(redListId))
			{
				if (redListId == CharaDef.UMA_ID)
				{
					for (int i = 0; i < 7; i++)
					{
						list.Add("？");
					}
				}
				else
				{
					int num = redListId / 100;
					int num2 = num * 100 + 1;
					int num3 = num * 100 + 7;
					for (int j = num2; j <= num3; j++)
					{
						string[] array = CharaDef.RedListInformationMap[j].Wording.Split(new char[]
						{
						'：'
						});
						if (array.Length >= 2)
						{
							list.Add(array[0]);
						}
					}
				}
			}
			return list;
		}

		public static CharaDef.RedList.Level GetRedListLevel(int redListId)
		{
			CharaDef.RedList.Level result = CharaDef.RedList.Level.INVALID;
			if (CharaDef.IsExistRedListId(redListId))
			{
				if (redListId == CharaDef.UMA_ID)
				{
				}
				result = (CharaDef.RedList.Level)(redListId % 100 % Enum.GetValues(typeof(CharaDef.RedList.Level)).Length);
			}
			return result;
		}

		public static string GetRedListWording(int redListId)
		{
			if (CharaDef.IsExistRedListId(redListId))
			{
				return CharaDef.RedListInformationMap[redListId].Wording;
			}
			return "";
		}

		public static bool IsExistRedListId(int redListId)
		{
			return CharaDef.RedListInformationMap.ContainsKey(redListId);
		}

		public static long AbnormalMask(CharaDef.ActionAbnormalMask msk)
		{
			return (long)msk;
		}

		public static long AbnormalMask(CharaDef.ActionAbnormalMask2 msk2)
		{
			return (long)msk2 << 32;
		}

		public static long AbnormalMask(CharaDef.ActionAbnormalMask msk, CharaDef.ActionAbnormalMask2 msk2)
		{
			return 0;
		}

		public const int INVALID_CHARA_ID = 0;

		private static readonly int UMA_ID = 510;

		private static readonly Dictionary<int, CharaDef.RedList> RedListInformationMap = new Dictionary<int, CharaDef.RedList>
	{
		{
			1,
			new CharaDef.RedList
			{
				Wording = "LC：軽度懸念"
			}
		},
		{
			2,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			3,
			new CharaDef.RedList
			{
				Wording = "VU：危急"
			}
		},
		{
			4,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧"
			}
		},
		{
			5,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅寸前"
			}
		},
		{
			6,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			7,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			8,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			9,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			10,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			101,
			new CharaDef.RedList
			{
				Wording = "LP：絶滅のおそれのある地域個体群"
			}
		},
		{
			102,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			103,
			new CharaDef.RedList
			{
				Wording = "VU：絶滅危惧II類"
			}
		},
		{
			104,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧IB類"
			}
		},
		{
			105,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅危惧IA類"
			}
		},
		{
			106,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			107,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			108,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			109,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			110,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			201,
			new CharaDef.RedList
			{
				Wording = "LP：絶滅のおそれのある地域個体群"
			}
		},
		{
			202,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			203,
			new CharaDef.RedList
			{
				Wording = "VU：絶滅危惧II類"
			}
		},
		{
			204,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧IB類"
			}
		},
		{
			205,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅危惧IA類"
			}
		},
		{
			206,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			207,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			208,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			209,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			210,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			301,
			new CharaDef.RedList
			{
				Wording = "LP：絶滅のおそれのある地域個体群"
			}
		},
		{
			302,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			303,
			new CharaDef.RedList
			{
				Wording = "VU：絶滅危惧II類"
			}
		},
		{
			304,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧IB類"
			}
		},
		{
			305,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅危惧IA類"
			}
		},
		{
			306,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			307,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			308,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			309,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			310,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			401,
			new CharaDef.RedList
			{
				Wording = "LC：軽度懸念"
			}
		},
		{
			402,
			new CharaDef.RedList
			{
				Wording = "NT：準絶滅危惧"
			}
		},
		{
			403,
			new CharaDef.RedList
			{
				Wording = "VU：危急"
			}
		},
		{
			404,
			new CharaDef.RedList
			{
				Wording = "EN：絶滅危惧"
			}
		},
		{
			405,
			new CharaDef.RedList
			{
				Wording = "CR：絶滅寸前"
			}
		},
		{
			406,
			new CharaDef.RedList
			{
				Wording = "EW：野生絶滅"
			}
		},
		{
			407,
			new CharaDef.RedList
			{
				Wording = "EX：絶滅"
			}
		},
		{
			408,
			new CharaDef.RedList
			{
				Wording = "DD：情報不足"
			}
		},
		{
			409,
			new CharaDef.RedList
			{
				Wording = "NE：未評価"
			}
		},
		{
			410,
			new CharaDef.RedList
			{
				Wording = "NoData"
			}
		},
		{
			CharaDef.UMA_ID,
			new CharaDef.RedList
			{
				Wording = "？？？：UMA"
			}
		}
	};

		public enum Type
		{
			INVALID,
			FRIENDS,
			ENEMY,
			BOSS,
			PARTS
		}

		public enum PartsType
		{
			DEFAULT,
			SUBUNIT
		}

		public enum AttributeType
		{
			ALL,
			RED,
			GREEN,
			BLUE,
			PINK,
			LIME,
			AQUA
		}

		[Flags]
		public enum AttributeMask
		{
			RED = 1,
			GREEN = 2,
			BLUE = 4,
			PINK = 8,
			LIME = 16,
			AQUA = 32
		}

		[Flags]
		public enum EnemyMask
		{
			BOSS = 1,
			MOB = 2,
			FRIENDS = 4,
			PARTS = 8
		}

		public static string GetEnemyMaskEn(EnemyMask em)
        {
			if (em.HasFlag(EnemyMask.BOSS) && em.HasFlag(EnemyMask.MOB) && em.HasFlag(EnemyMask.PARTS) && !em.HasFlag(EnemyMask.FRIENDS)) return "Cerulean";
			if (em.HasFlag(EnemyMask.BOSS) && em.HasFlag(EnemyMask.PARTS) && !em.HasFlag(EnemyMask.MOB) && !em.HasFlag(EnemyMask.FRIENDS)) return "Boss";
			string output = "";
			if (em.HasFlag(EnemyMask.BOSS)) output += "Boss, ";
			if (em.HasFlag(EnemyMask.PARTS)) output += "Parts, ";
			if (em.HasFlag(EnemyMask.MOB)) output += "Mob, ";
			if (em.HasFlag(EnemyMask.FRIENDS)) output += "Friend, ";
			if (output.EndsWith(", "))
			{
				output = output.Substring(0, output.Length - 2);
			}
			return output;
		}

		[Flags]
		public enum HealthMask
		{
			HEALTH = 1,
			POISON = 2,
			STUN = 4,
			SLEEP = 8,
			SEAL = 16,
			ICE = 32,
			BLEED = 64,
			UNHEAL = 128,
			MP_NOCOUNT = 256,
			BUFF_INVALID = 512,
			PARALYSIS = 1024,
			SILENCE = 2048,
			INAUDIBLE = 4096,
			BURNED = 8192,
			FOCUS = 16384,
			IMPATIENCE = 32768
		}

		public static string GetHealthMaskEn(HealthMask hm)
        {
			string output = "";
			if (hm.HasFlag(HealthMask.HEALTH)) output += "health, ";
			if (hm.HasFlag(HealthMask.POISON)) output += "poison, ";
			if (hm.HasFlag(HealthMask.STUN)) output += "stun, ";
			if (hm.HasFlag(HealthMask.SLEEP)) output += "sleep, ";
			if (hm.HasFlag(HealthMask.SEAL)) output += "exhaustion, ";
			if (hm.HasFlag(HealthMask.ICE)) output += "freeze, ";
			if (hm.HasFlag(HealthMask.BLEED)) output += "pain, ";
			if (hm.HasFlag(HealthMask.UNHEAL)) output += "bravado, ";
			if (hm.HasFlag(HealthMask.MP_NOCOUNT)) output += "amnesia, ";
			if (hm.HasFlag(HealthMask.BUFF_INVALID)) output += "gloom, ";
			if (hm.HasFlag(HealthMask.PARALYSIS)) output += "paralysis, ";
			if (hm.HasFlag(HealthMask.SILENCE)) output += "discord, ";
			if (hm.HasFlag(HealthMask.INAUDIBLE)) output += "lost flag, ";
			if (hm.HasFlag(HealthMask.BURNED)) output += "overheat, ";
			if (hm.HasFlag(HealthMask.FOCUS)) output += "marked, ";
			if (hm.HasFlag(HealthMask.IMPATIENCE)) output += "impatient, ";
			if (output.EndsWith(", "))
			{
				output = output.Substring(0, output.Length - 2);
			}
			return output;
		}

		public enum HealthMaskType
		{
			DEFAULT,
			SELF
		}

		[Flags]
		public enum AbilityTraits
		{
			without = 1,
			savanna = 2,
			jungle = 4,
			cave = 8,
			desert = 16,
			waterside = 32,
			cold_district = 64,
			mountain = 128,
			city = 256,
			stadium = 512
		}

		[Flags]
		public enum AbilityTraits2
		{
			without = 1,
			night = 2
		}

		public class RedList
		{
			public string Wording { get; set; }

			public enum Level
			{
				INVALID,
				L1,
				L2,
				L3,
				L4,
				L5,
				L6,
				L7,
				DD,
				NE,
				NOTHING
			}
		}

		public enum OrderCardType
		{
			INVALID,
			BEAT,
			ACTION,
			TRY
		}

		public enum ActionTargetType
		{
			INVALID,
			DEFAULT,
			ENEMY_SIDE_ALL,
			MY_SIDE_ALL,
			ENEMY_FRONT_ALL,
			ENEMY_SIDE_RANDOM,
			ENEMY_FRONT_RANDOM,
			SELF,
			INVALID2,
			MY_FRONT_ALL,
			MY_SIDE_RANDOM,
			MY_FRONT_RANDOM,
			ENEMY_LOWER_HP,
			MY_LOWER_HP,
			ENEMY_UPPER_HP,
			MY_UPPER_HP,
			WITHOUT_SELF,
			WITHOUT_SELF_ALL,
			ENEMY_SIDE_CAPITATION,
			ENEMY_FRONT_CAPITATION
		}

		public static string GetTargetTypeEn(ActionTargetType type)
		{
			return type switch
			{
				ActionTargetType.INVALID => "invalid",
				ActionTargetType.DEFAULT => "target",
				ActionTargetType.ENEMY_SIDE_ALL => "all enemies",
				ActionTargetType.MY_SIDE_ALL => "all allies",
				ActionTargetType.ENEMY_FRONT_ALL => "all front enemies*",
				ActionTargetType.ENEMY_SIDE_RANDOM => "random side enemy*",
				ActionTargetType.ENEMY_FRONT_RANDOM => "random front enemy*",
				ActionTargetType.SELF => "you",
				ActionTargetType.INVALID2 => "invalid2",
				ActionTargetType.MY_FRONT_ALL => "all front Friends*",
				ActionTargetType.MY_SIDE_RANDOM => "random side friend*",
				ActionTargetType.MY_FRONT_RANDOM => "random front friend*",
				ActionTargetType.ENEMY_LOWER_HP => "enemy with lowest HP",
				ActionTargetType.MY_LOWER_HP => "friend with lowest HP",
				ActionTargetType.ENEMY_UPPER_HP => "enemy with highest HP",
				ActionTargetType.MY_UPPER_HP => "friend with highest HP",
				ActionTargetType.WITHOUT_SELF => "friends other than you",
				ActionTargetType.WITHOUT_SELF_ALL => "all other than you",
				ActionTargetType.ENEMY_SIDE_CAPITATION => "enemy side capitation*",
				ActionTargetType.ENEMY_FRONT_CAPITATION => "enemy front capitation*",
				_ => ""
			};
		}

		public enum EffectDispType
		{
			INVALID,
			EACH,
			CENTER_DIRECT,
			GROUP,
			HEAD_TO_TARGET
		}

		public enum ActionDamageType
		{
			INVALID,
			NEAR,
			FAR,
			CENTER
		}

		public enum ActionBuffType
		{
			INVALID,
			POISON,
			STUN,
			ADD_DAMAGE,
			RCV_DAMAGE,
			INVALID_atk,
			INVALID_def,
			MP,
			AVOID,
			HATE,
			BEAT_DAMAGE,
			ACTION_DAMAGE,
			TRY_DAMAGE,
			TURN_MP,
			HEAL,
			TURN_HEAL,
			ABSORB,
			PLASM_DURATION,
			SLEEP,
			SEAL,
			HIT_RATE,
			RECOVER,
			RESIST,
			RESURRECT,
			SP_ADD_DAMAGE,
			SP_HIT_RATE,
			SP_RCV_DAMAGE,
			SP_AVOID,
			ICE,
			BLEED,
			UNHEAL,
			REFLECT,
			ANTI_REFLECT,
			COVER_LOWER,
			COVER_RANDOM,
			MP_NOCOUNT,
			RCV_ALLDAMAGE,
			HATE_HALF,
			PER_DAMAGE,
			BUFF_INVALID,
			DEBUFF_INVALID,
			TICKLING,
			MP_DOUBLE,
			PARALYSIS,
			SILENCE,
			HEAL_AMOUNT,
			MP_AMOUNT,
			INAUDIBLE,
			BURNED,
			FOCUS,
			SCHEDULED,
			ADD_OKAWARI,
			UPPER_OKAWARI,
			IMPATIENCE,
			ADD_PLASM
		}

		public static string GetBuffNameEn(ActionBuffType buff)
        {
			return buff switch
			{
				ActionBuffType.INVALID => "invalid",
				ActionBuffType.POISON => "poison",
				ActionBuffType.STUN => "stun",
				ActionBuffType.ADD_DAMAGE => "dealt damage amount",
				ActionBuffType.RCV_DAMAGE => "received damage amount",
				ActionBuffType.INVALID_atk => "ignore atk",
				ActionBuffType.INVALID_def => "ignore def",
				ActionBuffType.MP => "add MP",
				ActionBuffType.AVOID => "evasion",
				ActionBuffType.HATE => "threat",
				ActionBuffType.BEAT_DAMAGE => "BEAT damage",
				ActionBuffType.ACTION_DAMAGE => "ACTION damage",
				ActionBuffType.TRY_DAMAGE => "TRY damage",
				ActionBuffType.TURN_MP => "add MP every turn",
				ActionBuffType.HEAL => "heal",
				ActionBuffType.TURN_HEAL => "heal every turn",
				ActionBuffType.ABSORB => "absorb",
				ActionBuffType.PLASM_DURATION => "kemonoplasm duration buff",
				ActionBuffType.SLEEP => "sleep",
				ActionBuffType.SEAL => "exhaustion",
				ActionBuffType.HIT_RATE => "precision buff",
				ActionBuffType.RECOVER => "remove",
				ActionBuffType.RESIST => "resist",
				ActionBuffType.RESURRECT => "revive",
				ActionBuffType.SP_ADD_DAMAGE => "dealt damage amount",
				ActionBuffType.SP_HIT_RATE => "precision buff",
				ActionBuffType.SP_RCV_DAMAGE => "received damage amount",
				ActionBuffType.SP_AVOID => "enemy evasion",
				ActionBuffType.ICE => "freeze",
				ActionBuffType.BLEED => "pain",
				ActionBuffType.UNHEAL => "bravado",
				ActionBuffType.REFLECT => "reflect damage",
				ActionBuffType.ANTI_REFLECT => "ingore reflect damage",
				ActionBuffType.COVER_LOWER => "guard_lower",
				ActionBuffType.COVER_RANDOM => "guard_random",
				ActionBuffType.MP_NOCOUNT => "amnesia",
				ActionBuffType.RCV_ALLDAMAGE => "received damage amount",
				ActionBuffType.HATE_HALF => "reduce threat by half",
				ActionBuffType.PER_DAMAGE => "per_damage",
				ActionBuffType.BUFF_INVALID => "gloom",
				ActionBuffType.DEBUFF_INVALID => "joy",
				ActionBuffType.TICKLING => "tickle master",
				ActionBuffType.MP_DOUBLE => "MP hoard",
				ActionBuffType.PARALYSIS => "paralysis",
				ActionBuffType.SILENCE => "discord",
				ActionBuffType.HEAL_AMOUNT => "heal amount",
				ActionBuffType.MP_AMOUNT => "MP amount",
				ActionBuffType.INAUDIBLE => "lost flag",
				ActionBuffType.BURNED => "overheat",
				ActionBuffType.FOCUS => "marked",
				ActionBuffType.SCHEDULED => "schedule",
				ActionBuffType.ADD_OKAWARI => "add rice bowl",
				ActionBuffType.UPPER_OKAWARI => "add rice bowl limit",
				ActionBuffType.IMPATIENCE => "impatient",
				ActionBuffType.ADD_PLASM => "add kemonoplasm",
				_ => ""
			};
        }

		public enum ActionHealType
		{
			INVALID
		}

		public enum ArtsBlowType
		{
			INVALID,
			MIN,
			LARGE
		}

		public enum ActionTimingType
		{
			INVALID,
			TOP,
			END
		}

		public enum ActionCameraType
		{
			INVALID,
			POINT_FRONT,
			POINT_BACK,
			GROUP_FOCUS,
			BULLET_FOCUS,
			OVERLOOK,
			OVERLOOK_GROUP,
			FRIEND_GROUP_FOCUS,
			ALL_GROUP_FOCUS,
			SIMPLE
		}

		public enum MonitorQuakeType
		{
			INVALID,
			INCREMENT,
			DECREMENT
		}

		public enum TargetNodeName
		{
			root,
			pelvis,
			j_head,
			j_mouth,
			j_wrist_r,
			j_wrist_l,
			j_chest,
			j_toe_r,
			j_toe_l,
			j_weapon_a,
			j_weapon_b,
			j_special
		}

		public enum EnemyActionPattern
		{
			INVALID,
			LOTTERY,
			ROTATION
		}

		[Flags]
		public enum ActionAbnormalMask
		{
			POISON = 1,
			STUN = 2,
			SLEEP = 4,
			SEAL = 8,
			ICE = 16,
			BLEED = 32,
			UNHEAL = 64,
			ADD_DAMAGE = 128,
			BEAT_DAMAGE = 256,
			ACTION_DAMAGE = 512,
			TRY_DAMAGE = 1024,
			SP_ADD_DAMAGE = 2048,
			RCV_DAMAGE = 4096,
			SP_RCV_DAMAGE = 8192,
			TURN_MP = 16384,
			AVOID = 32768,
			SP_AVOID = 65536,
			HIT_RATE = 131072,
			SP_HIT_RATE = 262144,
			HATE = 524288,
			TURN_HEAL = 1048576,
			ABSORB = 2097152,
			PLASM_DURATION = 4194304,
			REFLECT = 8388608,
			ANTI_REFLECT = 16777216,
			COVER = 33554432,
			MP_NOCOUNT = 67108864,
			RCV_ALLDAMAGE = 134217728,
			HATE_HALF = 268435456,
			HEAL_AMOUNT = 536870912,
			MP_AMOUNT = 1073741824
		}

		[Flags]
		public enum ActionAbnormalMask2
		{
			PER_DAMAGE = 1,
			BUFF_INVALID = 2,
			DEBUFF_INVALID = 4,
			TICKLING = 8,
			MP_DOUBLE = 16,
			PARALYSIS = 32,
			SILENCE = 64,
			INAUDIBLE = 128,
			BURNED = 256,
			FOCUS = 512,
			SCHEDULED = 1024,
			IMPATIENCE = 2048,
			ADD_PLASM = 4096
		}

		public static string GetAbnormalTypeEn(ActionAbnormalMask hm)
        {
			string output = "";
			if (hm.HasFlag(ActionAbnormalMask.POISON)) output += "poison, ";
			if (hm.HasFlag(ActionAbnormalMask.STUN)) output += "stun, ";
			if (hm.HasFlag(ActionAbnormalMask.SLEEP)) output += "sleep, ";
			if (hm.HasFlag(ActionAbnormalMask.SEAL)) output += "exhaustion, ";
			if (hm.HasFlag(ActionAbnormalMask.ICE)) output += "freeze, ";
			if (hm.HasFlag(ActionAbnormalMask.BLEED)) output += "pain, ";
			if (hm.HasFlag(ActionAbnormalMask.UNHEAL)) output += "bravado, ";
			if (hm.HasFlag(ActionAbnormalMask.ADD_DAMAGE)) output += "damage up, ";
			if (hm.HasFlag(ActionAbnormalMask.BEAT_DAMAGE)) output += "BEAT up, ";
			if (hm.HasFlag(ActionAbnormalMask.ACTION_DAMAGE)) output += "ACTION up, ";
			if (hm.HasFlag(ActionAbnormalMask.TRY_DAMAGE)) output += "TRY up, ";
			if (hm.HasFlag(ActionAbnormalMask.SP_ADD_DAMAGE)) output += "damage up, ";
			if (hm.HasFlag(ActionAbnormalMask.RCV_DAMAGE)) output += "dmg receive up, ";
			if (hm.HasFlag(ActionAbnormalMask.SP_RCV_DAMAGE)) output += "dmg receive up, ";
			if (hm.HasFlag(ActionAbnormalMask.TURN_MP)) output += "mp per turn, ";
			if (hm.HasFlag(ActionAbnormalMask.AVOID)) output += "avoid up, ";
			if (hm.HasFlag(ActionAbnormalMask.SP_AVOID)) output += "avoid up, ";
			if (hm.HasFlag(ActionAbnormalMask.HIT_RATE)) output += "precision up, ";
			if (hm.HasFlag(ActionAbnormalMask.SP_HIT_RATE)) output += "precision up, ";
			if (hm.HasFlag(ActionAbnormalMask.HATE)) output += "threat up, ";
			if (hm.HasFlag(ActionAbnormalMask.TURN_HEAL)) output += "heal per turn, ";
			if (hm.HasFlag(ActionAbnormalMask.ABSORB)) output += "absorb, ";
			if (hm.HasFlag(ActionAbnormalMask.PLASM_DURATION)) output += "plasm duration, ";
			if (hm.HasFlag(ActionAbnormalMask.REFLECT)) output += "reflect, ";
			if (hm.HasFlag(ActionAbnormalMask.ANTI_REFLECT)) output += "ignore reflect, ";
			if (hm.HasFlag(ActionAbnormalMask.COVER)) output += "guard, ";
			if (hm.HasFlag(ActionAbnormalMask.MP_NOCOUNT)) output += "amnesia, ";
			if (hm.HasFlag(ActionAbnormalMask.RCV_ALLDAMAGE)) output += "all dmg receive, ";
			if (hm.HasFlag(ActionAbnormalMask.HATE_HALF)) output += "threat half, ";
			if (hm.HasFlag(ActionAbnormalMask.HEAL_AMOUNT)) output += "heal amount, ";
			if (hm.HasFlag(ActionAbnormalMask.MP_AMOUNT)) output += "MP amount, ";
			if (output.EndsWith(", "))
			{
				output = output.Substring(0, output.Length - 2);
			}
			return output;
		}

		public static string GetAbnormalTypeEn(ActionAbnormalMask2 hm)
		{
			string output = "";
			if (hm.HasFlag(ActionAbnormalMask2.PER_DAMAGE)) output += "per damage, ";
			if (hm.HasFlag(ActionAbnormalMask2.BUFF_INVALID)) output += "gloom, ";
			if (hm.HasFlag(ActionAbnormalMask2.DEBUFF_INVALID)) output += "joy, ";
			if (hm.HasFlag(ActionAbnormalMask2.TICKLING)) output += "tickle master, ";
			if (hm.HasFlag(ActionAbnormalMask2.MP_DOUBLE)) output += "MP hoard, ";
			if (hm.HasFlag(ActionAbnormalMask2.PARALYSIS)) output += "paralysis, ";
			if (hm.HasFlag(ActionAbnormalMask2.SILENCE)) output += "discord, ";
			if (hm.HasFlag(ActionAbnormalMask2.INAUDIBLE)) output += "lost flag, ";
			if (hm.HasFlag(ActionAbnormalMask2.BURNED)) output += "overheat, ";
			if (hm.HasFlag(ActionAbnormalMask2.FOCUS)) output += "marked, ";
			if (hm.HasFlag(ActionAbnormalMask2.SCHEDULED)) output += "schedule, ";
			if (hm.HasFlag(ActionAbnormalMask2.IMPATIENCE)) output += "impatient, ";
			if (hm.HasFlag(ActionAbnormalMask2.ADD_PLASM)) output += "add kemonoplasm, ";
			if (output.EndsWith(", "))
			{
				output = output.Substring(0, output.Length - 2);
			}
			return output;
		}

		public enum ConditionType
		{
			ABOVE,
			BELOW,
			EQUAL
		}

		public enum AiType
		{
			STUPID,
			CLEVER
		}
	}

}
