using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameoverUI : MonoBehaviour
{
    string reasonText = "Remeber to <color=#e74c3c>{0}!";
    string weeksText = "You were in the business for <color=#e67e22>{0} <color=#fff>weeks!";

    public TextMeshProUGUI reason, weeks;

    public void Gameover(string _reason)
    {
        reason.text = string.Format(reasonText, _reason);
        weeks.text = string.Format(weeksText, App.Instance.stats.weeksSurvived);
    }

    public void Restart()
    {
        Destroy(App.Instance.gameObject);
        SceneManager.LoadScene("game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
