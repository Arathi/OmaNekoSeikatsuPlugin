using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmaNekoSeikatsuPlugin;

// HarmonyPatch(
//    typeof(BestTenDotUIFontApplier),
//    "EnsureChineseFallbackFont"
// ]
class EnsureChineseFallbackFontPatch {
    static bool Prefix(object __instance) {
        var traverse = Traverse.Create(__instance);
        var _hasCheckedFallbackFont = traverse.Field("_hasCheckedFallbackFont").GetValue<bool>();
        var _font = traverse.Field("_font").GetValue<Font>();
        var ChineseFallbackOsFontNames = traverse.Field("ChineseFallbackOsFontNames").GetValue<string[]>();

        if (_hasCheckedFallbackFont)
        {
            Debug.Log("中文字体已经Fallback");
            return false;
        }

        if (_font == null) {
            Debug.Log("未获取到基准字体");
            return false;
        }

        if (!_font.dynamic)
        {
            Debug.Log("字体不是动态的");
            return false;
        }

        // if (_font.HasCharacter('啊'))
        // {
        //     Debug.Log("已存在中文字体");
        //     return false;
        // }

        Debug.Log($"获取默认 Fallback 字体 {_font.fontNames.Length} 个");
        foreach (var fontName in _font.fontNames)
        {
            Debug.Log($"Fallback字体 {fontName}");
        }

        List<string> list = [.. _font.fontNames];
        foreach (var fontName in ChineseFallbackOsFontNames)
        {
            if (!list.Contains(fontName))
            {
                list.Add(fontName);
                Debug.Log($"添加Fallback字体 {fontName}");
            }
        }
        _font.fontNames = list.ToArray();
        traverse.Field("_hasCheckedFallbackFont").SetValue(true);

        return false;
    }
}
