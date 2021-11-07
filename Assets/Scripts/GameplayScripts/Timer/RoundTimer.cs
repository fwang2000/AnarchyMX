using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundTimer : AnarchyCountdownTimer
{
    private static RoundTimer singletonInstance;

    private void Awake()
    {
        singletonInstance = this;
    }

    public override void OnEnable()
    {
        this.timerDuration = 480f;
        this.timerText = GetComponent<TextMeshProUGUI>();
        this.CountdownPropsTime = AnarchyGame.ROUND_TIMER_PROP;
        base.OnEnable();
    }

    protected override void OnCountdownTimerHasExpired()
    {
        return;
    }
}
