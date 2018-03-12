using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour {

    public float padding = 0.05f, bounceHeight = 0.2f;
    public bool bounce = false;
    Vector3 oldPos;
    public GameObject coinObject;
    void Start()
    {
        oldPos = transform.position;
    }
    
    bool done = false;
    void Update ()
    {
        if(bounce)
        {
            transform.position = Vector3.Lerp(transform.position, oldPos + Vector3.up * bounceHeight, Time.deltaTime * 10f);
            if(Mathf.Abs(transform.position.y - oldPos.y) >= bounceHeight-0.05f)
            {
                bounce = false;
            }
            if(!done)
            {
                Instantiate(coinObject, transform.position + (Vector3.up*(1.1f)), Quaternion.identity);
                GameManager.score++;
                done = true;
            }
        }
        if(done)
        {
            GetComponent<Animator>().Play("die");
        }
        if(!bounce)
        {
            transform.position = Vector3.Lerp(transform.position, oldPos, Time.deltaTime * 10f);
        }
    }
}
