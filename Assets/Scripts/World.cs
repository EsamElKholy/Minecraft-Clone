using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World 
{
    public WorldSettings worldSettings;

    private ChunkData[,,] chunks;
    private List<ChunkCoordinates> activeChunks = new List<ChunkCoordinates>();

    private Vector3 spawnPosition;
    private ChunkCoordinates lastPlayerChunkCoordinates;
    private Transform root;

    public Vector3 WorldSizeInVoxels
    {
        get
        {
            return new Vector3(worldSettings.worldSizeInChunks.x * worldSettings.chunkSettings.chunkSize.x, worldSettings.worldSizeInChunks.y * worldSettings.chunkSettings.chunkSize.y, worldSettings.worldSizeInChunks.z * worldSettings.chunkSettings.chunkSize.z);
        }
    }

    public World(WorldSettings settings, Transform root) 
    {
        worldSettings = settings;
        Random.InitState(worldSettings.seed);
        this.root = root;
    }

    public void Init(Transform player, float playerHeight) 
    {
        spawnPosition = GetSpawnPosition(playerHeight);

        GenerateWorld();
        player.position = spawnPosition;
        lastPlayerChunkCoordinates = GetChunkCoordinatesFromWorldPosition(player.position);
    }

    public Vector3 GetSpawnPosition(float playerHeight) 
    {
        return new Vector3(worldSettings.worldSizeInChunks.x * worldSettings.chunkSettings.chunkSize.x * worldSettings.chunkSettings.voxelSize.x / 2,
                   (worldSettings.chunkSettings.chunkSize.y * worldSettings.chunkSettings.voxelSize.y / 2) + playerHeight,
                   worldSettings.worldSizeInChunks.z * worldSettings.chunkSettings.chunkSize.z * worldSettings.chunkSettings.voxelSize.z / 2);
    }

    public void GenerateWorld()
    {
        chunks = new ChunkData[(int)worldSettings.worldSizeInChunks.x, (int)worldSettings.worldSizeInChunks.y, (int)worldSettings.worldSizeInChunks.z];

        int startX = Mathf.Clamp((int)(worldSettings.worldSizeInChunks.x / 2) - (int)worldSettings.maxChunkViewDistance.x, 0, (int)(worldSettings.worldSizeInChunks.x / 2) - (int)worldSettings.maxChunkViewDistance.x);
        int endX = (int)(worldSettings.worldSizeInChunks.x / 2) + (int)worldSettings.maxChunkViewDistance.x;

        int startY = Mathf.Clamp((int)(worldSettings.worldSizeInChunks.y / 2) - (int)worldSettings.maxChunkViewDistance.y, 0, (int)(worldSettings.worldSizeInChunks.y / 2) - (int)worldSettings.maxChunkViewDistance.y);
        int endY = (int)(worldSettings.worldSizeInChunks.y / 2) + (int)worldSettings.maxChunkViewDistance.y;

        int startZ = Mathf.Clamp((int)(worldSettings.worldSizeInChunks.z / 2) - (int)worldSettings.maxChunkViewDistance.z, 0, (int)(worldSettings.worldSizeInChunks.z / 2) - (int)worldSettings.maxChunkViewDistance.z);
        int endZ = (int)(worldSettings.worldSizeInChunks.z / 2) + (int)worldSettings.maxChunkViewDistance.z;

        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                for (int k = startZ; k < endZ; k++)
                {
                    GenerateChunkFromChunkCoordinates(i, j, k);
                }
            }
        }

        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                for (int k = startZ; k < endZ; k++)
                {
                    SetupChunkGameObjects(i, j, k);
                }
            }
        }
    }

    public ChunkCoordinates GetChunkCoordinatesFromWorldPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / worldSettings.chunkSettings.chunkSize.x);
        int y = Mathf.FloorToInt(position.y / worldSettings.chunkSettings.chunkSize.y);
        int z = Mathf.FloorToInt(position.z / worldSettings.chunkSettings.chunkSize.z);

        ChunkCoordinates coordinates = new ChunkCoordinates(x, y, z);

        return coordinates;
    }

    public void UpdateWorld(Transform player)
    {
        var chunkCoordinates = GetChunkCoordinatesFromWorldPosition(player.position);

        if (lastPlayerChunkCoordinates == chunkCoordinates)
        {
            return;
        }

        for (int i = 0; i < activeChunks.Count; i++)
        {
            if ((activeChunks[i].x < chunkCoordinates.x - (int)worldSettings.maxChunkViewDistance.x || activeChunks[i].x >= chunkCoordinates.x + (int)worldSettings.maxChunkViewDistance.x) ||
                (activeChunks[i].y < chunkCoordinates.y - (int)worldSettings.maxChunkViewDistance.y || activeChunks[i].y >= chunkCoordinates.y + (int)worldSettings.maxChunkViewDistance.y) ||
                (activeChunks[i].z < chunkCoordinates.z - (int)worldSettings.maxChunkViewDistance.z || activeChunks[i].z >= chunkCoordinates.z + (int)worldSettings.maxChunkViewDistance.z))
            {
                chunks[activeChunks[i].x, activeChunks[i].y, activeChunks[i].z].IsActive = false;
                activeChunks.RemoveAt(i);
                i--;
            }
        }

        for (int i = chunkCoordinates.x - (int)worldSettings.maxChunkViewDistance.x; i < chunkCoordinates.x + (int)worldSettings.maxChunkViewDistance.x; i++)
        {
            for (int j = chunkCoordinates.y - (int)worldSettings.maxChunkViewDistance.y; j < chunkCoordinates.y + (int)worldSettings.maxChunkViewDistance.y; j++)
            {
                for (int k = chunkCoordinates.z - (int)worldSettings.maxChunkViewDistance.z; k < chunkCoordinates.z + (int)worldSettings.maxChunkViewDistance.z; k++)
                {
                    if (IsChunkInWorld(i, j, k))
                    {
                        if (chunks[i, j, k] == null)
                        {
                            GenerateChunkFromChunkCoordinates(i, j, k);
                        }
                        else
                        {
                            chunks[i, j, k].IsActive = true;
                            activeChunks.Add(new ChunkCoordinates(i, j, k));
                        }
                    }
                }
            }
        }

        for (int i = chunkCoordinates.x - (int)worldSettings.maxChunkViewDistance.x; i < chunkCoordinates.x + (int)worldSettings.maxChunkViewDistance.x; i++)
        {
            for (int j = chunkCoordinates.y - (int)worldSettings.maxChunkViewDistance.y; j < chunkCoordinates.y + (int)worldSettings.maxChunkViewDistance.y; j++)
            {
                for (int k = chunkCoordinates.z - (int)worldSettings.maxChunkViewDistance.z; k < chunkCoordinates.z + (int)worldSettings.maxChunkViewDistance.z; k++)
                {
                    if (IsChunkInWorld(i, j, k))
                    {
                        if (chunks[i, j, k].chunkObject == null)
                        {
                            SetupChunkGameObjects(i, j, k);
                        }                       
                    }
                }
            }
        }

        lastPlayerChunkCoordinates = chunkCoordinates;
    }

    public bool IsChunkInWorld(int x, int y, int z)
    {
        if (x >= 0 && x < worldSettings.worldSizeInChunks.x && y >= 0 && y < worldSettings.worldSizeInChunks.y && z >= 0 && z < worldSettings.worldSizeInChunks.z)
        {
            return true;
        }

        return false;
    }

    public bool IsVoxelInWorld(Vector3 worldPosition)
    {
        if (worldPosition.x >= 0 && worldPosition.x < WorldSizeInVoxels.x && worldPosition.y >= 0 && worldPosition.y < WorldSizeInVoxels.y && worldPosition.z >= 0 && worldPosition.z < WorldSizeInVoxels.z)
        {
            return true;
        }

        return false;
    }

    public bool IsWorldVoxelSolid(Vector3 voxelWorldPosition) 
    {
        if (IsVoxelInWorld(voxelWorldPosition))
        {
            var chunkPosition = GetChunkCoordinatesFromWorldPosition(voxelWorldPosition);
            if (chunks[chunkPosition.x, chunkPosition.y, chunkPosition.z] != null)
            {
                var voxelPosition = GetChunkVoxelFromWorldCoordinates(voxelWorldPosition);
                return chunks[chunkPosition.x, chunkPosition.y, chunkPosition.z].GetVoxelType((int)voxelPosition.x, (int)voxelPosition.y, (int)voxelPosition.z).IsSolid;
            }
        }

        return false;
    }

    public Vector3 GetChunkVoxelFromWorldCoordinates(Vector3 worldPosition) 
    {
        int x = (int)worldPosition.x % (int)worldSettings.chunkSettings.chunkSize.x;

        int y = (int)worldPosition.y % (int)worldSettings.chunkSettings.chunkSize.y;

        int z = (int)worldPosition.z % (int)worldSettings.chunkSettings.chunkSize.z;

        return new Vector3(x, y, z);
    }

    public VoxelType DetermineVoxelType(Vector3 positionInVoxels) 
    {
        if (IsVoxelInWorld(positionInVoxels) == false)
        {
            return worldSettings.chunkSettings.voxelTypeCollection.GetVoxelType("Air");
        }

        float randomHeightPercentage = Noise.Get2DPerlin(positionInVoxels, WorldSizeInVoxels, 0, 1);
        int terrainHeight = Mathf.FloorToInt(randomHeightPercentage * WorldSizeInVoxels.y);

        if (positionInVoxels.y < terrainHeight)
        {
            int bedrockThreshold = (terrainHeight / 6);
            int stoneThreshold = (terrainHeight * 3 / 6);
            
            if (positionInVoxels.y <= bedrockThreshold)
            {
                return worldSettings.chunkSettings.voxelTypeCollection.GetVoxelType("Bedrock");
            }
            else if (positionInVoxels.y > bedrockThreshold && positionInVoxels.y <= stoneThreshold)
            {
                return worldSettings.chunkSettings.voxelTypeCollection.GetVoxelType("Stone");

            }
            else
            {
                return worldSettings.chunkSettings.voxelTypeCollection.GetVoxelType("Dirt");
            }           
        }
        else if (positionInVoxels.y == terrainHeight)
        {
            return worldSettings.chunkSettings.voxelTypeCollection.GetVoxelType("Grass");
        }
        else
        {
            return worldSettings.chunkSettings.voxelTypeCollection.GetVoxelType("Air");
        }
    }

    private void GenerateChunkFromChunkCoordinates(int x, int y, int z)
    {
        ChunkCoordinates coordinates = new ChunkCoordinates(x, y, z);

        ChunkData chunk = new ChunkData(this, coordinates);
        
        chunks[x, y, z] = chunk;

        activeChunks.Add(coordinates);
    }

    private void SetupChunkGameObjects(int x, int y, int z) 
    {
        var chunk = chunks[x, y, z];

        var chunkGameObject = new GameObject("Chunk_" + chunk.chunkCoordinates.x + "," + chunk.chunkCoordinates.y + "," + chunk.chunkCoordinates.z);
        chunkGameObject.transform.SetParent(root);
        chunk.chunkObject = chunkGameObject;

        var meshFilter = chunkGameObject.AddComponent<MeshFilter>();
        var meshRenderer = chunkGameObject.AddComponent<MeshRenderer>();

        chunk.SetupMesh(ref meshFilter);

        worldSettings.chunkSettings.voxelMaterial.SetTexture(worldSettings.chunkSettings.textureAtlas.Name, worldSettings.chunkSettings.textureAtlas.Atlas);

        meshRenderer.sharedMaterial = worldSettings.chunkSettings.voxelMaterial;
    }
}
