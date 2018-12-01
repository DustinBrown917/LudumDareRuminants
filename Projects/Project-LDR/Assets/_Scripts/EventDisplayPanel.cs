using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDisplayPanel : MonoBehaviour {

    [SerializeField] private GameObject eventDisplayPrefab;
    private EventDisplay[] eventDisplays;

    private void Awake()
    {
        SetUpEventDisplays();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetUpEventDisplays()
    {
        eventDisplays = new EventDisplay[DayManager.DAY120_MAX_EVENTS];

        for(int i = 0; i < eventDisplays.Length; i++)
        {
            eventDisplays[i] = Instantiate(eventDisplayPrefab, transform).GetComponent<EventDisplay>();
            eventDisplays[i].gameObject.SetActive(false);
        }
    }

    public void SetEvents(List<GameEvent> events)
    {
        for (int i = 0; i < eventDisplays.Length; i++)
        {
            if (i < events.Count)
            {
                eventDisplays[i].SetEvent(events[i]);
                eventDisplays[i].gameObject.SetActive(true);
            }
            else
            {
                eventDisplays[i].gameObject.SetActive(false);
            }
        }
    }
}
