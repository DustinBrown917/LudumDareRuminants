using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {

    //Singleton pattern
    private static GameEventManager instance_ = null;
    public static GameEventManager Instance { get { return instance_; } }

    [SerializeField] private GameEvent[] gameEvents;

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
    public GameEvent GetRandomEvent()
    {
        int maximumIndex = 0;

        for(maximumIndex = 0; maximumIndex < gameEvents.Length; maximumIndex++)
        {
            if(gameEvents[maximumIndex].difficulty >= DayManager.Instance.CurrentDay)
            {
                break;
            }
        }

        return gameEvents[UnityEngine.Random.Range(0, maximumIndex + 1)];
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
