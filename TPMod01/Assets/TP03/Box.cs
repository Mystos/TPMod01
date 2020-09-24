using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Box
{
    public Vector3 start;
    public Vector3 end;

    public Box(Vector3 start , Vector3 end)
    {
        this.start = start;
        this.end = end;
    }


    public static Box CreateBoundingBox(List<Sphere> listSphere)
    {
        if(listSphere.Count <= 0)
        {
            return null;
        }
        Vector3 start = listSphere[0].origin;
        Vector3 end = listSphere[0].origin;

        foreach (Sphere sphere in listSphere)
        {
            start = Vector3.Min(sphere.origin - new Vector3(sphere.radius, sphere.radius, sphere.radius), start);
            end = Vector3.Max(sphere.origin + new Vector3(sphere.radius, sphere.radius, sphere.radius), end);
        }

        return new Box(start, end);
    }

    static Box CreateBox(Vector3 start, Vector3 end)
    {
        Vector3 pos = (end - start) / 2;
        Vector3 size = new Vector3(pos.x - start.x, pos.y - start.y, pos.z - start.z);

        return new Box(pos, size);
    }

}
