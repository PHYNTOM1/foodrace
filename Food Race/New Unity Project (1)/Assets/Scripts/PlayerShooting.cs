using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet;
    private bool shooting = false;
    public float shootCD = 0f;
    [SerializeField]
    private float shootCDReal = 0f;
    public float distToKart = 2f;
    //private float left = 1f;
    private CartController cc;

    public bool overheated = false;
    public float overheatValue = 0f;
    public float overheatShot = 10f;
    public float ohDecayMult = 2.5f;

    public Slider overheatSlider;
    public Image ohSliderFill;
    public Color ohColor;
    public Color normalColor;

    public VisualEffect muzzleFlash;
    public SoundManagement sm;

    void Start()
    {
        sm = PlacementManagement.Instance.GetComponent<SoundManagement>();
        shooting = false;
        cc = GetComponent<CartController>();
        if (ohSliderFill == null)
        {
            Slider[] sls = (Slider[]) FindObjectsOfType(typeof(Slider));
            foreach (Slider sl in sls)
            {
                if (sl.gameObject.name == "Overheat")
                {
                    overheatSlider = sl;
                    Transform[] ts = overheatSlider.gameObject.GetComponentsInChildren<Transform>();
                    List<GameObject> gos = new List<GameObject>();
                    foreach (Transform t in ts)
                    {
                        gos.Add(t.gameObject);
                    }
                    foreach  (GameObject go in gos)
                    {
                        if (go.name == "Fill")
                        {
                            ohSliderFill = go.GetComponent<Image>();
                        }
                    }
                }
            };
        }
        ohSliderFill.color = normalColor;
    }


    void Update()
    {
        if (cc.notRacing == false && cc.stunned == false)
        {
            if (!overheated)
            {
                if (shooting == true && shootCDReal <= 0)
                {
                    Shoot();            
                }
            }
        }

        if (overheated && overheatValue > 0)
        {
            overheatValue -= Time.deltaTime * ohDecayMult * 5f;            
        }
        else if (overheated)
        {
            sm.PlayOneShot("clickcheckswoosh");
            overheated = false;
            overheatValue = 0f;
            ohSliderFill.color = normalColor;
        }

        if (!overheated)
        {
            if (overheatValue > 0)
            {
                if (cc.drifting == true)
                {
                    overheatValue -= Time.deltaTime * ohDecayMult * 3f;
                }
                else
                {
                    overheatValue -= Time.deltaTime * ohDecayMult;
                }
            }
        }
        overheatSlider.value = (overheatValue / 100f);

        if (shootCDReal > 0)
    	{
            shootCDReal -= Time.deltaTime;
    	}

        shooting = false;
    }

    public void ShootInput(bool b)
    {
        shooting = b;
    }

    private void Shoot()
    {
        sm.PlayOneShot("laser3");
        muzzleFlash.Play();
        GameObject b = Instantiate(bullet, (this.gameObject.transform.position + (this.gameObject.transform.forward.normalized * distToKart)), Quaternion.Euler(90f + this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z));
        /*
        GameObject b = Instantiate(bullet, (this.gameObject.transform.position + (this.gameObject.transform.forward.normalized * distToKart) + (this.gameObject.transform.right.normalized * left * 0.5f)), Quaternion.Euler(90f + this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z));

        if (left == 1)
        {
            left = -1f;
        }
        else
        {
            left = 1f;
        }
        */

        b.transform.parent = null;

        shootCDReal = shootCD;
        AddOverheat(overheatShot);
    }

    public void AddOverheat(float a)
    {
        if (!overheated)
        {
            overheatValue += a;

            if (overheatValue >= 100f)
            {
                overheatValue = 100f;
                Overheat();
            }
        }
    }

    public void Overheat()
    {
        sm.PlayOneShot("heavyswitch");
        sm.PlayOneShot("impact");
        overheated = true;
        overheatValue = 100f;
        ohSliderFill.color = ohColor;
    }

}
