using UnityEngine;
using System.IO;
using UnityEngine.Events;
using Juto;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using Juto.Audio;

public class App : Singleton<App>
{
    // (Optional) Prevent non-singleton constructor use.
    protected App() { }


    public Stats stats;
    public List<Game> games = new List<Game>();
    public Game currentGame = new Game();
    public Sprite[] covers;
    public bool typing;

    public int week = 32;

    public UIManager uiManager;

    public GameStatUI gameStatUI;
    public GameoverUI gameoverUI;
    public DrawableCanvas coverCreator;
    public Tutorial tut;

    public const int RENT = 115, WEEKTIME = 50;

    public ProgressBar weekBar;

    private bool showedWeekTut = false;

    private void Start()
    {
        stats = new Stats();
        uiManager = FindObjectOfType<UIManager>();
        StartCoroutine(TimeManager());

        tut.StartTutorial("welcome");
    }

    IEnumerator TimeManager()
    {
        float timeOfDay = 0;
        while (true)
        {
            if(!tut.Active && !uiManager.createGameMenu.activeSelf)
                timeOfDay += Time.deltaTime;

            if(timeOfDay >= WEEKTIME)
            {
                WeekOver();
                timeOfDay = 0;
            }

            weekBar.ChangeProcentage(timeOfDay / WEEKTIME);

            yield return null;
        }

    }

    private void WeekOver()
    {
        week++;

        if (week > 52)
            week = 1;

        stats.weeksSurvived++;

        uiManager.weekText.text = "Week " + week;

        for (int i = 0; i < games.Count; i++)
        {

            if (games[i].Popularity > 0)
            {
                if (games[i].business_model == Game.Business_model.freemium)
                {
                    float iap = 1;

                    switch (games[i].iap)
                    {
                        case Game.IAP.heavy:
                            iap = 2;
                            break;
                        case Game.IAP.medium:
                            iap = 1.5f;
                            break;
                    }

                    int earned = (int)(games[i].Popularity * (40 + (stats.iapUpgrade * 10)) * iap);
                    stats.money += earned;
                    games[i].moneyEarned += earned;

                    games[i].Popularity -= (iap - 0.5f)/10;
                }

                //Fall or increase 
                float popularity = 0;
                if (Random.Range(0, 100) > 90)
                {
                    popularity = Random.Range(0f, 2f + (games.Count / 100) + stats.marketing);
                }
                else
                {
                    float max = 0.25f - (stats.marketing / 10) - games.Count / 100;
                    popularity = Mathf.Clamp(popularity - Random.Range(0, max), -100, 0);
                }

                games[i].Popularity += popularity;

                int mult = (games[i].business_model == Game.Business_model.freemium) ? 2 : 1;
                Instance.stats.fans += (int)(games[i].Popularity * mult / 100);
            }
            else
                Debug.Log(games[i].name + " er done!");
        }

        if (!showedWeekTut)
        {
            showedWeekTut = true;
            App.Instance.tut.StartTutorial("week");
        }

        stats.money -= (int)(RENT - App.Instance.stats.rent);
    }

    private void Update()
    {
        if(!gameoverUI.gameObject.activeSelf)
        {
            if (stats.food <= 0)
            {
                gameoverUI.gameObject.SetActive(true);
                gameoverUI.Gameover("eat");

            }
            else if (stats.money <= 0)
            {
                gameoverUI.gameObject.SetActive(true);
                gameoverUI.Gameover("pay rent");
            }
        }
    }

    public void FinishedDevelopingGame()
    {
        currentGame.weekDeveloped = App.Instance.week;
        uiManager.AddGameToViewlist(currentGame);
        games.Add(App.Instance.currentGame);
        currentGame = new Game();
    }

    [System.Serializable]   
    public class Game
    {
        public string name;
        public int weekDeveloped, scope, price;
        private float popularity;
        public IAP iap;
        public Business_model business_model;
        public int moneyEarned;
        public Sprite cover;
        public State state;
        public int progress;

        public float codeQuality, graphicQuality, soundQuality;

        public enum State
        {
            programming,
            art,
            sound,
            cover,
            done
        };

        public enum Business_model
        {
            premium, 
            freemium,
            f2p
        }

        public enum IAP
        {
            heavy, 
            medium,
            low
        };

        public void ChangeState(State _state)
        {
            progress = 0;
            state = _state;
        }

        public float Popularity
        {
            get
            {
                float quality = (codeQuality + graphicQuality + soundQuality) / 3;
                return popularity + quality;
            }
            set
            {
                popularity = value;
            }
        }

        public Game()
        {

        }

        public Game (string _name, string _iap,string _Business_model,string _scope, int _price)
        {
            switch (_scope.ToLower())
            {
                case "small":
                    scope = 1;
                    break;
                case "medium":
                    scope = 2;
                    break;
                case "a":
                    scope = 4;
                    break;
                case "aa":
                    scope = 8;
                    break;
                case "aaa":
                    scope = 16;
                    break;
            }

            switch (_iap.ToLower())
            {
                case "low":
                    iap = IAP.low;
                    break;
                case "medium":
                    iap = IAP.medium;
                    break;
                case "heavy":
                    iap = IAP.heavy;
                    break;
            }


            Debug.Log(_Business_model);
            switch (_Business_model.ToLower())
            {
                case "freemium":
                    business_model = Business_model.freemium;
                    break;
                case "premium":
                    business_model = Business_model.premium;
                    break;
                case "f2p":
                    business_model = Business_model.f2p;
                    break;
            }

            name = _name;
            //iap = _iap;
            progress = 0;
            popularity = Random.Range(0f, 2.5f + (Instance.games.Count / 100) + Instance.stats.marketing) + (Instance.stats.fans+1)/1250;
            state = State.programming;
            scope = 1;
            codeQuality = graphicQuality = soundQuality = 1;

            price = _price;
        }


        public string PopularityAsString
        {
            get
            {
                string r = "";
                if (Popularity <= 0)
                    r = "Dead";
                else if (Popularity > 0f && Popularity < 0.25f)
                    r = "Dying out";
                else if (Popularity > 0.25f && Popularity < 0.75f)
                    r = "Meh";
                else if (Popularity > 0.75 && Popularity < 1.1f)
                    r = "Semi popular";
                else if (Popularity > 1.1f && Popularity < 1.4f)
                    r = "Popular";
                else if (Popularity > 1.4f && Popularity < 2)
                    r = "Hyper popular";
                else if (Popularity > 2)
                    r = "Extremly popular";

                return r;
            }
        }

        public string BMText()
        {
            string r = "";

            switch (business_model)
            {
                case Business_model.freemium:
                    r = "Game uses " + iap.ToString() + " iap/adventisement";
                    break;
                case Business_model.premium:
                    business_model = Business_model.premium;
                    r = "Game sells for " + price.ToString() + "/u";
                    break;
                case Business_model.f2p:
                    r = "Game is free to play";
                    break;
            }

            return r;
        }
    }

}
