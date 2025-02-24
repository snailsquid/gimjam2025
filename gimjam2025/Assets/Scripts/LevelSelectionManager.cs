using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public GameObject levelButton;
    public GameObject levelImage3D;
    public List<Level> levels;
    [SerializeField] private List<Image> image;
    [SerializeField] private List<GameObject> image3D;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelection;
    [SerializeField] private Transform content;
    [SerializeField] private Transform contentImage3D;
    [SerializeField] private LoadingScreen loadingScreen;
    private int xPos;

    void Start()
    {
        levels = new List<Level>
        {
            new Level("Level 1", image[0], true, "Level 1", image3D[0]),
            new Level("Level 2", image[1], true, "Level 2", image3D[1]),
            new Level("Level 3", image[2], false, "Level 3", image3D[2]),
            new Level("Level 4", image[3], false, "Level 4", image3D[3]),
            new Level("Level 5", image[4], false, "Level 5", image3D[4]),
            new Level("Level 6", image[5], false, "Level 6", image3D[5])
        };
        UpdateLevels();
    }
    /*void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > highestLevelCleared)
            {
                levelButtons[i].interactable = false;
            }
        }
    }*/

    public void SelectLevel()
    {
        mainMenu.SetActive(false);
        levelSelection.SetActive(true);
        UpdateLevels();
    }

    public void UpdateLevels()
    {
    xPos = 0;
    foreach(Transform child1 in content)
    {
        Destroy(child1.gameObject);
    }
    foreach(Transform child2 in contentImage3D)
    {
        Destroy(child2.gameObject);
    }
    foreach(Level level in levels)
    {
        GameObject newObject = Instantiate(levelButton);
        newObject.transform.SetParent(content);
        newObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        TMP_Text text = newObject.transform.GetChild(1).GetComponent<TMP_Text>();
        text.text = level.displayText;

        Button button = newObject.GetComponent<Button>();
        button.onClick.AddListener(()=>{
            loadingScreen.LoadLevelBtn(level.scene);
        });

        if(!level.isUnlocked)
        {
            button.interactable = false;
        }

        GameObject newImage3D = Instantiate(levelImage3D, new Vector3(xPos, 0, 0), Quaternion.identity);
        newImage3D.transform.SetParent(contentImage3D);

        GameObject newImage3DObject = Instantiate(level.prefab, new Vector3(xPos, 0, 0), Quaternion.identity);
        newImage3DObject.transform.SetParent(newImage3D.transform);
        xPos -= 20;

        Camera cameraToRender = newImage3D.transform.GetChild(0).GetComponent<Camera>();
        RenderTexture renderTexture = new RenderTexture(256,256,0);
        cameraToRender.targetTexture = renderTexture;
        RawImage targetRawImage = button.transform.GetChild(0).GetComponent<RawImage>();
        targetRawImage.texture = renderTexture;
    }
    }
}
