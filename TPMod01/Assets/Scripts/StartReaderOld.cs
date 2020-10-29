using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReaderOld : MonoBehaviour
{
    public Material mat;
    string path = @"D:\bunny.off";
    string savepath = @"D:\bunny2.off";

    List<Vector3> listVertex = new List<Vector3>();
    List<int> listIndices = new List<int>();
    List<int> listEdges = new List<int>();

    // Start is called before the first frame update
    void Start()
    {

        OFFReaderOld.ReadFile(path, out listVertex, out listIndices, out float maxNormal);
        listVertex = GetNormalizeValue(listVertex, maxNormal);
        //OFFReader.WriteFile(savepath, listVertex, listIndices, listEdges);



        gameObject.AddComponent<MeshFilter>();          // Creation d'un composant MeshFilter qui peut ensuite être visualisé
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = mat;


        Mesh msh = new Mesh();
        msh.vertices = listVertex.ToArray();
        msh.triangles = listIndices.ToArray();

        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.transform.position = GetGravityCenter(listVertex);


    }

    public static Vector3 GetGravityCenter(List<Vector3> listVertex)
    {
        Vector3 gc = Vector3.zero;
        foreach(Vector3 vec in listVertex)
        {
            gc += vec; 
        }
        return gc / listVertex.Count;
    }

    public static List<Vector3> GetNormalizeValue(List<Vector3> listVertex, float maxNormal)
    {
        List<Vector3> listVecResult = new List<Vector3>();
        foreach(Vector3 vec in listVertex)
        {
            listVecResult.Add(vec / Mathf.Abs(maxNormal));
        }

        return listVecResult;
    }

    private void OnDrawGizmos()
    {
        Box boundingBox = Box.CreateBoundingBox(listVertex);

        Vector3 pos = (boundingBox.end - boundingBox.start) / 2;
        Vector3 size = new Vector3(pos.x - boundingBox.start.x, pos.y - boundingBox.start.y, pos.z - boundingBox.start.z);
        Gizmos.DrawWireCube(pos, size);
    }

}
