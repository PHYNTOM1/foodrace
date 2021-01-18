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
    
    public HealthControl hc;

    void Start()
    {
        sk = GameObject.Find("AllSkillsHolder").GetComponent<Skills>();
        hc = GetComponent<HealthControl>();

        if (GetComponent<CartController>().isPlayer)
        {
            skillATK = GameObject.Find("AttackSkillImage").GetComponent<Image>();
            skillDEF = GameObject.Find("DefenseSkillImage").GetComponent<Image>();

            skillATK.enabled = false;
            skillDEF.enabled = false;
        }

        allSkills = new Dictionary<int, string>();
    }

    public void OnChange()
    {
        if (GetComponent<CartController>().isPlayer)
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
                    FindNextFlamethrower().GetComponent<AoEMapAttack>().Attack();
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
                    FindObjectOfType<SoundManagement>().Play("SkillShield");
                    hc.Shielded(true);
                    allSkills.Remove(2);
                    break;
                default:

                    break;
            }

            OnChange();
        }
    }

    public GameObject FindNextFlamethrower()
    {
        List<GameObject> fls = sk.flamethrowers;

        PlacementManagement pm = FindObjectOfType<PlacementManagement>();

        int pos;
        int _pos = pm.GetPosition(this.gameObject);
        if (_pos == -1)
        {
            Debug.Log("ERROR WHILE TRYING TO FIND A FLAMETHROWER!\nNO KART POSITION CAN BE RETURNED!");
            return null;
        }
        else if (_pos >= 0 && _pos <= 3)
        {
            pos = 1;
        }
        else
        {
            pos = _pos -2;
        }

        GameObject _racer = pm.racers[pos -1];
        GameObject fl = fls[Random.Range(0, fls.Count)];

        if (pos == 1)
        {
            float _dist = Vector3.Distance(_racer.transform.position, fls[0].transform.position);
            for (int i = 0; i < fls.Count; i++)
            {
                float _d = Vector3.Distance(_racer.transform.position, fls[i].transform.position);
                if (_d < _dist)
                {
                    fl = fls[i];
                    _dist = _d;
                }
            }
        }
        else
        {
            float _dist = Vector3.Distance(_racer.transform.position, fls[0].transform.position);

            for (int i = 0; i < fls.Count; i++)
            {
                float _d = Vector3.Distance(_racer.transform.position, fls[i].transform.position);
                float _d2 = Vector3.Distance(fls[i].transform.position, _racer.GetComponent<WaypointManager>().Waypoints[_racer.GetComponent<WaypointManager>().currentWaypointIndex].gameObject.transform.position);
                float _d3 = Vector3.Distance(fls[i].transform.position, _racer.GetComponent<WaypointManager>().Waypoints[_racer.GetComponent<WaypointManager>().currentWaypointIndex -1].gameObject.transform.position);
                if (_d < _dist && _d2 > _d3)
                {
                    fl = fls[i];
                    _dist = _d;
                }            
            }
        }

        return fl;
    }
}
