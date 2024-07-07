using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private GameObject playerStopFlag;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !playerStopFlag.GetComponent<SceneControl>().playerStop)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }
}
