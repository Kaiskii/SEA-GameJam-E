using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarrotEngine
{
    [DisallowMultipleComponent]
    public class UIImageBinder : MonoBehaviour
    {
        Image uiImage;
#pragma warning disable 0649
        [SerializeField] private string spriteName;
#pragma warning restore 0649

        private void Awake()
        {
            uiImage = GetComponent<Image>();
            if (uiImage == null)
            {
                ConsoleDebugger.LogError("No Sprite Renderer found in object: " + name);
            }
        }

        private void Start()
        {
            uiImage.sprite = Toolbox.Instance.GetManager<SpriteAtlasManager>().GetSprite(spriteName);
        }
    }
}