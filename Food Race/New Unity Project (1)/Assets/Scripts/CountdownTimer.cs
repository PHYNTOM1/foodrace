using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    private float currentTime = 0f;
    public float startingTime = 5f;
    public bool doCD = true;

    public TextMeshProUGUI cdText;
    public GameObject kart;

    void Start()
    {
        cdText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
        kart = GameObject.FindGameObjectWithTag("Player");
        ResetStartTimer();
    }

    void Update()
    {
        if (doCD)
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

                /*
                foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
                {
                    kart.GetComponent<CartController>().notRacing = false;
                }
                */

                kart.GetComponent<CartController>().notRacing = false;
                doCD = false;
            }
            else if (currentTime <= 0.75f)
            {
                cdText.text = "START";
                cdText.fontSize = 130;
            }
        }
    }

    public void ResetStartTimer()
    {
        doCD = true;
        currentTime = startingTime;

        if (cdText == null)
        {
            cdText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
            cdText.fontSize = 100;
        }

        /*
        foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
        {
            kart.GetComponent<CartController>().notRacing = true;
        }
        */

        kart.GetComponent<CartController>().notRacing = true;
    }

}
