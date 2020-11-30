using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScreen : MonoBehaviour
{
    public Text debugText;
    public VoxelGenerator voxelGenerator;

    float frameRate;
    float timer;

    int halfWorldSizeInVoxelsX;
    int halfWorldSizeInVoxelsY;
    int halfWorldSizeInVoxelsZ;

    int halfWorldSizeInChunksX;
    int halfWorldSizeInChunksY;
    int halfWorldSizeInChunksZ;

    bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init() 
    {
        if (voxelGenerator != null && voxelGenerator.world != null)
        {
            halfWorldSizeInVoxelsX = (int)voxelGenerator.world.WorldSizeInVoxels.x / 2;
            halfWorldSizeInVoxelsY = (int)voxelGenerator.world.WorldSizeInVoxels.y / 2;
            halfWorldSizeInVoxelsZ = (int)voxelGenerator.world.WorldSizeInVoxels.z / 2;

            halfWorldSizeInChunksX = (int)voxelGenerator.world.worldSettings.worldSizeInChunks.x / 2;
            halfWorldSizeInChunksY = (int)voxelGenerator.world.worldSettings.worldSizeInChunks.y / 2;
            halfWorldSizeInChunksZ = (int)voxelGenerator.world.worldSettings.worldSizeInChunks.z / 2;

            initialized = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized == false)
        {
            Init();
        }

        if (Input.GetKeyUp(KeyCode.F3))
        {
            if (debugText != null)
            {
                debugText.gameObject.SetActive(!debugText.gameObject.activeSelf);
            }
        }

        if (debugText != null && debugText.gameObject.activeSelf && initialized)
        {            
            string text = frameRate + " fps";

            if (voxelGenerator != null)
            {
                text += "\n\n";
                text += "XYZ: " + (Mathf.FloorToInt(voxelGenerator.player.transform.position.x) - halfWorldSizeInVoxelsX) + " / " + 
                    (Mathf.FloorToInt(voxelGenerator.player.transform.position.y) - halfWorldSizeInVoxelsY) + " / " +
                    (Mathf.FloorToInt(voxelGenerator.player.transform.position.z) - halfWorldSizeInVoxelsZ);
                text += "\n";
                text += "Chunk: " + (voxelGenerator.world.currentPlayerChunkCoordinates.x - halfWorldSizeInChunksX) + " / " + 
                    (voxelGenerator.world.currentPlayerChunkCoordinates.y - halfWorldSizeInChunksY) + " / " +
                    (voxelGenerator.world.currentPlayerChunkCoordinates.z - halfWorldSizeInChunksZ);
            }

            debugText.text = text;
        }

        if (timer > 1f)
        {
            frameRate = (int)(1f / Time.unscaledDeltaTime);
            timer = 0;
        }
        else
        { 
            timer += Time.deltaTime;
        }
    }
}
