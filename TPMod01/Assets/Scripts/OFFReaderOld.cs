﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class OFFReaderOld
{

    public static void ReadFile(string path, out List<Vector3> listVertex,out List<int> listIndices, out float maxNormal)
    {
        FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //BufferedStream bs = new BufferedStream(fs);
        StreamReader sr = new StreamReader(fs);

        string line;
        int i = 0;
        int nbVertex = 0;
        int nbFace = 0;
        int nbEdge = 0;
        listVertex = new List<Vector3>();
        listIndices = new List<int>();
        maxNormal = 0;
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
                format.NumberDecimalDigits = 6;
                string[] vector = line.Split(' ');
                double x = double.Parse(vector[0], format);
                double y = double.Parse(vector[1], format);
                double z = double.Parse(vector[2], format);
                Vector3 vec = new Vector3((float)x, (float)y, (float)z);
                maxNormal = Mathf.Max(vec.magnitude, maxNormal);

                listVertex.Add(vec);
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
    }

    public static void WriteFile(string savePath, List<Vector3> listVertex, List<int> listIndices, List<int> listEdges)
    {
        var format = new NumberFormatInfo();
        format.NegativeSign = "-";
        format.NumberDecimalSeparator = ".";
        format.NumberDecimalDigits = 10;

        StreamWriter outputFile = new StreamWriter(savePath);
        outputFile.WriteLine("OFF");
        outputFile.WriteLine(listVertex.Count + " " + listIndices.Count / 3 + " " + listEdges.Count);

        foreach (Vector3 vertex in listVertex)
        {
            outputFile.WriteLine(vertex.x.ToString(format) + " " + vertex.y.ToString(format) + " " + vertex.z.ToString(format));
        }

        for (int i = 0; i <= listIndices.Count - 1; i += 2)
        {
            outputFile.WriteLine("3 " + listIndices[i] + " " + listIndices[i+1] + " " + listIndices[i+2]);
        }
    }
}

