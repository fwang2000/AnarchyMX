using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RoomTimer : MonoBehaviour
{
    private float timeUntilKick;
    private bool timerActive;
    private Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timeUntilKick = 60f;
        timerActive = true;
        timerText = GetComponent<Text>();
        StartCoroutine("StartCountdown", timeUntilKick);
    }

    public IEnumerator StartCountdown(float kickTime)
    {
        float currTime = kickTime;
        while (currTime > 0)
        {
            timerText.text = currTime.ToString();
            currTime--;
            yield return new WaitForSeconds(1.0f);
        }

        if (currTime <= 0 && timerActive)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(SceneManagerHelper.ActiveSceneBuildIndex - 1);
        }
    }

    public void DisableTimer()
    {
        timerActive = false;
        StopCoroutine("StartCountdown");
        transform.gameObject.SetActive(false);
    }
}
