using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLaserReverseShot : MonoBehaviour
{
    [SerializeField]
    private int _speed = 4;



    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.Translate(Vector3.up * _speed *Time.deltaTime);

        if(transform.position.y > 8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
        }
    }
}
