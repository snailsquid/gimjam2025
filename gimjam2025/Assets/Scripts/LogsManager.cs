using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class LogsManager : MonoBehaviour
{
    public static LogsManager instance {get; private set;}

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
    
    void Start()
    {
        logs = new List<Log>
        {
            new Log("Title 1", "1. Lorem ipsum"),
            new Log("Title 2", "2. Lorem ipsum"),
            new Log("Title 3", "3. Lorem ipsum"),
            new Log("Title 4", "4. Lorem ipsum"),
            new Log("Title 5", "5. Lorem ipsum")
        };

        UpdateLogs();
    }

    public void Logs()
    {
        mainMenu.SetActive(false);
        logMenu.SetActive(true);
        UpdateLogs();
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
        foreach(Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach(Log log in logs)
        {
            GameObject newObject = Instantiate(logButton);
            newObject.transform.SetParent(content);
            TMP_Text text = newObject.transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = log.title;
            Button button = newObject.GetComponent<Button>();
            button.onClick.AddListener(()=>{
                DisplayTitleContent(log.title);
                DisplayDescriptionContent(log.description);
            });
        }
    }
}
