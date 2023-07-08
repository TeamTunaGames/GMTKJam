using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : Singleton<CanvasScript>
{
    public List<RectTransform> dicePositions;
    [SerializeField] private GameObject dicePositionGroup;
    public List<GameObject> dices;
    [SerializeField] GameObject diceGroup;
    public Sprite[] diceSprites;

    private void Start()
    {
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

    private void OnGUI()
    {
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

    public void SortDices()
    {
        dices.Sort((left, right) => left.transform.position.x.CompareTo(right.transform.position.x));

        for (int i = 0; i < dices.Count; i++)
        {
            dices[i].GetComponent<DiceScript>().targetPosition = dicePositions[i].position.x;
        }
    }
}