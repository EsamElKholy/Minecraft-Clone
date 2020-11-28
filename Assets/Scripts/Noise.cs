using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
    public static float Get2DPerlin(Vector3 position, Vector3 size, float offset, float scale) 
    {
        return Mathf.PerlinNoise(((position.x + 0.1f) / size.x * scale) + offset, ((position.z + 0.5f) / size.z * scale) + offset);
    }

    public static bool Get3DPerlin(Vector3 position, float offset, float scale, float threshold) 
    {
        float x = (position.x + offset + 0.1f) * scale;
        float y = (position.y + offset + 0.1f) * scale;
        float z = (position.z + offset + 0.1f) * scale;

        float XY = Mathf.PerlinNoise(x, y);
        float YZ = Mathf.PerlinNoise(y, z);
        float XZ = Mathf.PerlinNoise(x, z);

        float YX = Mathf.PerlinNoise(y, x);
        float ZY = Mathf.PerlinNoise(z, y);
        float ZX = Mathf.PerlinNoise(z, x);

        float average = (XY + YZ + XZ + YX + ZY + ZX) / 6f;

        if (average > threshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
