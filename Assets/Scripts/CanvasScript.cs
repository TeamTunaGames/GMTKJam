using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : Singleton<CanvasScript>
{
    public List<RectTransform> dicePositions;
    [SerializeField] private GameObject dicePositionGroup;
    public List<GameObject> dices;
    [SerializeField] GameObject diceGroup;
    public Sprite[] diceSprites;
    [SerializeField] private GameObject gameUIGroup;
    [SerializeField] private GameObject pauseMenu;
    public bool paused;
    [SerializeField] private GameObject winText;
    [SerializeField] private Animator transitionAnimator;

    private void Start()
    {
        transitionAnimator.gameObject.SetActive(true);
        transitionAnimator.CrossFade("FadeOut",0);
        //Get all available dices
        dices = new List<GameObject>();

        for (int i = 0; i < diceGroup.transform.childCount; i++)
        {
            GameObject currentDice = diceGroup.transform.GetChild(i).gameObject;

            if (currentDice.activeSelf)
            {
                dices.Add(currentDice);
            }
        }
        //Get all available positions
        dicePositions = new List<RectTransform>();

        for (int i = 0; i < dicePositionGroup.transform.childCount; i++)
        {
            RectTransform currentDicePosition = dicePositionGroup.transform.GetChild(i).GetComponent<RectTransform>();

            if (currentDicePosition.gameObject.activeSelf)
            {
                dicePositions.Add(currentDicePosition);
            }
        }
        //Make dices align to the center
        if (dicePositions.Count % 2 == 0)
        {
            dicePositionGroup.transform.Translate(new Vector3(-60, 0));
        }

        SortDices(null);
    }

    public void SortDices(GameObject heldDice)
    {
        dices.Sort((left, right) => left.transform.position.x.CompareTo(right.transform.position.x));

        for (int i = 0; i < dices.Count; i++)
        {
            if (dices[i] != heldDice)
            {
                dices[i].GetComponent<DiceScript>().targetPosition = dicePositions[i].position.x;
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameUIGroup.SetActive(false);
        MusicManager.Instance.PauseMusic();
        paused = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameUIGroup.SetActive(true);
        MusicManager.Instance.UnpauseMusic();
        paused = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
        Unpause();
    }

    private void OnGUI()
    {
        SortDices(null);
    }

    public IEnumerator WinAnimation()
    {
        winText.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        transitionAnimator.CrossFade("FadeIn", 0);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}