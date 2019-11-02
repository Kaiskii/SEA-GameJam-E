using System;
using UnityEngine;

namespace CarrotEngine
{
    public class AudioEventEmitter : MonoBehaviour
    {
        [SerializeField] private AudioEventType playEvent = AudioEventType.None;
        [SerializeField] private AudioEventType stopEvent = AudioEventType.None;
        private AudioManager audioManager
        {
            get
            {
                audioManager = audioManager ?? Toolbox.Instance.GetManager<AudioManager>() ?? null;
                if (audioManager == null) { ConsoleDebugger.LogWarningFormat("Can't find Audio Manager. Source: {0}", gameObject.name); }
                return audioManager;
            }
            set
            {
                audioManager = value;
            }
        }
#pragma warning disable 0649
        [SerializeField] private string audioKey;
#pragma warning restore 0649

        [SerializeField] private AudioManager.ClipType clipType = AudioManager.ClipType.SFX;

        private Guid audioSourceKey;

        protected virtual void Start()
        {
            RunEvent(AudioEventType.ObjectStart);
        }

        protected virtual void OnDestroy()
        {
            RunEvent(AudioEventType.ObjectDestroy);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            RunEvent(AudioEventType.TriggerEnter);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            RunEvent(AudioEventType.TriggerExit);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            RunEvent(AudioEventType.TriggerEnter2D);
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            RunEvent(AudioEventType.TriggerExit2D);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            RunEvent(AudioEventType.CollisionEnter);
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            RunEvent(AudioEventType.CollisionExit);
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            RunEvent(AudioEventType.CollisionEnter2D);
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {
            RunEvent(AudioEventType.CollisionExit2D);
        }

        protected virtual void OnEnable()
        {
            RunEvent(AudioEventType.ObjectEnable);
        }

        protected virtual void OnDisable()
        {
            RunEvent(AudioEventType.ObjectDisable);
        }

        private void RunEvent(AudioEventType type)
        {
            if (playEvent == type && audioManager != null) { audioSourceKey = audioManager.PlayAudioClip(audioKey, clipType); }
            else if (stopEvent == type && audioSourceKey != Guid.Empty) { audioManager.StopAudio(audioSourceKey); }
        }
    }

    enum AudioEventType
    {
        None,
        ObjectStart,
        ObjectDestroy,
        TriggerEnter,
        TriggerExit,
        TriggerEnter2D,
        TriggerExit2D,
        CollisionEnter,
        CollisionExit,
        CollisionEnter2D,
        CollisionExit2D,
        ObjectEnable,
        ObjectDisable
    }

}