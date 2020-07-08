using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    public int damage = 1;


    public int team = 0;

    public bool spinning = false;
    public float speedRotate = 0f;

    public Transform destroyEffect = null;
    public float timeToLive = 20f; // seconds

    private HealthScript myTargetHealthScript = null;

    void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    private void Update()
    {
        if (spinning)
        {
            transform.Rotate(Vector3.forward * speedRotate * Time.deltaTime);
        }
    }

    public void onImpact(Transform target)
    {
        if (destroyEffect != null)
        {
            var effect = Instantiate(destroyEffect);
            effect.transform.position = gameObject.GetComponent<Transform>().position;

        }
        // Play impact sound
        myTargetHealthScript = target.gameObject.GetComponent<HealthScript>();
        if (myTargetHealthScript != null && myTargetHealthScript.active && !myTargetHealthScript.immuneToShots)
            // TODO: Sound effect here?

        Destroy(gameObject);
    }
}
