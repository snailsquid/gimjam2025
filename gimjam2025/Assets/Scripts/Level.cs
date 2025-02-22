using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private Image image;
    [SerializeField] private bool isUnlocked;
    [SerializeField] private string displayText;
}