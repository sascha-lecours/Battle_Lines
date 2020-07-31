using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public string button1 = "1";
    public string button2 = "2";
    public string button3 = "3";
    public string button4 = "4";
    public string cancelButton = "q";
    public string[] myButtons;
    public GameObject[] myCards;
    public GameObject[] myBuildings;
    public GameObject selectedCard;
    public int moneyPerTick = 4;
    public float timePerMoneyTick = 3f;
    public int currentMoney = 0;
    private float moneyTimer = 0f;
    public Text MoneyCounter;

    private BuildingScript[] myBuildingScripts = { null, null, null }; 

    enum State
    {
        CardSelection,
        PlacingCard
    }
    State state;

    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }

    // Start is called before the first frame update
    void Start()
    {
        myButtons = new string[] { button1, button2, button3, button4 };
        for(var i = 0; i < myCards.Length; i++)
        {
            var cs = myCards[i].GetComponent<CardScript>() as CardScript;
            cs.SetButton(myButtons[i]);
        }
        for(var i = 0; i < myBuildings.Length; i++)
        {
            myBuildingScripts[i] = myBuildings[i].GetComponent<BuildingScript>() as BuildingScript;
        }
    }



    // Update is called once per frame
    void Update()
    {
        incrementMoney();
        // Update state
        ContinueState();
    }

    void SelectCard(GameObject card)
    {
        UnselectCard();
        var selCardScript = card.GetComponent<CardScript>();
        selCardScript.selected = true;
        selectedCard = card;
    }

    void incrementMoney()
    {
        moneyTimer += Time.deltaTime;
        if(moneyTimer >= timePerMoneyTick)
        {
            currentMoney += moneyPerTick;
            moneyTimer = 0;
            updateMoneyCounter();
        }
    }

    void updateMoneyCounter()
    {
        MoneyCounter.text = "$" + currentMoney;
    }

    void SetOrKeepState(State state)
    {
        if (this.state == state) return;
        EnterState(state);
    }

    void UnselectCard()
    {
        if(selectedCard != null)
        {
            var selCardScript = selectedCard.GetComponent<CardScript>();
            selCardScript.selected = false;
            selectedCard = null;
        }
    }

    void tryToSelectCard(GameObject card)
    {
        var toSpawn = card.GetComponent<CardScript>().spawnSubject;
        var us = toSpawn.GetComponent<UnitScript>() as UnitScript;
        if (currentMoney >= us.cost)
        {
            SelectCard(card);
            EnterState(State.PlacingCard);
        }

        // TODO - else play some sort of "nope" sound
        
    }

    void ExitState()
    {
    }

    void PayForUnit(UnitScript us)
    {
        currentMoney -= us.cost;
        updateMoneyCounter();
    }

    void EnterState(State state)
    {
        ExitState();
        switch (state)
        {
            case State.CardSelection:
                // TODO: Display card selection buttons, grey out building buttons.
                UnselectCard();
                break;
            case State.PlacingCard:
                // TODO: Grey out card selection buttons, Set buildings to display buttons.
                break;
        }

        this.state = state;
        stateStartTime = Time.time;
    }

    void ContinueState()
    {
        switch (state)
        {

            case State.CardSelection:
                // Take input to select card
                for (var i = 0; i < myCards.Length; i++)
                {
                    if (Input.GetKeyDown(myButtons[i])) {
                        tryToSelectCard(myCards[i]);
                    }
                }
                
                
                break;

            case State.PlacingCard:
                // Take input to select lane
                for (var i = 0; i < myBuildings.Length; i++)
                {
                    if (Input.GetKeyDown(myButtons[i]) && i < 3)
                    {
                        var toSpawn = selectedCard.GetComponent<CardScript>().spawnSubject;
                        var us = toSpawn.GetComponent<UnitScript>() as UnitScript;
                        PayForUnit(us);
                        myBuildingScripts[i].spawnUnit(selectedCard.GetComponent<CardScript>().spawnSubject);
                        EnterState(State.CardSelection);
                        break;
                    }
                }

                if (Input.GetKeyDown(cancelButton))
                {
                    EnterState(State.CardSelection);
                }
                // Once a lane is chosen, spawn the unit, update the cooldown/greyed out status of the card if applicable, deduct cost
                break;
        }
    }
}
