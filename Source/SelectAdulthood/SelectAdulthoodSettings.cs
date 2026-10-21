using System.Collections.Generic;
using Verse;

namespace SelectAdulthood;

public class SelectAdulthoodSettings : ModSettings
{
    public bool HideMechs;
    public Dictionary<string, int> RaceAdulthood = new();

    public bool VerboseLogging;
    private List<string> raceAdulthoodKeys;
    private List<int> raceAdulthoodValues;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
        Scribe_Values.Look(ref HideMechs, "HideMechs");
        Scribe_Collections.Look(ref RaceAdulthood, "RaceAdulthoods", LookMode.Value,
            LookMode.Value,
            ref raceAdulthoodKeys, ref raceAdulthoodValues);
    }

    public void ResetManualValues()
    {
        raceAdulthoodKeys = [];
        raceAdulthoodValues = [];
        RaceAdulthood = new Dictionary<string, int>();
    }
}