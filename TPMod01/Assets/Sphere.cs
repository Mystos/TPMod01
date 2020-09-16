using System.Collections;
using System.Collections.Generic;
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

        for (int p = 0; p < parallel; p++)
        {
            phi = p * (float)Mathf.PI / parallel;

            for (int m = 0; m < meridian; m++)
            {
                theta = m * 2 * (float)Mathf.PI / meridian;
                x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                z = radius * Mathf.Cos(phi);

                vertices.Add(new Vector3(x, y, z));
                vertices.Add(new Vector3(x, -y, z));
            }
        }
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
}
