using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {

    public bool isIngame = true; 
	public bool emulateGaze = false;

    private bool isGazeCursorActive = false;
    private bool isGameMenuActive = false;
    private bool isTimeCounting = false;
    private bool is3DViewActive = true; 
    public float gemSize;  

    private int addScore = 0;
    private int seconds = 0;
    private int minutes = 0; 
    public int score = 0;
    public int min_gems; 
    public int gridSize;
    public float distance;
    public GameObject [] gems = new GameObject[4];

    public GameObject[,] gemMap;
    public Texture gazeCursor;
	public EffectsCamera camEffects; 
	
    
    public GUIStyle style;
    public GUIStyle standardFont;
    public GUIStyle tutorialBackgroundScreen;
    public GUIStyle standardFontTutorials;
    
    public GUIStyle button;

    public Texture interactionScreen;
    public Texture moneyScreen;


    string textGazeCursor = "(H): Show GazeCursor";
    string textTimerCursor = "(S): Start Timer";
    string text3DButton = "(W) disable 3DView";

    List<GameObject> gemsCollected = new List<GameObject>();
    Vector3 gazePositionRight;
    Vector3 gazePositionLeft;

    Vector3 gazeRayVector;

    Vector3 cameraStartPosition;

    private GameObject oldOne; 

    public int posLeftFonts;

    void OnGUI()
    {

        #region pauseScreen
        //PauseScreen
        if(!isIngame)
        {

            GUI.BeginGroup(new Rect(Screen.width * 0.25f, Screen.width * 0.02f, Screen.width * 0.5f, Screen.height), "HowToPlay", tutorialBackgroundScreen);


            GUI.Label(new Rect(posLeftFonts,125, Screen.width * 0.5f, Screen.width * 0.5f), "Shoot collections of gems with the same colors.", standardFontTutorials);
            GUI.Label(new Rect(posLeftFonts, 150, Screen.width * 0.5f, Screen.width * 0.5f), "Aim with your gaze. Shot with the Spacebar.", standardFontTutorials);

            GUI.DrawTexture(new Rect(posLeftFonts, 200, Screen.width * 0.25f, Screen.width * 0.15f),interactionScreen);
            


            GUI.Label(new Rect(posLeftFonts, 450, Screen.width * 0.5f, Screen.width * 0.5f), "Only collections with more than three Gems will score.", standardFontTutorials);
            GUI.Label(new Rect(posLeftFonts, 475, Screen.width * 0.5f, Screen.width * 0.5f), "Earn more Score with bigger collections.", standardFontTutorials);

            GUI.DrawTexture(new Rect(posLeftFonts, 525, Screen.width * 0.25f, Screen.width * 0.15f), moneyScreen);
            if (GUI.Button(new Rect(Screen.width * 0.175f, Screen.height * 0.85f, Screen.width * 0.15f, Screen.width * 0.05f), "Start Game: Hit Spacebar", button))
            {
                isIngame = true; 
            }

            GUI.EndGroup();
        }
        #endregion

        #region ingame
        //ingameScreen
        else
        {
            GUI.BeginGroup(new Rect(Screen.width * 0.01f, Screen.width * 0.02f, Screen.width * 0.2f, Screen.height * 0.5f), "", style);
            GUI.Label(new Rect(10, 10, 1024, Screen.height), "Score: " + score, standardFont);
            
            //TIMER
            if(seconds<=9)
            GUI.Label(new Rect(10, 40, Screen.width * 0.1f, Screen.height * 0.1f), "Time: " +minutes +":0" + seconds, standardFont);
            
            else
                GUI.Label(new Rect(10, 40, Screen.width * 0.1f, Screen.height * 0.1f), "Time: " + minutes + ":" + seconds, standardFont);
            
            GUI.EndGroup();




            #region GameMenu
            if (isGameMenuActive)
            {
                GUI.BeginGroup(new Rect(Screen.width * 0.01f, Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.3f), "GameMenu", style);

                if (GUI.Button(new Rect(10, 60, Screen.width * 0.1f, Screen.width * 0.03f), "(C): Start Calibration", button))
                    gazeMonoComponent.StartCalibration();

                if (GUI.Button(new Rect(10, 110, Screen.width * 0.1f, Screen.width * 0.03f), textGazeCursor, button))
                {
                    checGazecursor();
                }

                if (GUI.Button(new Rect(10, 160, Screen.width * 0.1f, Screen.width * 0.03f), textTimerCursor, button))
                {

                    checkTimer();
                }

                if (GUI.Button(new Rect(10, 210, Screen.width * 0.1f, Screen.width * 0.03f), text3DButton, button))
                {
                    check3DView();
                }


                GUI.EndGroup();
            }
            #endregion

            Vector2 posGaze = gazeModel.posGazeLeft;
            if(isGazeCursorActive)
            GUI.DrawTexture(new Rect(posGaze.x-gazeCursor.width*0.5f, posGaze.y-gazeCursor.height*0.5f, gazeCursor.width, gazeCursor.height), gazeCursor);

        }
        #endregion

    }

    void Start () {

        cameraStartPosition = transform.position; 

		camEffects = gameObject.GetComponent<EffectsCamera>();

        gemMap = new GameObject[gridSize, gridSize];
        //Init the Playground
	        for (int x=0; x<gridSize;x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    instantiateGem(x, y);
                }
            }

        //start repeating of Counter
            InvokeRepeating("addSec", 1f, 1f);

	}
	
	void Update () {

        Screen.lockCursor = true;
        Screen.showCursor = false; 


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isGameMenuActive)
                isGameMenuActive = false;
            else if (!isGameMenuActive)
                isGameMenuActive = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            gazeMonoComponent.StartCalibration();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            checGazecursor();
        }


        if(Input.GetKeyDown(KeyCode.S))
        {
            checkTimer();
              
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            check3DView();
        }

        if(Input.GetKeyDown(KeyCode.Space)&&!isIngame)
        {
            isIngame = true; 
        }

        #region calculateGazeData
        
            //BOTH EYES
        if (gazeModel.diameter_leftEye == 0 && gazeModel.diameter_rightEye == 0)
        {
            Debug.LogWarning("BOTH EYES");
            return; 
        }


            //ONE EYE
        else if ((gazeModel.diameter_rightEye == 0 && gazeModel.diameter_leftEye != 0) || 
                 (gazeModel.diameter_rightEye != 0 && gazeModel.diameter_leftEye == 0))
        {
            Debug.LogWarning("ONE EYE");

            if(gazeModel.diameter_leftEye==0)
            {
                gazePositionRight = gazeModel.posGazeRight;
                gazePositionRight.y = Screen.height - gazePositionRight.y;

                gazeRayVector = gazePositionLeft;

            }

            else if(gazeModel.diameter_rightEye == 0)
            {

                gazePositionLeft = gazeModel.posGazeLeft;
                gazePositionLeft.y = Screen.height - gazePositionLeft.y;

                gazeRayVector = gazePositionLeft;
            }
        }

            // Valid Data
        else
        {
            //Mirror Y of the Vectors
            gazePositionRight = gazeModel.posGazeRight;
            gazePositionRight.y = Screen.height - gazePositionRight.y;

            gazePositionLeft = gazeModel.posGazeLeft;
            gazePositionLeft.y = Screen.height - gazePositionLeft.y;

            gazeRayVector = (gazePositionLeft + gazePositionRight) * 0.5f;
        }


        #endregion


  

        Ray rayFromMouse = Camera.main.ScreenPointToRay(gazeRayVector);
        RaycastHit hit;

        if(Physics.Raycast(rayFromMouse, out hit, distance))
        {
            if ((hit.collider.tag == "Gem00")||(hit.collider.tag == "Gem01") || (hit.collider.tag == "Gem02") || (hit.collider.tag == "Gem03"))
            {
                if (isIngame)
                {
                    if (oldOne == null)
                    {
                        oldOne = hit.collider.gameObject;
                        oldOne.GetComponent<GemBehavior>().setIsInGaze(true);
                        camEffects.playBingSound();
                    }

                    else
                    {
                        oldOne.GetComponent<GemBehavior>().setIsInGaze(false);
                        oldOne = hit.collider.gameObject;
                        oldOne.GetComponent<GemBehavior>().setIsInGaze(true);
                        camEffects.playBingSound();
                    }

                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        if(oldOne!= null)
                        {
                            checkGridForNeigbours(oldOne);
                            oldOne = null; 
                        }
                    }
                }

            }
        }

        else
        {
            Debug.Log("HIT NOTHING");
            if (oldOne != null)
            {
                oldOne.GetComponent<GemBehavior>().setIsInGaze(false);
                oldOne = null;
            }
        }


        if (addScore > 0)
        {
            score += addScore;
            addScore = 0;
        }

          
	}

    private void checkGridForNeigbours(GameObject objectToCheck)
    {
        gemsCollected = new List<GameObject>();
        gemsCollected.Add(objectToCheck);
        checkDirectNeigbours(objectToCheck, null, true, true);
        gemsCollected.Add(objectToCheck);



        for (int i=0; i<gemsCollected.Count;i++)
        {
            checkDirectNeigbours(gemsCollected[i], null, true, true);
        }

        if (gemsCollected.Count > min_gems)
        {

            calculatePoints();

            
            for (int i = 1; i < gemsCollected.Count; i++)
            {
				camEffects.createExplosion(gemsCollected[i].transform.position,0);
				gemsCollected[i].GetComponentInChildren<GemBehavior>().finishGem();
                //Destroy(gemsCollected[i]);
                gemMap[(int)gemsCollected[i].transform.position.x, (int)gemsCollected[i].transform.position.y] = null; 
                
                Debug.Log("BÄM");
            }
        }
        gemsCollected.Clear();

        refillLines();
    }

    private void checkDirectNeigbours(GameObject startObject, GameObject previousStartObject, bool checkHorizonally, bool checkVertically)
    {

        int x = (int)startObject.transform.position.x;
        int y = (int)startObject.transform.position.y;
        if (checkHorizonally)
        {
            if (x > 0f)
            {
                if (gemMap[x - 1, y].tag == startObject.tag && elementInNotArrayList(gemMap[x - 1, y]))
                    gemsCollected.Add(gemMap[x - 1, y]);
            }

            if (x < gridSize - 1)
            {
                if (gemMap[x + 1, y].tag == startObject.tag && elementInNotArrayList(gemMap[x + 1, y]))
                    gemsCollected.Add(gemMap[x + 1, y]);
            }
        }

        if (checkVertically)
        {
            if (y > 0f && startObject != previousStartObject && elementInNotArrayList(gemMap[x, y-1]))
            {
                if (gemMap[x, y-1].tag == startObject.tag)
                    gemsCollected.Add(gemMap[x, y-1]);
                
            }

            if (y < gridSize - 1 && startObject != previousStartObject && elementInNotArrayList(gemMap[x, y+1]))
            {
                if (gemMap[x, y+1].tag == startObject.tag)
                    gemsCollected.Add(gemMap[x, y+1]);

            }
        }

    }

    private void refillLines()
    {
        for (int x = 0; x < gridSize; x++)
        {
            
            for (int y = 0; y < gridSize; y++)
            {
                if (gemMap[x, y] == null)
                {
                    int nextElement= findNextElement(x,y);
                    
                    if(nextElement == 0)
                    {
                        instantiateGem(x, y);
                    }
                    
                    else
                    {
                        gemMap[x, y] = gemMap[x, nextElement];
						gemMap[x,y].GetComponent<GemBehavior>().moveObjectTo(new Vector3(x,y,0));
						//gemMap[x, y].transform.position = new Vector3(x, y, 0);
                        gemMap[x, nextElement] = null; 
                    }
                }
            }
        }
    }

    private int findNextElement(int x, int y)
    {

        for (int i = y; i < gridSize;i++)
        {
            if (gemMap[x, i] != null)
                return i;
        }
        return 0; 
    }

    private void instantiateGem(int x, int y)
    {
        int randomNum = Random.Range(0, gems.Length);
        GameObject gem  = (GameObject)Instantiate(gems[randomNum], new Vector3(x*gemSize, y*gemSize, 0), gems[randomNum].transform.rotation);
		gem.transform.rotation = Quaternion.Euler(Vector3.zero);
		gemMap[x, y] = gem; 
    }

    private bool elementInNotArrayList(GameObject testObject)
    {
        foreach (GameObject e in gemsCollected)
        {
            if (e == testObject)
                return false; 
        }
        return true; 
    }

    private void calculatePoints()
    {

        int countElements = gemsCollected.Count - min_gems;
        
        if (countElements < 2)
        {
            camEffects.playScoreSound();

        }

        else if (countElements >= 2 && countElements < 5)
        {
            addScore += gemsCollected.Count * 200;
			camEffects.playExplodeSound();
        }

        else if (countElements >= 5&& countElements < 7)
        {
            addScore += gemsCollected.Count * 300;
			camEffects.playExplodeSound();
        }

        else if (countElements >9)
        {
            addScore += gemsCollected.Count * 500;
			camEffects.playExplodeSound();
			camEffects.shakeCam();
        }

    }

    private void checkTimer()
    {

        if (isTimeCounting)
        {
            textTimerCursor = "(S): RestartTimer";
            seconds = 0;
            minutes = 0;
            isTimeCounting = false;
        }
        else
        {
            textTimerCursor = "(S): StopTimer";
            isTimeCounting = true;
        }
    }

    private void check3DView()
    {
        if (is3DViewActive)
        {
            is3DViewActive = false;
            GetComponent<View3D_WithGaze>().enabled = false;
            gameObject.transform.position = cameraStartPosition;
            gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            text3DButton = "(W) enable 3DView";

        }
        else
        {
            is3DViewActive = true;

            GetComponent<View3D_WithGaze>().enabled = true; 
            text3DButton = "(W) disable 3DView";
        }
    }

    private void checGazecursor()
    {
        if (isGazeCursorActive)
        {
            textGazeCursor = "(H): ShowGazeCursor";
            isGazeCursorActive = false;
        }
        else
        {
            textGazeCursor = "(H): HideGazeCursor";
            isGazeCursorActive = true;
        }
    }
    
    private void addSec()
    {
        if(isTimeCounting)
        {
            Debug.Log("AddSec");
            seconds++;
            if(seconds>=60)
            {
                minutes++;
                seconds = 0;
            }
        }
    }

}