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
        //world.UpdateWorld(player);
    }    
}
