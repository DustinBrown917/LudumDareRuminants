﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject {

    public string eventName;
    public string description;
}
