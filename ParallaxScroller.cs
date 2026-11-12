// ISTA 425 / INFO 525 Algorithms for Games
//
// Sample code file

//idea for how I handle infinitie paralax scrolling
// 2. step flip, so each layer has a boolean to signify when the inner and outer layers have been swapped
        //Bool finishedSwap is going to be true when swap is done, false when a half swap has been made 
        // and true when full swap has been made
// 2. backgroundID will be an intiger in the main file that tells the paralax scroller which layer to sample from. 
// 3. In the local file, since backgroundID swaps happen in phases, localBackgroundID will be used to keep track of the local background
        //between swap phases
// 4. getBackgroundID will request the background ID from the main file and update the localBackground if a swap has been completed. 

//other logistics:
    //swap outer background -> inner background


//Ideas for solution:
    //Each layer has a param called "paralax level", so the idea would be to rcreate a script called swap background. 
    //SwapBackground.cs
        //1. Has a var called backgroundID
        //2.  has a var bool "isInnerBackground" 
            //this var is public and so that the user can set the background as inner or outer to accelerate the swap speed
        //3. a function called swapBackground(bool backGRoundID, currBackground, isOuter)
                //swaps the backgrounds in the order of innerBackground->outerBackground. IsOuter tells the script which background to swap


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [Tooltip("Camera to which parallax is relative")]
    public GameObject parallaxCamera;

    [Tooltip("Level of parallax for this depth layer")]
    public float parallaxLevel;

    [Tooltip("enter file names of paralax levels")]
    public string[] frameList;
    public int x = 1;

    // the GC provides useful I/O and utility methods
    GameObject GC;
    GameController eventSystem;
        private SpriteRenderer spriteRenderer;

    float startPos;
    bool sceneChanged = true;
    int scene = 0;
    public int index = 1;
    float size;
    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController");
        eventSystem = GC.GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        startPos = transform.position.x;

        SoundManager.Instance.PlaySound(SoundManager.SoundType.Night, true);
        sceneChanged = true;
        scene = 0;
        size = spriteRenderer.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {

        float moveR = GC.GetComponent<GameController>().playerMove[0];
        transform.position = new Vector3(transform.position.x- (parallaxLevel * Time.deltaTime * moveR), transform.position.y, transform.position.z);

        
        // if(parallaxLevel*Time.deltaTime*moveR)
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        // float size = GetComponent<SpriteRenderer>().bounds.size.x;
        foreach (ParallaxScroller child in GetComponentsInChildren<ParallaxScroller>())    
{


if (Mathf.Abs(transform.position.x - startPos) >= size)
{
    float overshoot = (startPos - size) - transform.position.x;

        transform.position = new Vector3(
            transform.position.x - (parallaxLevel * Time.deltaTime * moveR),
            transform.position.y,
            transform.position.z
        );
        

            // ParallaxScroller component in GetComponents<ParallaxScroller>();
            // if()
            // if(eventSystem.sceneNumber == scene && sceneChanged){
                // transform.position = new Vector3(startPos,transform.position.y, transform.position.z);
                // x++;
                    if(parallaxLevel == 0.2f)
                    {
                        eventSystem.GetComponent<GameController>().increaseParalaxCount();

                    }


                if(sceneChanged == true){
                    sceneChanged = false;                    
                    for(int i = 0; i<transform.childCount; i++)
                    {
                        spriteRenderer.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(frameList[index]);
                        // spriteRenderer.transform.GetChild(i).spriteRenderer.sprite = Resources.Load<Sprite>("Background/layer 5");  
                        transform.position = new Vector3(startPos,transform.position.y, transform.position.z);
                        
                    }

                }
                else if(sceneChanged == false){
                    sceneChanged = true;
                                        spriteRenderer.sprite = Resources.Load<Sprite>(frameList[index]);
                    index = eventSystem.GetComponent<GameController>().getBackgroundIndex();//get index to set background to
                }
                
        }        
        // TODO: Part of the parallax scrolling algorithm may go here.
    }


    }

} 



