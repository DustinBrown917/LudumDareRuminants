using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class HTPStage : MonoBehaviour {

    private CanvasGroup cg;
    private Coroutine cr_FadeIn;

    private bool doneFading_ = false;
    public bool DoneFading { get { return doneFading_; } }

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        cg.alpha = 0;
        StartFadeIn();
    }

    public void InterruptFade()
    {
        if (doneFading_) { return; }
        CoroutineManager.HaltCoroutine(ref cr_FadeIn, this);
        cg.alpha = 1.0f;
        doneFading_ = true;
    }

    void StartFadeIn()
    {
        CoroutineManager.BeginCoroutine(Fade(1.0f, 1.0f), ref cr_FadeIn, this);
    }

    private IEnumerator Fade(float targetAlpha, float time)
    {
        float initialAlpha = cg.alpha;
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t / time);
            yield return null;
        }

        cg.alpha = targetAlpha;

        doneFading_ = true;
    }
}
