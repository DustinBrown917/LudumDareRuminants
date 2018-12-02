using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour {

    [SerializeField] private Stats statToWatch;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = statToWatch.ToString() + ": ";

        switch (statToWatch)
        {
            case Stats.SOCIAL:
                text.text += Player.Instance.currentSocial;
                break;
            case Stats.SLEEP:
                text.text += Player.Instance.currentSleep;
                break;
            case Stats.SUCCESS:
                text.text += Player.Instance.currentSuccess;
                break;
            default:
                break;
        }
    }
}
