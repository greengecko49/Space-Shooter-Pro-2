using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamRadar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Radar Detected " + other.name);

        if(other.tag == "Player")
        {

            if(transform.position.y > other.transform.position.y)
            {
                transform.parent.GetComponent<EnemySpaceTrain>().ChasePlayer(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            transform.parent.GetComponent<EnemySpaceTrain>().ChasePlayer(false);
        }
    }
}
