using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarrotEngine
{
    public class AudioUIEventEmitter : MonoBehaviour
    {
        [SerializeField] Button button;

        [SerializeField] private AudioUIEventType playEvent = AudioUIEventType.None;
        //[SerializeField] private AudioEventType stopEvent = AudioEventType.None;

        private AudioManager _audioManager;
        private AudioManager audioManager
        {
            get
            {
                _audioManager = _audioManager ?? Toolbox.Instance.GetManager<AudioManager>() ?? null;
                if (_audioManager == null) { ConsoleDebugger.LogWarningFormat("Can't find Audio Manager. Source: {0}", gameObject.name); }
                return _audioManager;
            }
            set
            {
                _audioManager = value;
            }
        }


#pragma warning disable 0649
        [SerializeField] private string audioKey;
#pragma warning restore 0649

        [SerializeField] private AudioManager.ClipType clipType = AudioManager.ClipType.SFX;

        private Guid audioSourceKey;

        void Start()
        {
            if (button == null)
            {
                button = this.GetComponent<Button>();
            }

            switch(playEvent)
            {
                case AudioUIEventType.OnClick:
                    button.onClick.AddListener(() => RunEvent(AudioUIEventType.OnClick));
                    break;

                default:
                    ConsoleDebugger.LogWarningFormat("{0} Audio UI Event type not handled", playEvent);
                    break;
            }
        }

        private void RunEvent(AudioUIEventType type)
        {
            if (playEvent == type && audioManager != null) { audioSourceKey = audioManager.PlayAudioClip(audioKey, clipType); }
            //else if (stopEvent == type && audioSourceKey != Guid.Empty) { audioManager.StopAudio(audioSourceKey); }
        }


        enum AudioUIEventType
        {
            None,
            OnClick,
            OnHover
        }

    }
}