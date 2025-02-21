using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EsapeMenuScript : MonoBehaviour
{
    public GameObject EscapeMenu;
    public Volume globalVolume;
    private DepthOfField dof;

    private bool toggleDof;
    public float focusDistance;

    private void Start()
    {
        EscapeMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EscapeMenu.activeSelf == false)
            {
                ShowEscapeMenu();
                toggleBg();
            }
            else
            {
                HideEscapeMenu();
                toggleBg();
            }
        }
    }
    private void ShowEscapeMenu()
    {
        EscapeMenu.SetActive(true);
    }
    private void HideEscapeMenu()
    {
        EscapeMenu.SetActive(false);
    }
    private void toggleBg()
    {
        toggleDof = !toggleDof;
        if (globalVolume.profile.TryGet(out dof))
        {
            dof.active = toggleDof;
            dof.focusDistance.value = focusDistance;
        }
    }
}
