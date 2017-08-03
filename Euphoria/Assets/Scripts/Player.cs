using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour {

    //INJECTION ABILITIES
    
    //Checks Player State
    public bool isHappy = false;
    public bool isSad = false;
    public bool isAngry = false;
    public bool isNormal = true;

    public bool atLevelEnd = false;
    public string playerState = "";
    public string startingPlayerState = "Normal";

    //Is colldiding with injection
    private bool canInject = false;

    //Movement variables
    float moveSpeed;
    float gravity = -20;
    float defaultMoveSpeed = 6;
    float happyMoveSpeed = 10;
    float sadMoveSpeed = 3;
    float angryMoveSpeed = 7;
    //
    Bounds playerBounds;

    //Jump physics vars
    float jumpVelocity = 8;
    float jumpHeight = 4f;
    float timeToJumpApex = .4f;
    
    Vector3 velocity;

    Controller2D controller;
    //Injection Station
    InjectionStation injectionStation;
    InjectionStation.injectionType injectType;
    //ASSIGN FADEMANAGER IN PROPERTIES WINDOW -- NULL EXECEPTION WILL BE THROWN OTHERWISE
    FadeManager fadeManager;
    string nextLevel;

    // Use this for initialization
    void Start () {
        controller = GetComponent<Controller2D>();

        //Initialize jump variables
        gravity = -((2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2));
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        //Force canInject to false
        canInject = false;

        //Set playerState to the desired starting point (useful for when changing states between levels)
        playerState = startingPlayerState;
        //Updates player state so player starts with appropriate abilities.
        UpdatePlayerState(playerState);

	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(canInject);
        //Checks for collisions above and below player, and sets y velocity 0 if collision returns true
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        //If player hits space, has the happy injection, and is grounded, player will jump!
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below && isHappy)
        {
            velocity.y = jumpVelocity;
        }

        //Movespeed
        velocity.x = input.x * moveSpeed;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        

        /*
         * IMPORTANT INFO REGARDING THE USE OF "E"
         * 
         * E will be used to interact with injection stations, pick up objects, drop objects, and destroy
         * destructible objects. ALL THIS WILL DEPEND ON PLAYER STATE
         */ 

        //If player is colliding with switches, and player presses "E", call the assigninject function
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (canInject == true)
            {
                Debug.Log("Injecting...");
                injectType = injectionStation.injectType;
                if (injectType == InjectionStation.injectionType.happy) { UpdatePlayerState("HappyState"); }
                if (injectType == InjectionStation.injectionType.sad) { UpdatePlayerState("SadState"); }
                if (injectType == InjectionStation.injectionType.angry) { UpdatePlayerState("AngryState"); }
                if (injectType == InjectionStation.injectionType.normal) { UpdatePlayerState("Normal"); }
            }

            else if (atLevelEnd == true)
            {
                fadeManager = GameObject.Find("FadeManager").GetComponent<FadeManager>();
                fadeManager.Fade(true, 1.5f);
                EndLevel(nextLevel);
            }

        }



	}

    /*
     * COLLISION HANDLING
     * 
     * MULTIPLE HANDLES
     * 
     * HANDLING INJECTIONS
     * If collided object is an injection station, game assigns injectionStation and injectType to appropriate values,
     * and allows player to hit "e" in order to apply appropriate injection
     * 
     * HANDLING LEVEL EXIT
     * If player collides with the end of the level, fade screen to white and change scenes
     * 
     * HANDLING DESTRUCTABLES
     * If player is in the "angry" state, game will assign appropriate vars to the destructible game object so player
     * can cast destroy on said object
     * 
     */ 
    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.CompareTag("InjectionStation"))
        {
            canInject = true;
            injectionStation = otherObject.GetComponent<InjectionStation>();
            injectType = injectionStation.injectType;
            Debug.Log(canInject);
        }
        else if (otherObject.CompareTag("Finish"))
        {
            LevelExit exit = otherObject.GetComponent<LevelExit>();
            nextLevel = exit.nextLevelName;
            Debug.Log(nextLevel);
            atLevelEnd = true;
        }
        else
        {
            Debug.Log("Unknown collission detected");
        }
    
    }

    /*
     *REMOVE ABILITY TO INJECT AFTER LEAVING INTERACTABLE LOCATION
     */
    private void OnTriggerExit2D(Collider2D otherObject)
    {
        canInject = false;
        atLevelEnd = false;
    }


    /*
     * HANDLING PLAYERSTATE
     * 
     * Updating the player state and being able to track it is crucial when changing between states in order
     * to complete levels. This function can be called to update the player state for applying injections,
     * ending levels, starting levels with certain traits, etc. This function will be able to be called at
     * any point to track the player's state throughout the game.
     * 
     */ 
    public string UpdatePlayerState(string newPlayerState)
    {
        if (newPlayerState == "HappyState")
        {
            //Sets all other states to false
            isSad = false;
            isAngry = false;
            isNormal = false;

            //Adds new player traits
            playerState = newPlayerState;
            isHappy = true;
            moveSpeed = happyMoveSpeed;
        }

        else if (newPlayerState == "SadState")
        {
            //Sets all other states to false
            isHappy = false;
            isAngry = false;
            isNormal = false;

            //Adds new player traits
            playerState = newPlayerState;
            isSad = true;
            moveSpeed = sadMoveSpeed;
        }

        else if (newPlayerState == "AngryState")
        {
            //Sets all other states to false
            isHappy = false;
            isSad = false;
            isNormal = false;

            //Adds new player traits
            playerState = newPlayerState;
            isAngry = true;
            moveSpeed = angryMoveSpeed;
        }

        else if (newPlayerState == "Normal") 
        {
            //Sets all other states to false
            isHappy = false;
            isSad = false;
            isAngry = false;

            //Restores default player traits
            playerState = newPlayerState;
            isNormal = true;
            moveSpeed = defaultMoveSpeed;
        }
        
        Debug.Log(playerState);
        return playerState;
    }

    /*
     * ENDING LEVEL FUNCTION. HANDLES ALL EXITING OF SCENES BASED ON LOCATION OF SCENE CHANGE
     */ 
    public void EndLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
