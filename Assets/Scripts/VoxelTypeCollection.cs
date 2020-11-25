using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Voxel Type Collection", menuName = "Voxel Type Collection")]
public class VoxelTypeCollection : ScriptableObject
{
    public List<VoxelType> VoxelTypes;

    public VoxelType GetVoxelType(string name) 
    {
        for (int i = 0; i < VoxelTypes.Count; i++)
        {
            if (VoxelTypes[i].TypeName == name)
            {
                return VoxelTypes[i];
            }
        }

        return ScriptableObject.Instantiate<VoxelType>(new VoxelType() { IsSolid = false, TypeName = "Air" });
    }
}
