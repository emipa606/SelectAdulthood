using System.Collections.Generic;
using Verse;

namespace SelectAdulthood
{
    public class SelectAdulthoodSettings : ModSettings
    {
        public Dictionary<string, int> RaceAdulthoods = new Dictionary<string, int>();
        private List<string> raceAdulthoodsKeys;
        private List<int> raceAdulthoodsValues;

        public bool VerboseLogging;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
            Scribe_Collections.Look(ref RaceAdulthoods, "RaceAdulthoods", LookMode.Value,
                LookMode.Value,
                ref raceAdulthoodsKeys, ref raceAdulthoodsValues);
        }

        public void ResetManualValues()
        {
            raceAdulthoodsKeys = new List<string>();
            raceAdulthoodsValues = new List<int>();
            RaceAdulthoods = new Dictionary<string, int>();
        }
    }
}