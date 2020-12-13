using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    public Image shieldImage;
    public Image shieldBG;

    public int maxHealth = 4;
    public int currHealth;

    public bool shielded;

    public List<GameObject> health;
    public List<GameObject> healthBG;

    [SerializeField]
    private GameObject cvs;

    void Start()
    {
        shieldImage = GameObject.Find("ShieldImage").GetComponent<Image>();
        shieldBG = GameObject.Find("ShieldBGImage").GetComponent<Image>();
        GameObject healthBGGO = GameObject.Find("HealthBGImage");
        GameObject healthGO = GameObject.Find("HealthImage");
        health.Clear();
        healthBG.Clear();
        health.Add(healthGO);
        healthBG.Add(healthBGGO);
        currHealth = maxHealth;

        cvs = GameObject.Find("Canvas");

        shieldImage = GameObject.Find("ShieldImage").GetComponent<Image>();
        for (int i = 1; i < maxHealth; i++)
        {
            GameObject healthBGGOi = Instantiate(healthBGGO) as GameObject;            
            healthBGGOi.transform.position = new Vector3(healthBGGO.transform.position.x - (i * 75f), healthBGGOi.transform.position.y, healthBGGOi.transform.position.z);
            healthBGGOi.transform.SetParent(cvs.transform, false);
            healthBG.Add(healthBGGOi);
        }

        for (int i = 1; i < maxHealth; i++)
        {
            GameObject healthGOi = Instantiate(healthGO) as GameObject;
            healthGOi.transform.position = new Vector3(healthGO.transform.position.x - (i * 75f), healthGOi.transform.position.y, healthGOi.transform.position.z);
            healthGOi.transform.SetParent(cvs.transform, false);
            health.Add(healthGOi);
        }

        Shielded(shielded);
        OnHPChange();
    }

    public void Shielded(bool s)
    {
        if (s)
        {
            shielded = true;
            shieldImage.enabled = true;
        }
        else
        {
            shielded = false;
            shieldImage.enabled = false;
        }

        OnHPChange();
    }

    public void Damage(int d)
    {
        if (shielded)
        {
            shielded = false;
        }
        else
        {
            currHealth -= d;
        }

        Shielded(shielded);
        OnHPChange();
    }

    private void OnHPChange()
    {
        if (currHealth > 0)
        {
            for (int i = 0; i < maxHealth; i++)
            {
                if (i < currHealth)
                {
                    health[i].GetComponent<Image>().enabled = true;
                }
                else
                {
                    health[i].GetComponent<Image>().enabled = false;
                }
            }
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        //START MINIGAME HERE
        //THEN RESET HEALTH AND ACTIVATE DRIVING
        Debug.Log(gameObject.name + " HAS DIED!");
    }
}
