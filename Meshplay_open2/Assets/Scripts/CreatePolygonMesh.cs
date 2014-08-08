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
	//mesh made by dodecagon
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
	Camera_operation script_camera;
	//Right hand opening judge
	bool Rhand_opening;

	void Start () {
		script_cubeman = GameObject.Find ("Cubeman").GetComponent<CubemanController>();
		script_camera = GameObject.Find ("Main Camera").GetComponent<Camera_operation>();

		//正１２角形の中心から頂点へ向かう基本ベクトルの設定
		for(int i=0;i<sections;i++){
			basevec[i] = new Vector3(0.0f, Mathf.Cos (2*Mathf.PI*i/sections), Mathf.Sin(2*Mathf.PI*i/sections));
		}

		//initialize mesh
		Init ();

		/*
		while(true){
			if(Init ()){break;}
		}
		*/
	}

	bool Init(){
		mesh = new Mesh();  
		newpos = RightHand.transform.position;

		//pathに追加
		path.Enqueue(newpos);

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

		//waitingTime経ったら描画
		sum_time += Time.deltaTime;
		if((sum_time > waitingTime) && RightHand.activeSelf && !script_camera.isSpinning)
		{
			newpos = RightHand.transform.position;
			//距離が短すぎる場合
			float delta_distance = Vector3.Distance(pos, newpos);
			if(delta_distance < 0.1f)
			{
				return;
			}

			sum_time = 0;

			//基本ベクトルを回転
			Quaternion rot = Quaternion.FromToRotation(new Vector3(1.0f, 0.0f, 0.0f), newpos - pos);
			pos = newpos;

			//ノイズ要素(noise_factorで決定
			newpos += noise_factor* (new Vector3(0.0f, 0.0f, Mathf.PerlinNoise (RightHand.transform.position.x/4, RightHand.transform.position.y/4)));

			//pathに追加
			path.Enqueue(newpos);

			//vertecesの設定
			int vertcount = newVertices.Count-1;

			//線の太さは指数関数で決定
			float weight = Mathf.Exp (-delta_distance*5);

			for(int i=0;i<sections;i++)
			{
				newVertices.Enqueue(rot*(weight*basevec[i]) + newpos);
				newUV.Enqueue(new Vector2(i/sections, 1.0f));
			}
			
			//UV
			
			//Triangles
			int tricount = newTriangles.Count-1;
			//Debug.Log("triangle"+tricount);
	


			//if(path.Count<=1000)

			{
				//一個前のメッシュのインデックス
				int indicesFrom = vertcount - (sections) + 1;
				//手が閉じている場合
				if(script_cubeman.isRHandopen()==2 || path.Count==2 ){
					//開→閉
					if(Rhand_opening){
						//一個前のメッシュを閉じる
						for(int i=0;i<sections-3;i++){
							newTriangles.Enqueue(indicesFrom);
							newTriangles.Enqueue(indicesFrom + i + 1);
							newTriangles.Enqueue(indicesFrom + i + 2);
						}
						//数合わせ
						for(int i = 0;i<sections*6 - 9*3;i++){
							newTriangles.Enqueue(0);
						}
						Rhand_opening = false;
					}
					else{
						//閉→閉の場合は適当
						for(int i = 0;i<sections*6;i++){
							newTriangles.Enqueue(0);
						}
					}
				}
				else{
					//Debug.Log ("pathcount = "+path.Count);
					//手が開いている場合
					//int indicesFrom = vertcount - (sections) + 1;
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
					Rhand_opening = true;
				}
			}

			/*
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
			*/


			mesh.Clear();
			mesh.vertices = newVertices.ToArray();
			mesh.uv = newUV.ToArray();
			mesh.triangles = newTriangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}
	}

	//initialize mesh
	public void resetMesh()
	{
		mesh.Clear();
		path.Clear();path.TrimExcess();
		newVertices.Clear();newVertices.TrimExcess();
		newUV.Clear();newUV.TrimExcess();
		newTriangles.Clear();newTriangles.TrimExcess();
		Init();
	}

	public Vector3 calc_centerofgravity()
	{
		Vector3 g = Vector3.zero;

		Vector3[] tmp = path.ToArray();
		for(int i=0;i<tmp.Length;i++)
		{
			g += tmp[i];
		}
		g /= tmp.Length;
		//Debug.Log (g);
		return g;
	}

	public int getPathCount(){
		return path.Count;
	}
}
