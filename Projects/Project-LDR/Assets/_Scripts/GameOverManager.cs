using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    [SerializeField] private Text descriptionText;

    [SerializeField, TextArea] private string socialText;
    [SerializeField, TextArea] private string sleepText;
    [SerializeField, TextArea] private string successText;


	// Use this for initialization
	void Start () {
        switch (GameManager.Instance.GameLostBy)
        {
            case Stats.SOCIAL:
                descriptionText.text = socialText;
                break;
            case Stats.SLEEP:
                descriptionText.text = sleepText;
                break;
            case Stats.SUCCESS:
                descriptionText.text = successText;
                break;
            default:
                break;
        }
    }
	
	public void ReturnToStart()
    {
        GameManager.Instance.LoadScene("00a_MainMenu");
    }
}
