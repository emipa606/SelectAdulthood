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
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static SelectAdulthoodMod instance;

    private static readonly Vector2 buttonSize = new Vector2(120f, 25f);

    private static readonly Vector2 searchSize = new Vector2(200f, 25f);

    private static Listing_Standard listing_Standard;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    private static string searchText = "";

    private static readonly Color alternateBackground = new Color(0.2f, 0.2f, 0.2f, 0.5f);

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
        instance = this;
        if (instance.Settings.RaceAdulthoods == null)
        {
            instance.Settings.RaceAdulthoods = new Dictionary<string, int>();
        }

        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.SelectAdulthood"));
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal SelectAdulthoodSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<SelectAdulthoodSettings>();
            }

            return settings;
        }

        set => settings = value;
    }


    private static void DrawButton(Action action, string text, Vector2 pos)
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

        listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        Text.Font = GameFont.Medium;
        var headerLabel = listing_Standard.Label("Select Adulthood");

        if (instance.Settings.RaceAdulthoods == null)
        {
            instance.Settings.RaceAdulthoods = new Dictionary<string, int>();
        }

        if (instance.Settings.RaceAdulthoods.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "SA.reset.confirm".Translate(),
                        delegate { instance.Settings.ResetManualValues(); }));
                }, "SA.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;
        listing_Standard.CheckboxLabeled("SA.logging.label".Translate(), ref Settings.VerboseLogging,
            "SA.logging.tooltip".Translate());
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SA.version.label".Translate(currentVersion));
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

        listing_Standard.End();

        var races = SelectAdulthood.AllRaces;
        if (!string.IsNullOrEmpty(searchText))
        {
            races = SelectAdulthood.AllRaces.Where(def =>
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = rect;
        borderRect.y += headerLabel.y + 90;
        borderRect.height -= headerLabel.y + 90;
        var scrollContentRect = rect;
        scrollContentRect.height = races.Count * 51f;
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
            var rowRect = scrollListing.GetRect(50);
            alternate = !alternate;
            if (alternate)
            {
                Widgets.DrawBoxSolid(rowRect.ExpandedBy(10, 0), alternateBackground);
            }

            var lifeStages = raceThing.race.lifeStageAges.Count - 1;
            if (!instance.Settings.RaceAdulthoods.ContainsKey(raceThing.defName) ||
                instance.Settings.RaceAdulthoods[raceThing.defName] > lifeStages)
            {
                instance.Settings.RaceAdulthoods[raceThing.defName] = lifeStages;
            }


            var raceLabel = $"{raceThing.label.CapitalizeFirst()} ({raceThing.defName})";
            if (raceLabel.Length > 55)
            {
                raceLabel = $"{raceLabel.Substring(0, 52)}...";
            }

            if (modInfo is { Length: > 55 })
            {
                modInfo = $"{modInfo.Substring(0, 52)}...";
            }

            var currentStage = raceThing.race.lifeStageAges[instance.Settings.RaceAdulthoods[raceThing.defName]];
            if (instance.Settings.RaceAdulthoods[raceThing.defName] != lifeStages)
            {
                GUI.color = Color.green;
            }

            instance.Settings.RaceAdulthoods[raceThing.defName] =
                (int)Math.Round((decimal)Widgets.HorizontalSlider(
                    rowRect,
                    instance.Settings.RaceAdulthoods[raceThing.defName], 0,
                    lifeStages, false,
                    $"{currentStage.def.label.CapitalizeFirst()} {currentStage.minAge}+ ({currentStage.def.defName})",
                    raceLabel,
                    modInfo), 4);
            GUI.color = Color.white;
        }

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