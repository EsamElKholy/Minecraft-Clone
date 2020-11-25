using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chunk Settings", menuName = "Chunk Settings")]
public class ChunkSettings : ScriptableObject
{    
    public Vector3 chunkSize;
    public Vector3 voxelSize;

    public Vector3 TotalSize 
    {
        get 
        {
            return new Vector3(chunkSize.x * voxelSize.x, chunkSize.y * voxelSize.y, chunkSize.z * voxelSize.z);
        }
    }

    public VoxelTypeCollection voxelTypeCollection;
    public TextureAtlas textureAtlas;
    public Material voxelMaterial;
}
