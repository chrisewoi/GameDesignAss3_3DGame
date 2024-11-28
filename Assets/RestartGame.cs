using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public GameObject GOPanel;
    public void Restart()
    {
        UIDisplay.Score = 0;
        UIDisplay.GameOver = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
        GOPanel.SetActive(false);

    }
}
