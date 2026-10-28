using System.IO;
using BepInEx;
using HarmonyLib;
using OmaNekoSeikatsuPlugin;
using UnityEngine;

[HarmonyPatch(
    typeof(BestTenDotUIFontApplier),
    "LoadFont"
)]
class LoadFontPatch {
    static bool Prefix(object __instance, ref object __result) {
        // var traverse = Traverse.Create(__instance);
        var font = LoadFont("BoutiqueBitmap9x9");
        if (font != null)
        {
            __result = font;
            return false;
        }
        return true;
    }

    static Font LoadFont(string name)
    {
        var lang = GameLocalization.CurrentLanguage;
        if (lang != GameLanguage.Chinese)
        {
            return Plugin.BestTenDOT;
        }

        // var path = Path.Combine(Paths.PluginPath, "Fonts", name);
        Debug.Log($"加载系统字体 {name}");
        // var font = new Font([path], 16);
        var font = Font.CreateDynamicFontFromOSFont(name, 16);
        // font.material = Plugin.BestTenDOT.material;
        return font;
    }
}

[HarmonyPatch(
    typeof(BestTenDotUIFontApplier),
    "HandleLanguageChanged"
)]
class HandleLanguageChangedPatch {
    static bool Prefix(object __instance) {
        var traverse = Traverse.Create(__instance);
        var lang = GameLocalization.CurrentLanguage;
        Debug.Log($"语言发生变化 {lang}");
        traverse.Field("_font").SetValue(null);
        return true;
    }
}
