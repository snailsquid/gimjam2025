using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class LogsManager : MonoBehaviour
{
    public static LogsManager instance { get; private set; }

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

    public List<Log> logs;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject logButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject logMenu;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] Sprite LogEnableNormal, LogEnableHover, LogDisableNormal, LogDisableHover;
    [SerializeField] Button logMenuButton;
    public List<Log> fullLogs = new List<Log>{
            new Log("Log #1 April 4th, 2030 11:10:35", "We have been put in charge of Neurotech's new project, Neuromechanical Engineering Using Generative Examination Network (N.E.U.G.E.N). It is supposedly an advanced AI training program specializing in mimicking human behavior to study how neural networks work in a simulated environment. \n Either way, we were introduced to it, and it has been given simple tasks to work with. We are to periodically expose it to new variables to check how the neural network grows over time.\n FINAL ASSESSMENT \n Item Assembled Successfully \n Cognitive Deviancy Index: 0.00 (Safe) \n Limb fluidity assessment recorded at 723.94/1000. Motor cohesion remains suboptimal—further iterative adjustments required."),
            new Log("Title 2", "2. Lorem ipsum"),
            new Log("Title 3", "3. Lorem ipsum"),
            new Log("Title 4", "4. Lorem ipsum"),
            new Log("Title 5", "5. Lorem ipsum")
    };
    void Start()
    {
        logs = new List<Log>();
        EnableButton(SecretManager.Instance.isLogsUnlocked);
        UpdateLogs();
    }

    public void Logs()
    {
        mainMenu.SetActive(false);
        logMenu.SetActive(true);
        UpdateLogs();
    }
    public void AddLog(int logNumber)
    {
        logs.Add(fullLogs[logNumber - 1]);
        UpdateLogs();
    }

    public void EnableButton(bool enable)
    {
        SpriteState spriteState = new SpriteState();

        if (enable)
        {
            spriteState = new SpriteState { highlightedSprite = LogEnableHover };

            logMenuButton.GetComponent<Image>().sprite = LogEnableNormal;
        }
        else
        {
            spriteState = new SpriteState { highlightedSprite = LogDisableHover };

            logMenuButton.GetComponent<Image>().sprite = LogDisableNormal;
        }
        logMenuButton.spriteState = spriteState;
    }
    public void DisplayTitleContent(string title_)
    {
        title.text = title_;
    }
    public void DisplayDescriptionContent(string description_)
    {
        description.text = description_;
    }

    public void UpdateLogs()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (Log log in logs)
        {
            GameObject newObject = Instantiate(logButton);
            newObject.transform.SetParent(content);
            newObject.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
            TMP_Text text = newObject.transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = log.title;
            Button button = newObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                DisplayTitleContent(log.title);
                DisplayDescriptionContent(log.description);
            });
        }
    }
}
