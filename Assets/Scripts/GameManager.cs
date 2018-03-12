using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static int score = 0;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(transform.gameObject);
	}

    public void BackToGame()
    {
        score = 0;
        SceneManager.LoadScene("Level1");
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}
