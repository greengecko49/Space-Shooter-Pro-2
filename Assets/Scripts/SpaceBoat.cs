using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBoat : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    public GameObject _photonPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private bool _isRespawning = true;

    private Player _player;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Laser _laser;


    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();




    }


    // Update is called once per frame
    void Update()
    {

        CalculateMovement();
        if (Time.time > _canFire)
        {
            FirePhoton();
        }

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < 0 && _isRespawning == true)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            int x = Random.Range(0, 100);
            if (x > 49)
            {
                transform.Rotate(0.0f, 0.0f, 90.0f);
            }
            else
            {
                transform.Rotate(0.0f, 0.0f, -90.0f);
            }


        }

        if (transform.position.x == 12.0f && _isRespawning == true)
        {
            Destroy(this.gameObject);
        }
        else if (transform.position.x == -12.0f && _isRespawning == true)
        {
            Destroy(this.gameObject);
        }
    }

    void FirePhoton()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;
        Instantiate(_photonPrefab, transform.position, Quaternion.identity);
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

        if (other.tag == "SpaceSword")
        {
            if (_player != null)
            {
                _player.AddScoreMethod();
            }

            Kill();

        }

        if (other.tag == "Laser")
        {
            HomingMissile missile = other.transform.GetComponent<HomingMissile>();
            if (missile != null)
            {
                missile.Explosion();
                Destroy(other.gameObject);
            }

            if (_player != null)
            {
                _player.AddScoreMethod();

            }

            Laser laser = other.transform.GetComponent<Laser>();
            if (laser != null && laser.IsEnemyLaser() == false)
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScoreMethod();
                }

                Kill();
            }









        }

    }

    public void Kill()
    {
        _speed = 0;


        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject);
        _spawnManager.EnemyCount();

    }




    public void StopRespawning()
    {

        _isRespawning = false;
    }


}