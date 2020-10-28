using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Grid
{
    [Range(1, 100)]
    [SerializeField]
    public int nbCell;
    public int oldNbCell = 0;
    


    public CubeGrid[,,] grid;
    public Vector3 lowerBorder = new Vector3(-1, -1, -1);
    public Vector3 upperBorder = new Vector3(1, 1, 1);
    public float offset;

    public Grid()
    {

    }

    internal void CreateGrid()
    {
        grid = new CubeGrid[nbCell, nbCell, nbCell];
        for (int i = 0; i < nbCell; i++)
        {
            for (int j = 0; j < nbCell; j++)
            {
                for (int k = 0; k < nbCell; k++)
                {
                    grid[i, j, k] = new CubeGrid();
                }
            }
        }
    }

    /// <summary>
    /// Attention : Le mesh doit être contenue entre -1 et 1 donc normaliser
    /// </summary>
    internal void SetBorders()
    {   
        lowerBorder = new Vector3(-1, -1, -1);
        upperBorder = new Vector3(1, 1, 1);
        offset = (upperBorder.x - lowerBorder.x) / nbCell;
    }

    internal int Getindex(Vector3 vertex)
    {
        return grid[Mathf.FloorToInt(vertex.x), Mathf.FloorToInt(vertex.y), Mathf.FloorToInt(vertex.z)].indexVertex;
    }

    internal Vector3 GetIndexesVertex(Vector3 vertex)
    {
        // Get CubeGrid index from vertex coordinates
        Vector3 temp = vertex - lowerBorder;
        return (temp / offset);
    }



}

public class CubeGrid
{

    public Vector3 average;
    public int number;
    public int indexVertex;

    public CubeGrid()
    {
        average = Vector3.zero;
        number = 0;
        indexVertex = -1;
    }

    internal void Add(Vector3 vertex)
    {
        average += vertex;
        number++;
    }

    internal void ComputeAverage()
    {
        if (number > 0)
        {
            average /= number;
        }
    }
}
