using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigDetector : MonoBehaviour
{
    public bool collided;

    private void Start()
    {
        collided = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Philosopher has collided with Trig!");
        collided = true;
    }

    public void ResetTrig()
    {
        collided = false;
    }
}
