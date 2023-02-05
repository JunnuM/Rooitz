using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxPair
{
    public GameObject parallaxLayer;
    public float multiplier;
}

public class ParallaxHandler : MonoBehaviour
{
    [SerializeField]
    public GameObject FocusedGameObject;

    [SerializeField]
    public List<ParallaxPair> ParallaxLayers;

    private Transform focusedTransform;
    private Vector3 focusedPrevPosition;
    // Start is called before the first frame update
    void Start()
    {
        focusedTransform = FocusedGameObject.transform;
        focusedPrevPosition = focusedTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // We need to know how much the focused object moved and then move other layers accordingly
        Vector3 distanceMoved = focusedTransform.position - focusedPrevPosition;
        focusedPrevPosition = focusedTransform.position;
        foreach (ParallaxPair pair in ParallaxLayers)
        {
            Vector3 currPosition = pair.parallaxLayer.transform.position;
            // We just want to scale the x-axis. No need to touch the others yet.
            pair.parallaxLayer.transform.position = new Vector3(currPosition.x + (distanceMoved.x * pair.multiplier), currPosition.y, currPosition.z);
        }
    }
}
