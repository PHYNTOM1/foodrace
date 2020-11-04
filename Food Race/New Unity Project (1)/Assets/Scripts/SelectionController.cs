using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionController : MonoBehaviour
{

    public enum SelectedCart
    {
        Light,
        Medium,
        Heavy        
    }

    SelectedCart currSelection;

    private Button arrowL;
    private Button arrowR;
    private Button confirm;
    private Text currCart;

    void Start()
    {
        arrowL = GameObject.Find("LeftArrow").GetComponent<Button>();
        arrowR = GameObject.Find("RightArrow").GetComponent<Button>();
        confirm = GameObject.Find("ConfirmButton").GetComponent<Button>();
        currCart = GameObject.Find("CurrentSelectionText").GetComponent<Text>();
        currSelection = SelectedCart.Medium;

        arrowL.onClick.AddListener(this.CartSelectionLeft);
        arrowR.onClick.AddListener(this.CartSelectionRight);
        confirm.onClick.AddListener(this.CartSelectionConfirm);
    }


    public void CartSelectionLeft()
    {
        UpdateCartSelection(-1);
    }

    public void CartSelectionRight()
    {
        UpdateCartSelection(1);
    }

    public void CartSelectionConfirm()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateCartSelection( int s )
    {
        if (currSelection >= SelectedCart.Heavy && s == 1)
        {
            currSelection = SelectedCart.Light;
        }
        else if (currSelection <= SelectedCart.Light && s == -1)
        {
            currSelection = SelectedCart.Heavy;
        }
        else
        {
            currSelection += s;
        }

        currCart.text = "";
        currCart.text = currSelection.ToString();
    }

}
