using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //    SkillHolder sh;
    CartController cc;
    PlayerShooting ps;
    public bool paused = false;
    public GameObject pausedPanel;

    void Start()
    {
        //        sh = GetComponent<SkillHolder>();
        cc = GetComponent<CartController>();
        ps = GetComponent<PlayerShooting>();
        paused = false;
        pausedPanel = GameObject.Find("PausedText");
        pausedPanel.SetActive(paused);
    }

    void Update()
    {
        cc.SteerInput(Input.GetAxis("Horizontal"));
        cc.AccelerationInput(Input.GetAxis("Vertical"));
        cc.DriftInput(Input.GetButton("Drifting"));

        ps.ShootInput(Input.GetButton("Shooting"));

        if (Input.GetButtonDown("Reset"))
        {
            if (paused == true)
            {
                paused = false;
                Time.timeScale = 1f;
                PlacementManagement.Instance.BackToMenu();
            }

            else
            {
                cc.ResetCart(false);
            }
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (paused == true)
            {
                
                paused = false;
                Time.timeScale = 1f;
            }
            else
            {
                paused = true;
                Time.timeScale = 0f;
            }

            /*
            if (paused)
            {
                pausedPanel.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                pausedPanel.GetComponent<CanvasGroup>().alpha = 0;
            }
            */
            pausedPanel.SetActive(paused);
        }

        /*
        if (Input.GetKey(KeyCode.I) && Time.timeScale < 2)
        {
            Time.timeScale += Time.deltaTime;
            Debug.Log("TimeScale: " + Time.timeScale);
        }
        if (Input.GetKey(KeyCode.U) && Time.timeScale > 0.2)
        {
            Time.timeScale -= Time.deltaTime;
            Debug.Log("TimeScale: " + Time.timeScale);
        }
        */
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            sh.ActivateATKSkill();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            sh.ActivateDEFSkill();
        }
        */
    }
}
