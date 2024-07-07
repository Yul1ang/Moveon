using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhiloAnim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if ((rb.velocity.magnitude > 0.01f))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }
}
