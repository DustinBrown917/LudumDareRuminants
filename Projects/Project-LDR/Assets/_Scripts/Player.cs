using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private static Player instance_;
    public static Player Instance { get { return instance_; } }

    //Member Variables
    public float maxSocial;
    public float maxSleep;
    public float maxSuccess;

    public float currentSocial;
    public float currentSleep;
    public float currentSuccess;

    public UnityEvent SocialChanged;
    public UnityEvent SleepChanged;
    public UnityEvent SuccessChanged;

    public Text socialText;
    public Text sleepText;
    public Text successText;
    public GameObject playerObj;

    private void Awake()
    {
        if(instance_ == null) { instance_ = this; }
        else if(instance_ != this) { Destroy(this.gameObject); }
    }

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

    public void ChangeSocial(float amount)
    {
        currentSocial = Mathf.Clamp(currentSocial + amount, 0, maxSocial);
        SocialChanged.Invoke();
    }

    public void ChangeSleep(float amount)
    {
        currentSleep = Mathf.Clamp(currentSleep + amount, 0, maxSleep);
        SleepChanged.Invoke();
    }

    public void ChangeSuccess(float amount)
    {
        currentSuccess = Mathf.Clamp(currentSuccess + amount, 0, maxSuccess);
        SuccessChanged.Invoke();
    }
}
