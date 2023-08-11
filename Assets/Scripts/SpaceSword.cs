using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSword : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
        }
    }
}
