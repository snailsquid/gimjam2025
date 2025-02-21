using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelection;

    public void SelectLevel()
    {
        mainMenu.SetActive(false);
        levelSelection.SetActive(true);
    }
}
