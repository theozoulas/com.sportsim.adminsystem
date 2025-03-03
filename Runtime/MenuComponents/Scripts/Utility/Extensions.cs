using UnityEngine;

namespace MenuComponents.Utility
{
    /// <summary>
    /// Extenstion Class <c>Extensions</c>
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Static Get Method <c>WriteCsvLine</c> Converts a string to a CSV field by enclosing it in double quotes and appending a comma.
        /// </summary>
        /// <param name="text">The string to be formatted as a CSV field.</param>
        /// <returns>A string with double quotes around the input text and a trailing comma.</returns>
        /// <remarks>
        /// This extension method adds proper CSV field formatting to any string,
        /// making it suitable for direct use in CSV file generation.
        /// </remarks>
        public static string WriteCsvLine(this string text) => '"' + text + '"' + ',';

        /// <summary>
        /// Static Get Method <c>ResizeTexture</c> Resizes the originalTexture to the dimensions specified by newWidth and newHeight.
        /// Uses RenderTexture and Graphics.Blit for high-quality resizing with the specified filter mode.
        /// The result is stored in the resizedTexture field which can be accessed via GetResizedTexture().
        /// </summary>
        /// <param name="originalTexture"></param>
        /// <param name="newSize"></param>
        /// <returns></returns>
        public static Texture2D ResizeTexture(this Texture2D originalTexture, Vector2Int newSize)
        {
            if (originalTexture == null) return null;

            var resizedTexture = new Texture2D
                (newSize.x, newSize.y, originalTexture.format, originalTexture.mipmapCount > 1)
                {
                    filterMode = FilterMode.Bilinear
                };

            var renderTexture
                = RenderTexture.GetTemporary(newSize.x, newSize.y, 0, RenderTextureFormat.ARGB32);

            RenderTexture.active = renderTexture;
            Graphics.Blit(originalTexture, renderTexture);

            resizedTexture.ReadPixels(new Rect(0, 0, newSize.x, newSize.y), 0, 0);
            resizedTexture.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);

            resizedTexture.name = originalTexture.name;

            return resizedTexture;
        }

        /// <summary>
        /// Static Get Method <c>CalculateConstrainedSize</c> Calculates a new size that fits within specified constraints while maintaining the original aspect ratio.
        /// If the original size already fits within the constraints, it is returned as-is (converted to integers).
        /// Otherwise, the size is scaled down proportionally to fit within the constraints.
        /// </summary>
        /// <param name="originalSize">The original width and height as a Vector2.</param>
        /// <param name="maxHeight">The maximum allowed height.</param>
        /// <param name="maxWidth">The maximum allowed width.</param>
        /// <returns>A Vector2Int containing the constrained width and height, preserving the original aspect ratio.</returns>
        public static Vector2Int CalculateConstrainedSize(this Vector2 originalSize, float maxHeight, float maxWidth)
        {
            if (originalSize.x <= maxWidth && originalSize.y <= maxHeight)
                return new Vector2Int((int)originalSize.x, (int)originalSize.y);

            var aspectRatio = originalSize.x / originalSize.y;
            var newWidth = maxWidth;
            var newHeight = maxHeight;

            if (newWidth / aspectRatio <= maxHeight) newHeight = newWidth / aspectRatio;
            else newWidth = newHeight * aspectRatio;

            return new Vector2Int((int)newWidth, (int)newHeight);
        }

        public static Vector2Int GetSizeAsVector2Int(this Texture2D texture2D)
        {
            return new Vector2Int(texture2D.width, texture2D.height);
        }
    }
}