using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simplification : MonoBehaviour
{
    // New Mesh Material
    public Material material;

    // Meshs
    private Mesh originalMesh;
    private Mesh simplifiedMesh;

    [SerializeField]
    // Grid Parameter
    private Grid grid;


    // Start is called before the first frame update
    void Start()
    {
        string path = @"D:\bunny.off";
        OFFReader.ReadFile(path, out List<Vector3> listVertex, out List<int> listIndices);
        originalMesh = new Mesh();
        originalMesh.vertices = listVertex.ToArray();
        originalMesh.triangles = listIndices.ToArray();

        gameObject.AddComponent<MeshFilter>().mesh = originalMesh;
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = material;

        grid = new Grid();
    }

    // Update is called once per frame
    void Update()
    {
        if (grid.oldNbCell != grid.nbCell)
        {
            grid.oldNbCell = grid.nbCell;
            simplifiedMesh = new Mesh();
            simplifiedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            Simplify();
        }
    }

    private void Simplify()
    {
        // Setting lower & upper borders
        grid.SetBorders();

        // CreateGrid
        grid.CreateGrid();

        // Add every vertices in their correct cube
        // Also compute average position in cube
        SortVertices();

        // Simplify triangles
        SimplifyTriangles();

        simplifiedMesh.RecalculateNormals();

        gameObject.GetComponent<MeshFilter>().mesh = simplifiedMesh;
    }

    private void SimplifyTriangles()
    {

        List<int> newTriangles = new List<int>();

        for (int i = 0; i < originalMesh.triangles.Length; i += 3)
        {

            int indexA = Getindex(grid.GetIndexesVertex(originalMesh.vertices[originalMesh.triangles[i]]));
            int indexB = Getindex(grid.GetIndexesVertex(originalMesh.vertices[originalMesh.triangles[i + 1]]));
            int indexC = Getindex(grid.GetIndexesVertex(originalMesh.vertices[originalMesh.triangles[i + 2]]));

            if (!IsSameCell(indexA, indexB, indexC))
            {

                newTriangles.Add(indexA);
                newTriangles.Add(indexB);
                newTriangles.Add(indexC);

            }

        }

        simplifiedMesh.triangles = newTriangles.ToArray();

    }

    private bool IsSameCell(int indexA, int indexB, int indexC)
    {

        if (indexA == indexB || indexB == indexC || indexC == indexA)
        {
            return true;
        }

        return false;
    }

    private int Getindex(Vector3 vertex)
    {
        return grid.grid[Mathf.FloorToInt(vertex.x), Mathf.FloorToInt(vertex.y), Mathf.FloorToInt(vertex.z)].indexVertex;
    }

    private void SortVertices()
    {

        // Sort vertices in corresponding cubes
        foreach (Vector3 vertex in originalMesh.vertices)
        {

            Vector3 indexes = grid.GetIndexesVertex(vertex);

            // Add vertex to found CubeGrid
            grid.grid[Mathf.FloorToInt(indexes.x), Mathf.FloorToInt(indexes.y), Mathf.FloorToInt(indexes.z)].Add(vertex);

        }


        // Compute average position in cube containing vertices
        // Also set the vertex index
        // And fill the new vertices list

        List<Vector3> newVertices = new List<Vector3>();
        int index = 0;

        for (int i = 0; i < grid.nbCell; i++)
        {
            for (int j = 0; j < grid.nbCell; j++)
            {
                for (int k = 0; k < grid.nbCell; k++)
                {

                    grid.grid[i, j, k].ComputeAverage();
                    newVertices.Add(grid.grid[i, j, k].average);
                    grid.grid[i, j, k].indexVertex = index;
                    index++;
                }
            }
        }

        simplifiedMesh.vertices = newVertices.ToArray();

    }

    private void DrawCubes(bool withAverage = false)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        float offset = (grid.upperBorder.x - grid.lowerBorder.x) / grid.nbCell;
        cube.transform.localScale = new Vector3(offset, offset, offset);

        for (int i = 0; i < grid.nbCell; i++)
        {
            float coordX = grid.lowerBorder.x + i * offset + (offset / 2);

            for (int j = 0; j < grid.nbCell; j++)
            {
                float coordY = grid.lowerBorder.y + j * offset + (offset / 2);

                for (int k = 0; k < grid.nbCell; k++)
                {
                    float coordZ = grid.lowerBorder.z + k * offset + (offset / 2);

                    if (grid.grid[i, j, k].number > 0)
                    {

                        Vector3 cubeCenter;
                        if (withAverage)
                        {
                            cubeCenter = grid.grid[i, j, k].average;
                        }
                        else
                        {
                            cubeCenter = new Vector3(coordX, coordY, coordZ);
                        }
                        Instantiate(cube, cubeCenter, Quaternion.identity);

                    }
                }
            }
        }

        Destroy(cube);
    }
}
