using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Juto.Audio;

public class CreateGameUI : MonoBehaviour
{
    public GameObject nameErrorMessage, iapErrorMessage, computer, iapObj, priceObj;
    public TMP_InputField name, price;
    public IAPSelector business_model, iap, scope;


    private void Update()
    {
        if(!string.IsNullOrEmpty(business_model.current.name))
        {
            switch (business_model.current.name)
            {
                case "Premium":
                    priceObj.SetActive(true);
                    iapObj.SetActive(false);
                    break;
                case "Freemium":
                    priceObj.SetActive(false);
                    iapObj.SetActive(true);
                    break;
                case "f2p":
                    priceObj.SetActive(false);
                    iapObj.SetActive(false);
                    break;
            }
        }
    }

    public void CreateGame()
    {
        bool stop = false;
        if (string.IsNullOrEmpty(iap.current.name) && business_model.current.name == "Freemium")
        {
            stop = true;
            iapErrorMessage.SetActive(true);
        }


        if (string.IsNullOrEmpty(name.text))
        {
            nameErrorMessage.SetActive(true);
            stop = true;
        }

        if (stop)
            return;




        string _name = name.text;
        string bm = business_model.current.name;
        string _iap = iap.current.name;
        string _scope = scope.current.name;


        int _price = 0;

        if(!string.IsNullOrEmpty(price.text))
            int.TryParse(price.text,out _price);

        App.Instance.currentGame = new App.Game(_name, _iap,bm,_scope,_price);

        //goto computer
        computer.SetActive(true);
        App.Instance.typing = true;
        gameObject.SetActive(false);
        App.Instance.uiManager.shopMenu.SetActive(false);
        App.Instance.uiManager.gameMenu.SetActive(false);
        AudioController.PlaySound(AudioDB.FindClip("click"));
    }

    private void OnEnable()
    {
        name.text = "";
        iap.Deselect();
    }
}
