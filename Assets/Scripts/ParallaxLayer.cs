using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float parallaxSpeed;
    [SerializeField] private float imageWidth;

    private float xPosition = 0;
    
    void Start()
    {
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxSpeed);
        float distanceToMove = cam.transform.position.x * parallaxSpeed;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y); 

        if (distanceMoved > xPosition + imageWidth)
        {
            xPosition += imageWidth;
        } else if (distanceMoved < xPosition - imageWidth)
        {
            xPosition -= imageWidth;
        }
    }
}
