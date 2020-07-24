using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealthBarScript : MonoBehaviour
{
    BuildingScript myBuildingScript;
    Vector3 localScale;
    private float scaleFactor = 0.5f;
    private float healthFraction = 1f;

    // Start is called before the first frame update
    void Start()
    {
        myBuildingScript = GetComponentInParent<BuildingScript>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        healthFraction = (Mathf.Max(0, (float)myBuildingScript.captureHealth) / (float)myBuildingScript.maxCaptureHealth);
        localScale.x = healthFraction * scaleFactor;
        transform.localScale = localScale;
    }
}
