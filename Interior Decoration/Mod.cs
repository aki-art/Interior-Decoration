using InteriorDecoration.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace InteriorDecoration
{
    // Bunch of random data this mod needs
    class Mod
    {
        public static string ModPath;
        public const string MOD_PREFIX = "Interior_Decoration_";
        public const string TILE_POSTFIX = "StainedGlassTile";

        public static UserSettings Settings { get; set; }

        // UI
        private const string ASSET_BUNDLE_FILE_NAME = "idsettingsui";
        public static Texture2D fabulousUnicornTexture;
        public static GameObject modSettingsScreenPrefab;

        // Mod compatibility
        public static Dictionary<string, CompMod> compatibleMods;

        public static void LoadAll()
        {
            LoadAssetBundle();
            fabulousUnicornTexture = GetTexture("assets", "sculpt_5_fx.png");
        }
        public static void SetUpModCompatibility()
        {
            compatibleMods = new Dictionary<string, CompMod>
            {
                // Tweak diamond statue tinting. 
                ["MaterialColor"] = new CompMod("MaterialColor.Painter, MaterialColor"),

                // Terrarium options 
                ["Roller Snakes"] = new CompMod("RollerSnake.SteelRollerSnakeConfig, RollerSnakeMerged"),
                ["I Love Slicksters - Morphs !"] = new CompMod("ILoveSlicksters.OwO_EffectPatch, ILoveSlicksters"),
                ["Fervine"] = new CompMod("Fervine.FervinePatches, Fervine-merged")
            };

            //List<string> detectedMods = compatibleMods.ToList().FindAll(m => Type.GetType(m.Value.MethodToCheck, false, false) != null).Select(m => m.Key).ToList();
            List<string> detectedMods = new List<string>();
               foreach (KeyValuePair<string, CompMod> mod in compatibleMods)
               {
                   try
                   {
                       if (Type.GetType(mod.Value.MethodToCheck, false, false) != null)
                       {
                           mod.Value.SetPresent(true);
                           detectedMods.Add(mod.Key);
                       }
                   }
                   catch { }
               }
            if (detectedMods.Count > 0)
                Log.Info($"Adding mod compatibility to {detectedMods.Count} mods: {detectedMods.Aggregate((i, j) => i + ", " + j)}");
        }

        // Loads a Unity Assetbundle
        public static void LoadAssetBundle()
        {
            Log.Info("Loading asset files... ");
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets", ASSET_BUNDLE_FILE_NAME);
            AssetBundle AssetBundle = AssetBundle.LoadFromFile(path);

            if (AssetBundle == null)
            {
                Log.Warning($"Failed to load AssetBundle from path {path}");
                return;
            }

            modSettingsScreenPrefab = AssetBundle.LoadAsset<GameObject>("ID1SettingsDialog");
        }

        // code by CynicalBusiness
        // Loads a custom texture for an existing atlas
        public static TextureAtlas GetCustomAtlas(string path, string fileName, TextureAtlas tileAtlas)
        {
            var tex = GetTexture(path, fileName, tileAtlas.texture.width, tileAtlas.texture.height);
            if (tex == null) return null;

            TextureAtlas atlas;
            atlas = ScriptableObject.CreateInstance<TextureAtlas>();
            atlas.texture = tex;
            atlas.vertexScale = tileAtlas.vertexScale;
            atlas.items = tileAtlas.items;

            return atlas;
        }

        public static Texture2D GetTexture(string path, string name, int width = 1, int height = 1)
        {
            Texture2D tex = null;
            string texFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path, name) + ".png";
            //Log.Debuglog("Loading image at: " + texFile);

            if (File.Exists(texFile))
            {
                var data = File.ReadAllBytes(texFile);
                tex = new Texture2D(width, height);
                tex.LoadImage(data);
            }
            //else
                //Debug.LogWarning($"Glass Sculptures Mod: Could not load texture at path {texFile}.");

            return tex;
        }

        public class CompMod
        {
            public string MethodToCheck;
            public bool IsPresent;

            public CompMod(string methodToCheck)
            {
                MethodToCheck = methodToCheck;
                IsPresent = false;
            }

            public void SetPresent(bool arg)
            {
                IsPresent = arg;
            }

        }
    }
}
