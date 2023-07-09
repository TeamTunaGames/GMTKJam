using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] GameObject mainTitleScreen;
    [SerializeField] GameObject levelSelectScreen;

    private void Start()
    {
        if (GameManager.Instance != null && CanvasScript.Instance != null)
        {
            GameManager.Instance.gameObject.SetActive(false);
            CanvasScript.Instance.gameObject.SetActive(false);
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if (GameManager.Instance != null && CanvasScript.Instance != null)
        {
            GameManager.Instance.gameObject.SetActive(true);
            CanvasScript.Instance.gameObject.SetActive(true);
        }
    }

    public void LoadTitleScreen()
    {
        mainTitleScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
    }

    public void LoadLevelSelectScreen()
    {
        mainTitleScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.Instance.levelNumber = 0;
        GameManager.Instance.LoadNextLevel();
    }
}
