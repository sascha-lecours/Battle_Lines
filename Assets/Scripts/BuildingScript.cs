using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public int captureHealth = 3;
    public int team = 1;

    public int maxCaptureHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxCaptureHealth = captureHealth;
    }

    void Capture(int capturePower)
    {
        captureHealth -= capturePower; // TODO: Move to its own method?
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Is this a shot?
        UnitScript unit = otherCollider.gameObject.GetComponent<UnitScript>();
        if (unit != null)
        {
            // Avoid friendly fire, apply ranged immunity
            if (unit.myTeam != team)
            {
                Capture(unit.capturePower);
                unit.onCapture();
            }
        }
    }
}
