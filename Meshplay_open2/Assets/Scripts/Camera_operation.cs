using UnityEngine;
using System.Collections;

public class Camera_operation : MonoBehaviour {
	CreatePolygonMesh script_mesh;
	CubemanController script_cubeman;
  //float x;
  //float k;
  // Use this for initialization
  	bool flag_camerarotation = false;
	bool hand_opening = false;
	Vector3 hand_pos_past;
	public GameObject lefthand;
	public bool isSpinning = false;
	float sum_time;
	float dir;

	//angular velocity
	public float w = 0.1f;
	//center of the rotation
	private Vector3 center;

  void Start () {
    script_mesh = GameObject.Find("Drawing_l").GetComponent<CreatePolygonMesh>();
	script_cubeman = GameObject.Find ("Cubeman").GetComponent<CubemanController>();
  }
  
  // Update is called once per frame
 	void Update () {
    //x = (x+script.delta_distance)/2.0f;
    //k = (0.3f*Mathf.Exp(-x/2.0f) + k)/2.0f;
    //Debug.Log(k);
    
    /*
    transform.position = new Vector3(Mathf.Sin(Time.time/k), 5.0f/20.0f, Mathf.Cos(Time.time/k));
    transform.position *=20.0f;
	  
    transform.LookAt(Vector3.zero);
    */

		if(isSpinning){
			/*
			//回転中の場合
			if(sum_time<0.5f)
			{
				sum_time += Time.deltaTime;
				transform.RotateAround(new Vector3(0.0f, 10.0f, 0.0f), Vector3.up, dir*2.5f);
			}
			else
			{
				sum_time = 0.0f;
				isSpinning = false;
				hand_opening = false;

			}*/
			//transform.RotateAround(new Vector3(0.0f, 10.0f, 0.0f), Vector3.up, dir*w);
			transform.RotateAround(script_mesh.calc_centerofgravity(), Vector3.up, dir*w);
			gesture();
		}
		else
		{
		
			gesture();
		}
	
	//Space keyでreset
	if (Input.GetKeyDown (KeyCode.Space)) {
		script_mesh.resetMesh();
	}
	//start to rotate
	if(Input.GetKeyDown(KeyCode.Tab)){	
			if(flag_camerarotation){flag_camerarotation = false;}
			else{flag_camerarotation = true;}
	}
  }

	void camera_reset(){
		transform.position = new Vector3(0, 13, 20);
		transform.rotation = Quaternion.Euler(0,180,0);
		isSpinning = false;
		dir = 0.0f;
	}

	void gesture(){
		if(script_cubeman.isLHandopen()==1){
			if(hand_opening){
				Vector3 delta = lefthand.transform.position - hand_pos_past;
				hand_pos_past = lefthand.transform.position;



				float vx = delta.x/Time.deltaTime;
				float vy = delta.y/Time.deltaTime;
				
				//右にスワイプ
				if(vx>40.0f){
					isSpinning = true;
					dir = 1.0f;
					
				}
				//左にスワイプ
				if(vx<-40.0f){
					isSpinning = true;
					dir = -1.0f;
				}

				if(!isSpinning){
					transform.RotateAround(script_mesh.calc_centerofgravity(), Vector3.up, 5*delta.x);
				}

				//下にスワイプ
				if(vy<-40.0f){
					//メッシュリセット
					script_mesh.resetMesh();
					//カメラの位置もリセット
					camera_reset();
				}
				
			}
			else
			{
				hand_opening =true;
				hand_pos_past = lefthand.transform.position;
			}
		}
		else
		{	
			hand_opening = false;
		}
	}
}
