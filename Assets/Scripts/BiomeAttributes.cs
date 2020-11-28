using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome Attributes", menuName = "Biome Attributes")]
public class BiomeAttributes : ScriptableObject
{
    public string Name;
    
    public int solidGroundHeight;
    public int terrainHeight;    

    public float noiseScale;

    public List<Lode> lodes;
}

[System.Serializable]
public class Lode 
{
    public string Name;
    public VoxelType voxelType;
    
    public int minHeight;
    public int maxHeight;

    public float noiseScale;
    public float noiseOffset;
    public float noiseThreshold;
}
