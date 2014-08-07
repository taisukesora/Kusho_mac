using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent( typeof( MeshRenderer ) )]
[RequireComponent( typeof( MeshFilter ) )]

public class SimpleMeshRenderer : MonoBehaviour {
  Mesh mesh;
  MeshFilter meshFilter;
  
  Vector3[] vertices;
  int[] triangles;
  Vector2[] uvs;
  
  int meshlength = 3*10;
  
  // Use this for initialization
  void Start () {
    mesh = new Mesh();
    meshFilter = (MeshFilter)GetComponent("MeshFilter");
  }
  
  // Update is called once per frame
  void Update () {
    mesh.Clear();
    
    vertices = new Vector3[3];
    vertices[0] = Vector3.zero;
    vertices[1] = new Vector3( 0, 0, 1 );
    vertices[2] = new Vector3( 1, 0, 0 );
		
    triangles = new int[3];
    triangles[0] = 0;
    triangles[1] = 1;
    triangles[2] = 2;
    
    uvs = new Vector2[3];
    uvs[0] = Vector2.zero;
    uvs[1] = Vector2.right;
    uvs[2] = Vector2.up;
    
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;
    
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    mesh.Optimize();
    
    meshFilter.sharedMesh = mesh;
    meshFilter.sharedMesh.name = "SimpleMesh";
  }
}