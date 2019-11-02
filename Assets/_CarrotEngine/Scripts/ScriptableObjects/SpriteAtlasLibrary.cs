using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.U2D;

namespace CarrotEngine
{
    [CreateAssetMenu(menuName = "Carrot Engine/SpriteAtlasLibrary", fileName = "SpriteAtlasLibrary")]
    public class SpriteAtlasLibrary : SerializedScriptableObject
    {
        [Header("Sprite Atlas Dictionary")]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine,
        IsReadOnly = false,
        KeyLabel = "Name",
        ValueLabel = "Sprite Atlas")]
        [GUIColor(0.7f, 0.7f, 1)]
        public Dictionary<string, SpriteAtlas> spriteAtlasDict = new Dictionary<string, SpriteAtlas>();
    }
}