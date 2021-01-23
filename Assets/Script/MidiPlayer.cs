using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Juto.Audio;

public class MidiPlayer : MonoBehaviour
{
    //r = 1;

    public AudioClip clip;

    public NodeKeys[] nodes;

    private const int PROGRESSINCREASE = 10;
    private NodeKeys nextKey;

    public int success, inAll;

    public float calc;

    public Color nextKeyColor, pressColor, normalColor;

    private bool playing = false;

    public GameObject nextAssignment;

    [System.Serializable]
    public class NodeKeys
    {
        public KeyCode key;
        public float pitch;
        public Image img;

        [HideInInspector]
        public AudioSource source;
    }

    private void OnEnable()
    {
        success = inAll = 0;
        nextAssignment.SetActive(false);

        nextKey = RandomKey();
        App.Instance.currentGame.progress = 0;
        playing = true;

    }

    public void Update()
    {
        if (!playing)
            return;


        foreach (NodeKeys node in nodes)
        {
            if (Input.GetKeyDown(node.key))
            {
                node.source = AudioController.PlaySound(clip).GetComponent<AudioSource>();
                node.source.pitch = node.pitch;

                if(node.key == nextKey.key)
                {
                    success++;
                    App.Instance.stats.soundProgress++;
                }

                inAll++;


                nextKey = RandomKey();
                node.img.color = pressColor;
                App.Instance.currentGame.progress += PROGRESSINCREASE;
            }
            else if(Input.GetKeyUp(node.key))
            {
                node.source.Stop();
                node.img.color = normalColor;
            }

            if (App.Instance.currentGame.progress >= 100)
            {
                node.img.color = normalColor;
            }

        }

        if (App.Instance.currentGame.progress >= 100)
        {
            playing = false;
            float c = (float)(success + 1f) / (float)(inAll + 1f);
            App.Instance.currentGame.soundQuality *= c * App.Instance.stats.SoundLevel;

            nextAssignment.SetActive(true);
            App.Instance.currentGame.state = App.Game.State.cover;
        }
    
    }

    private NodeKeys RandomKey()
    {
        KeyCode oldKey = KeyCode.None;
        if(nextKey != null)
        {
            nextKey.img.color = normalColor;
            oldKey = nextKey.key;

        }


        NodeKeys n = nodes[Random.Range(0, nodes.Length)];

        while(n.key == oldKey)
        {
            n = nodes[Random.Range(0, nodes.Length)];
        }

        n.img.color = nextKeyColor;


        return n;
    }

}
