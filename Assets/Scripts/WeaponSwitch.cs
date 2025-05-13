using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] weapons;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("S");
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].activeInHierarchy == true)
                {
                    Debug.Log(i + "is active");
                    weapons[i].SetActive(false);
                    weapons[(i + 1) % weapons.Length ].SetActive(false);
                    break;
                }
            }
        }
    }
}