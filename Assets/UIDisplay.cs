using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour, IImage
{
    public TMP_Text speedText;
    public PlayerMovement playerController;

    public float speed;
    public float speedMultiplier;

    public TMP_Text timerText;
    public float timer;
    public float maxTimer;

    public TMP_Text GOScore;
    public GameObject GOPanel;

    public static float Score;
    public static bool GameOver;

    public TMP_Text topSpeedText;

    public Image crosshair;


    public DestroyOnCollision ship;
    public PlayerMovement playerMovement;
    public float topSpeed;

    public ScoreTracking scoreTracking;

    public TMP_Text score1Text;
    public TMP_Text score2Text;
    public TMP_Text score3Text;
    public TMP_Text score4Text;
    public TMP_Text score5Text;

    private bool hasSaved = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerMovement>();
        //speedText = GetComponentInChildren<TMP_Text>();
        speed = playerController.GetSpeed();
        timer = maxTimer;
        GOPanel.SetActive(false);
        ship = FindObjectOfType<DestroyOnCollision>();
        playerMovement = GetComponent<PlayerMovement>();
        scoreTracking = GetComponent<ScoreTracking>();
    }

    private void Awake()
    {
        GOPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        speed = playerController.GetSpeed();
        speed = Mathf.RoundToInt(speed * speedMultiplier);
        speedText.text = speed + "km/h";
        if (topSpeed < speed) topSpeed = speed;
        // Timer
        timer -= Time.deltaTime;
        timerText.text = Mathf.RoundToInt(timer).ToString();
        if (timer < 0 || GameOver)
        {
            timer = 0;
            GOPanel.SetActive(true);
            Cursor.visible = true;
            crosshair.gameObject.SetActive(false);
            topSpeedText.text = "Top Speed: " + topSpeed.ToString() + "KM/H";
            GOScore.text = Score.ToString();
            float[] scoreList = scoreTracking.GetNewScoreList(Score);
            if(scoreList.Length >=5) {
                score1Text.text = scoreList[0].ToString();
                score2Text.text = scoreList[1].ToString();
                score3Text.text = scoreList[2].ToString();
                score4Text.text = scoreList[3].ToString();
                score5Text.text = scoreList[4].ToString();
            }
            if(!hasSaved) {
                scoreTracking.SaveScores(scoreList);
                hasSaved = true;
            }
            //topSpeedText.text = "Top Speed: " + playerMovement.topSpeedReached.ToString();
            if (ship != null) ship.DestroyShip();
            
        }
    }

    public void AddTime(float time)
    {
        timer += time;
        Debug.Log($"{time} second added to timer");
    }

    public void SetFill(float amount) {
        crosshair.fillAmount = Mathf.Lerp(1, 1f, amount);
    }
}
