using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;
using System;

namespace OmaNekoSeikatsuPlugin;

[BepInPlugin(
    MyPluginInfo.PLUGIN_GUID,
    MyPluginInfo.PLUGIN_NAME,
    MyPluginInfo.PLUGIN_VERSION
)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; private set; }

    internal static new ManualLogSource Logger;

    public ConfigEntry<string> Japanese { get; private set; }

    public ConfigEntry<string> English { get; private set; }

    public ConfigEntry<string> Chinese { get; private set; }

    private void Awake()
    {
        Instance = this;
        Logger = base.Logger;
        Debug.Log($"插件 OmaNeko-Seikatsu Plugin 加载完成！");

        FontManager.Initialize();
        var fontNames = FontManager.Instance.FontNames;
        InitializeConfigEntries(fontNames);

        var harmony = new Harmony("com.undsf.omanekoseikatsu");
        harmony.PatchAll();
    }

    private void InitializeConfigEntries(string[] fontNames)
    {
        var acceptableValueList = new AcceptableValueList<string>(fontNames);

        Japanese = Config.Bind(
            "Fonts",
            "Japanese",
            "BestTen-DOT",
            new ConfigDescription(
                "选择日文字体",
                acceptableValueList
            )
        );
        Japanese.SettingChanged += OnFontChanged;

        English = Config.Bind(
            "Fonts",
            "English",
            "BestTen-DOT",
            new ConfigDescription(
                "选择英文字体",
                acceptableValueList
            )
        );
        English.SettingChanged += OnFontChanged;
        
        Chinese = Config.Bind(
            "Fonts",
            "Chinese",
            "BestTen-DOT",
            new ConfigDescription(
                "选择中文字体",
                acceptableValueList
            )
        );
        Chinese.SettingChanged += OnFontChanged;
    }

    private void OnFontChanged(object sender, EventArgs e)
    {
        var traverse = Traverse.Create(typeof(BestTenDotUIFontApplier));
        Debug.Log($"字体发生变化");
        traverse
            .Field("_instance")
            .Field("_font")
            .SetValue(null);
    }
}
