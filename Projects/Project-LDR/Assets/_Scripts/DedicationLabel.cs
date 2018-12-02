using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DedicationLabel : MonoBehaviour {

    [SerializeField] private DayManager.TimeOfDay targetTimeOfDay;

    private Text labelText;

    private void Awake()
    {
        labelText = GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        DayManager.Instance.DayEnd += DayManager_DayEnd;

        switch (targetTimeOfDay)
        {
            case DayManager.TimeOfDay.Morning:
                DayManager.Instance.MorningDedicationChanged.AddListener(UpdateLabel);
                break;
            case DayManager.TimeOfDay.Evening:
                DayManager.Instance.EveningDedicationChanged.AddListener(UpdateLabel);
                break;
            default:
                break;
        }
    }

    private void DayManager_DayEnd(object sender, DayManager.DayEndArgs e)
    {
        labelText.text = "Nothing";
    }

    public void UpdateLabel()
    {
        Stats s = (Stats)DayManager.Instance.DedicatedStatIndex;

        labelText.text = s.ToString();
    }
}
