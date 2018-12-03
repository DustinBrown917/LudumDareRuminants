using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Image Pool", menuName = "Dedication Image Pool")]
public class DedicationImagePool : ScriptableObject {

    [SerializeField] Sprite[] images;

    public Sprite GetImage(Stats s)
    {
        int i = (int)s;

        return images[i];
    }
}
