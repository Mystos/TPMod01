using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartReader : MonoBehaviour
{
    public Material mat;
    public Counter count;
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

        count = new Counter(msh);

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

[Serializable]
public class Counter
{

    private class Edge
    {
        private int item1;
        private int item2;

        public Edge(int i1, int i2)
        {
            this.item1 = i1;
            this.item2 = i2;
        }

        public override int GetHashCode()
        {
            return this.item1.GetHashCode() ^ this.item2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Edge);
        }

        public bool Equals(Edge obj)
        {
            return (this.item1 == obj.item1 && this.item2 == obj.item2)
                || (this.item1 == obj.item2 && this.item2 == obj.item1);
        }
    }

    // Mesh
    private Mesh mesh;

    // Edge count
    private Dictionary<Edge, int> edgeCount;

    // Edge count per vertex
    private int[] edgeCountPerVertex;
    private int minEdgeCountVertex;
    private int maxEdgeCountVertex;

    public Counter(Mesh mesh)
    {

        this.mesh = mesh;
        this.edgeCount = new Dictionary<Edge, int>();

        this.edgeCountPerVertex = new int[mesh.vertexCount];
        this.minEdgeCountVertex = int.MaxValue;
        this.maxEdgeCountVertex = int.MinValue;

        BuildLists(this.mesh);

    }

    private void BuildLists(Mesh mesh)
    {

        // Edge count
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Edge edgeA = new Edge(mesh.triangles[i], mesh.triangles[i + 1]);
            Edge edgeB = new Edge(mesh.triangles[i + 1], mesh.triangles[i + 2]);
            Edge edgeC = new Edge(mesh.triangles[i + 2], mesh.triangles[i]);

            AddAndCount(edgeA);
            AddAndCount(edgeB);
            AddAndCount(edgeC);


            this.edgeCountPerVertex[mesh.triangles[i]]++;
            this.edgeCountPerVertex[mesh.triangles[i + 1]]++;
            this.edgeCountPerVertex[mesh.triangles[i + 2]]++;
        }

        //Edge count per vertex
        foreach (int v in this.edgeCountPerVertex)
        {
            this.minEdgeCountVertex = Math.Min(this.minEdgeCountVertex, v);
            this.maxEdgeCountVertex = Math.Max(this.maxEdgeCountVertex, v);
        }

    }

    private void AddAndCount(Edge edge)
    {
        if (!edgeCount.ContainsKey(edge))
        {
            edgeCount.Add(edge, 0);
        }
        edgeCount[edge]++;
    }

    internal int getEdgeCountSharedNoneOrOneFace()
    {
        int count = 0;

        foreach (KeyValuePair<Edge, int> kvp in this.edgeCount)
        {
            if (kvp.Value != 2)
            {
                count++;
            }
        }

        return count;
    }

    internal int getMinEdgeCountPerVertex()
    {
        return this.minEdgeCountVertex;
    }

    internal int getMaxEdgeCountPerVertex()
    {
        return this.maxEdgeCountVertex;
    }

    // Return total edge number
    internal int getEdgeNumber()
    {
        return edgeCount.Count;
    }

}
