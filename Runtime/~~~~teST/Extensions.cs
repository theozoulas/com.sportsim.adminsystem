using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif    

namespace Editor.Scripts
{
    public static class Extensions 
    {
#if UNITY_EDITOR
        public static void SetTextureToReadable(this Texture2D texture)
        {
            if (texture.isReadable) return;
            
            var errorTexturePath = GetAssetPath(texture);
            var textureImporter = (TextureImporter)AssetImporter.GetAtPath(errorTexturePath);
            textureImporter.isReadable = true;
            textureImporter.SaveAndReimport();
        }

#endif    
    }
}
