using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    

    RankingEndscreen re;

    private void Start()
    {
        re = FindObjectOfType<RankingEndscreen>();
        //re.gameObject.SetActive(false);    
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

    public void LoadRankings()
    {
        re = FindObjectOfType<RankingEndscreen>();
        re.gameObject.SetActive(true);
        re.UpdateRankings();
        re.DisplayStats();
        
        gameObject.SetActive(false);
    }
}
