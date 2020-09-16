using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Material mat;

    public int height = 40;
    public int radius = 5;
    public int meridian = 5;
    public int parallel = 5;
    //public Vector3[] vertices = new Vector3[2 * n + 2];
    public List<Vector3> vertices = new List<Vector3>();
    int[] triangles;
    int k = 0;

    // Start is called before the first frame update
    void Start()
    {
        triangles = new int[5000];

        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;
        DrawSphere();

    }

    // Update is called once per frame
    void Update()
    {
        DrawSphere();
    }

    void DrawSphere()
    {
        vertices.Clear();
        float x, y, z, theta, phi;
        triangles = new int[896547];
        k = 0;

        for (int p = 0; p <= parallel; p++)
        {
            phi = p * (float)Mathf.PI / parallel;

            for (int m = 0; m <= meridian; m++)
            {
                theta = m * 2 * (float)Mathf.PI / meridian;
                x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                z = radius * Mathf.Cos(phi);

                vertices.Add(new Vector3(x, y, z));
            }
        }

        Mesh msh = new Mesh();
        msh.vertices = vertices.ToArray();
        msh.triangles = GenIndices().ToArray();

        gameObject.GetComponent<MeshFilter>().mesh = msh;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        foreach (var point in vertices)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

    private void AddTriangle(int a, int b, int c)
    {
        triangles[k] = a;
        triangles[k + 1] = b;
        triangles[k + 2] = c;
        k = k + 3;
    }

    private List<int> GenIndices()
    {
        List<int> indices = new List<int>();
        int k1, k2;
        for (int i = 0; i < parallel; i++)
        {
            k1 = i * (meridian + 1);
            k2 = k1 + meridian + 1;

            for (int j = 0; j < meridian; j++, k1++, k2++)
            {
                if(i != 0)
                {
                    indices.Add(k1);
                    indices.Add(k2);
                    indices.Add(k1 + 1);
                }

                if(i != parallel - 1)
                {
                    indices.Add(k1 + 1);
                    indices.Add(k2);
                    indices.Add(k2 + 1);
                }
            }
        }

        return indices;
    }
}
