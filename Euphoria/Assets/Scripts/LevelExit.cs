using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour {

    public string nextLevelName;
    private void Start()
    {
        //If my dumb ass forgets to add the "Finish" tag to the object, add it for me gg
        if (this.CompareTag("Finish")) { }
        else { this.tag = "Finish"; }

    }

}
