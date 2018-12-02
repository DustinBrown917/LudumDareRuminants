using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {

    //Singleton pattern
    private static GameEventManager instance_ = null;
    public static GameEventManager Instance { get { return instance_; } }

    [SerializeField] private GameEvent[] gameEvents;
    private List<GameEvent> availableEvents = new List<GameEvent>();

    private void Awake()
    {
        if(instance_ == null)
        {
            instance_ = this;
        } else if(instance_ != this)
        {
            Destroy(this);
        }

        SortEvents();
        availableEvents = new List<GameEvent>(gameEvents);
    }

    private void Start()
    {
        DayManager.Instance.DayEnd += DayManager_DayEnd;
    }

    private void DayManager_DayEnd(object sender, DayManager.DayEndArgs e)
    {
        availableEvents = new List<GameEvent>(gameEvents);
    }

    private void OnDestroy()
    {
        if(instance_ == this)
        {
            instance_ = null;
        }
    }

    /// <summary>
    /// Gets a random event within range of the difficulty of the day.
    /// </summary>
    /// <returns></returns>
    public GameEvent GetRandomEvent(int day)
    {
        int maximumIndex = 0;

        for(maximumIndex = 0; maximumIndex < availableEvents.Count; maximumIndex++)
        {
            if(gameEvents[maximumIndex].difficulty > day)
            {
                break;
            }
        }

        if(availableEvents.Count == 0 || availableEvents[0].difficulty > day) { return null; }

        GameEvent ge = availableEvents[UnityEngine.Random.Range(0, maximumIndex)];
        availableEvents.Remove(ge);
        return ge;
    }

    /// <summary>
    /// Sorts gameEvents in ascending order of difficulty where index 0 is the lowest difficulty.
    /// </summary>
    private void SortEvents()
    {
        GameEvent temp;

        for (int i = 0; i < gameEvents.Length; i++)
        {
            for (int j = 0; j < gameEvents.Length - 1; j++)
            {
                if (gameEvents[j].difficulty > gameEvents[j + 1].difficulty)
                {
                    temp = gameEvents[j + 1];
                    gameEvents[j + 1] = gameEvents[j];
                    gameEvents[j] = temp;
                }
            }
        }
    }
}
