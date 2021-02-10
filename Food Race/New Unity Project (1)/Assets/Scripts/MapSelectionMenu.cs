using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectionMenu : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;
    public int currSel = 0;
    public List<Image> selImages = new List<Image>();

    void Start()
    {
        Image[] imgs = GetComponentsInChildren<Image>();
        foreach (Image i in imgs)
        {
            if (i.gameObject.GetComponent<Button>() == null && i.gameObject.name != "Panel")
            {
                selImages.Add(i);
                i.enabled = false;
            }
        }

        selImages[0].enabled = true;

        nextButton = GameObject.Find("Next Button").GetComponent<Button>();
        prevButton = GameObject.Find("Prev Button").GetComponent<Button>();
    }


    public void AddSelection(bool d)
    {
        if (d)
        {
            currSel++;
            if (currSel >= selImages.Count)
            {
                currSel = 0;
            }
        }
        else
        {
            currSel--;
            if (currSel < 0)
            {
                currSel = selImages.Count -1;
            }
        }

        foreach (Image i in selImages)
        {
            i.enabled = false;
        }

        selImages[currSel].enabled = true;
        PlacementManagement.Instance.selectedMap = currSel;
    }
}
