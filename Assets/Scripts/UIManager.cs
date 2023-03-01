using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using System.Threading;
using System;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _gameOvertext;
    [SerializeField]
    private TextMeshProUGUI _restartText;

    private Player _player;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private  ProgressBarCircle _progressBar;

    [SerializeField]
    private Slider _thrusterSlider;

    [SerializeField]
    private TextMeshProUGUI _thrusterPercent;

    [SerializeField]
    private TextMeshProUGUI _ammoText;

    private IEnumerator flickerCoroutine;

    public float animSpeedInSec = 1f;
    bool keepAnimating = false;

    [SerializeField]
    private GameManager _gameManager;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "Score: " + 00;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _ammoText.text ="Ammo: 15";
        _progressBar.BarValue = 100;
    }

    public void Updatescore()
    {

        if (_player != null)
        {
            _scoreText.text = "Score: " + _player.GetScore(Random.Range(10, 50));
        }
        
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0)
        {
            _LivesImg.sprite = _liveSprites[currentLives];
        }

        if (currentLives == 0)
        {
            GameOver();
        }
    }

    public void UpdateAmmo(int currentAmmo)
    {
        _ammoText.text =  "Ammo: " + currentAmmo.ToString();
        _progressBar.BarValue = currentAmmo;
    }

    public void UpdateThruster(float fuelPercentage)
    {
        _thrusterSlider.value = fuelPercentage;
        _thrusterPercent.text = Mathf.RoundToInt(fuelPercentage) + "%";
    }

    public void GameOver()
    {
        if (_gameManager == null)
        {
            Debug.Log("GameManager is null.");
        }

            _restartText.enabled = true;
            _restartText.gameObject.SetActive(true);
            _gameManager.GameOver();
            flickerCoroutine = ScreenFlicker();
            StartCoroutine(flickerCoroutine);
    }

    IEnumerator ScreenFlicker()
    {
        {
            _gameOvertext.enabled = true;
            _gameOvertext.gameObject.SetActive(true);
            keepAnimating   = true;
            Color currentColor = _gameOvertext.color;

            Color invisibleColor = _gameOvertext.color;
            invisibleColor.a = 0; //Set Alpha to 0

            float oldAnimSpeedInSec = animSpeedInSec;
            float counter = 0;
            while (keepAnimating)
            {
                //Hide Slowly
                while (counter < oldAnimSpeedInSec)
                {
                    if (!keepAnimating)
                    {
                        yield break;
                    }

                    //Reset counter when Speed is changed
                    if (oldAnimSpeedInSec != animSpeedInSec)
                    {
                        counter = 0;
                        oldAnimSpeedInSec = animSpeedInSec;
                    }

                    counter += Time.deltaTime;
                    _gameOvertext.color = Color.Lerp(currentColor, invisibleColor, counter / oldAnimSpeedInSec);
                    yield return null;
                }

                yield return null;


                //Show Slowly
                while (counter > 0)
                {
                    if (!keepAnimating)
                    {
                        yield break;
                    }

                    //Reset counter when Speed is changed
                    if (oldAnimSpeedInSec != animSpeedInSec)
                    {
                        counter = 0;
                        oldAnimSpeedInSec = animSpeedInSec;
                    }

                    counter -= Time.deltaTime;
                    _gameOvertext.color = Color.Lerp(currentColor, invisibleColor, counter / oldAnimSpeedInSec);
                    yield return null;
                }
            }
        }
    }

    //Call to Start animation
    void startTextMeshAnimation()
    {
        if (keepAnimating)
        {
            return;
        }
        keepAnimating = true;
        StartCoroutine(ScreenFlicker());
    }

    //Call to Change animation Speed
    void changeTextMeshAnimationSpeed(float animSpeed)
    {
        animSpeedInSec = animSpeed;
    }

    //Call to Stop animation
    void stopTextMeshAnimation()
    {
        keepAnimating = false;
    }
         
}
