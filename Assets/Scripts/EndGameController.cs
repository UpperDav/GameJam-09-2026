using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndGameController : MonoBehaviour
{
    [Header("Generated references")]
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text heading;
    [SerializeField] private TMP_Text message;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Scene to return to")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public static EndGameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (replayButton != null)
            replayButton.onClick.AddListener(Replay);
        else
            Debug.LogError("EndGameController: Replay button is not assigned.", this);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        else
            Debug.LogError("EndGameController: Main menu button is not assigned.", this);

        if (endGamePanel != null)
            endGamePanel.SetActive(false);
        else
            Debug.LogError("EndGameController: End game panel is not assigned.", this);

        var stringManager = FindFirstObjectByType<StringManager>();
        if (stringManager != null)
        {
            Debug.LogWarning("String manager not null subscribing");
            stringManager.OnHanged += ShowDefeat;
        }
    }

    public void ShowVictory()
    {
        Show("SPECTACLE TERMINÉ", "La dernière note résonne. Bravo !");
    }

    public void ShowDefeat()
    {
        Debug.LogWarning("show defeat");
        Debug.Log("ShowDefeat called - displaying game over screen.");
        Show("AU BOUT DU FIL", "Les quatre fils ont cédé. Le rideau tombe.");
    }

    private void Show(string title, string description)
    {
        Debug.LogWarning("In the show method");
        if (heading == null || message == null || endGamePanel == null)
        {
            Debug.LogWarning("EndGameController: Game-over UI references are not assigned.", this);
            return;
        }

        heading.text = title;
        message.text = description;
        endGamePanel.SetActive(true);
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Quick tests without waiting for gameplay to be finished.
    [ContextMenu("TEST - Victory (Play Mode)")]
    private void TestVictory() { if (Application.isPlaying) ShowVictory(); }

    [ContextMenu("TEST - Defeat (Play Mode)")]
    private void TestDefeat() { if (Application.isPlaying) ShowDefeat(); }

#if UNITY_EDITOR
    public void EditorAssign(GameObject panel, TMP_Text title, TMP_Text description, Button replay, Button menu)
    {
        endGamePanel = panel;
        heading = title;
        message = description;
        replayButton = replay;
        mainMenuButton = menu;
    }
#endif
}
