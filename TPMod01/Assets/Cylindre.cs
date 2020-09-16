using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylindre : MonoBehaviour
{
    public float radius = 1f;
    public int meridian = 6;
    public float cylHeight = 5;

    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();

        DrawMesh();
    }

    // Update is called once per frame
    void Update()
    {
        DrawMesh();
    }

    public void DrawMesh()
    {
        Vector3[] vertices = new Vector3[4 * meridian];            // Création des structures de données qui accueilleront sommets et  triangles
        int[] triangles = new int[6 * meridian];


        for (int m = 0; m < meridian; m++)
        {

            float teta = (2* Mathf.PI / meridian * m ); 

            vertices[m] = new Vector3(radius*Mathf.Cos(teta), radius * Mathf.Sin(teta), -cylHeight / 2);            // Remplissage de la structure sommet 
            vertices[m + meridian] = new Vector3(radius * Mathf.Cos(teta), radius * Mathf.Sin(teta), cylHeight/2);

            int nm = (m < meridian - 1) ? m + 1 : 0;
            int i = m * meridian;
            triangles[i] = m;                      // Remplissage de la structure triangle. Les sommets sont représentés par leurs indices
            triangles[i + 1] = m + meridian;                       // les triangles sont représentés par trois indices (et sont mis bout à bout)
            triangles[i + 2] = nm;

            triangles[i + 3] = m + meridian;
            triangles[i + 4] = m + 1 + meridian;
            triangles[i + 5] = nm;
        }
       

        Mesh msh = new Mesh();                          // Création et remplissage du Mesh

        msh.vertices = vertices;
        msh.triangles = triangles;

        gameObject.GetComponent<MeshFilter>().mesh = msh;           // Remplissage du Mesh et ajout du matériel
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}
