using GRisk.Data;
using UnityEngine;

namespace GRisk.Util
{
    public class GRThings
    {
        public static void ownerColorize(Renderer renderer, uint owner)
        {
            renderer.material.color = GRData.playerStyleDict[owner].color;
        }

        public static void ownerColorize(Renderer renderer, uint owner, float tint)
        {
            renderer.material.color = GRData.playerStyleDict[owner].color * tint;
        }

        public enum ScaleAxis { X, Y, Z }
        
        public static void absSizer(GameObject go, Vector3 targetSize)
        {
            Vector3 currentSize = go.GetComponentInChildren<Renderer>().bounds.size;

            go.transform.localScale = new Vector3(
                currentSize.x / targetSize.x,
                currentSize.y / targetSize.y,
                currentSize.y / targetSize.z
            );
        }
        
        public static void absSizerOne(GameObject go, float targetSize, ScaleAxis targetAxis = ScaleAxis.X)
        {
            Renderer renderer = go.GetComponentInChildren<Renderer>();

            Vector3 currentSize = renderer.bounds.size;
    
            float currentVal = targetAxis switch
            {
                ScaleAxis.X => currentSize.x,
                ScaleAxis.Y => currentSize.y,
                ScaleAxis.Z => currentSize.z,
                _ => currentSize.x
            };

            // Calculate uniform scale factor
            float scaleFactor = targetSize / currentVal;
    
            // Apply uniform scaling to preserve aspect ratio
            go.transform.localScale = Vector3.one * scaleFactor;
        }
    }
}