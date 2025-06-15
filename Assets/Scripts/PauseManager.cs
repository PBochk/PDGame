using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject pauseScreen;
    private PauseUI pauseUI;
    private Button resumeButton;
    private Button exitButton;
    private void Start()
    {
        pauseUI = GameObject.FindGameObjectWithTag("PauseUI").GetComponent<PauseUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        pauseScreen = pauseUI.pauseScreen;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;
        resumeButton = pauseUI.resume;
        resumeButton.onClick.AddListener(ResumeGame);
        exitButton = pauseUI.exit;
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
        resumeButton.onClick.RemoveAllListeners();
        pauseScreen.SetActive(false);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
