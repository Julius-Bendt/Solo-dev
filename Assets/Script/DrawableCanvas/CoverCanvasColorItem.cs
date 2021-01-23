using Juto.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoverCanvasColorItem : MonoBehaviour
{
    public DrawableCanvas.ColorItem c;
    private Image img;
    public void Init(DrawableCanvas.ColorItem _c)
    {
        c = _c;

        img = GetComponent<Image>();
        img.color = c.c;
    }

    public void OnClick()
    {
        AudioController.PlaySound(AudioDB.FindClip("click"));
        App.Instance.coverCreator.UpdateColor(c);
    }
}
