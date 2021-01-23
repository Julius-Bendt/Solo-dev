using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Juto.Audio;
using System.Text.RegularExpressions;

public class ComputerUI : MonoBehaviour
{
    public string[] code;

    public static int index;

    private int defaultSplit = 4;

    public TextAsset codeText;

    int pressesTillFinished;


    public TextMeshProUGUI text, startTyping;
    public GameObject createArtObj;
    private RectTransform textRect;

    private const int MINTAPS = 150, MAXTAPS = 250;

    private const string bugString = "<#E74C3C>BUGS<#FFFFFF>";

    private int inAll;

    void OnEnable()
    {
        textRect = text.GetComponent<RectTransform>();
        int _split = defaultSplit * App.Instance.stats.pressUpgrade;

        code = CompileString(_split);
        startTyping.gameObject.SetActive(true);
        startTyping.text = "Start typing!";
        index = 0;

        createArtObj.SetActive(false);

        if (!string.IsNullOrEmpty(App.Instance.currentGame.name))
            index = App.Instance.currentGame.progress;


        pressesTillFinished = Random.Range(MINTAPS,MAXTAPS) - ((App.Instance.stats.pressUpgrade - 1) * 50) + (App.Instance.games.Count * 20);
    }


    private void Update()
    {
        if (App.Instance.typing && index < pressesTillFinished && !App.Instance.tut.Active)
        {
            if (Input.anyKeyDown)
            {
                if(startTyping.text != "Programming finished!\n<size=50%>go create some art for it!")
                    startTyping.gameObject.SetActive(false);

                if(Input.GetKeyDown(KeyCode.Backspace))
                {
                    if(index > 0)
                    {
                        int split = (23 > defaultSplit * App.Instance.stats.pressUpgrade) ? 23 : defaultSplit * App.Instance.stats.pressUpgrade;
                        text.text = text.text.Remove(text.text.Length - split);
                        index--;

                    }
                }
                else
                {
                    float chance = 10 * (10 - App.Instance.stats.ProgrammingLevel) / 10;
                    if (Random.Range(0, 100) <= chance && text.text.Length > 25)
                    {
                        text.text += bugString;
                    }
                    else
                    {
                        text.text += code[index];
                        index++;
                        App.Instance.stats.programmingProgress++;
                    }

                    inAll++;
                }



                //Play audio
                AudioController.PlaySound(AudioDB.FindClip("computer_click"),false,0.5f);

                float preferredHeight = LayoutUtility.GetPreferredHeight(textRect);
                if (preferredHeight > 1100)
                {
                    float height = preferredHeight - 1100;
                    textRect.localPosition = new Vector3(0, height, 0);
                }

                if (index >= pressesTillFinished)
                {
                    startTyping.gameObject.SetActive(true);
                    startTyping.text = "Programming finished!\n<size=50%>go create some art for it!";
                    createArtObj.SetActive(true);

                    App.Instance.currentGame.state = App.Game.State.art;

                    textRect.localPosition = new Vector3(0, 0, 0);
                    App.Instance.typing = false;
                    App.Instance.uiManager.gameMenuButton.text = "Back";
                    AudioController.PlaySound(AudioDB.FindClip("weekover"));

                    int bugs = Regex.Matches(text.text, bugString, RegexOptions.IgnoreCase).Count;
                    text.text = "";
                    float quality = (float)((index + 2)-((bugs + 1) * Mathf.PI)) / (float)(index + 2);

                    App.Instance.currentGame.codeQuality *= quality * App.Instance.stats.ProgrammingLevel;

                }
            }
        }
    }


    private string[] CompileString(int split)
    {
        string input = codeText.text;
        List<string> output = input
            .Select((c, i) => new { letter = c, group = i / split })
            .GroupBy(l => l.group, l => l.letter)
            .Select(g => string.Join("", g))
            .ToList();

        return output.ToArray();
    }
}
