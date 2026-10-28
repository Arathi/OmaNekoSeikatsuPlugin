using System.IO;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace OmaNekoSeikatsuPlugin;

// [HarmonyPatch(
//     typeof(Resources),
//     "Load",
//     new System.Type[] {
//         typeof(string),
//         typeof(System.Type)
//     }
// )]
class ResourcesLoadPatch {
    static bool Prefix(string path, System.Type systemTypeInstance, ref Object __result) {
        if (systemTypeInstance == typeof(Font)) {
            var lang = GameLocalization.CurrentLanguage;
            var font = LoadFont(path, lang);
            if (font != null) {
                Debug.Log("字体替换完成");
                Debug.Log($"material = {font.material}");
                Debug.Log($"material.shader = {font.material.shader}");
                Debug.Log($"material.mainTexture = {font.material.mainTexture}");
                __result = font;
                return false;
            }
        }
        return true;
    }

    static Font LoadFont(string path, GameLanguage lang) {
        Debug.Log($"加载 {lang} 字体 {path}");
        Font font = null;
        switch (path) {
            case "fonts/BestTen-DOT":
                // font = ReplaceFont(path, "BoutiqueBitmap9x9_1.93.ttf");
                font = ReplaceFont(path, "BestTen-DOT-v1.00.otf");
                break;
        }
        return font;
    }

    static Font ReplaceFont(string source, string fontName) {
        Debug.Log($"字体 {source} 替换为 {fontName}");
        var fontDir = Path.Combine(Paths.PluginPath, "Fonts");
        var fontPath = Path.Combine(fontDir, fontName);

        Font font = null;
        if (File.Exists(fontPath)) {
            font = Plugin.NotoSans;
        } else {
            Debug.Log($"字体不存在 {fontName}");
        }
        return font;
    }
}
