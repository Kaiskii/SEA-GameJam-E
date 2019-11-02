using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotEngine
{
    [DisallowMultipleComponent]
    public class SpriteBinder : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
#pragma warning disable 0649
        [SerializeField] private string spriteName;
#pragma warning restore 0649

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                ConsoleDebugger.LogError("No Sprite Renderer found in object: " + name);
            }
        }

        private void Start()
        {
            spriteRenderer.sprite = Toolbox.Instance.GetManager<SpriteAtlasManager>().GetSprite(spriteName);
        }
    }
}