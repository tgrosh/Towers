using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    public GameObject endPointCamera;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.gameObject.CompareTag("PlayerBase") && other.GetComponent<Agent>() != null)
        {
            UI.instance.ShowGameOver();
            endPointCamera.SetActive(true);
        }
    }
}
