using UnityEngine;
using System.Collections;

public class GazeInteraction_AimWithGaze : MonoBehaviour {

    public float maxDistance = 150f;
    public float damping = 0.00600f; 
    public Vector3 midPoint;
    public Texture2D gazeCursor;
	// Use this for initialization
	void Start () {
    
    }
	
	// Update is called once per frame
    void OnGUI()
    {
       // GUI.DrawTexture(new Rect(midPoint.x, midPoint.y, gazeCursor.width, gazeCursor.height), gazeCursor);

    }
    
    void LateUpdate () {
        midPoint = gazeModel.posGazeRight;
       

        
        if (Input.GetKeyDown(KeyCode.R))
        {

                Vector3 gazePointInWorldSpace = gazeModel.posGazeRight;
                gazePointInWorldSpace.y = Camera.main.pixelHeight - gazePointInWorldSpace.y;
            Ray ray = Camera.main.ScreenPointToRay(gazePointInWorldSpace);
            RaycastHit hit; 

            if(Physics.Raycast(ray,out hit,maxDistance))
            {
                GameObject hitVisualisation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                hitVisualisation.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                hitVisualisation.transform.position = hit.point;
            }

        }

	}
}
