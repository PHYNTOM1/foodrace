using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingEndscreen : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI rankingsText;

    public float bestTime = 0f;

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

        /*float f = PlacementManagement.Instance.GetBestTimeOverall();
        if (f == 0f)
        {
            bestTime = 0f;
        }
        else
        {
            bestTime = f;
        }
        */
    }

    public void DisplayStats()
    {
        string p;
        if (PlacementManagement.Instance.finishers.Count > 0)
        {
            p = PlacementManagement.Instance.bestRacer.name;
        }
        else
        {
            p = "empty";
        }
        bestTimeText.SetText("{0:3}s by: " + p, bestTime);

        string t = "";
        for (int i = 0; i < PlacementManagement.Instance.finishers.Count; i++)
        {
            t += (i+1) + ". " + PlacementManagement.Instance.finishers[i].name + "\n";
        }
        
        rankingsText.SetText(t);
    }

}
