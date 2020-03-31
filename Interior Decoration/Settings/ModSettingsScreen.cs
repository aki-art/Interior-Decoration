using InteriorDecoration.FUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static InteriorDecoration.InteriorDecorStrings.IDUI.IDMODSETTINGSSCREEN;

namespace InteriorDecoration.Settings
{
    class ModSettingsScreen : KScreen
    {
        private bool shown = false;
        public bool pause = true;
        public const float SCREEN_SORT_KEY = 300f;

        private FButton cancelButton;
        private FButton confirmButton;
        private FButton githubButton;
        private FButton steamButton;

        private Text versionLabel;
        private Text authorNote;

        private FToggle fabulousUnicornsToggle;
        private FToggle iceHatchToggle;
        private FToggle biscuitToggle;

        private FSpeedSlider stainedGlassSpeedSlider;

        private FCycle lanternFuelCycle;
        private FCycle aquariumPacuCycle;


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            #region set object references       
            // This would be handled by Unity normally via Unity magic
            const string path = "ScrollView/Viewport/Content/Panel";

            cancelButton = transform.Find("CancelButton").gameObject.AddComponent<FButton>();
            confirmButton = transform.Find("OKButton").gameObject.AddComponent<FButton>();
            githubButton = transform.Find("GithubButton").gameObject.AddComponent<FButton>();
            steamButton = transform.Find("SteamButton").gameObject.AddComponent<FButton>();

            versionLabel = transform.Find("VersionLabel").gameObject.GetComponent<Text>();
            authorNote = transform.Find("AuthorNote").gameObject.GetComponent<Text>();

            iceHatchToggle = transform.Find(path + "/GlassSculptureSettingsPanel/IceHatchToggle/Toggle").gameObject.AddComponent<FToggle>();
            fabulousUnicornsToggle = transform.Find(path + "/GlassSculptureSettingsPanel/FabUnicornsToggle/Toggle").gameObject.AddComponent<FToggle>();
            biscuitToggle = transform.Find(path + "/FountainSettingsPanel/BiscuitToggle/Toggle").gameObject.AddComponent<FToggle>();

            lanternFuelCycle = transform.Find(path + "/LanternSettingsPanel/CycleSelectorPanel/CycleSelector").gameObject.AddComponent<FCycle>();
            aquariumPacuCycle = transform.Find(path + "/AquariumSettingsPanel/CycleSelectorPanel/CycleSelector").gameObject.AddComponent<FCycle>();

            stainedGlassSpeedSlider = transform.Find(path + "/GlassTileSettingsPanel/SpeedSlider/Slider").gameObject.AddComponent<FSpeedSlider>();

            #endregion

            // Stained glass tiles speed slider
            stainedGlassSpeedSlider.fSlider.mapValue = x => x / 20f + 1f;
            stainedGlassSpeedSlider.fSlider.reverseMapValue = x => (x - 1f) * 20f;

            stainedGlassSpeedSlider.AssignRanges(new List<FSpeedSlider.Range> {
                new FSpeedSlider.Range(1f, STAINEDSPEED.NOBONUS, Color.grey),
                new FSpeedSlider.Range(1.05f, STAINEDSPEED.SMALL, Color.grey),
                new FSpeedSlider.Range(1.25f, STAINEDSPEED.REGULAR, Color.white),
                new FSpeedSlider.Range(1.3f, STAINEDSPEED.HASTY, Color.white),
                new FSpeedSlider.Range(1.5f, STAINEDSPEED.METAL, Color.yellow),
                new FSpeedSlider.Range(1.55f, STAINEDSPEED.FAST, Color.red)
            });

            // Lantern options
            lanternFuelCycle.Options = new List<FCycle.CycleOption>()
            {
                new FCycle.CycleOption("Oil", LANTERNCYCLE.OIL, null),
                new FCycle.CycleOption("Petroleum", LANTERNCYCLE.PETROLEUM, null)
            };

            // Aquarium options
            aquariumPacuCycle.Options = new List<FCycle.CycleOption>()
            {
                new FCycle.CycleOption("Stasis", PACUCYLCE.STASIS, PACUCYLCE.STASISDESC ),
                new FCycle.CycleOption("Normal", PACUCYLCE.NORMAL, PACUCYLCE.NORMALDESC ),
                new FCycle.CycleOption("TenTimes", PACUCYLCE.TENTIMES, PACUCYLCE.TENTIMESDESC )
            };
            aquariumPacuCycle.showDescriptions = true;

            SetSettings(Mod.Settings);

            ConsumeMouseScroll = true;
            activateOnSpawn = true;
            gameObject.SetActive(false);
        }

        public void ShowDialog()
        {
            if (transform.parent.GetComponent<Canvas>() == null && transform.parent.parent != null)
                transform.SetParent(transform.parent.parent);
            transform.SetAsLastSibling();
            versionLabel.GetComponent<Text>().text = "v" + typeof(ModSettingsScreen).Assembly.GetName().Version.ToString();

            // Buttons
            cancelButton.OnClick += OnClickCancel;
            confirmButton.OnClick += OnClickApply;
            githubButton.OnClick += OnClickGithub;
            steamButton.OnClick += OnClickSteam;

            UIHelper.AddSimpleToolTip(githubButton.gameObject, "Open Asphalt Tiles on Github");
            UIHelper.AddSimpleToolTip(steamButton.gameObject, "Open Asphalt Tiles on Steam");

            gameObject.SetActive(true);
        }

        private void SetSettings(UserSettings values)
        {
            aquariumPacuCycle.SetValue(values.AquariumPacuLifeCycle.ToString());
            lanternFuelCycle.SetValue(values.LanternCycle.ToString());

            fabulousUnicornsToggle.IsOn = values.GlassSculpturesFabUnicorns;
            iceHatchToggle.IsOn = values.GlassSculpturesIceHatch;
            biscuitToggle.IsOn = values.BiscuitLiquid;
            //safeFolderToggle.IsOn = values.UseSafeFolder;

            stainedGlassSpeedSlider.SetValue(values.StainedGlassTileSpeedMultiplier);
        }

        public void OnClickApply()
        {
            Enum.TryParse(aquariumPacuCycle.GetValue(), out UserSettings.PacuLifeCycle pacuLC);
            Mod.Settings.AquariumPacuLifeCycle = pacuLC;
            Enum.TryParse(lanternFuelCycle.GetValue(), out UserSettings.LanternFuel lanternFC);
            Mod.Settings.LanternCycle = lanternFC;

            Mod.Settings.GlassSculpturesFabUnicorns = fabulousUnicornsToggle.IsOn;
            Mod.Settings.GlassSculpturesIceHatch = iceHatchToggle.IsOn;
            Mod.Settings.BiscuitLiquid = biscuitToggle.IsOn;
            // Mod.Settings.UseSafeFolder = safeFolderToggle.IsOn;

            Mod.Settings.StainedGlassTileSpeedMultiplier = stainedGlassSpeedSlider.fSlider.Value;

            SettingsManager.SaveSettings();
            Deactivate();
        }

        public void OnClickCancel()
        {
            Deactivate();
        }
        public void OnClickGithub()
        {
            Application.OpenURL("https://github.com/aki-art/ONI-Asphalt-Tile");
        }
        public void OnClickSteam()
        {
            Application.OpenURL("https://steamcommunity.com/sharedfiles/filedetails/?id=1979475408");
        }
    }
}
