using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public List<string> allATKSkills;
    public List<string> allDEFSkills;

    public List<GameObject> flamethrowers;

    private void Start()
    {
        GameObject[] ft = GameObject.FindGameObjectsWithTag("Flamethrower");

        for (int i = 0; i < ft.Length; i++)
        {
            flamethrowers.Add(ft[i]);
        }
    }
}
