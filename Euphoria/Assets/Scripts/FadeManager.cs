using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeManager : MonoBehaviour
{

    public static FadeManager Instance { set; get; }

    public Image fadeImage;
    public bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    private void Awake()
    {
        Instance = this;
        Fade(false, 3f);
    }

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
        StartCoroutine(WaitForFade());

    }

    private void Update()
    {

        if(!isInTransition)
        {
            return;
        }

        transition += (isShowing) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

        if(transition > 1 || transition <0)
        {
            isInTransition = false;
        }

    }

    IEnumerator WaitForFade()
    {
        Debug.Log("Waiting");
        yield return new WaitWhile(() => !isInTransition);
    }


}
