using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;


    public void Update()
    {
        // if the r key was pressed
        //restart the current scene
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver )
        {
            SceneManager.LoadScene(1); //load current scene
        }

        if (Input.GetKeyDown(KeyCode.Escape) ) 
        {
            SceneManager.LoadScene(0);
            Application.Quit();
        }
    }

    public void GameOver()
    {
        if (_isGameOver == false)
        {
            _isGameOver= true;
        }


    }
}
