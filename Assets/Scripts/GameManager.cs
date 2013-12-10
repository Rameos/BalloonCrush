using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public int addScore = 0; 
    public int score = 0;
    public int min_gems; 
    public int gridSize;
    public float distance;
    public GameObject [] gems = new GameObject[4];

    public GameObject[,] gemMap;

    List<GameObject> gemsCollected = new List<GameObject>();

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width * 0.1f, Screen.height* 0.1f),"Score: "+score);
    }

    void Start () {

        gemMap = new GameObject[gridSize, gridSize];
        //Init the Playground
	        for (int x=0; x<gridSize;x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    instantiateGem(x, y);
                }
            }
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // !!!!MOUSE!!!!
            Ray rayFromMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 

            if(Physics.Raycast(rayFromMouse, out hit, distance))
            {
                Debug.Log("SEARCH");
                checkGridForNeigbours(hit.collider.gameObject); 
            }

        }

        while(addScore>0)
        { score++;
            addScore--;
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
                Destroy(gemsCollected[i]);
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
                        gemMap[x, y].transform.position = new Vector3(x, y, 0);
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
        gemMap[x, y] = (GameObject)Instantiate(gems[randomNum], new Vector3(x, y, 0), gems[randomNum].transform.rotation);
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
        
        if (countElements < 5)
        {
            addScore += gemsCollected.Count * 100;
        }

        else if (countElements >= 5 && countElements < 7)
        {
            addScore += gemsCollected.Count * 200;
        }

        else if (countElements >= 7&& countElements < 10)
        {
            addScore += gemsCollected.Count * 300;
        }

        else if (countElements >10)
        {
            addScore += gemsCollected.Count * 500;
        }

    }

}