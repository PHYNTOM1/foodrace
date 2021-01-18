using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    SkillHolder sh;
    CartController cc;

    void Start()
    {
        sh = GetComponent<SkillHolder>();
        cc = GetComponent<CartController>();
    }

    void Update()
    {
        cc.SteerInput(Input.GetAxis("Horizontal"));
        cc.AccelerationInput(Input.GetAxis("Vertical"));
        cc.DriftInput(Input.GetButton("Drifting"));


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
