using UnityEngine;
using System.Collections;

public class Camera_operation : MonoBehaviour {
  CreatePolygonMesh script;
  //float x;
  //float k;
  // Use this for initialization
  bool flag_camerarotation = false;
  void Start () {
    script = GameObject.Find("Drawing_l").GetComponent<CreatePolygonMesh>();
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

	if(!script.drawing && flag_camerarotation){
    	transform.RotateAround(new Vector3(0.0f, 10.0f, 0.0f), Vector3.up, 0.5f);
	}
	else{
			//transform.RotateAround(new Vector3(0.0f, 10.0f, 0.0f), Vector3.up, 0.11f);
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

	void OnGUI()
	{
		Rect rect = new Rect(50, 50, 100, 50);
		bool isClicked = GUI.Button(rect, "Reset");
		if (isClicked)
		{
			script.resetMesh();
		}
		
	}

}
