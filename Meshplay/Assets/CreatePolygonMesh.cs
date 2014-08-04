using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePolygonMesh : MonoBehaviour {
  private Mesh mesh;
  // Use this for initialization
  Queue<Vector3> path = new Queue<Vector3>();
  Queue<Vector3> newVertices = new Queue<Vector3>();
  Queue<Vector2> newUV = new Queue<Vector2>();
  Queue<int> newTriangles = new Queue<int>();
  const int sections = 12;
  //vectors
  Vector3[] basevec;

  //
  int count = 0;
  public bool mousePointed = false;

  //
  public GameObject RightHand;
  private Vector3 newpos;
  public float delta_distance;
  public float weight2;


  //timer
  private float timer;
  private float waitingTime=0.01f;
  public bool drawing;

  void Start () {
	basevec = new Vector3[sections];
    mesh = new Mesh();    
    //初期path
    path.Enqueue(new Vector3(0.0f, 0.0f, 0.0f));
    
    //点の設定
	for(int i=0;i<sections;i++){
		basevec[i] = new Vector3(0.0f, Mathf.Cos (2*Mathf.PI*i/sections), Mathf.Sin(2*Mathf.PI*i/sections));
	}
    for(int i=0;i<sections;i++)
      {
		newVertices.Enqueue(basevec[i]);
		newUV.Enqueue(new Vector2(i/sections, 0.0f));
      }

    //Triangles
    /*
    newTriangles.Enqueue(1);newTriangles.Enqueue(2);newTriangles.Enqueue(0);
    newTriangles.Enqueue(2);newTriangles.Enqueue(3);newTriangles.Enqueue(0);
    newTriangles.Enqueue(3);newTriangles.Enqueue(4);newTriangles.Enqueue(0);
    */

    mesh.vertices = newVertices.ToArray();
    mesh.uv = newUV.ToArray();
    mesh.triangles = newTriangles.ToArray();
    
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();

    GetComponent<MeshFilter>().sharedMesh = mesh;
    GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
  }
  
  // Update is called once per frame
  void Update () {
    
    /*
    //点をウニョウニョ動かす、生き物っぽい
    float k = 0.01f;
    Vector3[] tmp = newVertices.ToArray();
    for(int i=0;i<tmp.Length;i++)
      {
	tmp[i].y += k*Mathf.Sin(Time.time+5*i);
	tmp[i].z += k*Mathf.Cos(Time.time+4*i);
      }
    newVertices.Clear();
    newVertices = new Queue<Vector3>(tmp);
	*/
	
    //delta_distance = 0.0f;
    
	drawing = false;
    timer += Time.deltaTime;
    if(timer>waitingTime)
      {
	//アクティブでない場合
	if(!RightHand.activeSelf)
	  {
	    return;
	  }

	//アクティブな場合
	timer = 0;
	drawing = true;
	//Quaternion rot = Quaternion.FromToRotation(RightHand.transform.position - newpos, new Vector3(1.0f, 0.0f, 0.0f));
	Quaternion rot = Quaternion.FromToRotation(new Vector3(1.0f, 0.0f, 0.0f), RightHand.transform.position - newpos);
	delta_distance = Vector3.Distance(newpos, RightHand.transform.position);
    newpos = Vector3.Lerp(newpos,RightHand.transform.position,.5f);
	int pathcount = path.Count-1;
	path.Enqueue(newpos);
    
	//点の設定
	//rot = Quaternion.identity;

	int vertcount = newVertices.Count-1;
	//float weight1 = 1/delta_distance;
	weight2 = (Mathf.Exp (-delta_distance*5)+2*weight2/3.0f);
	Debug.Log("weight ,delta_distance ="+ weight2); 
	for(int i=0;i<sections;i++)
	  {
	    newVertices.Enqueue(rot*(5.0f*weight2/10.0f*basevec[i]) + newpos);
		newUV.Enqueue(new Vector2(i/sections, 1.0f));
	  }

	//UV

	//Triangles
	int tricount = newTriangles.Count-1;
	//Debug.Log("triangle"+tricount);
	if(path.Count<=150)
	  {	  
		int indicesFrom = vertcount - (sections) + 1;
		for(int i = 0; i < sections-1; i++){
			newTriangles.Enqueue(indicesFrom + i);
			newTriangles.Enqueue(indicesFrom + (sections) + i);
			newTriangles.Enqueue(indicesFrom + (i+1));
			
			newTriangles.Enqueue(indicesFrom + (sections) + (i));
			newTriangles.Enqueue(indicesFrom + (sections) + (i+1));
			newTriangles.Enqueue(indicesFrom + (i+1));

		}
		newTriangles.Enqueue(indicesFrom + sections -1);
		newTriangles.Enqueue (indicesFrom + (sections)*2 -1);
		newTriangles.Enqueue(indicesFrom);

		newTriangles.Enqueue(indicesFrom + sections*2 -1);
		newTriangles.Enqueue (indicesFrom + (sections));
		newTriangles.Enqueue(indicesFrom);

	  }
	else
	  {
	    //順にデキュー
	    path.Dequeue();
	    for(int i=0;i<sections;i++)
	      {
		newVertices.Dequeue();
		newUV.Dequeue();
	      }
	  }
			
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			mesh.Clear();
			mesh.vertices = newVertices.ToArray();
			mesh.uv = newUV.ToArray();
			mesh.triangles = newTriangles.ToArray();
			
			
			
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();


      }
    count++;

    
  }
}
