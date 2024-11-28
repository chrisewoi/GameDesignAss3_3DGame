using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PowerupBar : MonoBehaviour
{
    public Image bar;
    public float fillAmount;
    public float minFill;
    public ClickToShoot powerupStats;
    
    void Start()
    {
        minFill = 0.13f;
        fillAmount = 0f;
        bar = GetComponent<Image>();
        powerupStats = FindObjectOfType<ClickToShoot>();
    }

    void Update()
    {
        fillAmount = powerupStats.powerupActive / powerupStats.powerupCooldown;
        SetFill(fillAmount);
    }

    public void SetFill(float amount)
    {
        //Adds some shake
        if(amount > 0) amount += Random.Range(0f, 0.015f);
        
        bar.fillAmount = Mathf.Lerp(minFill,1f,amount);
    }
}
