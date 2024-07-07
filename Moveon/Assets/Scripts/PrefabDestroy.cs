using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDestroy : MonoBehaviour
{ 
    void Update()
    {
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }
}
