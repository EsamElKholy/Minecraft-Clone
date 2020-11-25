using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{
    public static float Get2DPerlin(Vector3 position, Vector3 size, float offset, float scale) 
    {
        return Mathf.PerlinNoise(((position.x + 0.5f) / size.x * scale) + offset, ((position.z + 0.5f) / size.z * scale) + offset);
    }
}
