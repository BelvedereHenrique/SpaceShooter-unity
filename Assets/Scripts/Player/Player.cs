using System;
using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    #region properties
    #region serializable
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private GameObject _shieldVisualizer = null;
    [SerializeField]
    private GameObject _thrusterVisualizer = null;
    [SerializeField]
    private GameObject _rightDamageVisualizer = null;
    [SerializeField]
    private GameObject _leftDamageVisualizer = null;
    [SerializeField]
    private AudioClip _laserShotAudioClip = null;
    #endregion
    #region nonserializable
    private SpawnManager _spawnManager = null;
    private AudioSource _audioSource = null;
    private float _nextFire = 0;
    #endregion
    #region powerups
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private GameObject _tripleShotPrefab = null;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldActivated = false;



    private float _speedBonus = 0f;
    #endregion
    #endregion

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        transform.transform.position = new Vector3(0, 0, 0);
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Missing AudioSource for player. Reference is null.");
        }
        else
        {
            _audioSource.clip = _laserShotAudioClip;
        }

    }

    void Update()
    {
        HandleMove();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            HandleFire();
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isShieldActivated)
        {
            _isShieldActivated = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives -= damage;
        ActivateDamageFire();
        Debug.Log($"Took damage: {_lives}");
        if (_lives <= 0)
        {
            Die();
        }
    }

    private void ActivateDamageFire()
    {
        if (_leftDamageVisualizer.activeSelf || _rightDamageVisualizer.activeSelf)
        {
            var nonActive = _leftDamageVisualizer.activeSelf ? _rightDamageVisualizer : _leftDamageVisualizer;
            nonActive.SetActive(true);
        }
        else
        {
            GameObject[] options = new GameObject[] { _leftDamageVisualizer, _rightDamageVisualizer };
            var selected = UnityEngine.Random.Range(0, 2);
            options[selected].SetActive(true);
        }
    }

    private void Die()
    {
        _spawnManager.OnPlayerDeath();
        Destroy(this.gameObject);
    }

    private void HandleMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput);
        transform.Translate(direction * (_speed + _speedBonus) * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 0));

        if (transform.position.x >= 11.1)
            transform.position = new Vector3(-11.1f, transform.position.y, 0);
        else if (transform.position.x <= -11.1)
            transform.position = new Vector3(11.1f, transform.position.y, 0);

    }

    private void HandleFire()
    {
        HandleShotFireRate();

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f), Quaternion.identity);

        _audioSource.Play();

    }

    private void HandleShotFireRate()
    {
        _nextFire = Time.time + _fireRate;
    }

    internal void ActivateTripleShotPowerup(int duration)
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerupTimer(duration));
    }

    IEnumerator TripleShotPowerupTimer(int duration)
    {

        yield return new WaitForSeconds(duration);
        _isTripleShotActive = false;
    }

    internal void ActivateSpeedPowerup(int duration, float speedBonus)
    {
        _speedBonus = speedBonus;
        _thrusterVisualizer.SetActive(true);
        StartCoroutine(SpeedPowerupTimer(duration));
    }

    IEnumerator SpeedPowerupTimer(int duration)
    {
        yield return new WaitForSeconds(duration);
        _speedBonus = 0f;
        _thrusterVisualizer.SetActive(false);
    }


    internal void ActivateShieldPowerup(int durationInSecs)
    {
        _isShieldActivated = true;
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldTPowerupTimer(durationInSecs));
    }

    IEnumerator ShieldTPowerupTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        _isShieldActivated = false;
        _shieldVisualizer.SetActive(false);
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetLives()
    {
        return _lives;
    }
}
