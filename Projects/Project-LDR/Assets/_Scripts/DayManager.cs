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
    public const int DAY1_MIN_EVENTS = 0;
    public const float DAY1_TIME = 5.0f;
    public const float DAY120_TIME = 1.5f;
    public const int DAY120_MIN_EVENTS = 5;
    public const int DAY1_MAX_EVENTS = 2;
    public const int DAY120_MAX_EVENTS = 6;
    public const int TIME_IN_DAY = 2;

    public static readonly string[] dayTimeStrings = { "Morning", "Evening" };
    [SerializeField] private Image dayTimeImage;
    [SerializeField] private Sprite[] dayTimeImages;
    private Coroutine cr_DayTimeImage;

    private TimeOfDay currentTimeOfDay_;
    public TimeOfDay CurrentTimeOfDay { get { return currentTimeOfDay_; } }

    [SerializeField] private Text countdownText;
    private Coroutine cr_CountdownText;

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

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource clickEffectSource;
    [SerializeField] private AudioSource dayEffectSource;
    

    private Coroutine cr_BeginDay;

    private void Awake()
    {
        if(instance_ == null) {
            instance_ = this;
        } else if (instance_ != this) {
            Destroy(this);
        }

        timer.TimeUp.AddListener(HandleTimerTimeOut);
    }

    private void Start()
    {
        GameManager.Instance.CurtainOpened += GameManager_CurtainOpened;
    }

    private void GameManager_CurtainOpened(object sender, EventArgs e)
    {
        CoroutineManager.BeginCoroutine(StartDayTimer(), ref cr_BeginDay, this);
    }

    private IEnumerator StartDayTimer()
    {
        countdownText.gameObject.SetActive(true);
        int countDownSeconds = 3;
        while(countDownSeconds > 0)
        {
            UpdateCountdownLabel(countDownSeconds);
            countDownSeconds -= 1;
            yield return new WaitForSeconds(1.0f);
        }
        countdownText.gameObject.SetActive(false);
        GameManager.Instance.SetPlay(true);
        musicSource.Play();
        StartDay();       
    }

    private void UpdateCountdownLabel(int timeLeft)
    {
        countdownText.text = timeLeft.ToString();
        CoroutineManager.BeginCoroutine(CoroutineManager.ShrinkScaleFrom(countdownText.transform, new Vector3(1.5f, 1.5f, 1.0f), Vector3.one, 1.0f), ref cr_CountdownText, this);
    }

    private void OnDestroy()
    {
        if(instance_ == this) {
            instance_ = null;
        }
        GameManager.Instance.CurtainOpened -= GameManager_CurtainOpened;
    }

    public void EndDay()
    {
        todaysEvents = new List<GameEvent>();

        OnDayEnd(new DayEndArgs(currentDay_));

        dayEffectSource.Play();

        if(currentDay_ == MaximumDay)
        {
            GameManager.Instance.WinGame();
        }

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
        if (!GameManager.Instance.Play) { return; }
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
            timer.SetStartTime(Mathf.Lerp(DAY1_TIME, DAY120_TIME, (float)currentDay_ / (float)MaximumDay));
            timer.RestartTimer();
        }
    }

    public void SetDedicatedStat(int i)
    {
        if(i < 0 || i > 2 || !GameManager.Instance.Play) { return; }
        dedicatedStatIndex_ = i;
        clickEffectSource.Play();
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
