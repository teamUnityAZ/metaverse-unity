using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GetPointOnMesh : MonoBehaviour
{
    Camera cam;
    public bool m_EnableMeshPoint;



    void Start()
    {
        cam = GetComponent<Camera>();


        UnityWebRequest www = new UnityWebRequest("www.facebook.com");

        
    }

    void Update()
    {
        //Mesh tmpmesh = new Mesh();
        //m_SkinMeshRenderer.BakeMesh(tmpmesh);
        //////m_MeshFilter.GetComponent<MeshFilter>().mesh = tmpmesh;
        //Vector3 l_Pos = m_SkinMeshRenderer.transform.TransformPoint(tmpmesh.vertices[121]);
        //m_Object.transform.position = l_Pos;


        if (m_EnableMeshPoint)
            GetPointOnMeshMethod();
    }

    void GetPointOnMeshMethod()
    {
        RaycastHit hit;

        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Mesh mesh = meshCollider.sharedMesh;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        if (Input.GetMouseButtonDown(0))
        {
            print(triangles[hit.triangleIndex * 3 + 0].ToString());
            print(triangles[hit.triangleIndex * 3 + 1].ToString());
            print(triangles[hit.triangleIndex * 3 + 2].ToString());
        }

        Transform hitTransform = hit.collider.transform;

        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);

        Debug.DrawLine(p0, p1);
        Debug.DrawLine(p1, p2);
        Debug.DrawLine(p2, p0);
    }
}
