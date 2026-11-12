// ISTA 425 / INFO 525 Algorithms for Games
//
// Sample code file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    BoxCollider2D wizardCollider;

    [Tooltip("Frame-rate independent movement")]
    public float MoveRate = 5.0f;

    [Tooltip("Player-relative ortho camera")]
    public GameObject playerCamera;

    [Tooltip("Time to fade sounds end of action, in seconds")]
    public float fadeTime = 0.1f;

    // local reference to GameController EventSystem
    GameObject     GC;
    GameController eventSystem;

    Animator       wizardAnim;
    SpriteRenderer wizardSprite;

    enum ActionType
    {
        Cast,
        Hack,
        Jump,
        Die
    }

    // Reality check: Is this character alive?
    bool dead = false;

    // animation state machine metadata
    int runTrigger    = Animator.StringToHash("isRunning");
    int idleTrigger   = Animator.StringToHash("isIdling");
    int jumpTrigger   = Animator.StringToHash("isJumping");
    int fallTrigger   = Animator.StringToHash("isFalling");
    int deathTrigger  = Animator.StringToHash("isDying");

    int attackTrigger = Animator.StringToHash("isAttacking");
    int attackType    = Animator.StringToHash("attackType");

    // Private method sends messages to the animation state machine based
    // on the current user input for running, attacking, etc.
    private void SetAnimState (float x, float y)
    {
        bool cast = false;
        bool hack = false;
        bool jump = false;
        bool die  = false;

        // get new inputs only if wizard is not dead
        if (!dead)
        {
            cast = eventSystem.getInput(GameController.ControlType.Cast);
            hack = eventSystem.getInput(GameController.ControlType.Hack);
            jump = eventSystem.getInput(GameController.ControlType.Jump);
            die  = eventSystem.getInput(GameController.ControlType.Die);
        }

        // set the state of the controller
        AnimatorStateInfo state = wizardAnim.GetCurrentAnimatorStateInfo(0);

        if (die)
        {
            wizardAnim.SetTrigger(deathTrigger);
            dead = true;
        }

        if (jump)
            wizardAnim.SetTrigger(jumpTrigger);

        // set movement state if wizard is moving left or right
        if (x != 0.0f)
        {
            // face the direction of move
            if (x > 0.0f)
                wizardSprite.flipX = false;
            else if (x < 0.0f)
                wizardSprite.flipX = true;

            wizardAnim.SetTrigger(runTrigger);
        }
        // the wizard is standing still, idling
        else if (x == 0.0f && y == 0.0f)
        {
            wizardAnim.SetTrigger(idleTrigger);

            // spell casting takes precendence over attacking
            if (cast)
            {
                wizardAnim.SetInteger(attackType, (int)ActionType.Cast);
                wizardAnim.SetTrigger(attackTrigger);
            }
            else if (hack)
            {
                wizardAnim.SetInteger(attackType, (int)ActionType.Hack);
                wizardAnim.SetTrigger(attackTrigger);
            }
        }
    }

    public void collision(){
         GameObject [] enemy = GameObject.FindGameObjectsWithTag("Enemies");
         foreach(GameObject kid in enemy){
            EnemyController child = kid.GetComponent<EnemyController>();
            if(child !=null){
            //    Debug.Log("a");
                if(child.eventSystem !=null){
                                //   Debug.Log("b");
                    if(child.eventSystem.IndicatorPrefab != null){
                                    //    Debug.Log("c");

                        if(child.eventSystem.IndicatorPrefab.AAbb != null){
                                        //   Debug.Log("d");

                           
                            if(child.eventSystem !=null){
            Vector2 min = child.eventSystem.IndicatorPrefab.AAbb.Min;
            Vector2 max = child.eventSystem.IndicatorPrefab.AAbb.Max;
            // if()

            float min_x = wizardCollider.bounds.min.x;
            float max_x = wizardCollider.bounds.max.x;
            // Debug.Log(max_x);

            float min_y = wizardCollider.bounds.min.y;
            float max_y = wizardCollider.bounds.max.y;
                        Debug.Log(min_x);
                        //  Debug.Log("jouijpoijopijo");

            // min_x<min[0] && max_x>min[0] && min_y<min[1] && max_y>min[1]
            //     ||
            //     max_x<max[1] && max_x>min[1] && max_y<max[1] && max_y>max[1]
            if(min_x < min[0] && max_x > min[0] && min_y < min[1] && max_y > min[1] &&
                min_x < max[0] && max_x > max[0] && min_y < min[1] && max_y > min[1] &&
                min_x < min[0] && max_x > min[0] && min_y < max[1] && max_y > max[1] &&
                min_x < max[0] && max_x > max[0] && min_y < max[1] && max_y > max[1])
            {
                dead = true;
            }
            }
         }
            }
            }
            }
         }
        // min
        // BoxCollider2D box;

         
    }
    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController");
        eventSystem = GC.GetComponent<GameController>();

        wizardAnim = GetComponent<Animator>();
        wizardSprite = GetComponent<SpriteRenderer>();
        wizardCollider = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float x = 0.0f;
        float y = 0.0f;

        // get new inputs only if wizard is not dead
        if (!dead)
        {
            // input from up/down, left/right keys
            x = eventSystem.getAxis(GameController.AxisType.X);
            y = eventSystem.getAxis(GameController.AxisType.Y);
        }

        // setup the wizard's current animation state.
        SetAnimState(x, y);

        Vector3 move = new Vector3(x, 0.0f, 0.0f) * MoveRate * Time.deltaTime;
        // if wizard is moving
        if (move != Vector3.zero)
        {
            
            // increment the scroller position for the background sprites
            float totalMove = eventSystem.playerMove.x + move.x;
            float clampMove = eventSystem.clamp(totalMove);

            eventSystem.scrollerMove.x = totalMove;
            eventSystem.playerMove.x   = totalMove;
        }
        collision();
    }
}