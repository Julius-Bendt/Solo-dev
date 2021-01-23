using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Juto.Audio;

public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI desc, button;
    private int index;

    private GameObject[] objs;

    private Tutorial.TutorialItem item;

    public void TriggerUI(Tutorial.TutorialItem _item)
    {
        index = -1; //Get to 0 at the "next" function, thus the index will be 0
        item = _item;
        button.text = "Next";
        Next();
    }

    public void StopTutorial()
    {
        if (objs != null)
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(false);
            }
        }

        gameObject.SetActive(false);
    }

    public void Next()
    {
        AudioController.PlaySound(AudioDB.FindClip("click"));
        index++;

        //Disable old objs
        if(objs != null)
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(false);
            }
        }


        if(index < item.actions.Length)
        {
            desc.text = item.actions[index].desc;

            objs = item.actions[index].enableObj;
            foreach (GameObject obj in item.actions[index].enableObj)
            {
                obj.SetActive(true);
            }

            if(index == item.actions.Length-1)
            {
                button.text = "Close";
            }
        }
        else
        {
            StopTutorial();
        }
    }
}
