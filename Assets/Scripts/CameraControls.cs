using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {
    
    public Transform target, leftBound, rightBound;
    Transform deadzone;
    Vector3 temp;
    public float speed = 10f;
    float screenWidth, screenHeight;
	void Start ()
    {
        deadzone = GameObject.Find("deadzone").GetComponent<Transform>();
        temp = transform.position;
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Screen.width/Screen.height;
	}
	
	void LateUpdate ()
    {
        float lw = leftBound.GetComponent<BoxCollider2D>().bounds.size.x / 2f;
        float rw = rightBound.GetComponent<BoxCollider2D>().bounds.size.x / 2f;
        float xdiff = (target.position.x - transform.position.x);
        if (xdiff >= deadzone.localScale.x / 2f && xdiff >= 0)
        {
            temp.x = target.transform.position.x - deadzone.localScale.x / 2f;
            temp.x = Mathf.Clamp(temp.x, leftBound.position.x + screenWidth + lw, rightBound.position.x - screenWidth - rw);
            transform.position = Vector3.MoveTowards(transform.position, temp, speed * Time.deltaTime);
        }
        else if (xdiff <= -deadzone.localScale.x / 2f && xdiff <= 0)
        {
            temp.x = target.transform.position.x + deadzone.localScale.x / 2f;
            temp.x = Mathf.Clamp(temp.x, leftBound.position.x + screenWidth + lw, rightBound.position.x - screenWidth - rw);
            transform.position = Vector3.MoveTowards(transform.position, temp, speed * Time.deltaTime);
        }
        else
        {
            temp.x = transform.position.x;
            temp.x = Mathf.Clamp(temp.x, leftBound.position.x + screenWidth + lw, rightBound.position.x - screenWidth - rw);
        }
	}
}
