using UnityEngine;
using System.Collections;

public class GemBehavior : MonoBehaviour {

    public bool isInGaze = false; 
    
	public Vector3 movePos;
	public float speed;
    public GameObject gemVis; 
	// Use this for initialization
	void Start () {
	
		movePos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(movePos != transform.position)
		transform.position = Vector3.MoveTowards(transform.position,movePos,Time.deltaTime*speed);

        if(isInGaze)
        {
            gemVis.transform.Rotate(Vector3.forward, 5);
        }

	}


    public void setIsInGaze(bool isInGaze)
    {
        this.isInGaze = isInGaze; 
    }

	public void moveObjectTo(Vector3 pos)
	{
		movePos = pos; 
	}

	public void finishGem()
	{
		animation.Play("GemEnd");
		StartCoroutine(finishanimaion());
	}

	IEnumerator finishanimaion()
	{

		yield return new WaitForSeconds(2f);
		Destroy(gameObject);
	}
}
