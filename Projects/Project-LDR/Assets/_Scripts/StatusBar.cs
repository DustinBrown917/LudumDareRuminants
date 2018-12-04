using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour {

    [SerializeField] private Stats statToWatch;
    [SerializeField] private Image fillImage;

	// Use this for initialization
	void Start () {
        UpdateBar();
	}

    public void UpdateBar()
    {

        switch (statToWatch)
        {
            case Stats.SOCIAL:
                fillImage.fillAmount = Player.Instance.currentSocial / Player.Instance.maxSocial;
                break;
            case Stats.SLEEP:
                fillImage.fillAmount = Player.Instance.currentSleep / Player.Instance.maxSleep;
                break;
            case Stats.SUCCESS:
                fillImage.fillAmount = Player.Instance.currentSuccess / Player.Instance.maxSuccess;
                break;
            default:
                break;
        }
    }
}
