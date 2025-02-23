using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utility 
{
    public static void DestroyAllChidren(Transform transform)
    {
        int numChildren = transform.childCount;
        for (int i = numChildren - 1; i > 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static Texture2D toTexture2D(this RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }

    public static int GetClosestTransformInList<T>(Vector2 worldPosition, T[] components) where T : Component
    {
        float closestDistance = Vector2.Distance(worldPosition, components[0].transform.position);
        int closestWayPointIndex = 0;
        for (int i = 1; i < components.Length; i++)
        {
            if (Vector2.Distance(components[i].transform.position, worldPosition) < closestDistance)
            {
                closestWayPointIndex = i;
            }
        }
        return closestWayPointIndex;
    }

    public static int PositiveModulo(int x, int m)
    {
        return (x % m + m) % m;
    }

   // public static float NormalizeHue(this float hueAngle)
   // {
   //     return hueAngle * 360
   // }
}
