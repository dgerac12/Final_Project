using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;

    public TextMeshProUGUI timeUpText;

    public float timeNum;
    public float graceTime;
    public TextMeshProUGUI timerText;

    private bool restart;

    private IEnumerator countdown;

    void Start()
    {
        countdown = Countdown();
        restart = false;
        timeUpText.text = "";
        StartCoroutine (countdown);
        Time.timeScale = 1;
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        timerText.text = ("Time Left: " + "" + timeNum);

        if (restart)
        {
            if (Input.GetKeyDown ("space"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        if (playerController.lives == 0)
        {
            Destroy(this);
        }

        if (string.Compare(playerController.winText.text, "You Win!") == 0)
        {
                StopCoroutine(countdown);
        }
    }

    IEnumerator Countdown()
    {
        while (true)
        {
            if (timeNum == 0)
            {
                playerController.overText.text = "Game Over";
                timeUpText.text = "Time is Up!";
                yield return new WaitForSeconds(graceTime);
                if (timeNum == 0)
                {
                    playerController.restartText.text = "Press 'Spacebar' to Restart";
                    restart = true;
                }
                break;
            }
            yield return new WaitForSeconds (1);
            timeNum--;
        }
    }
}
