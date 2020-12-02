using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 3f;

    public Text cdText;

    public List<string> placements;
    public Text placementText;
    public GameObject[] karts;

    void Start()
    {
        karts = GameObject.FindGameObjectsWithTag("Player");
        ResetStartTimer();
        placements.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowResults();
        }

        foreach (GameObject go in karts)
        {
            if (go.GetComponent<LapTracker>().finished)
            {
                placements.Add(go.name);
                go.GetComponent<LapTracker>().finished = false;
            }
        }

        /*
        if (currentTime > -1)
        {
            currentTime -= 1 * Time.deltaTime;
            cdText.text = currentTime.ToString("0");
        }

        if (currentTime <= -1)
        {
            cdText.text = " ";
            cdText.fontSize = 80;
            
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
        */
    }

    public void ResetStartTimer()
    {
        currentTime = startingTime;

        if (cdText == null)
        {
            cdText = GameObject.Find("CountdownText").GetComponent<Text>();
            cdText.fontSize = 80;
        }

        foreach (GameObject kart in GameObject.FindGameObjectsWithTag("Player"))
        {
            kart.GetComponent<CartController>().notRacing = true;
        }
    }
        
    public void ShowResults()
    {
        placementText.text = "";

        for (int i = 0; i < placements.Count; i++)
        {
            placementText.text += i + 1 + ". " + placements[i] + "\n";
        }
    }
}
