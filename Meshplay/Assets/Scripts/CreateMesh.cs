using UnityEngine;
using System.Collections;

public class CreateMesh : MonoBehaviour {
  public Vector3[] newVertices;
  public Vector2[] newUV;
  public int[] newTriangles;

  int meshlength = 3*10;
  // Use this for initialization
  void Start () {
    Mesh mesh = new Mesh();
    GetComponent<MeshFilter>().mesh = mesh;
    
    //set
    newVertices = new Vector3[meshlength];
    newTriangles = new int[meshlength];
    newUV = new Vector2[meshlength];

    for(int i=0;i<meshlength/3;i++)
      {
	/*
	newVertices[3*i] = new Vector3(-meshlength/(3*2)+i, Mathf.Sin(Time.time), Mathf.Cos(Time.time));
	newVertices[3*i+1] = new Vector3(-meshlength/(3*2)+i, Mathf.Sin(Time.time+i), Mathf.Cos(Time.time+i));
	newVertices[3*i+2] = new Vector3(-meshlength/(3*2)+i, Mathf.Sin(Time.time+2*i), Mathf.Cos(Time.time+2*i));
	*/

	newVertices[3*i] = new Vector3(-meshlength/(3*2)+i, 0, 0);
	newVertices[3*i+1] = new Vector3(-meshlength/(3*2)+i, 0, 1);
	newVertices[3*i+2] = new Vector3(-meshlength/(3*2)+i, 1, 1);

	newTriangles[3*i] = 3*i;
	newTriangles[3*i+1] = 3*i+1;
	newTriangles[3*i+2] = 3*i+2;
	
	newUV[3*i] = Vector2.zero;
	newUV[3*i+1] = Vector2.up;
	newUV[3*i+2] = Vector2.right;
      }
    
    
    mesh.vertices = newVertices;
    mesh.uv = newUV;
    mesh.triangles = newTriangles;

    mesh.RecalculateNormals();
    mesh.RecalculateBounds();

    //meshFilter.sharedMesh = mesh;
    //meshFilter.sharedMesh.name = "SimpleMesh";
  }
  
  // Update is called once per frame
  void Update () {
    /*
    Mesh mesh = GetComponent<MeshFilter>().mesh;
    Vector3[] vertices = mesh.vertices;
    Vector3[] normals = mesh.normals;
    int i=0;
    while(i<vertices.Length)
      {
	//vertices[i] += normals[i]*Mathf.Sin(Time.time);
	i++;
      }
    mesh.vertices = vertices;
    */

    
  }
}
