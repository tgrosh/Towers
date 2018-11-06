using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCamera : MonoBehaviour {
    public float panSpeed;
    public float zoomSpeed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime);
        }
    }
}
