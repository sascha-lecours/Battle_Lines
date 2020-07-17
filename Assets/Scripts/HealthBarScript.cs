using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    HealthScript myHealthScript;
    Vector3 localScale;
    private float scaleFactor = 0.25f;
    private float healthFraction = 1f;

    // Start is called before the first frame update
    void Start()
    {
        myHealthScript = GetComponentInParent<HealthScript>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        healthFraction = (Mathf.Max(0, (float)myHealthScript.hp) / (float)myHealthScript.maxHp);
        localScale.x = healthFraction * scaleFactor;
        transform.localScale = localScale;
    }
}
