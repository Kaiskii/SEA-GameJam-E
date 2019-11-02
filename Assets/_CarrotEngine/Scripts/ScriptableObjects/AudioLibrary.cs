using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace CarrotEngine
{
    [CreateAssetMenu(menuName = "Carrot Engine/AudioLibrary", fileName = "AudioLibrary")]
    public class AudioLibrary : SerializedScriptableObject
    {
        [Header("Audio Library")]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine,
                IsReadOnly = false,
                KeyLabel = "Name",
                ValueLabel = "Audio Clip")]
        [GUIColor(0.7f, 0.7f, 1)]
        public Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

        //TODO: Load to dictionary on unity editor

    }
}
