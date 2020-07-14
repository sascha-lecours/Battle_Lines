using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public Texture cardImage;
    public string cardName;
    public Transform spawnSubject;
    public int cost = 0;
    Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
        ren.material.mainTexture = cardImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
