using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class StartTimer : AnarchyCountdownTimer
{
    public static StartTimer singletonInstance;

    [SerializeField] private CurseTimer curseTimer;
    [SerializeField] private RoundTimer roundTimer;

    private void Awake()
    {
        singletonInstance = this;
    }

    public override void OnEnable()
    {
        this.timerDuration = 10f;
        this.timerText = GetComponent<TextMeshProUGUI>();
        this.CountdownPropsTime = AnarchyGame.START_TIMER_PROP;
        base.OnEnable();
    }

    protected override string DisplayTime(float countdown)
    {
        return Mathf.CeilToInt(countdown).ToString();
    }

    protected override void OnCountdownTimerHasExpired()
    {
        curseTimer.SetStartTime();
        roundTimer.SetStartTime();
        AnarchyGame.FreezeAllPlayers(false);
        return;
    }
}
