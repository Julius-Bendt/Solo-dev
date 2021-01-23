using Juto.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class GameItem : MonoBehaviour
{
    public App.Game game;
    public TextMeshProUGUI name;

    public void Start()
    {
        name.text = game.name;
    }

    public void OnClick()
    {
        App.Instance.gameStatUI.Render(game);
    }
}
