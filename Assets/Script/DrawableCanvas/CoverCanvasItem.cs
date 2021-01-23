using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CoverCanvasItem : MonoBehaviour, IPointerEnterHandler
{
    public Vector2Int location;
    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    public void ChangeColor()
    {
        img.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0))
        {
            img.color = App.Instance.coverCreator.currentColor.c;
            App.Instance.coverCreator.UpdateGrid(location);
        }
    }
}
