using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkCoordinates 
{
    public int x;
    public int y;
    public int z;

    public ChunkCoordinates(int x, int y, int z) 
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public class ChunkData 
{
    public GameObject chunkObject;
    public ChunkCoordinates chunkCoordinates;
    public World world;
    public bool isInitialized;

    private bool isActive;
    public bool IsActive 
    {
        get 
        {
            return isActive;
        }

        set 
        {
            isActive = value;

            if (chunkObject != null)
            {
                chunkObject.SetActive(value);
            }
        }
    }

    private Vector3 worldPosition;   
    private Vector3[,,] voxelPositions;
    private VoxelType[,,] voxelTypes;

    public ChunkData(World world, ChunkCoordinates coordinates, bool initializeOnLoad) 
    {
        if (world.worldSettings.chunkSettings.chunkSize.x <= 0)
        {
            world.worldSettings.chunkSettings.chunkSize.x = 1;
        }

        if (world.worldSettings.chunkSettings.chunkSize.y <= 0)
        {
            world.worldSettings.chunkSettings.chunkSize.y = 1;
        }

        if (world.worldSettings.chunkSettings.chunkSize.z <= 0)
        {
            world.worldSettings.chunkSettings.chunkSize.z = 1;
        }

        world.worldSettings.chunkSettings.chunkSize.x = (int)world.worldSettings.chunkSettings.chunkSize.x;
        world.worldSettings.chunkSettings.chunkSize.y = (int)world.worldSettings.chunkSettings.chunkSize.y;
        world.worldSettings.chunkSettings.chunkSize.z = (int)world.worldSettings.chunkSettings.chunkSize.z;

        this.world = world;

        chunkCoordinates = coordinates;
        worldPosition = new Vector3(world.worldSettings.chunkSettings.chunkSize.x * coordinates.x * world.worldSettings.chunkSettings.voxelSize.x, world.worldSettings.chunkSettings.chunkSize.y * coordinates.y * world.worldSettings.chunkSettings.voxelSize.y, world.worldSettings.chunkSettings.chunkSize.z * coordinates.z * world.worldSettings.chunkSettings.voxelSize.z);

        if (initializeOnLoad)
        {
            Init();
        }
    }

    public void Init() 
    {
        voxelPositions = new Vector3[(int)world.worldSettings.chunkSettings.chunkSize.x, (int)world.worldSettings.chunkSettings.chunkSize.y, (int)world.worldSettings.chunkSettings.chunkSize.z];
        voxelTypes = new VoxelType[(int)world.worldSettings.chunkSettings.chunkSize.x, (int)world.worldSettings.chunkSettings.chunkSize.y, (int)world.worldSettings.chunkSettings.chunkSize.z];

        float width = world.worldSettings.chunkSettings.chunkSize.x * world.worldSettings.chunkSettings.voxelSize.x;
        float height = world.worldSettings.chunkSettings.chunkSize.y * world.worldSettings.chunkSettings.voxelSize.y;
        float depth = world.worldSettings.chunkSettings.chunkSize.z * world.worldSettings.chunkSettings.voxelSize.z;

        Bounds chunkBounds = new Bounds(worldPosition, new Vector3(width, height, depth));

        for (int w = 0; w < world.worldSettings.chunkSettings.chunkSize.x; w++)
        {
            for (int h = 0; h < world.worldSettings.chunkSettings.chunkSize.y; h++)
            {
                for (int d = 0; d < world.worldSettings.chunkSettings.chunkSize.z; d++)
                {
                    Vector3 center = new Vector3(chunkBounds.min.x + w * world.worldSettings.chunkSettings.voxelSize.x, chunkBounds.min.y + h * world.worldSettings.chunkSettings.voxelSize.y, chunkBounds.min.z + d * world.worldSettings.chunkSettings.voxelSize.z);
                    voxelPositions[w, h, d] = center;
                    voxelTypes[w, h, d] = world.DetermineVoxelType(GetChunkVoxelInWorldCoordinates(new Vector3(w, h, d), chunkCoordinates));
                }
            }
        }

        isInitialized = true;
    }

    public Vector3 GetChunkVoxelInWorldCoordinates(Vector3 voxelPositionInChunk, ChunkCoordinates chunkCoordinates) 
    {
        return new Vector3(voxelPositionInChunk.x + (chunkCoordinates.x * world.worldSettings.chunkSettings.chunkSize.x), 
            voxelPositionInChunk.y + (chunkCoordinates.y * world.worldSettings.chunkSettings.chunkSize.y), 
            voxelPositionInChunk.z + (chunkCoordinates.z * world.worldSettings.chunkSettings.chunkSize.z));
    }

    public int GetVoxelIndex(int x, int y, int z) 
    {
        if (x < world.worldSettings.chunkSettings.chunkSize.x && y < world.worldSettings.chunkSettings.chunkSize.y && z < world.worldSettings.chunkSettings.chunkSize.z)
        {
            return x + (int)world.worldSettings.chunkSettings.chunkSize.x * (y + (int)world.worldSettings.chunkSettings.chunkSize.y * z);
        }

        return -1;
    }

    public Vector3 GetVoxelPosition(int x, int y, int z) 
    {
        return voxelPositions[x, y, z];
    }

    public VoxelType GetVoxelType(int x, int y, int z)
    {
        return voxelTypes[Mathf.Clamp(x, 0, (int)world.worldSettings.chunkSettings.chunkSize.x - 1), 
            Mathf.Clamp(y, 0, (int)world.worldSettings.chunkSettings.chunkSize.y - 1), 
            Mathf.Clamp(z, 0, (int)world.worldSettings.chunkSettings.chunkSize.z - 1)];
    }

    public bool IsVoxelInChunk(int x, int y, int z) 
    {
        if (x >= 0 && x < world.worldSettings.chunkSettings.chunkSize.x && y >= 0 && y < world.worldSettings.chunkSettings.chunkSize.y && z >= 0 && z < world.worldSettings.chunkSettings.chunkSize.z)
        {
            return true;
        }

        return false;
    }

    public List<bool> CheckVoxelAdjacents(int x, int y, int z, ref List<bool> adjacents) 
    {
        if (adjacents.Count == 0)
        {
            adjacents = new List<bool>();
            
            for (int i = 0; i < 6; i++)
            {
                adjacents.Add(false);
            }
        }

        var chunkVoxelInWorldCoordinates = GetChunkVoxelInWorldCoordinates(new Vector3(x, y, z), chunkCoordinates);
      
        {
            int up = (int)chunkVoxelInWorldCoordinates.y + 1;
            int down = (int)chunkVoxelInWorldCoordinates.y - 1;
                
            int front = (int)chunkVoxelInWorldCoordinates.z + 1;
            int back = (int)chunkVoxelInWorldCoordinates.z - 1;
                
            int left = (int)chunkVoxelInWorldCoordinates.x - 1;
            int right = (int)chunkVoxelInWorldCoordinates.x + 1;

            if (world.IsWorldVoxelSolid(new Vector3(chunkVoxelInWorldCoordinates.x, up, chunkVoxelInWorldCoordinates.z)))
            {
                adjacents[(int)VoxelFaces.UP] = false;
            }
            else
            {
                adjacents[(int)VoxelFaces.UP] = true;
            }

            if (world.IsWorldVoxelSolid(new Vector3(chunkVoxelInWorldCoordinates.x, down, chunkVoxelInWorldCoordinates.z)))

            {
                adjacents[(int)VoxelFaces.DOWN] = false;
            }
            else
            {
                adjacents[(int)VoxelFaces.DOWN] = true;
            }

            if (world.IsWorldVoxelSolid(new Vector3(chunkVoxelInWorldCoordinates.x, chunkVoxelInWorldCoordinates.y, front)))
            {
                adjacents[(int)VoxelFaces.FRONT] = false;
            }
            else
            {
                adjacents[(int)VoxelFaces.FRONT] = true;
            }

            if (world.IsWorldVoxelSolid(new Vector3(chunkVoxelInWorldCoordinates.x, chunkVoxelInWorldCoordinates.y, back)))
            {
                adjacents[(int)VoxelFaces.BACK] = false;
            }
            else
            {
                adjacents[(int)VoxelFaces.BACK] = true;
            }

            if (world.IsWorldVoxelSolid(new Vector3(left, chunkVoxelInWorldCoordinates.y, chunkVoxelInWorldCoordinates.z)))
            {
                adjacents[(int)VoxelFaces.LEFT] = false;
            }
            else
            {
                adjacents[(int)VoxelFaces.LEFT] = true;
            }

            if (world.IsWorldVoxelSolid(new Vector3(right, chunkVoxelInWorldCoordinates.y, chunkVoxelInWorldCoordinates.z)))
            {
                adjacents[(int)VoxelFaces.RIGHT] = false;
            }
            else
            {
                adjacents[(int)VoxelFaces.RIGHT] = true;
            }
        }        

        return adjacents;
    }

    public void SetupMesh(ref MeshFilter meshFilter) 
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        List<bool> adjacents = new List<bool>();

        int previousVerticesCount = 0;

        for (int i = 0; i < world.worldSettings.chunkSettings.chunkSize.x; i++)
        {
            for (int j = 0; j < world.worldSettings.chunkSettings.chunkSize.y; j++)
            {
                for (int k = 0; k < world.worldSettings.chunkSettings.chunkSize.z; k++)
                {
                    CheckVoxelAdjacents(i, j, k, ref adjacents);

                    previousVerticesCount = vertices.Count;

                    if (voxelTypes[i, j, k].IsSolid)
                    {
                        VoxelData.GetVoxelVertices(ref vertices, adjacents, GetVoxelPosition(i, j, k), world.worldSettings.chunkSettings.voxelSize);
                        VoxelData.GetUVs(ref uvs, adjacents, voxelTypes[i, j, k], world.worldSettings.chunkSettings.textureAtlas);                
                        VoxelData.GetVoxelIndices(ref indices, adjacents, previousVerticesCount);
                    }
                }
            }
        }

        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.uv = uvs.ToArray();
        
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }
}
