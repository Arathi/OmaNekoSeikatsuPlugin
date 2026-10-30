using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class FontManager
{
    private const string FONT_NAME_BEST_TEN_DOT = "BestTen-DOT";

    public static FontManager Instance { get; private set; }

    public AssetBundle PixelFontsBundle { get; private set; }

    public Dictionary<string, Font> Fonts { get; private set; }

    public static void Initialize()
    {
        Debug.Log("正在初始化字体管理器");
        var bundle = LoadPixelFontsBundle();
        var manager = new FontManager(bundle);
        Instance = manager;
    }

    private FontManager(AssetBundle bundle)
    {
        PixelFontsBundle = bundle;
        Fonts = new Dictionary<string, Font>();
        LoadInternalFonts();
        LoadExternalFonts();
        Debug.Log($"字体加载完成，共计 {Fonts.Count} 个");
    }

    public static Font Default
    {
        get
        {
            return BestTenDot;
        }
    }

    public static Font BestTenDot
    {
        get
        {
            var name = FONT_NAME_BEST_TEN_DOT;
            return Instance.LoadFont(name);
        }
    }

    public string[] FontNames {
        get
        {
            var names = new List<string>();
            foreach (var name in Fonts.Keys)
            {
                names.Add(name);
            }
            return names.ToArray();
        }
    }

    private static AssetBundle LoadPixelFontsBundle()
    {
        var dll = Assembly.GetExecutingAssembly().Location;
        var pluginDir = Path.GetDirectoryName(dll);
        var fileName = "pixelfonts.bundle";
        var path = Path.Combine(pluginDir, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"像素字体包不存在 {path}");
            return null;
        }

        Debug.Log($"正在加载像素字体包 {path}");
        var bundle = AssetBundle.LoadFromFile(path);
        return bundle;
    }

    private void LoadInternalFonts()
    {
        Debug.Log("正在加载内部字体 ...");
        var fonts = Resources.LoadAll<Font>("");
        foreach (var font in fonts)
        {
            Debug.Log($"加载内部字体 {font.name}");
            Fonts[font.name] = font;
        }
        Debug.Log($"内部字体加载完成，共 {fonts.Length} 个");
    }

    private void LoadExternalFonts()
    {
        Debug.Log("正在加载外部字体 ...");
        var fonts = PixelFontsBundle.LoadAllAssets<Font>();
        foreach (var font in fonts)
        {
            Debug.Log($"加载外部字体 {font.name}");
            Fonts[font.name] = font;
        }
        Debug.Log($"外部字体加载完成，共 {fonts.Length} 个");
    }

    public Font LoadFont(string name)
    {
        var font = this.Fonts[name];
        if (font == null)
        {
            Debug.LogWarning($"找不到字体 {name}");
        }
        return font;
    }
}