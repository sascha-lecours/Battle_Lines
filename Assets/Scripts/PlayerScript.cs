using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public string button1 = "1";
    public string button2 = "2";
    public string button3 = "3";
    public string button4 = "4";
    public string[] myButtons;
    public Transform[] myCards;
    public Transform[] myDeck;

    private int nextAvailableButtonIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        myButtons = new string[] { button1, button2, button3, button4 };
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
