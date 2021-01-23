using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public int money = 1000, iapUpgrade = 1, pressUpgrade = 1, fans;
    public float marketing, rent, appEarnings;

    public int metabolismUpgrades;

    public int weeksSurvived = 0;

    public int programmingProgress, artProgress, soundProgress;
    private int programmingLevel, artLevel, soundLevel;


    public int ProgrammingLevel
    {
        get
        {
            return 1;
            return Mathf.Clamp(programmingProgress / (int)(Mathf.Pow(7, programmingLevel) / 10), 1, 10);
        }
    }

    public int GraphicLevel
    {
        get
        {
            return 1;
            return Mathf.Clamp(artProgress / (int)(Mathf.Pow(4, artLevel) / 10),1,10);
        }
    }

    public int SoundLevel
    {
        get
        {
            return 1;
            return Mathf.Clamp(soundProgress / (int)(Mathf.Pow(3, soundLevel) / 10), 1, 10);
        }
    }



    public float sleepRate = 0.2f, socialRate = 0.2f, relaxRate = 0.2f;
    public float sleep, social, relax, food;
    public int maxSleep = 50, maxSocial = 60, maxRelax = 40, maxFood = 35;

    public float hungerRate
    {
        get
        {
            return 0.2f - (metabolismUpgrades * 0.01f);
        }
    }

    public Stats()
    {
        food = maxFood;
        sleep = maxSleep;
        social = maxSocial;
        relax = maxRelax;
    }
}
