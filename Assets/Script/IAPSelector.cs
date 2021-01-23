using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Juto.Audio;

public class IAPSelector : MonoBehaviour
{
    public Color imgOn, imgOff, textOn, textOff;

    public Options[] options;
    public Options current;

    [System.Serializable]
    public struct Options
    {
        public string name;
        public Image img;
        public TextMeshProUGUI txt;
    }
   
    public void Select(string name)
    {
        if(!string.IsNullOrEmpty(current.name))
        {
            current.img.color = imgOff;
            current.txt.color = textOff;
        }


        foreach (Options option in options)
        {
            if(option.name == name)
            {
                current = option;
                current.img.color = imgOn;
                current.txt.color = textOn;
            }
        }
    }

    public void Deselect()
    {
        if (!string.IsNullOrEmpty(current.name))
        {
            current.img.color = imgOff;
            current.txt.color = textOff;
        }
    }

}
