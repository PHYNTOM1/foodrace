using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    private float maxTime1 = 18f;
    private float maxTime2 = 28f;
    private float maxTime3 = 38f;
    private float maxTime4 = 50f;

    public float roundTimer = 0f;
    public float bestRound = 0f;
    public float cpTimer = 0f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerCPText;
    public TextMeshProUGUI timerMaxText;
    public TextMeshProUGUI roundNum;
    public Image cpResetFill;
    public TextMeshProUGUI cpTimeLeft;
    //    public TextMeshProUGUI placementText;

    public float[] roundTimes = { 0f, 0f, 0f };
    public float[] cpTimes = { 0f, 0f, 0f, 0f };
    public float[] cpBestTimes = { 0f, 0f, 0f, 0f };
    public bool isLocalPlayer = false;

    private LapTracker lt;
//    private int oldLap;
    private CartController cc;

    private PlacementManagement pm;

    void Start()
    {
        lt = GetComponent<LapTracker>();
        cc = GetComponent<CartController>();
        pm = PlacementManagement.Instance;

        RoundTimerReset();

//        if (isLocalPlayer)
//        {
            timerCPText = GameObject.Find("CPTimerText").GetComponent<TextMeshProUGUI>();
            timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
            timerMaxText = GameObject.Find("TimerMaxText").GetComponent<TextMeshProUGUI>();
            roundNum = GameObject.Find("RoundNumber").GetComponent<TextMeshProUGUI>();
            cpResetFill = GameObject.Find("CPResetFill").GetComponent<Image>();
            cpTimeLeft = GameObject.Find("CPTimeLeft").GetComponent<TextMeshProUGUI>();
        //SetResetTimes(pm.selectedMap);

            timerCPText.SetText("Checkpoints:\n1: {0:3} | Best: {1:3}\n2: {2:3} | Best: {3:3}\n3: {4:3} | Best: {5:3}\n4: {6:3} | Best: {7:3}", 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
            timerText.SetText("Current Lap: {0:3}\nBest Lap: {1:3}\n\nLap 1: {2:3} \nLap 2: {3:3} \nLap 3: {4:3} \nFull Course: {5:3}", 0f, 0f, 0f, 0f, 0f, 0f);
            timerMaxText.SetText("/{0:0}\n/{1:0}\n/{2:0}\n/{3:0}", maxTime1, maxTime2, maxTime3, maxTime4);
            UpdateRound();
//            placementText = GameObject.Find("PlacementPositionText").GetComponent<TextMeshProUGUI>();
//            placementText.SetText("-.");
//        }
    }

    void Update()
    {
        if (lt == null)
        {
            lt = GetComponent<LapTracker>();
        }
        if (cc == null)
        {
            cc = GetComponent<CartController>();
        }
        if (pm == null)
        {
            pm = PlacementManagement.Instance;
        }
        if (timerCPText == null)
        {
            timerCPText = GameObject.Find("CPTimerText").GetComponent<TextMeshProUGUI>();
            timerCPText.SetText("Checkpoints:\n1:  " + pm.ConvertTimerInText(cpTimes[0]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[0]) + "\n2: " + pm.ConvertTimerInText(cpTimes[1]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[1]) + "\n3: " + pm.ConvertTimerInText(cpTimes[2]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[2]) + "\n4: " + pm.ConvertTimerInText(cpTimes[3]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[3]));
        }
        if (timerText == null)
        {
            timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
            timerText.SetText("Current Lap: " + pm.ConvertTimerInText(roundTimer) + "\nBest Lap:" + pm.ConvertTimerInText(bestRound) + "\n\nLap 1: " + pm.ConvertTimerInText(roundTimes[0]) + " \nLap 2: " + pm.ConvertTimerInText(roundTimes[1]) + " \nLap 3: " + pm.ConvertTimerInText(roundTimes[2]) + " \nFull Course: " + pm.ConvertTimerInText(roundTimes[0] + roundTimes[1] + roundTimes[2]));
        }
        if (timerMaxText == null)
        {
            timerMaxText = GameObject.Find("TimerMaxText").GetComponent<TextMeshProUGUI>();
            //SetResetTimes(pm.selectedMap);
        }
        if (roundNum == null)
        {
            roundNum = GameObject.Find("RoundNumber").GetComponent<TextMeshProUGUI>();
            UpdateRound();
        }
        if (cpResetFill == null)
        {
            cpResetFill = GameObject.Find("CPResetFill").GetComponent<Image>();
        }
        if (cpTimeLeft == null)
        {
            cpTimeLeft = GameObject.Find("CPTimeLeft").GetComponent<TextMeshProUGUI>();
        }


        if (lt.finished == false && cc.notRacing == false)
        {
            roundTimer += Time.deltaTime;
            cpTimer += Time.deltaTime;
            
            if (lt.checkpointsPassed.Count > 0)
            {
                if (lt.checkpointsPassed[0] == false && cpTimer >= maxTime1)
                {
                    cc.ResetCart(true);
                }
                else if (lt.checkpointsPassed[1] == false && cpTimer >= maxTime2)
                {
                    cc.ResetCart(true);
                }
                else if (lt.checkpointsPassed[2] == false && cpTimer >= maxTime3)
                {
                    cc.ResetCart(true);
                }
                else if (lt.checkpointsPassed[3] == false && cpTimer >= maxTime4)
                {
                    cc.ResetCart(true);
                }
            }

            //            if (isLocalPlayer)
            //            {
            timerCPText.SetText("Checkpoints:\n1:  " + pm.ConvertTimerInText(cpTimes[0]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[0]) + "\n2: " + pm.ConvertTimerInText(cpTimes[1]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[1]) + "\n3: " + pm.ConvertTimerInText(cpTimes[2]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[2]) + "\n4: " + pm.ConvertTimerInText(cpTimes[3]) + " | Best: " + pm.ConvertTimerInText(cpBestTimes[3]));
            timerText.SetText("Current Lap: " + pm.ConvertTimerInText(roundTimer) + "\nBest Lap:" + pm.ConvertTimerInText(bestRound) + "\n\nLap 1: " + pm.ConvertTimerInText(roundTimes[0]) + " \nLap 2: " + pm.ConvertTimerInText(roundTimes[1]) + " \nLap 3: " + pm.ConvertTimerInText(roundTimes[2]) + " \nFull Course: " + pm.ConvertTimerInText(roundTimes[0] + roundTimes[1] + roundTimes[2]));
            UpdateResetFill();
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
        if (roundNum != null)
        {
            UpdateRound();
        }
        else
        {
            roundNum = GameObject.Find("RoundNumber").GetComponent<TextMeshProUGUI>();
            UpdateRound();
        }
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

        if (roundNum != null)
        {
            UpdateRound();
        }
        else
        {
            roundNum = GameObject.Find("RoundNumber").GetComponent<TextMeshProUGUI>();
            UpdateRound();
        }
    }

    public void SetResetTimes(int i)
    {
        switch (i)
        {
            case 0:

                maxTime1 = 8f;
                maxTime2 = 16f;
                maxTime3 = 22f;
                maxTime4 = 30f;
                break;
            case 1:

                maxTime1 = 12f;
                maxTime2 = 24f;
                maxTime3 = 42f;
                maxTime4 = 52f;
                break;
            case 2:

                maxTime1 = 18f;
                maxTime2 = 28f;
                maxTime3 = 38f;
                maxTime4 = 50f;
                break;
        }

        if (timerMaxText != null)
        {
            timerMaxText.SetText("/{0:0}\n/{1:0}\n/{2:0}\n/{3:0}", maxTime1, maxTime2, maxTime3, maxTime4);
        }
    }

    public void UpdateRound()
    {
        roundNum.SetText("{0:0}", lt.lap);
    }

    public void UpdateResetFill()
    {
        if (lt.checkpointsPassed[0] == false)
        {
            cpResetFill.fillAmount = (cpTimer / maxTime1);
            cpTimeLeft.SetText(pm.ConvertTimerInText(maxTime1 - cpTimer));
        }
        else if (lt.checkpointsPassed[1] == false)
        {
            cpResetFill.fillAmount = (cpTimer / (maxTime2 - maxTime1));
            cpTimeLeft.SetText(pm.ConvertTimerInText(maxTime2 - cpTimer));
        }
        else if (lt.checkpointsPassed[2] == false)
        {
            cpResetFill.fillAmount = (cpTimer / (maxTime3 - maxTime1 - maxTime2));
            cpTimeLeft.SetText(pm.ConvertTimerInText(maxTime3 - cpTimer));
        }
        else if (lt.checkpointsPassed[3] == false)
        {
            cpResetFill.fillAmount = (cpTimer / (maxTime4 - maxTime1 - maxTime2 - maxTime3));
            cpTimeLeft.SetText(pm.ConvertTimerInText(maxTime4 - cpTimer));
        }
        else
        {
            cpResetFill.fillAmount = 0f;
            cpTimeLeft.SetText("GOAL");

        }
    }

}
