using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurseTimer : AnarchyCountdownTimer
{
    public static CurseTimer singletonInstance;

    private void Awake()
    {
        singletonInstance = this;
    }

    public override void OnEnable()
    {
        this.timerDuration = 60f;
        this.timerText = GetComponent<TextMeshProUGUI>();
        this.CountdownPropsTime = AnarchyGame.CURSE_TIMER_PROP;
        base.OnEnable();
    }

    protected override void OnCountdownTimerHasExpired()
    {
        return;
    }
}
