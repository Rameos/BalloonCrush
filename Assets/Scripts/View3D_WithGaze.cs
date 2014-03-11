using UnityEngine;
using System.Collections;

public class View3D_WithGaze : MonoBehaviour {

	public int xPosOffset = 5;

	public int HeadPositionStart =0; 
    public float filter = 0.05f; 
    public bool enableGaze = false; 
    public bool enableMouseDemo = false;
    public GameObject focusPoint;

    public float sensivity = 15f;

    public float minimumX =-4.5f;
    public float maximumX = 4.5f;

  //  public float minimumY =-60F;
  //  public float maximumY = 60F;

    private float rotationX = 0;

    private float differenceLastFrame; 

    private Vector3 positionCamera;

    private Vector3 startheadPosition;
    private Vector3 headPos = Vector3.zero;
    private Vector3 posRightEye = Vector3.zero;
    private Vector3 posLeftEye = Vector3.zero;
	

    void Start () {

        startheadPosition = transform.position;
        focusPoint.renderer.enabled = false;
        transform.LookAt(focusPoint.transform.position);
        startheadPosition = Vector3.zero;
	}
	
	void Update () {
        transform.LookAt(focusPoint.transform.position);

        if(enableGaze)
            {

                

            #region checkGazedata 

            //Both Eyes detected
            if(gazeModel.diameter_leftEye>0 && gazeModel.diameter_rightEye>0)
            {
                posRightEye = gazeModel.posRightEye;
                posLeftEye = gazeModel.posLeftEye;

            }
            
            //One eye is detected
            else if((gazeModel.diameter_rightEye>0&&gazeModel.diameter_leftEye<=0)||gazeModel.diameter_rightEye<=0&&gazeModel.diameter_leftEye>0)
            {
                //overwrite the zeroVector with a valid Data: RIGHT
                if(gazeModel.diameter_rightEye>0)
                {
                    posRightEye = gazeModel.posRightEye;
                    posRightEye = gazeModel.posRightEye;
                }

                //overwrite the zeroVector with a valid Data: LEFT
                else if(gazeModel.diameter_leftEye>0)
                {
                    posLeftEye = gazeModel.posLeftEye;
                    posRightEye = gazeModel.posLeftEye;
                }

            }
                // No Eyes detected
            else
            {
                Debug.LogWarning("No Eyes");
            }


                
                headPos = (posRightEye + posLeftEye) * 0.5f;
            #endregion

            if (headPos != null)
            {

				float differenceX = (Mathf.RoundToInt(headPos.x - HeadPositionStart)*0.5f)*0.1f;

				
                /*  //Calculate Difference Between Old& New Position
                float differenceX = startheadPosition.x - gazeModel.posHead.x;
                float differenceY = startheadPosition.y - gazeModel.posHead.y;

                Debug.Log("PosHead_" + startheadPosition);
                Debug.Log("Difference = " +differenceX);
                
                if(Mathf.Abs(differenceX)>filter)
                    rotationX -= differenceX * sensivity;

                if (Mathf.Abs(differenceY) > filter)
                    rotationY -= differenceY * sensivity;

                //Save the Old Data; 
                startheadPosition = gazeModel.posHead;
                */
                if(Mathf.Abs(differenceX-differenceLastFrame)>filter)
                {
                    rotationX = differenceX + xPosOffset;
                    differenceLastFrame = differenceX;
                }
				
            }
            else
            {
                //transform.position = startPosition;
            }
        }

        if(enableMouseDemo)
        {
            rotationX += (Input.GetAxis("Mouse X") * sensivity);
        }

        Vector3 posObject = gameObject.transform.position;
        	   
		posObject.x = rotationX;

	    if(posObject.x>minimumX&&posObject.x<=maximumX)
        gameObject.transform.position = posObject;

	}


    private float clampPos(float pos, float min, float max)
    {
        if (pos < min)
            pos = min; 
        
        if (pos> max)
            pos = max;

        return Mathf.Clamp(pos, min, max); 
    }
}
