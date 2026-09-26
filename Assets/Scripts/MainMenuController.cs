
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";

    private GameObject title;
    private GameObject subtitle;
    private GameObject menuButtons;
    private GameObject instructionsPanel;

    void Awake()
    {
        Transform canvas = transform;

        title = canvas.Find("Title").gameObject;
        subtitle = canvas.Find("Subtitle").gameObject;
        menuButtons = canvas.Find("MenuButtons").gameObject;
        instructionsPanel = canvas.Find("InstructionsPanel").gameObject;

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
        Button button = menuButtons.transform
            .Find(buttonName).GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    public void ShowInstructions()
    {
        title.SetActive(false);
        subtitle.SetActive(false);
        menuButtons.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        title.SetActive(true);
        subtitle.SetActive(true);
        menuButtons.SetActive(true);
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
