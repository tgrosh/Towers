using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRelativeToCamera : MonoBehaviour {
    public float objectScale = 1.0f;
    private Vector3 initialScale;

    // set the initial scale, and setup reference camera
    void Start()
    {
        // record initial scale, use this as a basis
        initialScale = transform.localScale;
    }

    // scale object relative to distance from camera plane
    void Update()
    {
        Plane plane = new Plane(Camera.main.transform.forward, Camera.main.transform.position);
        float dist = plane.GetDistanceToPoint(transform.position);
        transform.localScale = initialScale * dist * objectScale;
    }
}
