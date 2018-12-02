using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegradableObject : MonoBehaviour {

    [SerializeField] private GameObject[] degradationItems;
    [SerializeField] private Stats degradeBadedOn;

    private void Start()
    {
        switch (degradeBadedOn)
        {
            case Stats.SOCIAL:
                Player.Instance.SocialChanged.AddListener(CheckDegradation);
                break;
            case Stats.SLEEP:
                Player.Instance.SleepChanged.AddListener(CheckDegradation);
                break;
            case Stats.SUCCESS:
                Player.Instance.SuccessChanged.AddListener(CheckDegradation);
                break;
            default:
                break;
        }

        CheckDegradation();
    }

    public void CheckDegradation()
    {
        float maxStat = 0;
        float currStat = 0;
        switch (degradeBadedOn)
        {
            case Stats.SOCIAL:
                maxStat = Player.Instance.maxSocial;
                currStat = Player.Instance.currentSocial;
                break;
            case Stats.SLEEP:
                maxStat = Player.Instance.maxSleep;
                currStat = Player.Instance.currentSleep;
                break;
            case Stats.SUCCESS:
                maxStat = Player.Instance.maxSuccess;
                currStat = Player.Instance.currentSuccess;
                break;
            default:
                break;
        }

        int upperIndex = (int)Mathf.Lerp(degradationItems.Length +1, 0, currStat / maxStat);

        for(int i = 0; i < degradationItems.Length; i++)
        {
            if(i < upperIndex)
            {
                degradationItems[i].SetActive(true);
            } else
            {
                degradationItems[i].SetActive(false);
            }
        }
    }
}
