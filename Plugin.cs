using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace OmaNekoSeikatsuPlugin;

[BepInPlugin(
    MyPluginInfo.PLUGIN_GUID,
    MyPluginInfo.PLUGIN_NAME,
    MyPluginInfo.PLUGIN_VERSION
)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private static Dictionary<string, Font> fonts = new Dictionary<string, Font>();

    public static Font BestTenDOT {
        get {
            var font = fonts["BestTen-DOT"];
            return font;
        }
    }

    public static Font NotoSans {
        get {
            var font = fonts["InventoryNotoSansSC-Regular"];
            return font;
        }
    }

    private void Awake()
    {
        Logger = base.Logger;
        Debug.Log($"插件 OmaNeko-Seikatsu Plugin 加载完成！");

        var fontList = Resources.LoadAll<Font>("");
        Debug.Log($"找到 {fontList.Length} 个 Font 资源");
        foreach (var font in fontList) {
            fonts[font.name] = font;
            Debug.Log($"字体 {font.name} ({font.fontSize}px) 已加载");
        }

        var fontNameList = Font.GetOSInstalledFontNames();
        Debug.Log($"找到 {fontNameList.Length} 个系统字体");
        foreach (var font in fontNameList) {
            Debug.Log($"系统字体 {font} 已安装");
        }

        var harmony = new Harmony("com.undsf.omanekoseikatsu");
        harmony.PatchAll();
    }
}
