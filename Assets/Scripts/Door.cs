using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("YO");
        SceneManager.LoadScene("Win");
    }
}
