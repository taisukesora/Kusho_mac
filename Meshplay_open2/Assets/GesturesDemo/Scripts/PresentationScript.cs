using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PresentationScript : MonoBehaviour 
{
	public float spinSpeed = 5;
	
	public bool autoChangeAlfterDelay = false;
	public float slideChangeAfterDelay = 10;

	private bool isSpinning = false;
	private float slideWaitUntil;
	private Quaternion targetRotation;
	
	private GestureListener gestureListener;
	public GameObject ArrowRight;
	public GameObject ArrowLeft;

	
	void Start() 
	{
		// hide mouse cursor
		Screen.showCursor = false;

		// delay the first slide
		slideWaitUntil = Time.realtimeSinceStartup + slideChangeAfterDelay;
		
		targetRotation = transform.rotation;
		isSpinning = false;
		
		// get the gestures listener
		gestureListener = Camera.main.GetComponent<GestureListener>();
	}
	
	void Update() 
	{
		// dont run Update() if there is no user
		KinectManager kinectManager = KinectManager.Instance;
		if(autoChangeAlfterDelay && (!kinectManager || !kinectManager.IsInitialized() || !kinectManager.IsUserDetected()))
			return;

		/*
		if(!isSpinning)
		{
			if(gestureListener)
			{
				if(gestureListener.IsSwipeLeft())
				{
					RotateToLeft();
					ArrowLeft.SetActive (true);
				}
				else if(gestureListener.IsSwipeRight())
				{
					RotateToRight();
					ArrowRight.SetActive(true);
				}
			}
			
			// check for automatic slide-change after a given delay time
			if(autoChangeAlfterDelay && Time.realtimeSinceStartup >= slideWaitUntil)
			{
				//RotateToRight();
			}
		}
		else
		{
			// spin the presentation
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, spinSpeed * Time.deltaTime);
			
			// check if transform reaches the target rotation. If yes - stop spinning
			float deltaTargetX = Mathf.Abs(targetRotation.eulerAngles.x - transform.rotation.eulerAngles.x);
			float deltaTargetY = Mathf.Abs(targetRotation.eulerAngles.y - transform.rotation.eulerAngles.y);
			
			if(deltaTargetX < 1f && deltaTargetY < 1f)
			{
				// delay the slide
				slideWaitUntil = Time.realtimeSinceStartup + slideChangeAfterDelay;
				isSpinning = false;
				ArrowLeft.SetActive(false);
				ArrowRight.SetActive(false);
			}
		}
		*/
	}
	

	private void RotateToRight()
	{
		// rotate the presentation
		float yawRotation = 90;
		Vector3 rotateDegrees = new Vector3(0f, yawRotation, 0f);
		targetRotation *= Quaternion.Euler(rotateDegrees);
		isSpinning = true;
	}
	
	
	private void RotateToLeft()
	{
		// rotate the presentation
		float yawRotation = -90;
		Vector3 rotateDegrees = new Vector3(0f, yawRotation, 0f);
		targetRotation *= Quaternion.Euler(rotateDegrees);
		isSpinning = true;
	}
	
	
}
