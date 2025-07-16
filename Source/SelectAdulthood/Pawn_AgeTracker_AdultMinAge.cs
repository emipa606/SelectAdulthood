using HarmonyLib;
using Verse;

namespace SelectAdulthood;

[HarmonyPatch(typeof(Pawn_AgeTracker), nameof(Pawn_AgeTracker.AdultMinAge), MethodType.Getter)]
public static class Pawn_AgeTracker_AdultMinAge
{
    public static void Postfix(Pawn ___pawn, ref float __result)
    {
        if (__result == 0)
        {
            return;
        }

        if (___pawn?.RaceProps == null)
        {
            return;
        }

        if (SelectAdulthoodMod.Instance.Settings.RaceAdulthood == null ||
            !SelectAdulthoodMod.Instance.Settings.RaceAdulthood.TryGetValue(___pawn.def.defName, out var adulthood))
        {
            return;
        }

        __result = ___pawn.RaceProps
            .lifeStageAges[adulthood].minAge;
    }
}