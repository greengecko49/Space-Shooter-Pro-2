using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _waveText;
    [SerializeField]
    private TMP_Text _ammoText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Slider _thrusterSlider;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _waveText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void Updatelives(int currentlives)
    {
        if(currentlives >= 0)
        {
          _LivesImg.sprite = _liveSprites[currentlives];
        }
        

        if (currentlives <= 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        

    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    public void UpdateThrusterUI(float value)
    {
        _thrusterSlider.value = value;
    }

    public void UpdateAmmo(int playerAmmo)
    {
        _ammoText.text = "Ammo " + playerAmmo + "/30";
    }

    IEnumerator WaveTextRoutine()
    {
        _waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        _waveText.gameObject.SetActive(false);
    }

    public void UpdateWave(int waveCount)
    {
        _waveText.text = "Wave " + waveCount.ToString();
        StartCoroutine(WaveTextRoutine());
    }
}
