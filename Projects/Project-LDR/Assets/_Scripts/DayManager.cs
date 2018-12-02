using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    public static readonly string[] dayTimeStrings = { "Morning", "Evening" };
    [SerializeField] private Image dayTimeImage;
    [SerializeField] private Sprite[] dayTimeImages;
    private Coroutine cr_DayTimeImage;

    private TimeOfDay currentTimeOfDay_;
    public TimeOfDay CurrentTimeOfDay { get { return currentTimeOfDay_; } }

    [SerializeField] private Timer timer;
    [SerializeField] private EventDisplayPanel edPanel;
    [SerializeField] private Text dayLabel;
    [SerializeField] private int timeAvailable_;
    public int TimeAvailable { get { return timeAvailable_; } }

    [SerializeField] private int currentDay_ = 1;
    public int CurrentDay { get { return currentDay_; } }

    private int dedicatedStatIndex_ = -1;
    public int DedicatedStatIndex { get { return dedicatedStatIndex_; } }

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

        StartDay();
    }

    public void SetTimeOfDay(TimeOfDay tod)
    {
        currentTimeOfDay_ = tod;
        dayLabel.text = "Day " + currentDay_.ToString();
        dayTimeImage.sprite = dayTimeImages[(int)currentTimeOfDay_];
        CoroutineManager.BeginCoroutine(CoroutineManager.ShrinkScaleFrom(dayTimeImage.transform, new Vector3(1.5f, 1.5f, 1.0f), Vector3.one, 0.25f), ref cr_DayTimeImage, this);
    }

    public void StartDay()
    {
        currentDay_++;
        SetTimeOfDay(TimeOfDay.Morning);
        timeAvailable_ = TIME_IN_DAY;

        GetTodaysEvents();

        edPanel.SetEvents(todaysEvents);

        ExecuteEvents();

        OnDayStarted(new DayStartArgs(currentDay_));

        timer.StartTimer();
    }

    public void ExecuteEvents()
    {
        foreach(GameEvent e in todaysEvents)
        {
            e.ExecuteEvent();
        }
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
        SetTimeOfDay(TimeOfDay.Evening);
        DecisionHandler.Instance.DedicateTimeTo((Stats)dedicatedStatIndex_);
        dedicatedStatIndex_ = -1;
        LoseTime();
        if(timeAvailable_ > 0) {
            timer.RestartTimer();
        }
    }

    public void SetDedicatedStat(int i)
    {
        if(i < 0 || i > 2) { return; }
        dedicatedStatIndex_ = i;
        if(currentTimeOfDay_ == TimeOfDay.Morning) { MorningDedicationChanged.Invoke(); }
        else if(currentTimeOfDay_ == TimeOfDay.Evening) { EveningDedicationChanged.Invoke(); }
    }

    /// <summary>
    /// Gets the number of events to spawn for this day.
    /// </summary>
    /// <returns>The number of events to spawn.</returns>
    public int GetNumOfEventsToday()
    {
        int minEvents = (int)Mathf.Lerp(DAY1_MIN_EVENTS, DAY120_MIN_EVENTS, (float)currentDay_ / (float)MaximumDay);
        int maxEvents = (int)Mathf.Lerp(DAY1_MAX_EVENTS, DAY120_MAX_EVENTS, (float)currentDay_ / (float)MaximumDay);

        return UnityEngine.Random.Range(minEvents, maxEvents + 1);
    }

    /// <summary>
    /// Populates the events for the day.
    /// </summary>
    public void GetTodaysEvents()
    {
        int numOfEvents = GetNumOfEventsToday();

        for (int i = 0; i < numOfEvents; i++)
        {
            GameEvent ge = GameEventManager.Instance.GetRandomEvent(currentDay_);
            if(ge == null) { break; }
            todaysEvents.Add(ge);
        }
    }
    /****************************************************************************************************/
    /********************************************** EVENTS **********************************************/
    /****************************************************************************************************/

    public UnityEvent MorningDedicationChanged;
    public UnityEvent EveningDedicationChanged;

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

    public enum TimeOfDay
    {
        Morning,
        Evening
    }
}
