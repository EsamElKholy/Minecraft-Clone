using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGenerator : MonoBehaviour
{
    public Transform player;

    public WorldSettings worldSettings;

    [HideInInspector]
    public World world;    

    // Start is called before the first frame update
    void Start()
    {
        world = new World(worldSettings, transform);
        world.Init(player, worldSettings.biomeAttributes.solidGroundHeight + worldSettings.biomeAttributes.terrainHeight);
    }

    private void Update()
    {
        world.UpdateWorld(player);

        if (world.chunksToCreate.Count > 0 && world.isCreatingChunks == false)
        {
            world.isCreatingChunks = true;

            StartCoroutine(CreateChunks(world, new List<ChunkCoordinates>(world.chunksToCreate)));
        }

        if (world.chunksToUpdate.Count > 0 && world.isUpdatingChunks == false)
        {
            world.isUpdatingChunks = true;

            StartCoroutine(UpdateChunks(world));
        }
    }

    private IEnumerator CreateChunks(World world, List<ChunkCoordinates> chunksToCreate)
    {
        for (int i = 0; i < chunksToCreate.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < world.activeChunks.Count; j++)
            {
                if (world.activeChunks[j] == chunksToCreate[i])
                {
                    found = true;
                    break;
                }
            }

            if (found == false && world.activeChunks.Count > 0)
            {
                for (int j = 0; j < world.chunksToCreate.Count; j++)
                {
                    if (chunksToCreate[i] == world.chunksToCreate[j])
                    {
                        world.chunksToCreate.RemoveAt(j);
                        break;
                    }
                }

                chunksToCreate.RemoveAt(i);
                i--;
            }
        }

        foreach (var chunkCoord in chunksToCreate)
        {
            world.InitializeChunk(chunkCoord.x, chunkCoord.y, chunkCoord.z);
            yield return null;
        }

        while (chunksToCreate.Count > 0)
        {
            world.SetupChunkGameObjects(chunksToCreate[0].x, chunksToCreate[0].y, chunksToCreate[0].z);

            bool found = false;
            for (int j = 0; j < world.activeChunks.Count; j++)
            {
                if (world.activeChunks[j] == chunksToCreate[0])
                {
                    found = true;
                    break;
                }
            }

            if (found == false && world.activeChunks.Count > 0)
            {
                world.chunks[chunksToCreate[0].x, chunksToCreate[0].y, chunksToCreate[0].z].IsActive = false;
            }

            for (int i = 0; i < world.chunksToCreate.Count; i++)
            {
                if (chunksToCreate[0] == world.chunksToCreate[i])
                {
                    world.chunksToCreate.RemoveAt(i);
                    break;
                }
            }

            chunksToCreate.RemoveAt(0);
            
            yield return new WaitForEndOfFrame();
        }

        world.isCreatingChunks = false;
    }

    private IEnumerator UpdateChunks(World world) 
    {
        while (world.chunksToUpdate.Count > 0)
        {
            world.chunksToUpdate[0].UpdateMesh();
            world.chunksToUpdate.RemoveAt(0);

            yield return new WaitForEndOfFrame();
        }

        world.isUpdatingChunks = false;
    }
}
