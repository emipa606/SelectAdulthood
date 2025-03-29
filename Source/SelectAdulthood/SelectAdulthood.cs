using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace SelectAdulthood;

[StaticConstructorOnStartup]
public class SelectAdulthood
{
    public static readonly List<ThingDef> AllRaces;

    static SelectAdulthood()
    {
        AllRaces = DefDatabase<ThingDef>.AllDefsListForReading
            .Where(def => def.race is { Animal: false, lifeStageAges.Count: > 1 } && !def.IsCorpse)
            .OrderBy(def => def.label).ToList();
        var harmony = new Harmony("Mlie.SelectAdulthood");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}