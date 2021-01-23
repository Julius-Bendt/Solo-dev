using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<TutorialItem> tutorials = new List<TutorialItem>();
    public TutorialUI ui;
    public GameObject window;
    private TutorialItem _item;

    public string CurrentActive
    {
        get
        {
            return _item.name;
        }
    }

    public bool Active
    {
        get
        {
            return window.activeSelf;   
        }
    }


    public void StartTutorial(string name)
    {
        if (!string.IsNullOrEmpty(_item.name))
            StopTutorial();

        foreach (TutorialItem item in tutorials)
        {
            if(item.name == name)
            {
                _item = item;
                window.SetActive(true);
                ui.TriggerUI(item);
            }
        }
    }

    public void StopTutorial()
    {
        ui.StopTutorial();
    }

    [System.Serializable]
    public struct TutorialItem
    {
        public string name;
        public TutorialItemAction[] actions;
    }

    [System.Serializable]
    public struct TutorialItemAction
    {
        public GameObject[] enableObj;
        public string desc;
    }
}
