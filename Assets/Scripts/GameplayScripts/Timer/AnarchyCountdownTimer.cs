using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public abstract class AnarchyCountdownTimer : MonoBehaviourPunCallbacks
{
    protected float timerDuration;
    protected TextMeshProUGUI timerText;
    protected string CountdownPropsTime;
    private bool isTimerRunning;
    private int startTime;

    public override void OnEnable()
    {
        base.OnEnable();
        Initialize();
    }

    public void Update()
    {
        if (!this.isTimerRunning) return;

        float countdown = TimeRemaining();
        this.timerText.text = DisplayTime(countdown);

        if (countdown > 0.0f) return;

        OnTimerEnds();
    }

    private void OnTimerRuns()
    {
        this.isTimerRunning = true;
        this.enabled = true;
    }

    private void OnTimerEnds()
    {
        this.isTimerRunning = false;
        this.enabled = false;
        this.timerText.text = string.Empty;

        OnCountdownTimerHasExpired();
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Initialize();
    }

    private void Initialize()
    {
        int propStartTime;
        if (TryGetStartTime(out propStartTime))
        {
            this.startTime = propStartTime;
            this.isTimerRunning = TimeRemaining() > 0;

            if (this.isTimerRunning)
            {
                OnTimerRuns();
            }
            else
            {
                OnTimerEnds();
            }
        }
    }

    public bool TryGetStartTime(out int startTimestamp)
    {
        startTimestamp = PhotonNetwork.ServerTimestamp;
        object startTimeFromProps;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownPropsTime, out startTimeFromProps))
        {
            startTimestamp = (int)startTimeFromProps;
            return true;
        }

        return false;
    }

    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - this.startTime;
        return this.timerDuration - timer / 1000f;
    }

    public void SetStartTime()
    {
        int startTime = 0;
        bool wasSet = TryGetStartTime(out startTime);

        Hashtable props = new Hashtable
        {
            { CountdownPropsTime, (int)PhotonNetwork.ServerTimestamp }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    protected virtual string DisplayTime(float countdown)
    {
        int timeRemaining = Mathf.CeilToInt(countdown);
        int minutes = timeRemaining / 60;
        int seconds = timeRemaining - (minutes * 60);

        string secondsText;

        if (seconds < 10)
        {
            secondsText = "0" + seconds.ToString();
        }
        else
        {
            secondsText = seconds.ToString();
        }

        return minutes.ToString() + ":" + secondsText;
    }

    protected abstract void OnCountdownTimerHasExpired();
}
