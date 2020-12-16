using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public float roundTimer = 0f;
    public float bestRound = 0f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI placementText;

    public float[] roundTimes = { 0f, 0f, 0f };
    public bool isLocalPlayer = false;

    private LapTracker lt;
    private int oldLap;

    void Start()
    {
        lt = GetComponent<LapTracker>();

        RoundTimerReset();

        if (isLocalPlayer)
        {
            timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
            timerText.SetText("Current Lap: {0:3}\nBest Lap: {1:3}\n\nLap 1: {2:3} \nLap 2: {3:3} \nLap 3: {4:3} \nFull Course: {5:3}", 0f, 0f, 0f, 0f, 0f, 0f);

            placementText = GameObject.Find("PlacementPositionText").GetComponent<TextMeshProUGUI>();
            placementText.SetText("-.");
        }
    }

    void Update()
    {
        if (!lt.finished)
        {
            roundTimer += Time.deltaTime;

            if (lt.lap > oldLap)
            {
                NextRound(lt.lap);
                oldLap++;
            }

            if (isLocalPlayer)
            {
                timerText.SetText("Current Lap: {0:3}\nBest Lap: {1:3}\n\nLap 1: {2:3} \nLap 2: {3:3} \nLap 3: {4:3} \nFull Course: {5:3}", Mathf.Abs(roundTimer), Mathf.Abs(bestRound), Mathf.Abs(roundTimes[0]), Mathf.Abs(roundTimes[1]), Mathf.Abs(roundTimes[2]), Mathf.Abs(roundTimes[0] + roundTimes[1] + roundTimes[2]));

                placementText.SetText("{0}.", FindObjectOfType<PlacementManagement>().GetPosition(this.gameObject));
            }
        }
    }

    public void NextRound(int l)
    {
        float _t = Mathf.RoundToInt(roundTimer * 1000f) / 1000f;

        if (bestRound == 0)
        {
            bestRound = _t;
        }
        else if (roundTimer < bestRound)
        {            
            bestRound = _t;
        }

        roundTimes[l - 2] = _t;
        roundTimer = 0f;
    }

    public void RoundTimerReset()
    {
        oldLap = 1;
        roundTimer = 0f;
        bestRound = 0f;
        for (int i = 0; i < roundTimes.Length; i++)
        {
            roundTimes[i] = 0f;
        }
    }
}
