
using HarmonyLib;
using UnityEngine;

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
