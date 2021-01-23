using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStatUI : MonoBehaviour
{
    public TextMeshProUGUI name, popularity, iap, moneyEarned, weekDeveloped;
    public Image cover;
    //public Window_Graph graph;

    private void Start()
    {
        StopRender();
    }

    public void Render(App.Game game)
    {
        name.text = "Name: " + game.name;
        popularity.text = "Popularity: " + game.PopularityAsString;
        iap.text = game.BMText();
        moneyEarned.text = "Money earned: " + game.moneyEarned;
        weekDeveloped.text = "Week developed: " + game.weekDeveloped;
        cover.color = Color.white;
        cover.sprite = game.cover;
    }

    public void StopRender()
    {
        name.text = popularity.text = iap.text = moneyEarned.text = weekDeveloped.text = "";
        cover.color = Color.clear;
    }
}
