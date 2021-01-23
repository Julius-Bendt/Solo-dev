using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    private Image fill;


    public void Start()
    {
        fill = transform.Find("Fill").GetComponent<Image>();
    }

    public void ChangeProcentage(float val)
    {
        fill.fillAmount = val;
    }
}
