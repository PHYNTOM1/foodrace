using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectionMenu : MonoBehaviour
{
    public Button nextButton;
    public Button prevButton;
    public int currSel = 0;
    public List<Image> selImages = new List<Image>();
    public TextMeshProUGUI mapName;
    public PlacementManagement pm;

    void Start()
    {
        pm = PlacementManagement.Instance;

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
        mapName = GameObject.Find("MapName").GetComponent<TextMeshProUGUI>();
        mapName.SetText(pm.map01Name);
    }


    public void AddSelection(bool d)
    {
        if (d)
        {
            pm.anim.SetTrigger("FlipN");
            currSel++;
            if (currSel >= selImages.Count)
            {
                currSel = 0;
            }
        }
        else
        {
            pm.anim.SetTrigger("FlipP");
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

        switch (currSel)
        {
            case 0:

                mapName.SetText(pm.map01Name);
                break;
            case 1:

                mapName.SetText(pm.map02Name);
                break;
            case 2:

                mapName.SetText(pm.map03Name);
                break;
        }
    }
}
