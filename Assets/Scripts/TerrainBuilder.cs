using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBuilder : MonoBehaviour
{
    [SerializeField] private Transform tileHolder;
    [SerializeField] private GameObject tilePrefab;

    public int Width {get; private set; }
    public int Height {get; private set; }


    private GameObject[,] terrainGrid;

    // Start is called before the first frame update
    void Start()
    {
        Width = 25;
        Height = 20;

        // set up grid
        terrainGrid = new GameObject[Width, Height];
        // fill in with a tile
        for (int countX = 0; countX < Width - 1; countX++)
        {
            for (int countY = 0; countY < Height - 1; countY++)
            {
                terrainGrid[countX, countY] = tilePrefab;
                float posX = countX * 2;
                float posY = countY * 2;
                GameObject newTile = Instantiate(tilePrefab, new Vector3(posX, posY, 0), Quaternion.identity, tileHolder);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
