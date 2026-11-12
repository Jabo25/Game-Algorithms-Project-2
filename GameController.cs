// ISTA 425 / INFO 525 Algorithms for Games
//
// Sample code file
//I htink this file avatar the user behavior and movement direction

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum AxisType
    {
        X,
        Y
    }

    public enum ControlType
    {
        Cast,
        Hack,
        Jump,
        Die,
        Quit,
        Show
    }

    [System.Serializable]
    public class InputMapping
    {
        [Tooltip("Game control type")]
        public ControlType type;
        [Tooltip("System key code")]
        public KeyCode key;
    }

    [Tooltip("Array of input mappings to player action types")]
    public InputMapping[] inputMappingArray;

    [Tooltip("Indicator prefab used to show collisions")]
    public BoxIndicator IndicatorPrefab;

    [Tooltip("Horizontal tiles of background (background width)")]
    public int numTiles = 3;

    // this is a fudge factor because the tiles are not exactly equal
    // to the camera width and I don't feel like setting a new pixel 
    // scale, reimporting and realigning all of the background layers.

    [Tooltip("Boundary padding when background doesn't exactly match camera FOV")]
    public float padding = 0.8f;

    public Vector3 scrollerMove;
    public Vector3 playerMove;

    private float layerWidth;

    // this class is used internally to query and update inputs and
    // enforces a one to one mapping between input keys and system
    // functions.
    private class InputStatus
    {
        public KeyCode key;
        public bool    status = false;
    }
    // inputs for the x, y axes of player motion
    private Vector2 inputAxes;
    // dictionary of all over valid input types
    private Dictionary<ControlType, InputStatus> inputStatusDictionary;

    public int backgroundIndex = 0;
    public int paralaxCounter = 0;
    public int randomBackgroundInt = 0;
            // 1. Initialize the random number generator

//this thing is important because we need it 
    public AABB CreateAABB(BoxCollider2D box)
    {
        var aabb = new AABB();
        aabb.SetCollider(box);
        

        return aabb;
    }

    // This method creates a visual indicator for a 2D box collider.
    public BoxIndicator CreateIndicator(AABB aabb)
    {
        var indicator = Instantiate(IndicatorPrefab);//duplicate
        indicator.SetAABB(aabb);//make new hit box and put it on the hit box

        return indicator;
    }

    // This method may be helpful to map player position to valid scrolling
    // range. Prevents player from leaving the left or right side of a map
    // as per clamp algorithm given in class (see GPAT Ch. 2).
    public float clamp(float pos)
    {
        float clampedPos;

        // equal to half the full length of the tiles, (n * width) / 2
        float halfLength = ((float) numTiles) * layerWidth / 2.0f;
        // the left and right bounds minus the half screen padding area
        float  leftBound = -(halfLength - layerWidth / 2.0f - padding);
        float rightBound =  (halfLength - layerWidth / 2.0f - padding);

        if      (pos < leftBound)
            clampedPos = leftBound;
        else if (pos > rightBound)
            clampedPos = rightBound;
        else
            clampedPos = pos;

        return clampedPos;
    }

    public float getAxis (AxisType axis)
    {
        return inputAxes[(int) axis];
    }

    public bool getInput (ControlType type)
    {
        bool input = false;

        if (inputStatusDictionary.ContainsKey (type))
            input = inputStatusDictionary[type].status;

        return input;
    }

    public void updateInput ()
    {
        inputAxes[0] = Input.GetAxisRaw("Horizontal");
        inputAxes[1] = Input.GetAxisRaw("Vertical");

        foreach (ControlType type in System.Enum.GetValues(typeof(ControlType)))
        {
            if (inputStatusDictionary.ContainsKey(type))
                inputStatusDictionary[type].status = Input.GetKeyDown(inputStatusDictionary[type].key);
        }
    }
    
    public void increaseParalaxCount()
    {
        paralaxCounter++;
    }

    //tells the paralaxScroler which background to switch to
    public int getBackgroundIndex()
    {
        if(randomBackgroundInt == paralaxCounter){
            backgroundIndex++;
            paralaxCounter = 0;
        }
        return backgroundIndex % 3;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        GameObject foreground = GameObject.FindGameObjectWithTag("Foreground");
        layerWidth = foreground.GetComponent<SpriteRenderer>().bounds.size.x;


        //choose a new background randomly
        randomBackgroundInt = Random.Range(1, 3);

        scrollerMove = Vector3.zero;
        playerMove   = Vector3.zero;

        // initialize motion axes and 1:1 mapping of keycode to status
        inputAxes = Vector2.zero;
        inputStatusDictionary = new Dictionary<ControlType, InputStatus> ();
        foreach (InputMapping mapping in inputMappingArray)
        {
            if (!inputStatusDictionary.ContainsKey (mapping.type))
                inputStatusDictionary[mapping.type] = new InputStatus ();

            inputStatusDictionary[mapping.type].key = mapping.key;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateInput();

        if (getInput(ControlType.Quit))
            Application.Quit();
    
Debug.Log(backgroundIndex + " background");


    // public AABB CreateAABB(BoxCollider2D box)
//     public BoxIndicator CreateIndicator(AABB aabb)

    }
}
