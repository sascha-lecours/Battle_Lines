using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    public int captureHealth = 3;
    public int team = 1;
    public Transform mySpawnPoint;

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

    public void spawnUnit(Transform unit)
    {
        var newSpawn = Instantiate(unit) as Transform;
        var us = newSpawn.GetComponent<UnitScript>();
        var hs = newSpawn.GetComponent<HealthScript>();
        hs.team = team;
        if (team == 2)
        {
            us.facing = -1;
        }
        newSpawn.position = mySpawnPoint.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getCaptured(UnitScript capturer)
    {
        if (capturer.myTeam != team)
        {
            Capture(capturer.capturePower);
            capturer.onCapture();
        }
    }
}
