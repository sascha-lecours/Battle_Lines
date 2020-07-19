﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    #region Public Properties
    public Image cardImage;
    public string cardName;
    public Transform spawnSubject;
    public Text title;
    public Text cardText;

    #endregion


    #region Private Properties
    private UnitScript us;
    private HealthScript hs;
    private MoveScript ms;

    private int hp;
    private float speed;
    private int damage;
    private float dps;
    private string specialText = "Special text here!";
    private int cost = 0;

    #endregion

    string cardScriptTemplate
    {
        get { return "HP: " + hp + " DPS: " + dps + "\n" + specialText; }
    }

    // Start is called before the first frame update
    void Start()
    {
        us = spawnSubject.GetComponent<UnitScript>();
        if (us != null) // If the spawn subject is a unit, just use its values for the card.
        {
            hs = spawnSubject.GetComponent<HealthScript>();
            ms = spawnSubject.GetComponent<MoveScript>();
            cardImage.sprite = spawnSubject.GetComponent<SpriteRenderer>().sprite;
            hp = hs.hp;
            speed = ms.speed.x;
            specialText = us.specialText;
            damage = us.attackDamage;
            dps = us.attackDamage / us.attackInterval;
            dps = Mathf.Round(dps * 100f) / 100f;
            cardText.text = cardScriptTemplate;
            title.text = us.name;
            // TODO: get cost from unitscript

        }



        // TODO: If it's not a unit, it's some sort of special effect. We'll need to look elsewhere then for its icon and text, etc. Probably boolean to make custom?
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
