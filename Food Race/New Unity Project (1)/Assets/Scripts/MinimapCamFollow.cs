using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamFollow : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.eulerAngles = new Vector3(90f, player.transform.eulerAngles.y, 0f);
    }
}
