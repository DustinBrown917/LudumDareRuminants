using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionHandler : MonoBehaviour {

    private static DecisionHandler instance_ = null;
    public static DecisionHandler Instance { get { return instance_; } }

    private Dictionary<Stats, StatModifier> statModifiers = new Dictionary<Stats, StatModifier>();

    private void Awake()
    {
        Debug.Log("This");
        if(instance_ == null) { instance_ = this; }
        else if(instance_ != this) { Destroy(this.gameObject); }
        statModifiers.Add(Stats.SOCIAL, new StatModifier());
        statModifiers.Add(Stats.SLEEP, new StatModifier());
        statModifiers.Add(Stats.SUCCESS, new StatModifier());
    }

    private void Start()
    {
        DayManager.Instance.DayEnd += DayManager_DayEnd;
    }

    private void DayManager_DayEnd(object sender, DayManager.DayEndArgs e)
    {
        ExecuteDecisions();
        ResetModifiers();
    }

    public StatModifier GetStatModifier(Stats stat)
    {
        if (!statModifiers.ContainsKey(stat)) { return null; }
        return statModifiers[stat];
    }

    private void ResetModifiers()
    {
        foreach(Stats s in statModifiers.Keys)
        {
            statModifiers[s].Reset();
        }
    }

    private void OnDestroy()
    {
        if(instance_ == this)
        {
            instance_ = null;
        }
    }

    public void DedicateTimeTo(Stats stat)
    {
        if (!statModifiers.ContainsKey(stat)) { return; }

        statModifiers[stat].timeDedication += 1;
        if(statModifiers[stat].timeDedication == 0) { statModifiers[stat].timeDedication = 1; }
        Debug.Log("Time dedicated to: " + stat.ToString());
    }

    public void ExecuteDecisions()
    {
        Player.Instance.ChangeSocial(statModifiers[Stats.SOCIAL].timeDedication * ((statModifiers[Stats.SOCIAL].timeDedication > 0)? statModifiers[Stats.SOCIAL].gainMultiplier : statModifiers[Stats.SOCIAL].damageMultiplier));
        Player.Instance.ChangeSleep(statModifiers[Stats.SLEEP].timeDedication * ((statModifiers[Stats.SLEEP].timeDedication > 0) ? statModifiers[Stats.SLEEP].gainMultiplier : statModifiers[Stats.SLEEP].damageMultiplier));
        Player.Instance.ChangeSuccess(statModifiers[Stats.SUCCESS].timeDedication * ((statModifiers[Stats.SUCCESS].timeDedication > 0) ? statModifiers[Stats.SUCCESS].gainMultiplier : statModifiers[Stats.SUCCESS].damageMultiplier));

        foreach(StatModifier sm in statModifiers.Values)
        {
            sm.Log();
        }
    }
}
