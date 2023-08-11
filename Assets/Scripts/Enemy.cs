using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _reverseLaserPrefab;
    [SerializeField]
    private GameObject _shield;

    private SpawnManager _spawnManager;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private bool _isAlive = true;
    private bool _isShieldOn;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_animator == null)
        {
            Debug.LogError("The animator is NULL");
        }

        int x = Random.Range(0, 100);
        if (x > 50)
        {
            _isShieldOn = true;
        }
        else
        {
            _isShieldOn = false;
        }

        _shield.SetActive(_isShieldOn);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && _isAlive == true)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    public void FireLaser()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }

            if (_isShieldOn == true)
            {
                _shield.SetActive(false);
                _isShieldOn = false;
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

            if (_isShieldOn == true)
            {
                _shield.SetActive(false);
                _isShieldOn = false;
                return;
            }

            Kill();


        }

        

        if (other.tag == "SpaceSword")
        {
            if(_player != null)
            {
                _player.AddScoreMethod();
            }

            if (_isShieldOn == true)
            {
                _shield.SetActive(false);
                _isShieldOn= false;
            }

            Kill();


        }

        


    }



    public void Kill()
    {
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _isAlive = false;
        _audioSource.Play();

        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.6f);
        _spawnManager.EnemyCount();
    }

    IEnumerator FireReverseLaser()
    {
        Instantiate(_reverseLaserPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5.0f);
    }

    public void ReverseLaserFiring()
    {
        StartCoroutine(FireReverseLaser());
    }

    public void StopRespawning()
    {
        _isAlive=false;
    }
}
