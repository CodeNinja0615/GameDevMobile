using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwipeDetector01 : MonoBehaviour
{
	public bool grounded = false;
	public float jumpHeight = 2.5f;
	private Vector2 fingerDownPos;
	private Vector2 fingerUpPos;
	float UpwardForce = 50f;
	public Rigidbody rb;
	public bool detectSwipeAfterRelease = false;
	static bool gameStarted = false;
	public float SWIPE_THRESHOLD = 20f;
	public float gravity = 20.0f;
	Vector3 defaultScale;
	public float restartDelay = 5f;
	public bool shrink = false;
	void Start()
	{
		defaultScale = transform.localScale;
	}
	float CalculateJumpVerticalSpeed()
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
	// Update is called once per frame
	void Update()
	{

		//if (Input.touchCount > 0)
		//    {
		//	
		//}
		if (rb.position.y > 2.5f)
		{
			rb.AddForce(0, -70 * Time.deltaTime, 0, ForceMode.VelocityChange);
		}
		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				fingerUpPos = touch.position;
				fingerDownPos = touch.position;
			}

			//Detects Swipe while finger is still moving on screen
			if (touch.phase == TouchPhase.Moved)
			{
				if (!detectSwipeAfterRelease)
				{
					fingerDownPos = touch.position;
					DetectSwipe();
				}
			}

			//Detects swipe after finger is released from screen
			if (touch.phase == TouchPhase.Ended)
			{
				fingerDownPos = touch.position;
				DetectSwipe();
			}
		}
	}

	void DetectSwipe()
	{

		if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
		{
			Debug.Log("Vertical Swipe Detected!");
			//if (fingerDownPos.y - fingerUpPos.y > 0)
			//{
			//	OnSwipeUp();
			//}
			if (fingerDownPos.y - fingerUpPos.y < 0)
			{
				shrink = true;
				OnSwipeDown();
			}
			fingerUpPos = fingerDownPos;

		}
		else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
		{
			Debug.Log("Horizontal Swipe Detected!");
			if (fingerDownPos.x - fingerUpPos.x > 0)
			{
				OnSwipeRight();
			}
			else if (fingerDownPos.x - fingerUpPos.x < 0)
			{
				OnSwipeLeft();
			}
			fingerUpPos = fingerDownPos;

		}
		else
		{
			Debug.Log("No Swipe Detected!");
		}
	}

	float VerticalMoveValue()
	{
		return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
	}

	float HorizontalMoveValue()
	{
		return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
	}

	//void OnSwipeUp()
	//{
	//	rb.velocity = new Vector3(rb.velocity.x, CalculateJumpVerticalSpeed(), rb.velocity.z);
	//}


	void OnSwipeDown()
	{
		//detectSwipeAfterRelease = false;

		//Do something when swiped down
		if (shrink == true)
		{
			FindObjectOfType<AudioManager>().Play("Shrink");
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(defaultScale.x, defaultScale.y * 0.2f, defaultScale.z), Time.deltaTime * 7);


		}
		else
		{
			transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, Time.deltaTime * 7);
		}
	}


	void OnSwipeLeft()
	{
		//Do something when swiped left
	}

	void OnSwipeRight()
	{
		//Do something when swiped right
	}

	//public void FixedUpdate()
	//{
	//	
	//}
}
