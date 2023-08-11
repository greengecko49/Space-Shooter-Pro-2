using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTank : MonoBehaviour
{


    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;


    private Player _player;
    private float _fireRate = 3.0f;
    private float _canFire = 0f;

    private bool _isRespawning = true;
    private bool _fireLaser = true;

    public bool _isDodging;
    public float _laserXpos;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Laser _laser;

    // Start is called before the first frame update
    void Start()
    {

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _laser = GetComponent<Laser>();
    }

    // Update is called once per frame
    void Update()
    {
        
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }


    }


    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -5f && _isRespawning == true)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
        else if (transform.position.y < -5f && _isRespawning == false)
        {
            Destroy(this.gameObject);
        }
        if (_isDodging)
        {
            if (_laserXpos > this.transform.position.x)
            {
                this.transform.position += Vector3.left * Time.deltaTime * (_speed * 2);
            }
            else
            {
                this.transform.position -= Vector3.right * Time.deltaTime * (_speed * 2);
            }
        }
    }


    public void DodgeLaser(bool dodging, float xPosLaser)
    {
        _isDodging = dodging;
        _laserXpos = xPosLaser;
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
            HomingMissile missile = other.transform.GetComponent<HomingMissile>();
            if(missile != null)
            {
                missile.Explosion();
            }

            Destroy(other.gameObject);
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

        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject);
        _spawnManager.EnemyCount();
    }


    public void StopRespawning()
    {
        _isRespawning = false;
    }







}
