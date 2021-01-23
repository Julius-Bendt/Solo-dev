using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Juto.Audio;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText, weekText, costText;
    public GameObject shopMenu, createGameMenu, computer, gameMenu,midiMenu, createCoverMenu;

    public TextMeshProUGUI greenButton, blueButton, gameMenuButton;

    public Transform gameView;
    public GameItem gameViewPrefab;

    private bool showedHungryTut = false;

    public bool ShopOpen
    {
        get
        {
            return shopMenu.activeSelf;
        }
    }

    public bool GameMenuOpen
    {
        get
        {
            return gameMenu.activeSelf;
        }
    }

    public void Update()
    {
        if (!App.Instance.tut.Active)
        {
            App.Instance.stats.food -= App.Instance.stats.hungerRate * Time.deltaTime;
            App.Instance.stats.sleep -= App.Instance.stats.sleepRate * Time.deltaTime;
            App.Instance.stats.relax -= App.Instance.stats.relaxRate * Time.deltaTime;
            App.Instance.stats.social -= App.Instance.stats.socialRate * Time.deltaTime;
        }


        if (App.Instance.stats.food <= App.Instance.stats.maxFood * 0.2f && !showedHungryTut)
        {
            showedHungryTut = true;
            App.Instance.tut.StartTutorial("hungry");
        }

        moneyText.text = "$" + App.Instance.stats.money.ToString();
    }


    public void CreateGame()
    {
        AudioController.PlaySound(AudioDB.FindClip("click"));

        if (App.Instance.tut.CurrentActive == "games_open")
        {
            App.Instance.tut.StopTutorial();
        }

        if (ShopOpen)
        {
            shopMenu.SetActive(false);
            blueButton.text = "Shop";
        }

        if (GameMenuOpen)
        {
            gameMenu.SetActive(false);
            gameMenuButton.text = "Back";
        }

        if (App.Instance.typing) //Back pressed
        {
            Debug.Log("typeing");

            if (string.IsNullOrEmpty(App.Instance.currentGame.name))
            {
                if(App.Instance.currentGame.state == App.Game.State.programming)
                    App.Instance.currentGame.progress += ComputerUI.index;
            }

            App.Instance.typing = false;
            computer.SetActive(false);
        }
        else //Computer open
        {
            //greenButton.text = "Back";

            if (string.IsNullOrEmpty(App.Instance.currentGame.name)) //No game there yet
                createGameMenu.SetActive(true);
            else
            {
                switch(App.Instance.currentGame.state)
                {
                    case App.Game.State.programming:
                        Debug.Log("game !null programming");
                        computer.SetActive(true);
                        createCoverMenu.SetActive(false);
                        midiMenu.SetActive(false);
                        App.Instance.typing = true;
                        break;
                    case App.Game.State.art:
                        Debug.Log("game !null drawing");
                        computer.SetActive(false);
                        midiMenu.SetActive(false);
                        createCoverMenu.SetActive(true);
                        App.Instance.typing = false;
                        break;
                    case App.Game.State.sound:
                        Debug.Log("game !null drawing");
                        computer.SetActive(false);
                        createCoverMenu.SetActive(false);
                        midiMenu.SetActive(true);
                        App.Instance.typing = false;
                        break;
                }

            }
        }

        ChangeGreenButtonText();
    }

    public void OpenGameMenu()
    {
        AudioController.PlaySound(AudioDB.FindClip("click"));

        if (App.Instance.tut.CurrentActive == "games_open")
        {
            App.Instance.tut.StopTutorial();
        }

        if (App.Instance.tut.CurrentActive == "welcome")
        {
            App.Instance.tut.StartTutorial("games_open");
        }

        if (ShopOpen)
        {
            shopMenu.SetActive(false);
            blueButton.text = "Shop";
        }

        if(GameMenuOpen)
        {
            gameMenuButton.text = "Games";
        }
        else
        {
            gameMenuButton.text = "Back";
        }

        if (App.Instance.typing)
        {
            if (string.IsNullOrEmpty(App.Instance.currentGame.name))
            {
                App.Instance.currentGame.progress += ComputerUI.index;
            }

            App.Instance.typing = false;
            computer.SetActive(false);
            createCoverMenu.SetActive(false);
        }

        ChangeGreenButtonText();
        gameMenu.SetActive(!GameMenuOpen);
    }

    public void OpenShop()
    {
        AudioController.PlaySound(AudioDB.FindClip("click"));

        if (App.Instance.tut.CurrentActive == "games_open")
        {
            App.Instance.tut.StopTutorial();
        }

        if (ShopOpen)
        {
            blueButton.text = "Shop";
        }
        else
        {
            blueButton.text = "Back";
        }

        if (GameMenuOpen)
        {
            gameMenu.SetActive(false);
            gameMenuButton.text = "Games";
        }

        if (App.Instance.typing)
        {
            if (string.IsNullOrEmpty(App.Instance.currentGame.name))
            {
                App.Instance.currentGame.progress += ComputerUI.index;
            }

            App.Instance.typing = false;
            computer.SetActive(false);
        }

        ChangeGreenButtonText();
        shopMenu.SetActive(!ShopOpen);
    }

    public void AddGameToViewlist(App.Game game)
    {
        Instantiate(gameViewPrefab, gameView, false).GetComponent<GameItem>().game = game;
    }

    private void ChangeGreenButtonText()
    {
        if(!string.IsNullOrEmpty(App.Instance.currentGame.name))
        {
            string s = "";

            switch (App.Instance.currentGame.state)
            {
                case App.Game.State.programming:
                    s = "Continue game";
                    break;
                case App.Game.State.art:
                    s = "Draw game cover";
                    break;
                case App.Game.State.sound:
                    s = "Create Game music";
                    break;
                case App.Game.State.done:
                    break;
                default:
                    break;
            }

            greenButton.text = s;
        }
        else
        {
            greenButton.text = "Create Game";
        }
      
    }


    public void MakeArt()
    {
        computer.SetActive(false);
        midiMenu.SetActive(false);
        createCoverMenu.SetActive(true);
        App.Instance.typing = false;
    }
}
