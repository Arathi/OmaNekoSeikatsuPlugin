using HarmonyLib;
using OmaNekoSeikatsuPlugin;
using UnityEngine;

[HarmonyPatch(
    typeof(BestTenDotUIFontApplier),
    "LoadFont"
)]
public class LoadFontPatch {
    private const string DefaultFontName = "BoutiqueBitmap-9px";

    static bool Prefix(object __instance, ref object __result) {
        var fontName = DefaultFontName;
        var lang = GameLocalization.CurrentLanguage;
        switch (lang)
        {
            case GameLanguage.Japanese:
                fontName = Plugin.Instance.Japanese.Value;
                break;
            case GameLanguage.English:
                fontName = Plugin.Instance.English.Value;
                break;
            case GameLanguage.Chinese:
                fontName = Plugin.Instance.Chinese.Value;
                break;
        }
        var font = FontManager.Instance.LoadFont(fontName);
        if (font != null)
        {
            __result = font;
            return false;
        }
        Debug.LogWarning($"字体加载失败：{fontName}");
        return true;
    }
}
