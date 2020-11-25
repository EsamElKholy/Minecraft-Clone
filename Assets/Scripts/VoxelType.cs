using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Voxel Type", menuName = "Voxel Type")]
public class VoxelType : ScriptableObject
{
    public string TypeName;
    public bool IsSolid;

    public int FrontFaceTextureID;
    public int BackFaceTextureID;
    public int UpFaceTextureID;
    public int DownFaceTextureID;
    public int LeftFaceTextureID;
    public int RightFaceTextureID;

    public int GetTextureIDForFace(VoxelFaces face) 
    {
        switch (face)
        {
            case VoxelFaces.UP:
                return UpFaceTextureID;
            case VoxelFaces.DOWN:
                return DownFaceTextureID;
            case VoxelFaces.BACK:
                return BackFaceTextureID;
            case VoxelFaces.FRONT:
                return FrontFaceTextureID;
            case VoxelFaces.LEFT:
                return LeftFaceTextureID;
            case VoxelFaces.RIGHT:
                return RightFaceTextureID;
            default:
                return 0;
        }
    }
}

