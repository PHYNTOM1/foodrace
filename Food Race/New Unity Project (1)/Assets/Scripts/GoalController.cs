﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider c)
    {
        if (c.name == "CartColl")
        {
            //SceneManager.LoadScene(1);

            /*
            c.gameObject.GetComponent<CartController>().lap++;
            Debug.Log(c.gameObject.name + "completed " + c.gameObject.GetComponent<CartController>().lap + ". lap!");

            if (c.gameObject.GetComponent<CartController>().lap == 2)
            {
                Debug.Log(c.gameObject.name + " has FINISHED!");
                Destroy(c.gameObject, 1f);
            }
            */
        }
    }

}
