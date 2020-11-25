using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Texture Atlas", menuName = "Texure Atlas")]
public class TextureAtlas : ScriptableObject
{
    public string Name;

    public int HorizontalTextureNumber;
    public int VerticalTextureNumber;

    public Texture Atlas;

    public List<string> TextureNames;

    public float NormalizedTexcoordX
    {
        get 
        {
            return 1f / (HorizontalTextureNumber != 0 ? HorizontalTextureNumber : 1f);
        }
    }

    public float NormalizedTexcoordY
    {
        get
        {
            return 1f / (VerticalTextureNumber != 0 ? VerticalTextureNumber : 1f);
        }
    }

    public Vector2 GetTextureCoordinatesByID(int id) 
    {
        float y = id / VerticalTextureNumber;
        float x = id - (y * HorizontalTextureNumber);

        x *= (float)NormalizedTexcoordX;
        y *= (float)NormalizedTexcoordY;

        y = 1f - y - (float)NormalizedTexcoordY;

        Vector2 texcoord = new Vector2(x, y);

        return texcoord;
    }

    public Vector2 GetNormalizedTexcoords() 
    {
        return new Vector2(NormalizedTexcoordX, NormalizedTexcoordY);
    }
}
