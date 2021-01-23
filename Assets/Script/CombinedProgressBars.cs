using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombinedProgressBars : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject main, other;

    public ProgressBar food, sleep, social, relax, needs;

    float test = 60;

    public void OnPointerEnter(PointerEventData eventData)
    {
        main.SetActive(false);
        other.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        main.SetActive(true);
        other.SetActive(false);
    }


    private void Update()
    {
        test -= Time.deltaTime;


        float f = App.Instance.stats.food / App.Instance.stats.maxFood;
        float s = App.Instance.stats.sleep / App.Instance.stats.maxSleep;
        float so = App.Instance.stats.social / App.Instance.stats.maxSocial;
        float r = App.Instance.stats.relax / App.Instance.stats.maxRelax;

        float mainProcentage = f + s + so + r;

        food.ChangeProcentage(f);
        sleep.ChangeProcentage(s);
        social.ChangeProcentage(so);
        relax.ChangeProcentage(r);
        needs.ChangeProcentage(mainProcentage / 4);



    }
}
