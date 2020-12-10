using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    public Dictionary<int, string> allSkills;
    //0 = empty, 1 = ATK, 2 = DEF

    public Image skillATK;
    public Image skillDEF;

    public Skills sk;
    
    //public HealthControl hc;
    //FOR EXAMPLE HEALTH / SHIELD

    void Start()
    {
        skillATK = GameObject.Find("AttackSkillImage").GetComponent<Image>();
        skillDEF = GameObject.Find("DefenseSkillImage").GetComponent<Image>();
        sk = GameObject.Find("AllSkillsHolder").GetComponent<Skills>();
        //hc = GetComponent<HealthControl>();

        skillATK.enabled = false;
        skillDEF.enabled = false;
        allSkills = new Dictionary<int, string>();
    }

    public void OnChange()
    {
        if (allSkills.ContainsKey(1))
        {
            skillATK.enabled = true;
        }
        else
        {
            skillATK.enabled = false;
        }

        if (allSkills.ContainsKey(2))
        {
            skillDEF.enabled = true;
        }
        else
        {
            skillDEF.enabled = false;
        }
    }

    public void ActivateATKSkill()
    {
        Debug.Log("pressed atk skill");
        if (allSkills.ContainsKey(1))
        {
            string s = "";
            allSkills.TryGetValue(1, out s);

            switch (s)
            {
                case "flamethrower":

                    Debug.Log("activated flamethrower");
                    sk.flamethrowers[Random.Range(0, sk.flamethrowers.Count)].GetComponent<AoEMapAttack>().Attack();
                    allSkills.Remove(1);
                    break;
                default:

                    break;
            }

            OnChange();
        }
    }

    public void ActivateDEFSkill()
    {
        Debug.Log("pressed def skill");
        if (allSkills.ContainsKey(2))
        {
            string s = "";
            allSkills.TryGetValue(2, out s);

            switch (s)
            {
                case "shield":

                    Debug.Log("activated shield");
                    //activate shield, for example like:
                    //hc.ActivateShield();
                    allSkills.Remove(2);
                    break;
                default:

                    break;
            }

            OnChange();
        }
    }
}
