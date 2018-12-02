using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject {

    public string eventName;
    public string description;
    public int difficulty;
    public Stats statEffected;
    public GameEventActions action;
    public int amount;

    public void ExecuteEvent()
    {
        StatModifier sm = DecisionHandler.Instance.GetStatModifier(statEffected);
        if(sm == null) { return; }

        switch (statEffected)
        {
            case Stats.SOCIAL:

                break;
            case Stats.SLEEP:
                break;
            case Stats.SUCCESS:
                break;
            case Stats.TIME:
                break;
            default:
                break;
        }
    }

}
