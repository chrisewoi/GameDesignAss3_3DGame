using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    public TMP_Text speedText;
    public PlayerMovement playerController;

    public float speed;
    public float speedMultiplier;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerMovement>();
        //speedText = GetComponentInChildren<TMP_Text>();
        speed = playerController.GetSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        speed = playerController.GetSpeed();
        speed = Mathf.RoundToInt(speed * speedMultiplier);
        speedText.text = speed + "km/h";
    }
}
