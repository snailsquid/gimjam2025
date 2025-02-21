using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVManager : MonoBehaviour
{
    public enum TVState {Tutorial,Preview,Submit}
    public TVState tvState;
    public enum SubmitState {X,V,Score}
    public SubmitState submitState;
    public Transform tutorial,preview,submit;
    public Transform x,v,score;
    public Transform spinningItem;
    public TMP_Text tutorialText;
    public TMP_Text scoreText;
    public float spinningSpeed, scoreValue;
    void Start()
    {
        tvState = TVState.Tutorial;
        submitState = SubmitState.X;
    }

    void Update()
    {
        switch(tvState)
        {
            case TVState.Tutorial:
                tutorial.gameObject.SetActive(true);
                preview.gameObject.SetActive(false);
                submit.gameObject.SetActive(false);
                break;
            case TVState.Preview:
                spinningItem.transform.Rotate(spinningSpeed,0,0);
                tutorial.gameObject.SetActive(false);
                preview.gameObject.SetActive(true);
                submit.gameObject.SetActive(false);
                break;
            case TVState.Submit:
                tutorial.gameObject.SetActive(false);
                preview.gameObject.SetActive(false);
                submit.gameObject.SetActive(true);
                switch(submitState)
                {
                    case SubmitState.X:
                        x.gameObject.SetActive(true);
                        v.gameObject.SetActive(false);
                        score.gameObject.SetActive(false);
                        break;
                    case SubmitState.V:
                        x.gameObject.SetActive(false);
                        v.gameObject.SetActive(true);
                        score.gameObject.SetActive(false);
                        break;
                    case SubmitState.Score:
                        x.gameObject.SetActive(false);
                        v.gameObject.SetActive(false);
                        score.gameObject.SetActive(true);
                        scoreText.text = scoreValue.ToString();
                        break;
                }
                break;
        }
    }
}
