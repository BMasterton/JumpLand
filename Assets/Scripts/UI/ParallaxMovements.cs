using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovements : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float matchCamXMovement = 0.0f; // range [0..1]. 1 matches camera move 
    [SerializeField] private float matchCamYMovement = 0.0f;// range [0..1]. 1 matches camera move 
    [SerializeField] private Vector2 offset; // allows us to offset our postiion relative to the camera
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() // use late update to make sure all object movement updates have happened already 
    {
        // move  the object relative to the camera
        transform.position = new Vector2(cam.position.x * matchCamXMovement, cam.position.y * matchCamYMovement) + offset;
    }
}
