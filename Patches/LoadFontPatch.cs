using HarmonyLib;
using OmaNekoSeikatsuPlugin;
using UnityEngine;

[HarmonyPatch(
    typeof(BestTenDotUIFontApplier),
    "LoadFont"
)]
class LoadFontPatch {
    static bool Prefix(object __instance, ref object __result) {
        var fontName = "BoutiqueBitmap-9px";
        var font = LoadBundleFont(fontName);
        if (font != null)
        {
            __result = font;
            return false;
        }
        Debug.LogWarning($"字体加载失败：{fontName}");
        return true;
    }

    static Font LoadOsFont(string name)
    {
        var lang = GameLocalization.CurrentLanguage;
        if (lang != GameLanguage.Chinese)
        {
            return Plugin.BestTenDot;
        }
        Debug.Log($"加载系统字体 {name}");
        var font = Font.CreateDynamicFontFromOSFont(name, 16);
        return font;
    }

    static Font LoadBundleFont(string name)
    {
        AssetBundle bundle = Plugin.PixelFontsBundle;
        if (bundle == null)
        {
            Debug.LogWarning("像素字体资源包打开失败");
            return null;
        }

        var font = bundle.LoadAsset<Font>(name);
        return font;
    }
}
