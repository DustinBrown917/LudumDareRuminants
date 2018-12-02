using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDisplay : MonoBehaviour {

    [SerializeField] private Text eventNameText;
    [SerializeField] private Text eventDescriptionText;
    [SerializeField] private Vector3 popInSize;
    [SerializeField] private float popInTime;



    private Image img;

    private Coroutine cr_Scale = null;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void SetEvent(GameEvent ev)
    {
        eventNameText.text = ev.eventName;
        eventDescriptionText.text = ev.description;
        img.color = GameEventManager.Instance.GetEventColour(ev.colourClassification);
        CoroutineManager.BeginCoroutine(CoroutineManager.ShrinkScaleFrom(transform, popInSize, Vector3.one, popInTime), ref cr_Scale, this);
    }


}
