using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private bool _isAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        if (_animator == null)
        {
            Debug.LogError("The animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && _isAlive == true)
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


        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
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

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _isAlive = false;
            _audioSource.Play();

            Destroy(this.gameObject, 2.6f);
            
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            
            if (_player != null)
            {
                _player.AddScoreMethod();
            }


            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _isAlive = false;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
            
        }
    }
}
