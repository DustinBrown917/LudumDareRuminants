using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier {


    public float gainMultiplier = 1;
    public float damageMultiplier = 1;
    public float timeDedication = -1;

    public StatModifier()
    {
    }

    public void Reset()
    {
        gainMultiplier = 1;
        damageMultiplier = 1;
        timeDedication = -1;
    }
}
