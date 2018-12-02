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
    public float amount;

    public int colourClassification;

    public void ExecuteEvent()
    {
        StatModifier sm = DecisionHandler.Instance.GetStatModifier(statEffected);

        switch (action)
        {
            case GameEventActions.INSTANT_ADD:
                break;
            case GameEventActions.INSTANT_SUBTRACT:
                break;
            case GameEventActions.MULTIPLY_GAIN:
                sm.gainMultiplier *= amount;
                break;
            case GameEventActions.MULTIPLY_DAMAGE:
                sm.damageMultiplier *= amount;
                break;
            default:
                break;
        }
    }

}
