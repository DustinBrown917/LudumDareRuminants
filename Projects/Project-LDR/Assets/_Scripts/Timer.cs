using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


    [SerializeField, Tooltip("The image that will represent the time remaining.")] private Image timerGraphic;
    [SerializeField, Tooltip("The transform of the text of the timer.")] private Transform timerTextTransform;
    [SerializeField, Tooltip("The text that will display the time remaining.")] private Text timerText;

    //Container for the text scaling coroutine.
    private Coroutine cr_TextScaling = null;
    //Container for the timer countdown coroutine.
    private Coroutine cr_timerRunning = null;

    [SerializeField, Tooltip("The time the timer should start from.")] private float startFromTime = 10;
    

	// Use this for initialization
	void Start () {
        
	}
	
    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        CoroutineManager.BeginCoroutine(RunTimer(startFromTime), ref cr_timerRunning, this);
    }

    public void SetStartTime(float t)
    {
        startFromTime = t;
    }

    /// <summary>
    /// Counts the timer down while also updating the graphic and text displays.
    /// </summary>
    /// <param name="fromTime">The time the timer should start from.</param>
    /// <returns></returns>
    private IEnumerator RunTimer(float fromTime)
    {
        float t = fromTime;
        int timeDisplayed = (int)fromTime + 1;
        SetLabelText(timeDisplayed.ToString());

        while(t > 0)
        {
            t -= Time.deltaTime;
            timerGraphic.fillAmount = t / fromTime;

            if(timeDisplayed != (int)t + 1)
            {
                timeDisplayed = (int)t + 1;
                SetLabelText(timeDisplayed.ToString());
            }

            yield return null;
        }

        SetLabelText(0.ToString());
        TimeUp.Invoke();
    }

    /// <summary>
    /// Sets the label of the timer to reflect the time remaining. If withAnimation is true, will pulse with the change.
    /// </summary>
    /// <param name="text">What should the label read?</param>
    /// <param name="withAnimation">Should the new text animate?</param>
    public void SetLabelText(string text, bool withAnimation = true)
    {
        timerText.text = text;
        if (withAnimation)
        {
            CoroutineManager.BeginCoroutine(CoroutineManager.ShrinkScaleFrom(timerTextTransform, new Vector3(2.0f, 2.0f, 2.0f), Vector3.one, 1.0f), ref cr_TextScaling, this);
        }
    }

    /// <summary>
    /// Resets then stars the timer.
    /// </summary>
    public void RestartTimer()
    {
        ResetTimer((int)startFromTime);
        StartTimer();
    }

    /// <summary>
    /// Resets the timer to 
    /// </summary>
    /// <param name="displayedTime"></param>
    public void ResetTimer(int displayedTime)
    {
        //CoroutineManager.HaltCoroutine(cr_timerRunning, this);
        SetLabelText(displayedTime.ToString(), false);
        timerGraphic.fillAmount = 1;
    }



    /****************************************************************************************************/
    /********************************************** EVENTS **********************************************/
    /****************************************************************************************************/

    public UnityEvent TimeUp;

}
