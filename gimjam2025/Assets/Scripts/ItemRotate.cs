using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemRotate : MonoBehaviour
{
    public float rotateSpeed = 50f;
    public float speed;
    public float bounceTime, rotateBounceTime;
    public (KeyCode, KeyCode) rotateLeftXKeybind = (KeyCode.A, KeyCode.D);
    public (KeyCode, KeyCode) rotateLeftYKeybind = (KeyCode.W, KeyCode.S);
    public KeyCode activateLeftKeybind = KeyCode.LeftShift;
    public (KeyCode, KeyCode) rotateRightXKeybind = (KeyCode.J, KeyCode.L);
    public (KeyCode, KeyCode) rotateRightYKeybind = (KeyCode.I, KeyCode.K);
    public KeyCode activateRightKeybind = KeyCode.Slash;
    public Color active, inactive;
    private Transform itemLeft, itemRight;
    public Image leftImage, rightImage;
    public Hand handLeft, handRight;
    public Transform handTransformLeft, handTransformRight;
    public bool debug = false;
    void Start()
    {
    }
    void FixedUpdate()
    {
        itemLeft = handLeft.heldItem;
        itemRight = handRight.heldItem;
        RotateLeft();
        MoveLeft();
        RotateRight();
        MoveRight();
        if (debug)
            ShiftChecker();
    }
    void ShiftChecker()
    {
        if (Input.GetKey(activateLeftKeybind))
            leftImage.color = active;
        else leftImage.color = inactive;
        if (Input.GetKey(
            activateRightKeybind
        ))
            rightImage.color = active;
        else rightImage.color = inactive;
    }
    void RotateLeft()
    {
        if (!Input.GetKey(activateLeftKeybind) || itemLeft == null) return;
        Vector3 rotate;
        int x = 0, y = 0;
        if (Input.GetKey(rotateLeftXKeybind.Item1))
            y = 1;
        else if (Input.GetKey(rotateLeftXKeybind.Item2))
            y = -1;
        if (Input.GetKey(rotateLeftYKeybind.Item1))
            x = 1;
        else if (Input.GetKey(rotateLeftYKeybind.Item2))
            x = -1;
        rotate = new Vector3(x, y, 0);
        if (x != 0 || y != 0)
            itemLeft.transform.DORotate(rotate * rotateSpeed * Time.deltaTime, rotateBounceTime, RotateMode.WorldAxisAdd).SetEase(Ease.OutElastic);
    }
    void RotateRight()
    {
        if (!Input.GetKey(activateRightKeybind) || itemRight == null) return;
        Vector3 rotate;
        int x = 0, y = 0;
        if (Input.GetKey(rotateRightXKeybind.Item1))
            y = 1;
        else if (Input.GetKey(rotateRightXKeybind.Item2))
            y = -1;
        if (Input.GetKey(rotateRightYKeybind.Item1))
            x = 1;
        else if (Input.GetKey(rotateRightYKeybind.Item2))
            x = -1;
        rotate = new Vector3(x, y, 0);
        if (x != 0 || y != 0)
            itemRight.transform.DORotate(rotate * rotateSpeed * Time.deltaTime, rotateBounceTime, RotateMode.WorldAxisAdd).SetEase(Ease.OutElastic);
    }
    void MoveLeft()
    {
        if (Input.GetKey(activateLeftKeybind) || handLeft == null) return;
        Vector3 move;
        int x = 0;
        int z = 0;
        if (Input.GetKey(rotateLeftXKeybind.Item1))
            x = -1;
        else if (Input.GetKey(rotateLeftXKeybind.Item2))
            x = 1;
        if (Input.GetKey(rotateLeftYKeybind.Item1))
            z = 1;
        else if (Input.GetKey(rotateLeftYKeybind.Item2))
            z = -1;
        move = new Vector3(x, 0, z);
        if (x != 0 || z != 0)
            handLeft.transform.DOLocalMove(handLeft.transform.position + (move.normalized * speed * Time.deltaTime), bounceTime).SetEase(Ease.OutElastic);
    }
    void MoveRight()
    {
        if (Input.GetKey(activateRightKeybind) || handRight == null) return;
        Vector3 move;
        int x = 0;
        int z = 0;
        if (Input.GetKey(rotateRightXKeybind.Item1))
            x = -1;
        else if (Input.GetKey(rotateRightXKeybind.Item2))
            x = 1;
        if (Input.GetKey(rotateRightYKeybind.Item1))
            z = 1;
        else if (Input.GetKey(rotateRightYKeybind.Item2))
            z = -1;
        move = new Vector3(x, 0, z);
        if (x != 0 || z != 0)
            handRight.transform.DOLocalMove(handRight.transform.position + (move.normalized * speed * Time.deltaTime), bounceTime).SetEase(Ease.OutElastic);
    }
}

