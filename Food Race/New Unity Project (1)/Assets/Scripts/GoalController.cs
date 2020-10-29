using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{

    private void OnTriggerEnter(Collider c)
    {
        if (c.name == "CartColl")
        {
            SceneManager.LoadScene(0);
        }
    }
}
