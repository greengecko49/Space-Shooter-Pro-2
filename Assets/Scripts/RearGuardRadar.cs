using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearGuardRadar : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            transform.parent.GetComponent<Enemy>().ReverseLaserFiring();
        }
    }



}
