using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingEndscreen : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI rankingsText;

    public float bestTime = 0f;

    void Start()
    {
        UpdateRankings();
        DisplayStats();
    }

    public void UpdateRankings()
    {
        if (bestTimeText == null)
        {
            bestTimeText = GameObject.Find("BestTimeText").GetComponent<TextMeshProUGUI>();
        }
        if (rankingsText == null)
        {
            rankingsText = GameObject.Find("RankingsText").GetComponent<TextMeshProUGUI>();
        }

        bestTime = PlacementManagement.Instance.GetBestTimeOverall();
    }

    public void DisplayStats()
    {
        bestTimeText.SetText("{0:3}s by: ", bestTime);
        bestTimeText.text += PlacementManagement.Instance.finishers[PlacementManagement.Instance.GetPositionFinishers(PlacementManagement.Instance.bestRacer)].name;

        string t = "";
        for (int i = 0; i < PlacementManagement.Instance.finishers.Count; i++)
        {
            t += (i+1) + ". " + PlacementManagement.Instance.finishers[i].name + "\n";
        }
        
        rankingsText.SetText(t);
    }

}
