using UnityEngine;
using System.Collections;

public class Explode_Timer : MonoBehaviour {

	public float waitingTime; 

	void Start () {
		StartCoroutine(destroyafterAnimation());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator destroyafterAnimation()
	{
		yield return new WaitForSeconds(waitingTime);
		Destroy(gameObject);
	}
}
