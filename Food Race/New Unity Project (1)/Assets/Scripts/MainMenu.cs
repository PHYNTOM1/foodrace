using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        PlacementManagement.Instance.CallOnAwake();
    }

    public void PlayGame()
    {
        PlacementManagement.Instance.StartGame();
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
