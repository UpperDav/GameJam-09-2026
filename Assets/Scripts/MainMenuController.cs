
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private GameObject? title;
    private GameObject? subtitle;
    private GameObject? menuButtons;
    private GameObject? instructionsPanel;

    void Awake()
    {
        title = transform.Find("Title").gameObject;
        subtitle = transform.Find("Subtitle").gameObject;
        menuButtons = transform.Find("MenuButtons").gameObject;
        instructionsPanel = transform.Find("InstructionsPanel").gameObject;

        // Connexion automatique des boutons
        Connect("PlayButton", PlayGame);
        Connect("InstructionsButton", ShowInstructions);
        Connect("QuitButton", QuitGame);

        Transform back = instructionsPanel.transform
            .Find("reno/BackButton");

        back.GetComponent<Button>().onClick.RemoveAllListeners();
        back.GetComponent<Button>().onClick.AddListener(HideInstructions);

        HideInstructions();
    }

    void Connect(string buttonName, UnityEngine.Events.UnityAction action)
    {
        if (menuButtons == null)
            return;

        Button button = menuButtons.transform
            .Find(buttonName).GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public void ShowInstructions()
    {
        title?.SetActive(false);
        subtitle?.SetActive(false);
        menuButtons?.SetActive(false);
        instructionsPanel?.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel?.SetActive(false);
        title?.SetActive(true);
        subtitle?.SetActive(true);
        menuButtons?.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
