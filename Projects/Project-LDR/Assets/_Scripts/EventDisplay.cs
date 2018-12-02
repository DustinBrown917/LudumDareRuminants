using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDisplay : MonoBehaviour {

    [SerializeField] private Text eventNameText;
    [SerializeField] private Text eventDescriptionText;
    [SerializeField] private Vector3 popInSize;
    [SerializeField] private float popInTime;
    private Coroutine cr_Scale = null;


    public void SetEvent(GameEvent ev)
    {
        eventNameText.text = ev.eventName;
        eventDescriptionText.text = ev.description;
        CoroutineManager.BeginCoroutine(CoroutineManager.ShrinkScaleFrom(transform, popInSize, Vector3.one, popInTime), ref cr_Scale, this);
    }
}
