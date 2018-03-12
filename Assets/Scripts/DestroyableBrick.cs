using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBrick : MonoBehaviour {

    
    IEnumerator DieTransition()
    {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.05f);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

	public void Die()
    {
        StartCoroutine(DieTransition());
    }
}
