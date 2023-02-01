using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
//using Image = UnityEngine.UIElements.Image;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _gameOvertext;

    private Player _player;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _LivesImg;

    //handle to text
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "Score:" + 00;
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
        //display img sprite
        //give it a new one based on the currentLives index
        _LivesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            Debug.Log("game over");
            
            _gameOvertext.enabled = true;
            _gameOvertext.gameObject.SetActive(true);
        }
    }
}
