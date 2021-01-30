using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public float roundTimer = 0f;
    public float bestRound = 0f;
    public float cpTimer = 0f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerCPText;
//    public TextMeshProUGUI placementText;

    public float[] roundTimes = { 0f, 0f, 0f };
    public float[] cpTimes = { 0f, 0f, 0f, 0f };
    public float[] cpBestTimes = { 0f, 0f, 0f, 0f };
    public bool isLocalPlayer = false;

    private LapTracker lt;
//    private int oldLap;
    private CartController cc;

    void Start()
    {
        lt = GetComponent<LapTracker>();
        cc = GetComponent<CartController>();

        RoundTimerReset();

//        if (isLocalPlayer)
//        {
            timerCPText = GameObject.Find("CPTimerText").GetComponent<TextMeshProUGUI>();
            timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
            
            timerCPText.SetText("Checkpoints:\n1: {0:3} | Best: {1:3}\n2: {2:3} | Best: {3:3}\n3: {4:3} | Best: {5:3}\n4: {6:3} | Best: {7:3}", 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
            timerText.SetText("Current Lap: {0:3}\nBest Lap: {1:3}\n\nLap 1: {2:3} \nLap 2: {3:3} \nLap 3: {4:3} \nFull Course: {5:3}", 0f, 0f, 0f, 0f, 0f, 0f);

//            placementText = GameObject.Find("PlacementPositionText").GetComponent<TextMeshProUGUI>();
//            placementText.SetText("-.");
//        }
    }

    void Update()
    {
        if (lt.finished == false && cc.notRacing == false)
        {
            roundTimer += Time.deltaTime;
            cpTimer += Time.deltaTime;

//            if (isLocalPlayer)
//            {
                timerCPText.SetText("Checkpoints:\n1: {0:3} | Best: {1:3}\n2: {2:3} | Best: {3:3}\n3: {4:3} | Best: {5:3}\n4: {6:3} | Best: {7:3}", Mathf.Abs(cpTimes[0]), Mathf.Abs(cpBestTimes[0]), Mathf.Abs(cpTimes[1]), Mathf.Abs(cpBestTimes[1]), Mathf.Abs(cpTimes[2]), Mathf.Abs(cpBestTimes[2]), Mathf.Abs(cpTimes[3]), Mathf.Abs(cpBestTimes[3]));
                timerText.SetText("Current Lap: {0:3}\nBest Lap: {1:3}\n\nLap 1: {2:3} \nLap 2: {3:3} \nLap 3: {4:3} \nFull Course: {5:3}", Mathf.Abs(roundTimer), Mathf.Abs(bestRound), Mathf.Abs(roundTimes[0]), Mathf.Abs(roundTimes[1]), Mathf.Abs(roundTimes[2]), Mathf.Abs(roundTimes[0] + roundTimes[1] + roundTimes[2]));

            //                placementText.SetText("{0}.", FindObjectOfType<PlacementManagement>().GetPosition(this.gameObject));
            //            }
        }
    }

    public void CompletedCP(int s)
    {
        float _t = Mathf.RoundToInt(cpTimer * 1000f) / 1000f;

        if (cpBestTimes[s] == 0f)
        {
            cpBestTimes[s] = _t;
        }
        else if (_t < cpBestTimes[s])
        {
            cpBestTimes[s] = _t;
        }

        cpTimes[s] = _t;
    }

    public void CompletedRound(int l)
    {
        float _t = Mathf.RoundToInt(roundTimer * 1000f) / 1000f;

        if (bestRound == 0f)
        {
            bestRound = _t;
        }
        else if (roundTimer < bestRound)
        {            
            bestRound = _t;
        }

       

        roundTimes[l - 2] = _t;
        roundTimer = 0f;
        cpTimer = 0f;
    }

    public void RoundTimerReset()
    {
//        oldLap = 1;
        roundTimer = 0f;
        bestRound = 0f;
        cpTimer = 0f;
        for (int i = 0; i < roundTimes.Length; i++)
        {
            roundTimes[i] = 0f;
        }
        for (int i = 0; i < cpTimes.Length; i++)
        {
            cpTimes[i] = 0f;
        }
        for (int i = 0; i < cpBestTimes.Length; i++)
        {
            cpBestTimes[i] = 0f;
        }
    }
}
