using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public bool start;
    public GameObject crosshair;
    private void Awake()
    {
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale < 1 && start)
        {
            crosshair.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale += Time.unscaledDeltaTime/2f;
            gameObject.transform.position = new Vector3(10000f, 100000f, 0);
        }
        if (Time.timeScale >= 1)
        {
            Time.timeScale = 1;
            Destroy(this.gameObject);
        }
    }

    public void StartGame()
    {
        start = true;
    }
}
