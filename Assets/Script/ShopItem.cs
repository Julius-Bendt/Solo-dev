using Juto.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    public int price;

    private int actualPrice;
    public float value;
    public string change;

    private void Start()
    {
        switch (change)
        {
            case "iap":
                actualPrice = (int)(price * App.Instance.stats.iapUpgrade * 2.84f);
                break;

            case "press":
                actualPrice = (int)(price * App.Instance.stats.pressUpgrade * Mathf.PI);
                break;

            case "hunger":
                actualPrice = price * App.Instance.stats.maxFood;
                break;

            case "marketing":
                actualPrice = (int)Mathf.Pow(price, App.Instance.stats.marketing+1);
                break;

            case "rent":
                actualPrice = (int)Mathf.Pow(price, App.Instance.stats.rent+1);
                break;

            case "app":
                actualPrice = (int)Mathf.Pow(price, App.Instance.stats.appEarnings+1);
                break;

            case "food":
                actualPrice = price;
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (App.Instance.stats.money >= actualPrice)
        {
            App.Instance.stats.money -= actualPrice;
            AudioController.PlaySound(AudioDB.FindClip("purchase_good"));
        }
        else
        {
            AudioController.PlaySound(AudioDB.FindClip("purchase_fail"));
            return;
        }


        switch (change)
        {
            case "iap":
                App.Instance.stats.iapUpgrade += (int)value;
                actualPrice = (int)(price * App.Instance.stats.iapUpgrade * Mathf.PI);
                break;

            case "press":
                App.Instance.stats.pressUpgrade += (int)value;
                actualPrice =   price * App.Instance.stats.pressUpgrade;
                break;

            case "hunger":
                App.Instance.stats.metabolismUpgrades += (int)value;
                actualPrice = price * App.Instance.stats.metabolismUpgrades;
                break;

            case "marketing":
                App.Instance.stats.marketing += value;
                actualPrice = (int)Mathf.Pow(price, App.Instance.stats.marketing+1);
                break;

            case "rent":
                App.Instance.stats.rent -= value;
                actualPrice = (int)Mathf.Pow(price, App.Instance.stats.rent+1);
                break;

            case "app":
                App.Instance.stats.appEarnings += value;
                actualPrice = (int)Mathf.Pow(price, App.Instance.stats.appEarnings+1);
                break;

            case "food":
                App.Instance.stats.food = Mathf.Clamp(App.Instance.stats.food + value,0, App.Instance.stats.maxFood);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        App.Instance.uiManager.costText.text = "-$" + actualPrice.ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        App.Instance.uiManager.costText.text = "";
    }

}
