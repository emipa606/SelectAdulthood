using System.Collections.Generic;
using Verse;

namespace SelectAdulthood;

public class SelectAdulthoodSettings : ModSettings
{
    public bool HideMechs;
    public Dictionary<string, int> RaceAdulthoods = new Dictionary<string, int>();
    private List<string> raceAdulthoodsKeys;
    private List<int> raceAdulthoodsValues;

    public bool VerboseLogging;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
        Scribe_Values.Look(ref HideMechs, "HideMechs");
        Scribe_Collections.Look(ref RaceAdulthoods, "RaceAdulthoods", LookMode.Value,
            LookMode.Value,
            ref raceAdulthoodsKeys, ref raceAdulthoodsValues);
    }

    public void ResetManualValues()
    {
        raceAdulthoodsKeys = [];
        raceAdulthoodsValues = [];
        RaceAdulthoods = new Dictionary<string, int>();
    }
}