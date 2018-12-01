using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {

    //Singleton pattern.
    private static DayManager instance_ = null;
    public static DayManager Instance { get { return instance_; } }

    [SerializeField] private int currentDay_ = 1;
    public int CurrentDay { get { return currentDay_; } }

    private List<GameEvent> todaysEvents = new List<GameEvent>();

    private void Awake()
    {
        if(instance_ == null) {
            instance_ = this;
        } else if (instance_ != this) {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if(instance_ == this) {
            instance_ = null;
        }
    }

    public void EndDay()
    {
        todaysEvents = new List<GameEvent>();
    }

    public void NextDay()
    {
        currentDay_++;
        
        GameEventManager.Instance.GetRandomEvent();
    }

    /****************************************************************************************************/
    /********************************************** EVENTS **********************************************/
    /****************************************************************************************************/

    public event EventHandler<DayEndArgs> DayEnd;

    public class DayEndArgs : EventArgs
    {
        public int NewDay;

        public DayEndArgs(int newDay)
        {
            NewDay = newDay;
        }
    }

    private void OnDayEnd(DayEndArgs e)
    {
        EventHandler<DayEndArgs> handler = DayEnd;
        
        if(handler != null)
        {
            handler(this, e);
        }
    }
}
