using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup PopUp;
    [SerializeField] float hideTime = 5;

    public float fadeTime;
    public float fadeAwayPersecond;
    private bool fadeIn = false;
    private bool fadeOut = false;
    public TimeManager ManageTime;

    void Start()
    {
        fadeAwayPersecond = 1 / fadeTime;
        PopUp.alpha = 0;
    }

    public void Show()
    {
        ManageTime.PauseTime();
        StartCoroutine(WaitCoroutine());
        ManageTime.StartTime();
    }
    IEnumerator WaitCoroutine()
    {
        fadeIn = true;
        yield return new WaitForSeconds(fadeTime);
        yield return new WaitForSeconds(hideTime);
        yield return new WaitForSeconds(fadeTime);
        fadeOut = true;
    }

    void Update()
    {
        if (fadeIn)
        {
            if (PopUp.alpha < 1)
            {
                PopUp.alpha += fadeAwayPersecond * Time.deltaTime;
                if (PopUp.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeOut)
        {
            if (PopUp.alpha >=0)
            {
                PopUp.alpha -= fadeAwayPersecond * Time.deltaTime;
                if (PopUp.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
