using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoxelFaces
{
	UP = 0,
	DOWN = 1,
	BACK = 2,
	FRONT = 3,
	LEFT = 4,
	RIGHT = 5
}

public static class VoxelData 
{   
	public static List<Vector3> GetVoxelVertices(ref List<Vector3> vertices, List<bool> adjacents, Vector3 centerPosition, Vector3 size) 
    {
		if (adjacents.Count < 6)
		{
			adjacents = new List<bool>();

			for (int i = 0; i < 6; i++)
			{
				adjacents.Add(true);
			}
		}
		
		Vector3 pos = centerPosition;

        if (adjacents[(int)VoxelFaces.UP])
        {
			// Face: UP             
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z - (size.z / 2.0f)));
        }


		if (adjacents[(int)VoxelFaces.DOWN])
		{
			// Face: DOWN                                                                                      
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z - (size.z / 2.0f)));
		}

		if (adjacents[(int)VoxelFaces.BACK])
		{
			// Face: BACK                                                                                      
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z - (size.z / 2.0f)));
		}

		if (adjacents[(int)VoxelFaces.FRONT])
		{
			// Face: FRONT                                                                                     
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z + (size.z / 2.0f)));
		}

		if (adjacents[(int)VoxelFaces.LEFT])
		{
			// Face: LEFT                                                                                      
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x - (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z + (size.z / 2.0f)));
		}

		if (adjacents[(int)VoxelFaces.RIGHT])
		{
			// Face: RIGHT                                                                                    
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z + (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y - (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z - (size.z / 2.0f)));
			vertices.Add(new Vector3(pos.x + (size.x / 2.0f), pos.y + (size.y / 2.0f), pos.z + (size.z / 2.0f)));
		}

		return vertices;
    }

    public static List<int> GetVoxelIndices(ref List<int> indices, List<bool> adjacents, int previousVerticesCount)
    {
        if (adjacents.Count < 6)
        {
			adjacents = new List<bool>();

            for (int i = 0; i < 6; i++)
            {
				adjacents.Add(true);
            }
        }

		int faceCount = 0;

		int count = previousVerticesCount;

		if (adjacents[(int)VoxelFaces.UP])
		{
			//Up
			indices.Add((0 + (4 * faceCount)) + ((count)));
			indices.Add((1 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));

			indices.Add((0 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((3 + (4 * faceCount)) + ((count)));

			faceCount++;
		}

		if (adjacents[(int)VoxelFaces.DOWN])
		{
			//Down                           
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((1 + (4 * faceCount)) + ((count)));
			indices.Add((0 + (4 * faceCount)) + ((count)));

			indices.Add((3 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((0 + (4 * faceCount)) + ((count)));

			faceCount++;
		}

		if (adjacents[(int)VoxelFaces.BACK])
		{
			//Back                        
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((1 + (4 * faceCount)) + ((count)));
			indices.Add((0 + (4 * faceCount)) + ((count)));

			indices.Add((3 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((0 + (4 * faceCount)) + ((count)));

			faceCount++;
		}

		if (adjacents[(int)VoxelFaces.FRONT])
		{
			//Front                       
			indices.Add((0 + (4 * faceCount)) + ((count)));
			indices.Add((1 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));

			indices.Add((0 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((3 + (4 * faceCount)) + ((count)));

			faceCount++;
		}

		if (adjacents[(int)VoxelFaces.LEFT])
		{
			//Left                          
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((1 + (4 * faceCount)) + ((count)));
			indices.Add((0 + (4 * faceCount)) + ((count)));

			indices.Add((3 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((0 + (4 * faceCount)) + ((count)));

			faceCount++;
		}

		if (adjacents[(int)VoxelFaces.RIGHT])
		{
			//Right                         
			indices.Add((0 + (4 * faceCount)) + ((count)));
			indices.Add((1 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));

			indices.Add((0 + (4 * faceCount)) + ((count)));
			indices.Add((2 + (4 * faceCount)) + ((count)));
			indices.Add((3 + (4 * faceCount)) + ((count)));
		}

		return indices;
    }

    public static List<Vector2> GetUVs(ref List<Vector2> uvs, List<bool> adjacents, VoxelType voxelType, TextureAtlas atlas) 
    {
		if (adjacents.Count < 6)
		{
			adjacents = new List<bool>();

			for (int i = 0; i < 6; i++)
			{
				adjacents.Add(true);
			}
		}		

		if (adjacents[(int)VoxelFaces.UP])
		{
			var textureID = voxelType.GetTextureIDForFace(VoxelFaces.UP);

			var origin = atlas.GetTextureCoordinatesByID(textureID);
			var offset = atlas.GetNormalizedTexcoords();
			
			// Face: UP             
			uvs.Add(new Vector2(origin.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y));
			uvs.Add(new Vector2(origin.x, origin.y));
		}

		if (adjacents[(int)VoxelFaces.DOWN])
		{
			var textureID = voxelType.GetTextureIDForFace(VoxelFaces.DOWN);

			var origin = atlas.GetTextureCoordinatesByID(textureID);
			var offset = atlas.GetNormalizedTexcoords();
			
			// Face: DOWN                                                                                      
			uvs.Add(new Vector2(origin.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y));
			uvs.Add(new Vector2(origin.x, origin.y));
		}

		if (adjacents[(int)VoxelFaces.BACK])
		{
			var textureID = voxelType.GetTextureIDForFace(VoxelFaces.BACK);

			var origin = atlas.GetTextureCoordinatesByID(textureID);
			var offset = atlas.GetNormalizedTexcoords();
			
			// Face: BACK         
			uvs.Add(new Vector2(origin.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x, origin.y + offset.y));
		}

		if (adjacents[(int)VoxelFaces.FRONT])
		{
			var textureID = voxelType.GetTextureIDForFace(VoxelFaces.FRONT);

			var origin = atlas.GetTextureCoordinatesByID(textureID);
			var offset = atlas.GetNormalizedTexcoords();
			
			// Face: FRONT                                                                                     
			uvs.Add(new Vector2(origin.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x, origin.y + offset.y));
		}

		if (adjacents[(int)VoxelFaces.LEFT])
		{
			var textureID = voxelType.GetTextureIDForFace(VoxelFaces.LEFT);

			var origin = atlas.GetTextureCoordinatesByID(textureID);
			var offset = atlas.GetNormalizedTexcoords();
			
			// Face: LEFT                                                                                      
			uvs.Add(new Vector2(origin.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x, origin.y + offset.y));
		}

		if (adjacents[(int)VoxelFaces.RIGHT])
		{
			var textureID = voxelType.GetTextureIDForFace(VoxelFaces.RIGHT);

			var origin = atlas.GetTextureCoordinatesByID(textureID);
			var offset = atlas.GetNormalizedTexcoords();
			
			// Face: RIGHT                                                                                    
			uvs.Add(new Vector2(origin.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y));
			uvs.Add(new Vector2(origin.x + offset.x, origin.y + offset.y));
			uvs.Add(new Vector2(origin.x, origin.y + offset.y));
		}

		return uvs;
    }
}
