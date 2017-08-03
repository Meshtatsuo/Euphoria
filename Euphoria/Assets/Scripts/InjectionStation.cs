using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionStation : MonoBehaviour {

    public enum injectionType { happy, sad, angry, normal };
    public injectionType injectType;


	// Use this for initialization
	void Start () {

        //If my dumb ass forgets to add the "InjectionStation" tag to the object, add it for me gg
        if (this.CompareTag("InjectionStation")) { }
        else { this.tag = "InjectionStation"; }

	}
}
