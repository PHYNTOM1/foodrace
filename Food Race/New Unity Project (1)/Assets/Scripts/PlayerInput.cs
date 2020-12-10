using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    SkillHolder sh;

    void Start()
    {
        sh = GetComponent<SkillHolder>();
    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            sh.ActivateATKSkill();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            sh.ActivateDEFSkill();
        }
    }
}
