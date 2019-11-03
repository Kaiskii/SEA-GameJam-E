using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace CarrotEngine
{
    /// <summary>
    /// Only for 2D audio 
    /// </summary>

    [DisallowMultipleComponent]
    public class AudioManager : SerializedMonoBehaviour, IManager
    {
        [SerializeField] private List<AudioLibrary> audioLibraryList = new List<AudioLibrary>();

        private GameObject bgmManager;
        private GameObject soundEffectManager;
        private GameObject voiceOverManager;

        private Guid bgmGuid;
        private AudioSource bgmAudioSource;
        private Dictionary<Guid, AudioSource> SFXDictionary = new Dictionary<Guid, AudioSource>();
        private Dictionary<Guid, AudioSource> VODictionary = new Dictionary<Guid, AudioSource>();

        private IEnumerator bgmCoroutine;

        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine,
        IsReadOnly = true,
        KeyLabel = "Name",
        ValueLabel = "AudioClip")]
        [GUIColor(0.7f, 0.7f, 1)]
        private Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

        #region Initializing
        public void InitializeManager()
        {
            if (soundEffectManager == null)
            {
                soundEffectManager = new GameObject("SFX Manager");
                soundEffectManager.transform.parent = transform;
            }

            if (voiceOverManager == null)
            {
                voiceOverManager = new GameObject("VO Manager");
                voiceOverManager.transform.parent = transform;
            }

            if (bgmManager == null)
            {
                bgmManager = new GameObject("BGM Manager");
                bgmGuid = Guid.NewGuid();
                bgmAudioSource = bgmManager.AddComponent<AudioSource>();
                bgmManager.transform.parent = transform;
            }

            AddAudioLibrary(audioLibraryList);
        }

        public void AddAudioLibrary(List<AudioLibrary> libraryList)
        {
            for (int i = 0; i < libraryList.Count; ++i)
            {
                AddAudioLibrary(libraryList[i]);
            }
        }

        public void AddAudioLibrary(AudioLibrary library)
        {
            foreach(string key in library.audioDict.Keys)
            {
                if (audioDictionary.ContainsKey(key))
                {
                    ConsoleDebugger.LogWarning("Contains Duplicate Key: " + key + " in " + library.name);
                }
                audioDictionary.Add(key, library.audioDict[key]);
            }
        }
        #endregion Initializing

        public Guid PlayAudioClip(string clipName, ClipType type)
        {
            if (string.IsNullOrWhiteSpace(clipName) || !audioDictionary.ContainsKey(clipName))
            {
                ConsoleDebugger.LogErrorFormat("No Audio Clip Found: {0}", clipName);
                return Guid.Empty;
            }

            AudioClip audioClip = audioDictionary[clipName];
            return PlayAudioClip(audioClip, type);
        }

        public Guid PlayAudioClip(AudioClip audioClip, ClipType clipType)
        {
            if (audioClip == null)
            {
                ConsoleDebugger.LogError("No Audio Clip Found");
                return Guid.Empty;
            }

            switch (clipType)
            {
                case ClipType.SFX:
                case ClipType.Voice:
                    return PlayOneShot(audioClip, clipType);

                case ClipType.BGM:
                    return BGMPlayer(audioClip);

                default:
                    return Guid.Empty;
            }
        }

        public bool StopAudio(Guid guid)
        {
            if (bgmGuid == guid)
            {
                BGMPlayerSettings(PlayBGMType.FadeOut);
                return true;
            }
            else if(SFXDictionary.ContainsKey(guid))
            {
                SFXDictionary[guid].Stop();
                return true;
            }
            else if(VODictionary.ContainsKey(guid))
            {
                VODictionary[guid].Stop();
                return true;
            }

            return false;
        }

        #region SFX and Voice
        Guid PlayOneShot(AudioClip audioClip, ClipType clipType)
        {
            Dictionary<Guid, AudioSource> audioDictRef;
            switch(clipType)
            {
                case ClipType.SFX:
                    audioDictRef = SFXDictionary;
                    break;

                case ClipType.Voice:
                    audioDictRef = VODictionary;
                    break;

                default:
                    ConsoleDebugger.LogError(clipType.ToString() + " unsupported type");
                    return Guid.Empty;
            }

            Guid newKey = Guid.NewGuid();
            foreach(Guid key in audioDictRef.Keys)
            {
                if (audioDictRef[key].isPlaying)
                {
                    continue;
                }
                else
                {
                    Debug.Log("Play SE: " + audioClip.name);
                                        
                    audioDictRef.Add(newKey, audioDictRef[key]);
                    audioDictRef.Remove(key);
                    audioDictRef[newKey].PlayOneShot(audioClip);

                    //audioList[i].volume = volume;
                    return newKey;
                }
            }

            Debug.Log("Instantiating new audio source");
            GameObject newAudioSourceObject = new GameObject();
            AudioSource newAudioSource = newAudioSourceObject.AddComponent<AudioSource>();
            audioDictRef.Add(newKey, newAudioSource);

            switch (clipType)
            {
                case ClipType.SFX:
                    newAudioSourceObject.transform.parent = soundEffectManager.transform;
                    //newAudioSource.volume = volume;
                    break;

                case ClipType.Voice:
                    newAudioSourceObject.transform.parent = voiceOverManager.transform;
                    //newAudioSource.volume = volume;
                    break;
            }

            newAudioSource.PlayOneShot(audioClip);

            Debug.Log("Play " + clipType.ToString() + ": " + audioClip.name);

            return newKey;
        }
        #endregion SFX and Voice

        #region BGMPlayer
        Guid BGMPlayer(AudioClip audioClip = null, PlayBGMType type = PlayBGMType.Repeat, float volume = 1f, float fadeDuration = 0.5f)
        {
            BGMPlayerSettings(type, volume, fadeDuration);

            bgmAudioSource.volume = volume;
            bgmAudioSource.clip = audioClip;
            bgmAudioSource.Play();

            return bgmGuid;
        }

        public void BGMPlayerSettings(PlayBGMType type = PlayBGMType.None, float volume = 1f, float fadeDuration = 0.5f)
        {
            // BGM Player Functions
            switch (type)
            {
                case PlayBGMType.Repeat:
                    if (bgmAudioSource.isPlaying) { bgmAudioSource.Stop(); }
                    bgmAudioSource.loop = true;
                    break;

                case PlayBGMType.Single:
                    if (bgmAudioSource.isPlaying) { bgmAudioSource.Stop(); }
                    bgmAudioSource.loop = false;
                    break;

                case PlayBGMType.Stop:
                    bgmAudioSource.Stop();
                    return;

                case PlayBGMType.FadeIn:
                case PlayBGMType.FadeOut:
                    if (bgmCoroutine != null)
                    {
                        StopCoroutine(bgmCoroutine);
                    }

                    bgmCoroutine = BGMFade(fadeDuration, type);
                    StartCoroutine(bgmCoroutine);
                    return;

                case PlayBGMType.None:
                default:
                    return;
            }
        }

        private IEnumerator BGMFade(float duration, PlayBGMType fade, float startVolume = -1f)
        {
            int dir = (fade == PlayBGMType.FadeOut) ? -1 : 1;

            if (startVolume >= 0)
            {
                bgmAudioSource.volume = startVolume;
            }

            float timer = duration;
            while (timer > 0)
            {
                bgmAudioSource.volume += Time.deltaTime / duration * dir;

                yield return null;
                timer -= Time.deltaTime;
                if (bgmAudioSource.volume == 1f || bgmAudioSource.volume == 0f)
                {
                    ConsoleDebugger.Log("Audio source is already at 0 or 1, stopping fade." + fade.ToString(), this);
                    ConsoleDebugger.Log("Fade lasted for (seconds): " + (duration - timer).ToString(), this);
                    yield break;
                }
            }

            Debug.Log("Completed BGM Fade In (true) /Out (false): " + fade.ToString() + " for duration: " + duration);
        }

        public enum PlayBGMType
        {
            None,
            Repeat,
            Single,
            Stop,
            FadeOut,
            FadeIn
        }

        public enum ClipType
        {
            Voice,
            SFX,
            BGM
        }
        #endregion BGMPlayer
    }
}