using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level
{
    public string scene;
    public Image image;
    public bool isUnlocked;
    public string displayText;
    public GameObject prefab;

    public Level(string scene, Image image, bool isUnlocked, string displayText, GameObject prefab)
    {
        this.scene = scene;
        this.image = image;
        this.isUnlocked = isUnlocked;
        this.displayText = displayText;
        this.prefab = prefab;
    }
}