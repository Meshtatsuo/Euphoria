using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteraction : MonoBehaviour {

    PolygonCollider2D turretCollider;
    Player player;

	// Use this for initialization
	void Start () {
        turretCollider = GetComponent<PolygonCollider2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

        
    
}
