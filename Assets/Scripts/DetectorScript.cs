using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorScript : MonoBehaviour
{
    public UnitScript myUnit;

    private bool triggered = false;
    private HealthScript hs;
    private int myTeam = 0;

    // Start is called before the first frame update
    void Start()
    {
        myUnit = GetComponentInParent<UnitScript>();
        hs = GetComponentInParent<HealthScript>();
        myTeam = hs.team;
    }

    void Update()
    {
        if(triggered) myUnit.EnemyDetected();
    }


    void OnTriggerEnter2D(Collider2D otherCollider) // TODO: Rewrite this to use collision rather than triggers, make units rigidbodies (dynamic), and set physics table so same-team units don't collide.
    {
        // Does it have a healthscript?
        HealthScript target = otherCollider.gameObject.GetComponent<HealthScript>();
        if (target != null && target.team != myTeam && !target.dead)
        {
            triggered = true;
        } else
        {
            //triggered = false; // TODO: this 'stop triggering' clause will need to be more sophisticated later
        }
    }

}
