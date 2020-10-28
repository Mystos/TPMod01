using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class CreateVolumeSphere : MonoBehaviour
{
    [Range(0.25f, 2f)]
    public float step;
    int nbCubeX;
    int nbCubeY;
    int nbCubeZ;
    public List<Sphere> listSphere = new List<Sphere>();
    public Box box;
    public bool drawCube = false;
    [Range(0f, 100f)]
    public float materialTreshold = 0;


    private void OnDrawGizmos()
    {
        box = Box.CreateBoundingBox(listSphere);
        Gizmos.color = Color.white;
        foreach (Sphere sphere in listSphere)
        {
            if(sphere.sphereType == SphereType.fill)
            {
                Gizmos.DrawSphere(sphere.origin, sphere.radius);
            }
            else
            {
                Gizmos.DrawWireSphere(sphere.origin, sphere.radius);
            }

            Gizmos.DrawWireSphere(sphere.origin, sphere.forceField);

        }
        Vector3 start = box.start;
        Vector3 end = box.end;
        Vector3 diag = end - start;



        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireCube(start + (end - start) / 2f, diag);

        nbCubeX = Mathf.CeilToInt((box.end.x - box.start.x) / step);
        nbCubeY = Mathf.CeilToInt((box.end.y - box.start.y) / step);
        nbCubeZ = Mathf.CeilToInt((box.end.z - box.start.z) / step);

        for (int k = 0; k < nbCubeZ; k++)
        {
            float startZ = box.start.z + k * step;

            for (int j = 0; j < nbCubeY; j++)
            {
                float startY = box.start.y + j * step;

                for (int i = 0; i < nbCubeX; i++)
                {
                    float potential = 0;
                    float startX = box.start.x + i * step;
                    int presence = 0;
                    Gizmos.color = Color.red;
                    foreach (Sphere sphere in listSphere)
                    {
                        if (CheckPresency(new Vector3(startX + step / 2, startY + step / 2, startZ + step / 2), sphere))
                        {
                            if (sphere.sphereType == SphereType.fill)
                            {
                                presence++;
                            }
                            else if (sphere.sphereType == SphereType.extrusion)
                            {
                                presence = 0;
                                break;
                            }

                        }
                        else
                        {
                            float distance = (new Vector3(startX + step / 2, startY + step / 2, startZ + step / 2) - sphere.origin).magnitude;
                            potential += Mathf.Clamp(sphere.forceField / distance, 0, 1000);
                        }

                    }

                    if (presence > 0 || potential > materialTreshold)
                    {
                        Gizmos.DrawCube(new Vector3(startX + step / 2, startY + step / 2, startZ + step / 2), new Vector3(step, step, step));
                    }
                }
            }
        }

        if (drawCube)
        {
            for (float z = box.start.z; z < box.end.z; z += step)
            {
                for (float y = box.start.y; y < box.end.y; y += step)
                {
                    for (float x = box.start.x; x < box.end.x; x += step)
                    {
                        Gizmos.DrawWireCube(new Vector3(x, y, z), new Vector3(step, step, step));
                    }
                }
            }
        }
    }



    public bool CheckPresency(Vector3 point, Sphere sphere)
    {
        Vector3 v = point - sphere.origin;
        float discr = v.x * v.x + v.y * v.y + v.z * v.z - sphere.radius * sphere.radius;
        if (discr <= 0) return true;
        else return false;
    }


}
