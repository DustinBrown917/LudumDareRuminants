using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDisplay : MonoBehaviour {

    [SerializeField] private Text eventNameText;
    [SerializeField] private Text eventDescriptionText;

    private GameEvent ev;


    public void SetEvent(GameEvent ev)
    {
        this.ev = ev;
        eventNameText.text = ev.eventName;
        eventDescriptionText.text = ev.description;
    }
}
