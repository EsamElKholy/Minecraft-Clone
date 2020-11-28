using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Settings", menuName = "World Settings")]
public class WorldSettings : ScriptableObject
{
    public ChunkSettings chunkSettings;
    public Vector3 worldSizeInChunks;
    public Vector3 maxChunkViewDistance;
    public int seed;
    public BiomeAttributes biomeAttributes;
}
