using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRotate : MonoBehaviour
{
    public float rotateSpeed = 50f;
    public (KeyCode, KeyCode) rotateLeftXKeybind = (KeyCode.A, KeyCode.D);
    public (KeyCode, KeyCode) rotateLeftYKeybind = (KeyCode.W, KeyCode.S);
    public KeyCode activateLeftKeybind = KeyCode.LeftShift;
    public (KeyCode, KeyCode) rotateRightXKeybind = (KeyCode.J, KeyCode.L);
    public (KeyCode, KeyCode) rotateRightYKeybind = (KeyCode.I, KeyCode.K);
    public KeyCode activateRightKeybind = KeyCode.Slash;
    public Color active, inactive;
    public GameObject itemLeft, itemRight;
    public Image leftImage, rightImage;
    public bool debug = false;

    void Update()
    {
        RotateLeft();
        RotateRight();
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
        if (Input.GetKey(rotateLeftXKeybind.Item1))
        {
            itemLeft.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(rotateLeftXKeybind.Item2))
        {
            itemLeft.transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(rotateLeftYKeybind.Item1))
        {
            itemLeft.transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(rotateLeftYKeybind.Item2))
        {
            itemLeft.transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
    void RotateRight()
    {
        if (!Input.GetKey(activateRightKeybind) || itemRight == null) return;
        if (Input.GetKey(rotateRightXKeybind.Item1))
        {
            itemRight.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(rotateRightXKeybind.Item2))
        {
            itemRight.transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(rotateRightYKeybind.Item1))
        {
            itemRight.transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(rotateRightYKeybind.Item2))
        {
            itemRight.transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}

