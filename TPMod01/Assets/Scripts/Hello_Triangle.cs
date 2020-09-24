using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_Triangle : MonoBehaviour
{
    [Range(1,10)]
    public int width = 1;
    [Range(1, 10)]
    public int height = 1;

    public Material mat;


    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        DrawMesh();
    }

    private void Update()
    {
        DrawMesh();
    }

    public void DrawMesh()
    {
        Vector3[] vertices = new Vector3[6 * height * width];            // Création des structures de données qui accueilleront sommets et  triangles
        int[] triangles = new int[6 * height * width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                int i = (x + y * width) * 6;
                vertices[i] = new Vector3(x, y, 0);            // Remplissage de la structure sommet 
                vertices[i + 1] = new Vector3(x, y + 1, 0);
                vertices[i + 2] = new Vector3(x + 1, y, 0);
                vertices[i + 3] = new Vector3(x + 1, y + 1, 0);


                triangles[i] = i;                      // Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
                triangles[i + 1] = i + 1;                       // les triangles sont représentés par trois indices (et sont mis bout à bout)
                triangles[i + 2] = i + 2;

                triangles[i + 3] = i + 3;
                triangles[i + 4] = i + 2;
                triangles[i + 5] = i + 1;
            }
        }

        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}