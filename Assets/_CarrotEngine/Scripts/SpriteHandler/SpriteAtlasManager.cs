using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CarrotEngine
{
    public class SpriteAtlasManager : MonoBehaviour, IManager
    {
        [SerializeField] private List<SpriteAtlasLibrary> spriteAtlasLibraryList = new List<SpriteAtlasLibrary>();
        private Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

        #region Initializing
        public void InitializeManager()
        {
            AddSpriteAtlasLibrary(spriteAtlasLibraryList);
        }

        public void AddSpriteAtlasLibrary(List<SpriteAtlasLibrary> libraryList)
        {
            for (int i = 0; i < libraryList.Count; ++i)
            {
                AddSpriteAtlasLibrary(libraryList[i]);
            }
        }

        public void AddSpriteAtlasLibrary(SpriteAtlasLibrary library)
        {
            foreach (string key in library.spriteAtlasDict.Keys)
            {
                Sprite[] spriteArray = new Sprite[library.spriteAtlasDict[key].spriteCount];
                library.spriteAtlasDict[key].GetSprites(spriteArray);

                for(int i = 0; i < spriteArray.Length; ++i)
                {
                    if (spriteDictionary.ContainsKey(spriteArray[i].name))
                    {
                        ConsoleDebugger.LogWarning("Contains Duplicate Key: " + key + " in " + library.name);
                    }
                    spriteDictionary.Add(spriteArray[i].name, spriteArray[i]);
                }
            }
        }
        #endregion Initializing

        public Sprite GetSprite(string spriteName)
        {
            if(spriteDictionary.ContainsKey(spriteName))
            {
                return spriteDictionary[spriteName];
            }

            ConsoleDebugger.LogError("Sprite not found: " + spriteName);
            return null;
        }

        
    }
}