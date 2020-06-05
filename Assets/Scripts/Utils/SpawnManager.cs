using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    #region containers
    [SerializeField]
    private GameObject _enemyContainer = null;

    [SerializeField]
    private GameObject _powerUpContainer = null;

    #endregion
    [SerializeField]
    private GameObject _enemyPrefab = null;

    [SerializeField]
    private Powerup[] _powerUps = null;
    [SerializeField]
    private float _enemySpawnFrequency = 5;

    [SerializeField]
    private bool _stopSpawningEnemies = false;
    [SerializeField]
    private bool _stopSpawningPowerups = false;

    private void Start()
    {
    }

    public void StartSpawning()
    {

        StartCoroutine(SpawnPowerUp());
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (!_stopSpawningEnemies)
        {
            var pos = new Vector3(Random.Range(-10f, 10f), 10, 0);
            var enemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.gameObject.transform);
            yield return new WaitForSeconds(_enemySpawnFrequency);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(15);
        while (!_stopSpawningPowerups)
        {
            int i = 0;
            if (_powerUps.Length <= 0)
            {
                StopCoroutine(SpawnPowerUp());
            }
            else if (_powerUps.Length > 1)
            {
                i = Random.Range(0, _powerUps.Length);
            }

            yield return _powerUps[i].SpawnRoutine(_powerUpContainer);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemies = true;
        _stopSpawningPowerups = true;
    }

}
