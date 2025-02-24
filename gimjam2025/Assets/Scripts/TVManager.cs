using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVManager : MonoBehaviour
{
    public enum TVState { Tutorial, Preview, Submit }
    public TVState tvState;
    public enum SubmitState { Waiting, X, Score }
    public SubmitState submitState;
    public Transform tutorial, preview, submit;
    public Transform waiting, x, score;
    public Transform spinningItem, rotateableItem, cam;
    public TMP_Text tutorialText;
    public TMP_Text scoreText, scoreLetter;
    public float spinningSpeed, scoreValue;
    public bool correct;
    public Vector3 defaultCameraPosition;
    public Transform zoomCameraPosition;
    public static TVManager Instance;
    void Start()
    {
        defaultCameraPosition = cam.transform.position;
        tvState = TVState.Preview;
        submitState = SubmitState.Waiting;
    }

    void Update()
    {
        switch (tvState)
        {
            case TVState.Tutorial:
                tutorial.gameObject.SetActive(true);
                preview.gameObject.SetActive(false);
                submit.gameObject.SetActive(false);
                cam.transform.position = defaultCameraPosition;
                submitState = SubmitState.Waiting;
                break;
            case TVState.Preview:
                spinningItem.transform.Rotate(spinningSpeed, 0, 0);
                tutorial.gameObject.SetActive(false);
                preview.gameObject.SetActive(true);
                submit.gameObject.SetActive(false);
                spinningItem.transform.SetParent(rotateableItem);
                cam.transform.position = defaultCameraPosition;
                submitState = SubmitState.Waiting;
                break;
            case TVState.Submit:
                tutorial.gameObject.SetActive(false);
                preview.gameObject.SetActive(false);
                submit.gameObject.SetActive(true);
                switch (submitState)
                {
                    case SubmitState.Waiting:
                        waiting.gameObject.SetActive(true);
                        x.gameObject.SetActive(false);
                        score.gameObject.SetActive(false);
                        cam.transform.position = zoomCameraPosition.position;
                        StartCoroutine(WaitingCoroutine());
                        break;
                    case SubmitState.X:
                        waiting.gameObject.SetActive(false);
                        x.gameObject.SetActive(true);
                        score.gameObject.SetActive(false);
                        YouAreWrongWompWomp();
                        break;
                    case SubmitState.Score:
                        waiting.gameObject.SetActive(false);
                        x.gameObject.SetActive(false);
                        score.gameObject.SetActive(true);
                        scoreText.text = scoreValue.ToString();
                        if (scoreValue >= 90)
                        {
                            scoreLetter.text = "S";
                        }
                        else if (scoreValue >= 75 && scoreValue <= 89)
                        {
                            scoreLetter.text = "A";
                        }
                        else if (scoreValue >= 50 && scoreValue <= 74)
                        {
                            scoreLetter.text = "B";
                        }
                        else
                        {
                            scoreLetter.text = "C";
                        }
                        CorrectItem();
                        break;
                }
                break;
        }
    }
    public void YouAreWrongWompWomp()
    {
        StartCoroutine(WrongCoroutine());
        //Bro reset the game pls i don't know how to
    }
    public void CorrectItem()
    {
        StartCoroutine(WaitforClick());
    }
    IEnumerator WaitingCoroutine()
    {
        yield return new WaitForSeconds(3);
        if (correct)
        {
            submitState = SubmitState.Score;
        }
        else
        {
            submitState = SubmitState.X;
        }
    }
    IEnumerator WrongCoroutine()
    {
        yield return new WaitForSeconds(2);
        tvState = TVState.Preview;
        submitState = SubmitState.Waiting;
        cam.transform.position = defaultCameraPosition;
    }
    IEnumerator WaitforClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tvState = TVState.Preview;
                yield break;
            }
            yield return null;
        }
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
