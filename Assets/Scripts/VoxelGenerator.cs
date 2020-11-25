using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGenerator : MonoBehaviour
{
    public Transform player;

    public WorldSettings worldSettings;

    private World world;    

    // Start is called before the first frame update
    void Start()
    {
        world = new World(worldSettings, transform);
        world.Init(player, 2f);
    }

    private void Update()
    {
        world.UpdateWorld(player);
    }    
}
