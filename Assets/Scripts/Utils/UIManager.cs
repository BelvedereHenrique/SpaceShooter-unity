using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text _scoreText = null;


    [SerializeField]
    private Image _livesImg = null;

    [SerializeField]
    private Text _gameOverText = null;

    [SerializeField]
    private Text _restartText = null;

    [SerializeField]
    private Sprite[] _livesSprites = null;

    private GameManager _gameManager = null;
    private bool _gameFlickerCoroutineRunning = false;
    private Player _playerRef;
    void Start()
    {
        _scoreText.text = $"Score: 0";
        _playerRef = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_playerRef == null)
        {
            Debug.LogError("Player reference on UIManager not found.");
        }
        if (_gameManager == null)
        {
            Debug.LogError("GameManager reference on UIManager not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerScore();
        UpdateLives();
    }

    private void UpdatePlayerScore()
    {
        if (_playerRef != null)
        {
            _scoreText.text = $"Score: { _playerRef.GetScore()}";
        }
    }

    private void UpdateLives()
    {
        int lives = _playerRef.GetLives();
        if (lives >= 0)
        {
            _livesImg.sprite = _livesSprites[lives];
        }

        if (lives == 0)
        {
            GameOverSequence();

        }

    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        if (!_gameFlickerCoroutineRunning)
        {
            _gameFlickerCoroutineRunning = true;
            StartCoroutine(SetGameOverFlicker());
        }
    }

    private IEnumerator SetGameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);

        }
    }
}
