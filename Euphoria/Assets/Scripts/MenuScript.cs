﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public void PlayGame(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }
    
    public void ViewOptions()
    {

    }


    public void FadeToWhite()
    {
       
    }

}
