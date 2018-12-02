using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //Member Variables
    public int maxSocial;
    public int maxSleep;
    public int maxSuccess;

    public int currentSocial;
    public int currentSleep;
    public int currentSuccess;

    public Text socialText;
    public Text sleepText;
    public Text successText;
    public GameObject playerObj;

    //Start and Update
    private void Start()
    {
        currentSocial = maxSocial;
        currentSleep = maxSleep;
        currentSuccess = maxSuccess;
    }

    private void Update()
    {
        socialText.text = "Social: " + currentSocial;
        sleepText.text = "Sleep: " + currentSleep;
        successText.text = "Success: " + currentSuccess;


        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeSocial(-1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSocial(1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeSleep(-1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeSleep(1);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeSuccess(-1);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeSuccess(1);
        }


    }

    public void ChangeSocial(int amount)
    {
        currentSocial += amount;
    }

    public void ChangeSleep(int amount)
    {
        currentSleep += amount;
    }

    public void ChangeSuccess(int amount)
    {
        currentSuccess += amount;
    }
}
