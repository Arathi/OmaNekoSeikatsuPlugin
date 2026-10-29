using System.IO;
using HarmonyLib;
using OmaNekoSeikatsuPlugin;
using UnityEngine;

[HarmonyPatch(
    typeof(BestTenDotUIFontApplier),
    "LoadFont"
)]
class LoadFontPatch {
    static bool Prefix(object __instance, ref object __result) {
        var fontName = "BoutiqueBitmap9x9_1.93";
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
            return Plugin.BestTenDOT;
        }
        Debug.Log($"加载系统字体 {name}");
        var font = Font.CreateDynamicFontFromOSFont(name, 16);
        return font;
    }

    static Font LoadBundleFont(string name)
    {
        var bundleName = "pixelfonts.bundle";
        var path = Path.Combine(Plugin.PluginDir, bundleName);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"找不到像素字体包 {path}");
            return null;
        }

        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        var assetNames = bundle.GetAllAssetNames();
        Debug.Log($"从 AssetBundle 中加载资源 {assetNames.Length} 个");
        foreach (var assetName in assetNames)
        {
            Debug.Log($"加载AssetBundle资源 {assetName}");
        }

        var font = bundle.LoadAsset<Font>(name);
        return font;
    }
}
