using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	void Update ()
	{

		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}

		else if (axes == RotationAxes.MouseX)
		{
            float rotationY = Input.GetAxis("Mouse X") * sensitivityX;
            if (Input.GetKey(KeyCode.Y))
            {
                Debug.Log("X AXIS");

                Vector3 gazePointInWorldSpace = gazeModel.posGazeRight;
                gazePointInWorldSpace.y = Camera.main.pixelHeight - gazePointInWorldSpace.y;

                Ray ray = Camera.main.ScreenPointToRay(gazePointInWorldSpace);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1500f))
                {
                    Vector3 hitPosition = hit.point;

                    Quaternion rotation = Quaternion.LookRotation(hit.point - transform.position);
                    transform.rotation = rotation;
                    Vector3 eulRot = transform.eulerAngles;
                    eulRot.x = 0;
                    eulRot.z = 0;
                    transform.eulerAngles = eulRot;

                    createPoint(hit.point);

                }

            }
            else
			transform.Rotate(0, rotationY, 0);
		}

		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            Quaternion localRotation = Quaternion.Euler(-rotationY, transform.localEulerAngles.y, 0);
           

            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("keyCode-Y");

                Vector3 gazePointInWorldSpace = gazeModel.posGazeRight;
                gazePointInWorldSpace.y = Camera.main.pixelHeight - gazePointInWorldSpace.y;

                Ray ray = Camera.main.ScreenPointToRay(gazePointInWorldSpace);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1500f))
                {
                    Vector3 hitY = new Vector3(0,hit.point.y,0);
                    Vector3 posY = new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f, 0);

                    Quaternion rotation = Quaternion.LookRotation(hit.point - transform.position);

                    transform.localRotation = Quaternion.Euler(new Vector3(rotation.eulerAngles.x,0f,0f));
                    Debug.Log("EulerAngles_:" + rotation.eulerAngles);

                    createPoint(hit.point);
                    // Bug: Local vs Global!


                    // X MOVEMENT

                }
            }

           // transform.localRotation = localRotation;

            float angle; 
            Vector3 axis = Vector3.zero;
            transform.rotation.ToAngleAxis(out angle, out axis);
            //Debug.Log("AXIS " +angle +" Axis " +axis);
		}
	}

    private void createPoint(Vector3 position)
    {
        GameObject hitVisualisation = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hitVisualisation.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        hitVisualisation.transform.position = position;
    }
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
}