using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class StolenStuff
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
}
