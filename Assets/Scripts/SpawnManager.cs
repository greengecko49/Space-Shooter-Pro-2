using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemySpaceTrain;
    [SerializeField]
    private GameObject _enemySpaceTank;
    [SerializeField]
    private GameObject _enemySpaceBoat;
    [SerializeField]
    private GameObject _bossShip;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    private int _waveCount;
    private int _enemyCount;
    [SerializeField]
    private int[] _waveEnemyTotal;
    [SerializeField]
    private float _enemyTimer;
    [SerializeField]
    private float _spaceTankTimer;
    [SerializeField]
    private float _spaceTrainTimer;
    [SerializeField]
    private float _spaceBoatTimer;

    private UIManager _uiManager;

    [SerializeField]
    private bool _isWaveActive = true;
    private bool _stopSpawning = false;

    private int _powerUpToSpawn;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        StartCoroutine(WaveSpawn());
    }




    public void StartSpawning()
    {
        _uiManager.UpdateWave(_waveCount + 1);
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnSpaceBoatRoutine());
        StartCoroutine(SpawnSpaceTankRoutine());
        StartCoroutine(SpawnSpaceTrainRoutine());


    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(_enemyTimer);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            ChoosAPowerUp();
            Instantiate(_powerups[_powerUpToSpawn], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    IEnumerator WaveSpawn()
    {
        while (_isWaveActive == true && _stopSpawning == false)
        {
            if (_enemyCount == _waveEnemyTotal[_waveCount])
            {
                Debug.Log("Stop Spawning");

                _stopSpawning = true;
                Enemy[] enemychildren = _enemyContainer.GetComponentsInChildren<Enemy>();
                for (int i = 0; i < enemychildren.Length; i++)
                {
                    enemychildren[i].StopRespawning();
                }

                SpaceBoat[] spaceboatchildren = _enemyContainer.GetComponentsInChildren<SpaceBoat>();
                for (int i = 0; i < spaceboatchildren.Length; i++)
                {
                    spaceboatchildren[i].StopRespawning();
                }

                EnemySpaceTrain[] enemyspacetrainchildren = _enemyContainer.GetComponentsInChildren<EnemySpaceTrain>();
                for (int i = 0; i < enemyspacetrainchildren.Length; i++)
                {
                    enemyspacetrainchildren[i].StopRespawning();
                }

                SpaceTank[] spacetankchildren = _enemyContainer.GetComponentsInChildren<SpaceTank>();
                for (int i = 0; i < spacetankchildren.Length; i++)
                {
                    spacetankchildren[i].StopRespawning();
                }
                _waveCount++;
                yield return new WaitForSeconds(8.0f);
                _stopSpawning = false;
                _enemyCount = 0;
                StartSpawning();

            }
            else if(_waveCount == 5)
            {
                _stopSpawning = true;
                SpawnBoss();
            }

            yield return null;

        }
    }


    void ChoosAPowerUp()
    {
        int weightedTotal = 100;

        int[] powerupTable =
        {
            30, //ammo
            10, //missile
            10, //health
            10, //shield
            10, //speed
            10, // sword
            10, //tripleshot
            10, //negative speed down
            //10, //supermode
        };
        int[] PowerupToAward =
        {
            3, //ammo
            6, //missile
            4, //health
            2, //shield
            1, //speed
            5, //sword
            0, //tripleshot
            7, //negative speed down
            //8, //supermode
        };

        foreach (var item in powerupTable)
        {
            weightedTotal += item;

        }

        var randomNumber = Random.Range(0, weightedTotal);
        var i = 0;

        foreach(var weight in powerupTable)
        {
            if (randomNumber <= weight)
            {
                _powerUpToSpawn = PowerupToAward[i];
                return;
            }
            else
            {

                i++;
                randomNumber -= weight;
            }

        }
        

    }


    IEnumerator SpawnSpaceTrainRoutine()
    {
        yield return new WaitForSeconds(_spaceTrainTimer);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newSpaceTrain = Instantiate(_enemySpaceTrain, posToSpawn, Quaternion.identity);
            newSpaceTrain.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(7.0f);
        }
    }

    IEnumerator SpawnSpaceBoatRoutine()
    {
        yield return new WaitForSeconds(_spaceBoatTimer);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(0, 10, 0);
            GameObject newSpaceBoat = Instantiate(_enemySpaceBoat, posToSpawn, Quaternion.identity);
            newSpaceBoat.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(7.0f);
        }
    }

    IEnumerator SpawnSpaceTankRoutine()
    {
        yield return new WaitForSeconds(_spaceTankTimer);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newSpaceTank = Instantiate(_enemySpaceTank, posToSpawn, Quaternion.identity);
            newSpaceTank.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(7.0f);
        }
    }

    void SpawnBoss()
    {
        Vector3 posToSpawn = new Vector3(0,10,0);
        GameObject newBoss = Instantiate(_bossShip, posToSpawn, Quaternion.identity);
    }




    public void EnemyCount()
    {
        _enemyCount++;
    }
}
