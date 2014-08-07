using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePolygonMesh : MonoBehaviour {

	//variables for mesh
	private Mesh mesh;
	Queue<Vector3> path = new Queue<Vector3>();
	Queue<Vector3> newVertices = new Queue<Vector3>();
	Queue<Vector2> newUV = new Queue<Vector2>();
	Queue<int> newTriangles = new Queue<int>();
	const int sections = 12;
	Vector3[] basevec = new Vector3[sections];

	public GameObject RightHand;
	private Vector3 pos, newpos;

	//perlin noise factor can be set at inspector
	public float noise_factor = 1.0f;
	
	//drawing at a certain interval
	private float sum_time;
	private float waitingTime=0.01f;
	
	CubemanController script_cubeman;
	//Right hand opening judge
	bool Rhand_opening;

	bool first = true;

	void Start () {
		/*
		if(RightHand.activeSelf==false)
		{
			Start ();
			return;
		}
		*/

		script_cubeman = GameObject.Find ("Cubeman").GetComponent<CubemanController>();
		mesh = new Mesh();  

		//正１２角形の中心から頂点へ向かう基本ベクトルの設定
		for(int i=0;i<sections;i++){
			basevec[i] = new Vector3(0.0f, Mathf.Cos (2*Mathf.PI*i/sections), Mathf.Sin(2*Mathf.PI*i/sections));
		}

		Init ();

		/*
		while(true){
			if(Init ()){break;}
		}
		*/
	}

	bool Init(){
		newpos = RightHand.transform.position;
		//距離が短すぎる場合
		float delta_distance = Vector3.Distance(Vector3.zero, newpos);
		//if(delta_distance<0.1f){return false;}
		//初期path
		path.Enqueue(RightHand.transform.position);
		Debug.Log ("newpos = "+newpos);

		for(int i=0;i<sections;i++)
		{
			newVertices.Enqueue(path.Peek() + basevec[i]);
			newUV.Enqueue(new Vector2(i/sections, 0.0f));
		}
		
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
		return true;
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

		sum_time += Time.deltaTime;
		if(sum_time > waitingTime)
		{
			//アクティブでない場合
			if(!RightHand.activeSelf)
			{
				return;
			}
			//if(script_cubeman.isRHandopen()==1){
			newpos = RightHand.transform.position;
			//距離が短すぎる場合
			float delta_distance = Vector3.Distance(pos, newpos);
			Quaternion rot = Quaternion.FromToRotation(new Vector3(1.0f, 0.0f, 0.0f), newpos - pos);
			pos = newpos;

			//ノイズ要素
			newpos += noise_factor* (new Vector3(0.0f, 0.0f, Mathf.PerlinNoise (RightHand.transform.position.x, RightHand.transform.position.y)));


			if(delta_distance < 0.1f)
			{
				return;
			}
			//アクティブな場合
			sum_time = 0;

			//Quaternion rot = Quaternion.FromToRotation(new Vector3(1.0f, 0.0f, 0.0f), newpos - pos);
			int pathcount = path.Count-1;
			path.Enqueue(newpos);
			
			//点の設定
			int vertcount = newVertices.Count-1;
			float weight = Mathf.Exp (-delta_distance*5);
			//Debug.Log("weight ,delta_distance ="+ weight2); 
			for(int i=0;i<sections;i++)
			{
				newVertices.Enqueue(rot*(weight*basevec[i]) + newpos);
				newUV.Enqueue(new Vector2(i/sections, 1.0f));
			}
			
			//UV
			
			//Triangles
			int tricount = newTriangles.Count-1;
			//Debug.Log("triangle"+tricount);
			if(path.Count<=1000)
			{
				//手が閉じている場合
				if(script_cubeman.isRHandopen()==2){
					//2連続で開いていると判定された場合
					if(Rhand_opening){
						for(int i = 0;i<sections*3;i++){
							newTriangles.Enqueue(0);
						}
					}
					else{
						//初めての場合は閉じているときと同じように動作
						Rhand_opening = true;
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
				}
				else{
					//手が開いている場合
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
				Rhand_opening = false;
			}
			}
			else
			{
				//順にデキュー
				path.Dequeue();
				for(int i=0;i<sections;i++)
				{
					newVertices.Dequeue();
					newUV.Dequeue();
					newTriangles.Dequeue();newTriangles.Dequeue();newTriangles.Dequeue();
					newTriangles.Dequeue();newTriangles.Dequeue();newTriangles.Dequeue();
				}
			}
			/*
			if(path.Count==2 && first){
				path.Dequeue();
				for(int i=0;i<sections;i++)
				{
					newVertices.Dequeue();
					newUV.Dequeue();
					//newTriangles.Dequeue();newTriangles.Dequeue();newTriangles.Dequeue();
					//newTriangles.Dequeue();newTriangles.Dequeue();newTriangles.Dequeue();
				}
				Debug.Log("for the first time");
				first = false;
			}*/

			mesh.Clear();
			mesh.vertices = newVertices.ToArray();
			mesh.uv = newUV.ToArray();
			mesh.triangles = newTriangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}
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
