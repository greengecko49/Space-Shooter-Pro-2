using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;

    private float _speedC = 6.0f;

    [SerializeField]
    private int _powerupID;

    Player player;
    [SerializeField]
    private AudioClip _clip;

    [SerializeField]
    public GameObject _explosionPrefab;

    private Player _player;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            direction = (_player.transform.position) - transform.position;
            direction = direction.normalized;
            transform.Translate(direction * _speedC * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        
        if (transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.AddAmmo();
                        break;
                    case 4:
                        player.Plus1Health();
                        break;
                    case 5:
                        player.ActivateSpaceSword();
                        break;
                    case 6:
                        player.ActivateMissiles();
                        break;
                    case 7:
                        player.ActivateSpeedDown();
                        break;
                    //case 8:
                        //player.ActivateSuperMode();
                        //break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }

            

            Destroy (this.gameObject);
        }


        //This is for when the powerups are destroyed
        if (other.tag == "Laser")
        {
            Laser laser = other.transform.GetComponent<Laser>();
            if (laser != null && laser.IsEnemyLaser() == true)
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy (this.gameObject);
            }
        }

    }
}
