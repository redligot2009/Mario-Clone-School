using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfterSeconds : MonoBehaviour {

    public float deathTime = 0.5f;

    IEnumerator Die()
    {
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
	// Use this for initialization
	void Start () {
        StartCoroutine(Die());
	}
	
	// Update is called once per frame
	void Update () {

    }
}
