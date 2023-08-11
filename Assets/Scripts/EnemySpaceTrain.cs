using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceTrain : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private SpawnManager _spawnManager;

    private bool _isAlive = true;
    private bool _isChasing = false;
    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();


    }

    // Update is called once per frame
    void Update()
    {
        
        CalculateMovement();


    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5f && _isAlive == true)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
        else if(transform.position.y < -5f && _isAlive == false)
        {
            Destroy(this.gameObject);
        }

        if (_isChasing)
        {
            Debug.Log("Moving Towards Player");
            Vector3 dir = this.transform.position - _player.transform.forward;
            dir = dir.normalized;
            this.transform.position -= dir * Time.deltaTime * (_speed);
        }
    }



    public void ChasePlayer(bool chasing)
    {
        Debug.Log("Is Chasing Player");
        _isChasing = chasing;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            

            Kill();

        }



        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScoreMethod();
            }

            

            Kill();


        }



        if (other.tag == "SpaceSword")
        {
            if (_player != null)
            {
                _player.AddScoreMethod();
            }

            

            Kill();


        }




    }


    public void Kill()
    {
        
        _speed = 0;
        _isAlive = false;
        

        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject);
        _spawnManager.EnemyCount();
    }

    public void StopRespawning()
    {
        _isAlive = false;
    }
}
