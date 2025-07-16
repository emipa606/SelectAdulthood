using System;
using System.Collections.Generic;
using System.Linq;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SelectAdulthood;

public class SelectAdulthoodMod : Mod
{
    private const float RowHeight = 65;

    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static SelectAdulthoodMod Instance;

    private static readonly Vector2 buttonSize = new(120f, 25f);

    private static readonly Vector2 searchSize = new(200f, 25f);

    private static Listing_Standard listingStandard;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    private static string searchText = "";

    private static readonly Color alternateBackground = new(0.2f, 0.2f, 0.2f, 0.5f);

    /// <summary>
    ///     The private settings
    /// </summary>
    private SelectAdulthoodSettings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public SelectAdulthoodMod(ModContentPack content)
        : base(content)
    {
        Instance = this;
        Instance.Settings.RaceAdulthood ??= new Dictionary<string, int>();

        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal SelectAdulthoodSettings Settings
    {
        get
        {
            settings ??= GetSettings<SelectAdulthoodSettings>();

            return settings;
        }
    }


    private static void drawButton(Action action, string text, Vector2 pos)
    {
        var rect = new Rect(pos.x, pos.y, buttonSize.x, buttonSize.y);
        if (!Widgets.ButtonText(rect, text, true, false, Color.white))
        {
            return;
        }

        SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera();
        action();
    }

    /// <summary>
    ///     The settings-window
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        base.DoSettingsWindowContents(rect);

        listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        Text.Font = GameFont.Medium;
        var headerLabel = listingStandard.Label("Adulthoods");

        Instance.Settings.RaceAdulthood ??= new Dictionary<string, int>();

        Text.Font = GameFont.Small;
        if (Instance.Settings.RaceAdulthood.Any())
        {
            drawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "SA.reset.confirm".Translate(),
                        delegate { Instance.Settings.ResetManualValues(); }));
                }, "SA.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        listingStandard.CheckboxLabeled("SA.logging.label".Translate(), ref Settings.VerboseLogging,
            "SA.logging.tooltip".Translate());
        listingStandard.CheckboxLabeled("SA.hidemechs.label".Translate(), ref Settings.HideMechs,
            "SA.hidemechs.tooltip".Translate());

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("SA.version.label".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((rect.width / 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((rect.width / 2) - (searchSize.x / 2), 0),
            searchSize), "SA.search".Translate());

        listingStandard.End();

        var races = SelectAdulthood.AllRaces;
        if (Settings.HideMechs)
        {
            races = races.Where(def => !def.race.IsMechanoid).ToList();
        }

        if (!string.IsNullOrEmpty(searchText))
        {
            races = SelectAdulthood.AllRaces.Where(def =>
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true)
                .ToList();
        }

        var borderRect = rect;
        borderRect.y += listingStandard.CurHeight;
        borderRect.height -= listingStandard.CurHeight;
        var scrollContentRect = rect;
        scrollContentRect.height = races.Count * RowHeight;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;


        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var raceThing in races)
        {
            var modInfo = raceThing.modContentPack?.Name;
            var raceInfo = raceThing.defName;
            var rowRect = scrollListing.GetRect(RowHeight);
            var slideRect = rowRect.RightPartPixels(rowRect.width - RowHeight);
            alternate = !alternate;
            if (alternate)
            {
                Widgets.DrawBoxSolid(rowRect.ExpandedBy(10, 0), alternateBackground);
            }

            var lifeStages = raceThing.race.lifeStageAges.Count - 1;
            if (!Instance.Settings.RaceAdulthood.ContainsKey(raceThing.defName) ||
                Instance.Settings.RaceAdulthood[raceThing.defName] > lifeStages)
            {
                Instance.Settings.RaceAdulthood[raceThing.defName] = lifeStages;
            }


            var raceLabel = $"{raceThing.label.CapitalizeFirst()}";
            if (raceLabel.Length > 55)
            {
                raceLabel = $"{raceLabel[..52]}...";
            }

            if (raceInfo is { Length: > 55 })
            {
                raceInfo = $"{raceInfo[..52]}...";
            }

            var currentStage = raceThing.race.lifeStageAges[Instance.Settings.RaceAdulthood[raceThing.defName]];
            if (Instance.Settings.RaceAdulthood[raceThing.defName] != lifeStages)
            {
                GUI.color = Color.green;
            }

            Instance.Settings.RaceAdulthood[raceThing.defName] =
                (int)Math.Round((decimal)Widgets.HorizontalSlider(
                    slideRect,
                    Instance.Settings.RaceAdulthood[raceThing.defName], 0,
                    lifeStages, false,
                    $"{currentStage.def.label.CapitalizeFirst()} {currentStage.minAge}+ ({currentStage.def.defName})",
                    raceLabel,
                    raceInfo), 4);
            Widgets.ThingIcon(rowRect.LeftPartPixels(RowHeight), raceThing);
            TooltipHandler.TipRegion(rowRect, modInfo);
            GUI.color = Color.white;
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Select Adulthood";
    }
}