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

    private void Awake()
    {
        if(instance_ == null) { instance_ = this; }
        else if(instance_ != this) { Destroy(this.gameObject); }
        currentSocial = maxSocial;
        currentSleep = maxSleep;
        currentSuccess = maxSuccess;
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
        if (currentSocial <= 0) { GameManager.Instance.LoseGame(Stats.SOCIAL); }
    }

    public void ChangeSleep(float amount)
    {
        currentSleep = Mathf.Clamp(currentSleep + amount, 0, maxSleep);
        SleepChanged.Invoke();
        if (currentSleep <= 0) { GameManager.Instance.LoseGame(Stats.SLEEP); }
    }

    public void ChangeSuccess(float amount)
    {
        currentSuccess = Mathf.Clamp(currentSuccess + amount, 0, maxSuccess);
        SuccessChanged.Invoke();
        if(currentSuccess <= 0) { GameManager.Instance.LoseGame(Stats.SUCCESS); }
    }
}
