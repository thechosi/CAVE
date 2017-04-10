using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(GeneratePlaneMesh))]
public class DeformableMesh : MonoBehaviour
{

    public float maximumDepression;
    public List<Vector3> originalVertices;
    public List<Vector3> modifiedVertices;

    private GeneratePlaneMesh plane;


    public void MeshRegenerated()
    {
        plane = GetComponent<GeneratePlaneMesh>();
        plane.mesh.MarkDynamic();
        originalVertices = plane.mesh.vertices.ToList();
        modifiedVertices = plane.mesh.vertices.ToList();
        Debug.Log("Mesh Regenerated");
    }


    public void AddDepression(Vector3 depressionPoint, float radius)
    {
        //var worldPos4 = this.transform.worldToLocalMatrix * depressionPoint;
        //var worldPos = new Vector3(worldPos4.x, worldPos4.y, worldPos4.z);
        for (int i = 0; i < modifiedVertices.Count; ++i)
        {
            var distance = (depressionPoint - modifiedVertices[i] ).magnitude;
            if (distance < radius)
            {
                var newVert = originalVertices[i] + Vector3.down * maximumDepression;
                modifiedVertices.RemoveAt(i);
                modifiedVertices.Insert(i, newVert);
            }
        }

        plane.mesh.SetVertices(modifiedVertices);
        Debug.Log("Mesh Depressed");
    }
}
