using Microsoft.SqlServer.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    public bool isTriggered;
    public float triggerCounter;
    private float triggerCounterReal;

    public Transform cyl;
    public Skills skills;

    public int type;    //1 = ATK, 2 = DEF
    public Material atkMat;
    public Material defMat;

    void Start()
    {
        isTriggered = false;
        skills = GameObject.Find("AllSkillsHolder").GetComponent<Skills>();

        MeshRenderer mr = cyl.GetComponent<MeshRenderer>();

        switch (type)
        {
            case 1:

                mr.material = atkMat;
                break;
            case 2:

                mr.material = defMat;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (isTriggered && triggerCounterReal > 0)
        {
            triggerCounterReal -= Time.deltaTime;
        }
        else if (isTriggered)
        {
            Debug.Log("detriggered");
            isTriggered = false;
            cyl.position = new Vector3(cyl.position.x, 1f, cyl.position.z);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (!isTriggered)
        {        
            if (c.gameObject.name == "WPCollider")
            {
                SkillHolder sh = c.gameObject.GetComponentInParent<SkillHolder>();
                switch (type)
                {
                    case 1:
                        if (!sh.allSkills.ContainsKey(1))
                        {
                            sh.allSkills.Add(1, skills.allATKSkills[Random.Range(0, skills.allATKSkills.Count)]);
                            GetTriggered();
                        }

                        break;
                    case 2:
                        if (!sh.allSkills.ContainsKey(2))
                        {
                            sh.allSkills.Add(2, skills.allDEFSkills[Random.Range(0, skills.allDEFSkills.Count)]);
                            GetTriggered();
                        }

                        break;
                    default:
                        break;
                }

                sh.OnChange();
            }
        }
    }

    private void GetTriggered()
    {
        isTriggered = true;
        triggerCounterReal = triggerCounter;
        cyl.position = new Vector3(cyl.position.x, -1f, cyl.position.z);
    }
}
