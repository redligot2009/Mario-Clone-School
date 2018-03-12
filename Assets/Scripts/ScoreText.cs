using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = GameManager.score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
