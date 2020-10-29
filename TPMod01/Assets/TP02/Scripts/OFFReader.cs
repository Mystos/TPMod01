using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class OFFReader
{
    public static void ReadFile(string path, out List<Vector3> listVertex,out List<int> listIndices)
    {
        FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //BufferedStream bs = new BufferedStream(fs);
        StreamReader sr = new StreamReader(fs);

        string line;
        int i = 0;
        int nbVertex = 0;
        int nbFace = 0;
        int nbEdge = 0;
        Vector3 gravityCenterPoint = Vector3.zero;
        float squaredMagnitudePoint = 0;
        listVertex = new List<Vector3>();
        listIndices = new List<int>();
        while ((line = sr.ReadLine()) != null)
        {

            if (i == 0 && line == "OFF")
            {
                // Erreur de format de fichier
                i++;
                continue;
            }else if (i == 1)
            {
                // Initialisation des variables
                string[] variable = line.Split(' ');
                nbVertex = Int32.Parse(variable[0]);
                nbFace = Int32.Parse(variable[1]);
                nbEdge = Int32.Parse(variable[2]);
            } else if (i > 1 && i <= nbVertex + 1)
            {
                var format = new NumberFormatInfo();
                format.NegativeSign = "-";
                format.NumberDecimalSeparator = ".";
                format.NumberDecimalDigits = 18;
                string[] vector = line.Split(' ');
                Vector3 newVertex = new Vector3();
                newVertex.x = (float)double.Parse(vector[0], format);
                newVertex.y = (float)double.Parse(vector[1], format);
                newVertex.z = (float)double.Parse(vector[2], format);
                listVertex.Add(newVertex);
                gravityCenterPoint += newVertex;
                // Getting the furthest point
                if (newVertex.sqrMagnitude > squaredMagnitudePoint)
                {
                    squaredMagnitudePoint = newVertex.sqrMagnitude;
                }
            }
            else if (i > nbVertex + 1 && i <= nbVertex + nbFace + 1)
            {
                string[] face = line.Split(' ');
                listIndices.Add(Int32.Parse(face[1]));
                listIndices.Add(Int32.Parse(face[2]));
                listIndices.Add(Int32.Parse(face[3]));
            } else if (i < nbFace)
            {
                // A implementer
            }
            i++;
        }

        // Getting real coordinates of gravity center point
        gravityCenterPoint /= nbVertex;

        // Centering the mesh around Vector3.zero
        centeringMesh(ref listVertex, ref gravityCenterPoint);

        //Normlaizing mesh in range [-1;1]
        normalizeMesh(ref listVertex, ref squaredMagnitudePoint);
    }

    public static void WriteFile(string savePath, List<Vector3> listVertex, List<int> listIndices, List<int> listEdges)
    {
        var format = new NumberFormatInfo();
        format.NegativeSign = "-";
        format.NumberDecimalSeparator = ".";
        format.NumberDecimalDigits = 18;

        FileStream fs = File.Open(savePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8);
        outputFile.WriteLine("OFF");
        outputFile.WriteLine(listVertex.Count + " " + listIndices.Count / 3 + " " + listEdges.Count);

        foreach (Vector3 vertex in listVertex)
        {
            outputFile.WriteLine(vertex.x.ToString(format) + " " + vertex.y.ToString(format) + " " + vertex.z.ToString(format));
        }

        for (int i = 0; i < listIndices.Count; i += 3)
        {
            outputFile.WriteLine("3 " + listIndices[i] + " " + listIndices[i+1] + " " + listIndices[i+2]);
        }

        outputFile.Close();

    }

    // Normalizing mesh
    static private void normalizeMesh(ref List<Vector3> vertices, ref float squaredMagnitudePoint)
    {

        // Normalizing with real magnitude
        // 1 sqrt is better than vertices.Lenght sqrt !
        float realMagnitude = Mathf.Sqrt(squaredMagnitudePoint);

        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] /= realMagnitude;
        }

    }

    static private void centeringMesh(ref List<Vector3> vertices, ref Vector3 gravityCenterPoint)
    {
        // Gravity point is computed in ReadOFF : it avoids reading every point each time

        // Get translation vector
        Vector3 translate = Vector3.zero - gravityCenterPoint;

        // Translate every vertex
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] += translate;
        }

    }
}

