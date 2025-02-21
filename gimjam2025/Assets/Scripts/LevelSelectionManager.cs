using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager instance {get; private set;}

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public Button[] levelButtons;
    public static int highestLevelCleared;
    [SerializeField] private int levelSceneIndex;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelection;

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > highestLevelCleared)
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void SelectLevel()
    {
        mainMenu.SetActive(false);
        levelSelection.SetActive(true);
    }
}
