using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    public Camera camera;
    public float x, y;
    public void Start()
    {
        camera = GetComponent<Camera>();
    }
    public void SetAspectRatio()
    {
        camera.aspect = x / y;
    }
}

[CustomEditor(typeof(CameraAspectRatio))]
[ExecuteInEditMode]
public class CameraAspectRatioEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CameraAspectRatio cameraAspectRatio = (CameraAspectRatio)target;
        if (GUILayout.Button("Set Aspect Ratio"))
        {
            cameraAspectRatio.SetAspectRatio();
        }
    }
}