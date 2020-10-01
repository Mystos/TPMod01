using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sphere
{
    public float radius;
    public Vector3 origin;
    public SphereType sphereType = SphereType.fill;
    public float forceField = 10;
    public Sphere()
    {

    }
}

public enum SphereType
{
    fill,
    extrusion
}
