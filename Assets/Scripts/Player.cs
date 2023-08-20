using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedBoost = 2.0f;
    [SerializeField]
    private float _thrusterSpeed = 10.5f;
    private Coroutine _cooldown;
    [SerializeField]
    private int _ammo = 15;
    [SerializeField]
    private int _maxAmmo = 30;
    [SerializeField]
    private float _fuel = 100f;
    [SerializeField]
    private float _fuelUsage = 8f;
    [SerializeField]
    private int _shieldHealth = 3;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private GameObject _spaceSword;
    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private GameObject _speedDown;
    [SerializeField]
    private GameObject _superMode;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private SpriteRenderer _playerShieldColor;
    [SerializeField]
    private Color _shieldColorFull, _shieldColorHalf, _shieldColorMinimum, _shieldColor0;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;

    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private bool _isFuelCoolDownActive = false;
    [SerializeField]
    private bool _isSpaceSwordActive = false;
    private bool _isMissileActive = false;
    private bool _isSpeedDownActive = false;
    private bool _isSuperModeActive = false;

    private SpawnManager _spawnManager;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;

    private CameraShake _cameraShake;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _cameraShake = Camera.main.GetComponent<CameraShake>();

        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager assigned");
        }

        _shieldVisualizer.SetActive(false);
        _spaceSword.SetActive(false);
        _speedDown.SetActive(false);
        _superMode.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        //player movement variables and Inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (_isSpeedBoostActive == false)
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * _speedBoost * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * _speedBoost * Time.deltaTime);
        }

        //Thruster Method

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_isFuelCoolDownActive && _fuel > 0)
            {
                if (_cooldown != null)
                {
                    StopCoroutine(_cooldown);

                }
                _isFuelCoolDownActive = false;
            }  
            if (_fuel > 0)
            {
                transform.Translate(Vector3.right * horizontalInput * _thrusterSpeed * Time.deltaTime);
                transform.Translate(Vector3.up * verticalInput * _thrusterSpeed * Time.deltaTime);
                _fuel -= _fuelUsage * Time.deltaTime;
                _uiManager.UpdateThrusterUI(_fuel);
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !_isFuelCoolDownActive)
        {
            
            _cooldown = StartCoroutine(ThrusterCoolDownRoutine());
        }


        //player movement boundaries
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isMissileActive == true)
        {
            Instantiate(_missilePrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else if (_ammo > 0)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _ammo--;
            _uiManager.UpdateAmmo(_ammo);
        }
        
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _shieldHealth--;

            switch (_shieldHealth)
            {
                case 2:
                    _playerShieldColor.color = _shieldColorHalf;
                    break;
                case 1:
                    _playerShieldColor.color = _shieldColorMinimum;
                    break;
                case 0:
                    _playerShieldColor.color = _shieldColor0;
                    _isShieldsActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
            }
             return;
        }
        
        
            _lives -= 1;
        StartCoroutine(_cameraShake.Shake(.10f, .2f));

        _uiManager.Updatelives(_lives);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        
        

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject); 
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());

    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldHealth = 3;
        _playerShieldColor.color = _shieldColorFull;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScoreMethod()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator ThrusterCoolDownRoutine()
    {
        _isFuelCoolDownActive = true;
        yield return new WaitForSeconds(1f);
        while (_fuel <= 100)
        {
            _fuel += 15 * Time.deltaTime;
            if (_fuel >= 100f)
            {
                _fuel = 100f;
                _isFuelCoolDownActive = false;
                
            }

            _uiManager.UpdateThrusterUI(_fuel);
            yield return null;
        }

        
    }

    public void AddAmmo()
    {
        _ammo += 15;
        if (_ammo > _maxAmmo)
        {
            _ammo = _maxAmmo;
        }

        _uiManager.UpdateAmmo(_ammo);
    }

    public void Plus1Health()
    {
        if (_lives == 3)
        {
            Debug.Log("Lives at MAX");
            return;
        }

        _lives++;
        _uiManager.Updatelives(_lives);

        if (_lives == 3)
        {
            _leftEngine.SetActive(false);
        }
        else if (_lives == 2)
        {
            _rightEngine.SetActive(false);
        }
    }

    public void ActivateSpaceSword()
    {
        _isSpaceSwordActive = true;
        _spaceSword.gameObject.SetActive(true);
        StartCoroutine(SpaceSwordPowerDown());

    }

    IEnumerator SpaceSwordPowerDown()
    {
        yield return new WaitForSeconds(15.0f);
        _isSpaceSwordActive = false;
        _spaceSword.gameObject.SetActive(false);
    }

    public void ActivateMissiles()
    {
        _isMissileActive = true;
        StartCoroutine(MissilePowerDown());
    }

    IEnumerator MissilePowerDown()
    {
        yield return new WaitForSeconds(10.0f);
        _isMissileActive = false;
    }

    public void ActivateSpeedDown()
    {
        _isSpeedDownActive = true;
        _speedDown.SetActive(true);
        _speed -= 2.0f;
        StartCoroutine(SpeedDownPowerDown());
    }

    IEnumerator SpeedDownPowerDown()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedDownActive=false;
        _speedDown.gameObject.SetActive(false);
        _speed += 2.0f;
    }

    //public void ActivateSuperMode()
    //{
        //_isSuperModeActive = true;
        //_superMode.SetActive(true);
        //if(_isSuperModeActive == true)
        //{
            //_lives += 3;
            //_lives++;
        //}
        
        //StartCoroutine(SuperModePowerDown());
    //}

    //IEnumerator SuperModePowerDown()
    //{
        //yield return new WaitForSeconds(8.0f);
        //_isSuperModeActive=false;
        //_superMode.gameObject.SetActive(false);
    //}
    

}
