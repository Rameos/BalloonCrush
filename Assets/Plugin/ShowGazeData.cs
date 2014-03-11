using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class ShowGazeData : MonoBehaviour {

    public Material testMaterial; 
    public bool drawImage = false;
    public bool visualisationOfAccuracyWindow = true;
    public Texture2D myTexture;
    public Texture2D gazeCursor;

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width * 0.3f, Screen.height * 0.3f), gazeModel.posRightEye.ToString());

        if (GUI.Button(new Rect(Screen.width * 0.3f, 0f, Screen.width * 0.1f, Screen.height * 0.1f), "Start Calibration"))
            gazeMonoComponent.StartCalibration();

        if (GUI.Button(new Rect(Screen.width * 0.4f, 0f, Screen.width * 0.1f, Screen.height * 0.1f), "Start Validation"))
        {
            if(visualisationOfAccuracyWindow)
            gazeMonoComponent.StartValidation(1);
            else
            gazeMonoComponent.StartValidation(0);
        }
        if (GUI.Button(new Rect(Screen.width * 0.5f, 0f, Screen.width * 0.1f, Screen.height * 0.1f), "Show Eye Image Monitor (Extern)"))
            gazeMonoComponent.ShowEyeImageMonitor();

        if (GUI.Button(new Rect(Screen.width * 0.6f, 0f, Screen.width * 0.1f, Screen.height * 0.1f), "Show Tracking Monitor (Extern)"))
            gazeMonoComponent.ShowTrackingMonitor();

        if(drawImage)
        {

            myTexture = gazeModel.trackingMonitor;
            testMaterial.mainTexture = myTexture;
            GUI.DrawTexture(new Rect(0,0,myTexture.width,myTexture.height),myTexture);

            Vector2 posGaze = gazeModel.posGazeLeft;

            GUI.DrawTexture(new Rect(posGaze.x, posGaze.y,gazeCursor.width,gazeCursor.height), gazeCursor);
        }

    }

    void Update()
    {
        
    }

}
