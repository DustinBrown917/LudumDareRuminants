using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance_;
    public static GameManager Instance { get { return instance_; } }

    [SerializeField] private Image curtain;

    [SerializeField] private bool play_;
    public bool Play { get { return play_; } }

    private Coroutine cr_FadeCurtain;

    private string sceneToLoad;

    private void Awake()
    {
        if (instance_ == null) {
            instance_ = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance_ != this) { Destroy(this.gameObject); }
        
    }

    // Use this for initialization
    void Start () {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDestroy()
    {
        if(instance_ == this)
        {
            instance_ = null;
        }
    }

    public void SetPlay(bool b)
    {
        play_ = b;
    }

    public void LoseGame(Stats stat)
    {

    }

    public void LoadScene(string scene)
    {
        sceneToLoad = scene;
        CoroutineManager.BeginCoroutine(FadeCurtainTo(1.0f, 1.0f), ref cr_FadeCurtain, this);
        CurtainClosed += LoadSceneFromEvent;

    }

    private void LoadSceneFromEvent(object sender, EventArgs e)
    {
        SceneManager.LoadScene(sceneToLoad);
        CurtainClosed -= LoadSceneFromEvent;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        CoroutineManager.BeginCoroutine(FadeCurtainTo(0, 1.0f), ref cr_FadeCurtain, this);
    }

    private IEnumerator FadeCurtainTo(float targetAlpha, float time)
    {
        Color initialColor = curtain.color;
        Color targetColor = curtain.color;
        targetColor.a = targetAlpha;

        float t = 0;

        while(t < time)
        {
            t += Time.deltaTime;
            curtain.color = Color.Lerp(initialColor, targetColor, t / time);
            yield return null;
        }

        curtain.color = targetColor;

        if(curtain.color.a == 1)
        {
            OnCurtainClosed();
        } else if(curtain.color.a == 0)
        {
            OnCurtainOpened();
        }

    }


    public event EventHandler CurtainClosed;

    private void OnCurtainClosed()
    {
        EventHandler handler = CurtainClosed;

        if(handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    public event EventHandler CurtainOpened;

    private void OnCurtainOpened()
    {
        EventHandler handler = CurtainOpened;

        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

}
