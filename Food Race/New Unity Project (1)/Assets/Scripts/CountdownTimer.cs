using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 3f;

    public TextMeshProUGUI cdText;
    public GameObject[] karts;

    void Start()
    {
        cdText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
        karts = GameObject.FindGameObjectsWithTag("Player");
        ResetStartTimer();
    }

    void Update()
    {
        if (currentTime > -1)
        {
            currentTime -= 1 * Time.deltaTime;            
            cdText.SetText(currentTime.ToString("0"));
        }

        if (currentTime <= -1)
        {
            cdText.SetText(" ");
            cdText.fontSize = 100;
            
            foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
            {
                kart.GetComponent<CartController>().notRacing = false;
            }
        }
        else if (currentTime <= 0.5f)
        {
            cdText.text = "START";
            cdText.fontSize = 130;
        }

    }

    public void ResetStartTimer()
    {
        currentTime = startingTime;

        if (cdText == null)
        {
            cdText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
            cdText.fontSize = 100;
        }

        foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
        {
            kart.GetComponent<CartController>().notRacing = true;
        }
    }

}
