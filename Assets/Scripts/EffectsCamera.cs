using UnityEngine;
using System.Collections;

public class EffectsCamera : MonoBehaviour {

	Vector3 originPosition; 
	Quaternion originRotation; 

	float shakeFactorRotation = 0.2f; 
	public float shakeDecay; 
	public float shakeIntensity;

	public GameObject hightScoreSound; 
	public GameObject explodeSound;
    public GameObject bingSound; 
	public GameObject FireWork_Small;

	void Start()
	{
	}

	void Update()
	{
		if(shakeIntensity>0)
		{
			transform.position = originPosition +Random.insideUnitSphere*shakeIntensity;

			transform.rotation = new Quaternion
				( originRotation.x +Random.Range(-shakeIntensity,shakeIntensity)*shakeFactorRotation,
				  originRotation.y +Random.Range(-shakeIntensity,shakeIntensity)*shakeFactorRotation,
				  originRotation.z +Random.Range(-shakeIntensity,shakeIntensity)*shakeFactorRotation,
				  originRotation.w +Random.Range(-shakeIntensity,shakeIntensity)*shakeFactorRotation
				 );

			shakeIntensity -= shakeDecay;
		}
	}

	public void createExplosion(Vector3 position,int color)
	{
		Debug.Log ("CreateExplosion" +position);
		GameObject explode  = (GameObject)Instantiate(FireWork_Small,position, FireWork_Small.transform.rotation);

	}

	public void playScoreSound()
	{
		hightScoreSound.audio.Play();
	}

	public void playExplodeSound()
	{
		explodeSound.audio.Play();
	}

    public void playBingSound()
    {
        bingSound.audio.Play();
    }

	public void shakeCam()
	{
		originPosition = transform.position; 
		originRotation = transform.rotation;

		shakeIntensity = 0.1f;
		shakeDecay = 0.01f;

	}
}
