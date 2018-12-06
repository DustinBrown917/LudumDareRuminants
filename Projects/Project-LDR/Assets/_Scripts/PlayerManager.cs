using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField] private Transform bedTransform;
    [SerializeField] private Transform deskTransform;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform doNothingTransform;

    [SerializeField] Transform playerTransform;
    private SpriteRenderer playerSpriteRenderer;

    [SerializeField] private Sprite mainGraphic;
    [SerializeField] private Sprite deskGraphic;
    [SerializeField] private Sprite bedGraphic;

    private Coroutine cr_Moving;
    private AudioSource audioSource;

    [SerializeField] private AudioClip socialClip;
    [SerializeField] private AudioClip sleepClip;
    [SerializeField] private AudioClip successClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        playerSpriteRenderer = playerTransform.gameObject.GetComponent<SpriteRenderer>();
	}
	

    public void MovePlayerTo()
    {
        playerSpriteRenderer.enabled = true;
        playerSpriteRenderer.sprite = mainGraphic;
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
        
        CoroutineManager.BeginCoroutine(LerpTo(playerTransform, t.position, 1.0f, s), ref cr_Moving, this);
    }

    public void HandleArriveAtLocation(Stats s)
    {
        switch (s)
        {
            case Stats.SOCIAL:
                playerSpriteRenderer.enabled = false;
                audioSource.clip = socialClip;
                audioSource.Play();
                break;
            case Stats.SLEEP:
                playerSpriteRenderer.sprite = bedGraphic;
                audioSource.clip = sleepClip;
                audioSource.Play();
                break;
            case Stats.SUCCESS:
                playerSpriteRenderer.sprite = deskGraphic;
                audioSource.clip = successClip;
                audioSource.Play();
                break;
            default:
                break;
        }       
    }

    public IEnumerator LerpTo(Transform trans, Vector3 targetPosition, float totalTime, Stats s)
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
        HandleArriveAtLocation(s);
    }


}
