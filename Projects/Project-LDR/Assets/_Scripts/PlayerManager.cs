using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private Transform bedTransform;
    [SerializeField] private Transform deskTransform;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform doNothingTransform;

    [SerializeField] Transform playerTransform;

    private Coroutine cr_Moving;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MovePlayerTo()
    {
        Stats s = (Stats)DayManager.Instance.DedicatedStatIndex;
        Transform t = null;

        switch (s)
        {
            case Stats.SOCIAL:
                t = doorTransform;
                break;
            case Stats.SLEEP:
                t = bedTransform;
                break;
            case Stats.SUCCESS:
                t = deskTransform;
                break;
            default:
                t = doNothingTransform;
                break;
        }

        CoroutineManager.BeginCoroutine(LerpTo(playerTransform, t.position, 1.0f), ref cr_Moving, this);
    }

    public static IEnumerator LerpTo(Transform trans, Vector3 targetPosition, float totalTime)
    {
        Vector3 initialPosition = trans.position;
        targetPosition.z = initialPosition.z;

        float t = 0;

        while (t < totalTime)
        {
            t += Time.deltaTime;
            trans.position = Vector3.Lerp(initialPosition, targetPosition, t / totalTime);
            yield return null;
        }

        trans.position = targetPosition;

    }
}
