using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine;
using System.Reflection;

namespace OmaNekoSeikatsuPlugin;

[BepInPlugin(
    MyPluginInfo.PLUGIN_GUID,
    MyPluginInfo.PLUGIN_NAME,
    MyPluginInfo.PLUGIN_VERSION
)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public static Font BestTenDot;
    public static AssetBundle PixelFontsBundle;

    public static string PluginDir
    {
        get {
            var dll = Assembly.GetExecutingAssembly().Location;
            var dir = Path.GetDirectoryName(dll);
            return dir;
        }
    }

    private void Awake()
    {
        Logger = base.Logger;
        Debug.Log($"插件 OmaNeko-Seikatsu Plugin 加载完成！");

        var fontList = Resources.LoadAll<Font>("");
        Debug.Log($"找到 {fontList.Length} 个 Font 资源");
        foreach (var font in fontList) {
            Debug.Log($"内部字体 {font.name} ({font.fontSize}px) 已加载");
            switch (font.name)
            {
                case "BestTen-DOT":
                    BestTenDot = font;
                    break;
            }
        }

        LoadPixelFontsBundle();

        var harmony = new Harmony("com.undsf.omanekoseikatsu");
        harmony.PatchAll();
    }

    private void LoadPixelFontsBundle()
    {
        var path = Path.Combine(PluginDir, "pixelfonts.bundle");
        if (!File.Exists(path))
        {
            Debug.LogWarning($"找不到像素字体包 {path}");
            return;
        }

        var bundle = AssetBundle.LoadFromFile(path);
        var fonts = bundle.LoadAllAssets<Font>();
        foreach (var font in fonts)
        {
            Debug.Log($"外部字体 {font.name}");
        }
        PixelFontsBundle = bundle;
    }
}
