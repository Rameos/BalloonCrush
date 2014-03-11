using UnityEngine;
using System.Collections;

public class FadeInMaterial : MonoBehaviour {

    public float maxAlpha; 
	public Material[] gemMaterial;
	private Color[] gemColor; 
	// Use this for initialization
	void Start () {
	
		gemColor = new Color[gemMaterial.Length];
		for(int i=0; i<gemMaterial.Length; i++)
		{
			gemColor[i] = gemMaterial[i].color; 
			
			gemColor[i].a =0; 
			gemMaterial[i].color = gemColor[i];
		}
		 
	}
	
	// Update is called once per frame
	void Update () {
	
		for(int i =0; i<gemMaterial.Length;i++)
		{
			if(gemMaterial[i].color.a<maxAlpha)
			{
				gemColor[i].a+=0.01f;
				gemMaterial[i].color = gemColor[i];
			}
		}
	
	}
}
