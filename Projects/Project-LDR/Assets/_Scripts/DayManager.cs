using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {

    //Singleton pattern.
    private static DayManager instance_ = null;
    public static DayManager Instance { get { return instance_; } }

    public const int MaximumDay = 120; //Should probably source this out to GameManager.
    public const int DAY1_MIN_EVENTS = 1;
    public const int DAY120_MIN_EVENTS = 5;
    public const int DAY1_MAX_EVENTS = 1;
    public const int DAY120_MAX_EVENTS = 7;
    public const int TIME_IN_DAY = 2;

    [SerializeField] private Timer timer;
    [SerializeField] private EventDisplayPanel edPanel;
    [SerializeField] private int timeAvailable_;
    public int TimeAvailable { get { return timeAvailable_; } }

    [SerializeField] private int currentDay_ = 1;
    public int CurrentDay { get { return currentDay_; } }

    [SerializeField] private List<GameEvent> todaysEvents = new List<GameEvent>();

    private void Awake()
    {
        if(instance_ == null) {
            instance_ = this;
        } else if (instance_ != this) {
            Destroy(this);
        }

        timer.TimeUp.AddListener(HandleTimerTimeOut);
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

        OnDayEnd(new DayEndArgs(currentDay_));
    }

    public void StartDay()
    {
        currentDay_++;
        timeAvailable_ = TIME_IN_DAY;

        GetTodaysEvents();

        edPanel.SetEvents(todaysEvents);

        OnDayStarted(new DayStartArgs(currentDay_));

        timer.StartTimer();
    }

    public void LoseTime()
    {
        timeAvailable_ -= 1;
        Debug.Log("Time lost. Time remaining: " + timeAvailable_.ToString());
        if(timeAvailable_ == 0)
        {
            EndDay();
        }
    }

    public void HandleTimerTimeOut()
    {
        LoseTime();
        if(timeAvailable_ > 0)
        {
            timer.RestartTimer();
        }
    }

    public void SelectionMade()
    {
        timer.RestartTimer();
    }

    public int GetNumOfEventsToday()
    {
        int minEvents = (int)Mathf.Lerp(DAY1_MIN_EVENTS, DAY120_MIN_EVENTS, (float)currentDay_ / (float)MaximumDay);
        int maxEvents = (int)Mathf.Lerp(DAY1_MAX_EVENTS, DAY120_MAX_EVENTS, (float)currentDay_ / (float)MaximumDay);

        return UnityEngine.Random.Range(minEvents, maxEvents + 1);
    }

    public void GetTodaysEvents()
    {
        int numOfEvents = GetNumOfEventsToday();

        for (int i = 0; i < numOfEvents; i++)
        {
            todaysEvents.Add(GameEventManager.Instance.GetRandomEvent(currentDay_));
        }
    }
    /****************************************************************************************************/
    /********************************************** EVENTS **********************************************/
    /****************************************************************************************************/

    #region DayStarted event.
    public event EventHandler<DayStartArgs> DayStarted;

    public class DayStartArgs : EventArgs
    {
        public int NewDay;

        public DayStartArgs(int newDay)
        {
            NewDay = newDay;
        }
    }

    private void OnDayStarted(DayStartArgs e)
    {
        Debug.Log("Day started.");
        EventHandler<DayStartArgs> handler = DayStarted;

        if(handler != null)
        {
            handler(this, e);
        }
    }
    #endregion

    #region DayEnd event
    public event EventHandler<DayEndArgs> DayEnd;

    public class DayEndArgs : EventArgs
    {
        public int DayThatJustEnded;

        public DayEndArgs(int endedDay)
        {
            DayThatJustEnded = endedDay;
        }
    }

    private void OnDayEnd(DayEndArgs e)
    {
        Debug.Log("Day ended.");
        EventHandler<DayEndArgs> handler = DayEnd;
        
        if(handler != null)
        {
            handler(this, e);
        }
    }
    #endregion
}
