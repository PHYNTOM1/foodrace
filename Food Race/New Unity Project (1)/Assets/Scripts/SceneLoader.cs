using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
   
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            PlacementManagement.Instance.BackToMenu();
        }
        

    }
}
