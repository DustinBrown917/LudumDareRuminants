using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DedicationImage : MonoBehaviour {

    [SerializeField] private DayManager.TimeOfDay targetTimeOfDay;
    [SerializeField] private DedicationImagePool pool;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Use this for initialization
    void Start()
    {
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
        image.enabled = false;
    }

    public void UpdateLabel()
    {
        image.enabled = true;
        Stats s = (Stats)DayManager.Instance.DedicatedStatIndex;

        image.sprite = pool.GetImage(s);
    }
}
