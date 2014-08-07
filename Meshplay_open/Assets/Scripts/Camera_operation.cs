using UnityEngine;
using System.Collections;

public class Camera_operation : MonoBehaviour {
	CreatePolygonMesh script;
	CubemanController script_cubeman;
  //float x;
  //float k;
  // Use this for initialization
  	bool flag_camerarotation = false;
	bool hand_opening = false;
	Vector3 hand_pos_past;
	public GameObject lefthand;
	private bool isSpinning = false;
	float sum_time;
	float dir;

  void Start () {
    script = GameObject.Find("Drawing_l").GetComponent<CreatePolygonMesh>();
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

			}
		}
		else
		{
			//回転していない場合の場合
			//isHandopen in cubeman script
			if(script_cubeman.isLHandopen()==1){
				if(hand_opening){
					Vector3 delta = lefthand.transform.position - hand_pos_past;
					hand_pos_past = lefthand.transform.position;
					float vx = delta.x/Time.deltaTime;
					float vy = delta.y/Time.deltaTime;

					//右にスワイプ
					if(vx>25.0f){
						isSpinning = true;
						dir = 1.0f;
					}
					//左にスワイプ
					if(vx<-25.0f){
						isSpinning = true;
						dir = -1.0f;
					}
					//下にスワイプ
					if(vy<-50.0f){
						//メッシュリセット
						script.resetMesh();
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
	
	//Space keyでreset
	if (Input.GetKeyDown (KeyCode.Space)) {
		script.resetMesh();
	}
	//start to rotate
	if(Input.GetKeyDown(KeyCode.Tab)){	
			if(flag_camerarotation){flag_camerarotation = false;}
			else{flag_camerarotation = true;}
	}
  }

	void camera_reset(){
		transform.position = new Vector3(0, 13, -20);
		transform.rotation = Quaternion.identity;
	}
}
