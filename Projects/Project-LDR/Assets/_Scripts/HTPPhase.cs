using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTPPhase : MonoBehaviour {

    private CanvasGroup cg;
    [SerializeField] private HTPStage[] stages;
    private Coroutine cr_Fade = null;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        cg.alpha = 1;
    }

    private void OnDisable()
    {
        foreach(HTPStage s in stages)
        {
            s.gameObject.SetActive(false);
        }
    }

    public void Next()
    {
        for(int i = 0; i < stages.Length; i++)
        {
            if (!stages[i].gameObject.activeSelf)
            {
                stages[i].gameObject.SetActive(true);
                if(i > 0) { stages[i - 1].InterruptFade(); }
                return;
            }
        }

        if(cr_Fade == null)
        {
            FadeTo(0);
        } else
        {
            CoroutineManager.HaltCoroutine(ref cr_Fade, this);
            cg.alpha = 0;
            OnPhaseDone();
        }
    }

    public void FadeTo(float alpha)
    {
        CoroutineManager.BeginCoroutine(Fade(alpha, 1.0f), ref cr_Fade, this);
    }

    private IEnumerator Fade(float targetAlpha, float time)
    {
        float initialAlpha = cg.alpha;
        float t = 0;
        while(t < time)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t / time);
            yield return null;
        }

        cg.alpha = targetAlpha;

        if (cg.alpha == 0) {
            OnPhaseDone();
            gameObject.SetActive(false);
        }
    }





    public event EventHandler PhaseDone;

    private void OnPhaseDone()
    {
        EventHandler handler = PhaseDone;

        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }
}
