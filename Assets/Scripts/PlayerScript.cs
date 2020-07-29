﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public string button1 = "1";
    public string button2 = "2";
    public string button3 = "3";
    public string button4 = "4";
    public string[] myButtons;
    public GameObject[] myCards;
    public GameObject selectedCard;

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
    }



    // Update is called once per frame
    void Update()
    {
        // Update state
        ContinueState();
    }

    void SelectCard(GameObject card)
    {
        var selCardScript = card.GetComponent<CardScript>();
        selCardScript.selected = true;
        selectedCard = card;
    }

    void SetOrKeepState(State state)
    {
        if (this.state == state) return;
        EnterState(state);
    }

    void ExitState()
    {
    }

    void EnterState(State state)
    {
        ExitState();
        switch (state)
        {
            case State.CardSelection:
                // Display card selection buttons, grey out building buttons.
                // Set SelectedCard to null.
                break;
            case State.PlacingCard:
                // Grey out card selection buttons, Set buildings to display buttons.
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
                        // TODO: Check if can afford selected card, off cooldown, etc.
                        SelectCard(myCards[i]);
                        // transition to placement here
                    }
                }
                
                
                break;

            case State.PlacingCard:
                // Choose a lane, if applicable.
                // Also take input to cancel and return to selection.
                // Once a lane is chosen, spawn the unit, update the cooldown/greyed out status of the card if applicable, deduct cost
                break;
        }
    }
}
