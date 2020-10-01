using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReader : MonoBehaviour
{
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        string path = @"D:\max.off";
        string savepath = @"D:\max2.off";

        List<Vector3> listVertex = new List<Vector3>();
        List<int> listIndices = new List<int>();
        List<int> listEdges = new List<int>();
        OFFReader.ReadFile(path, out listVertex, out listIndices);
        OFFReader.WriteFile(savepath, listVertex, listIndices, listEdges);
        

        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;

        Mesh msh = new Mesh();
        msh.vertices = listVertex.ToArray();
        msh.triangles = listIndices.ToArray();

        gameObject.GetComponent<MeshFilter>().mesh = msh;

    }

    public static Vector3 GetGravityCenter(List<Vector3> listVertex)
    {
        Vector3 cg = Vector3.zero;
        foreach(Vector3 vec in listVertex)
        {
            cg += vec; 
        }
        return cg / listVertex.Count;
    }
}
