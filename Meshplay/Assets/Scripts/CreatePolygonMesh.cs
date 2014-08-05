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
	private Vector3 pos, newpos;
	public float delta_distance;
	public float weight2;
	public float noise_factor = 1.0f;
	
	//timer
	private float timer;
	private float waitingTime=0.01f;
	public bool drawing;

	
	void Start () {
		basevec = new Vector3[sections];
		mesh = new Mesh();    
		//初期path
		path.Enqueue(RightHand.transform.position);

		//点の設定
		for(int i=0;i<sections;i++){
			basevec[i] = new Vector3(0.0f, Mathf.Cos (2*Mathf.PI*i/sections), Mathf.Sin(2*Mathf.PI*i/sections));
		}

		for(int i=0;i<sections;i++)
		{
			newVertices.Enqueue(path.Peek() + basevec[i]);
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

			newpos = RightHand.transform.position;
			//距離が短すぎる場合
			delta_distance = Vector3.Distance(pos, newpos);
			pos = newpos;
			newpos += noise_factor* (new Vector3(0.0f, 0.0f, Mathf.PerlinNoise (RightHand.transform.position.x, RightHand.transform.position.y)));


			if(delta_distance < 0.1f)
			{
				return;
			}
			//アクティブな場合
			timer = 0;

			if(delta_distance > 0.3f){
				drawing = true;
			}
			
			Quaternion rot = Quaternion.FromToRotation(new Vector3(1.0f, 0.0f, 0.0f), newpos - pos);
			int pathcount = path.Count-1;
			path.Enqueue(newpos);
			
			//点の設定
			int vertcount = newVertices.Count-1;
			weight2 = Mathf.Exp (-delta_distance*5);
			//Debug.Log("weight ,delta_distance ="+ weight2); 
			for(int i=0;i<sections;i++)
			{
				newVertices.Enqueue(rot*(weight2*basevec[i]) + newpos);
				newUV.Enqueue(new Vector2(i/sections, 1.0f));
			}
			
			//UV
			
			//Triangles
			int tricount = newTriangles.Count-1;
			//Debug.Log("triangle"+tricount);
			if(path.Count<=1000)
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
			mesh.Clear();
			mesh.vertices = newVertices.ToArray();
			mesh.uv = newUV.ToArray();
			mesh.triangles = newTriangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}
		count++;
		
		
	}
	
	public void resetMesh()
	{
		mesh.Clear();
		path.Clear();path.TrimExcess();
		newVertices.Clear();newVertices.TrimExcess();
		newUV.Clear();newUV.TrimExcess();
		newTriangles.Clear();newTriangles.TrimExcess();
		
		Start();
	}
	
	
}
