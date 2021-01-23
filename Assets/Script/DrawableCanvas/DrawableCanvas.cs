using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawableCanvas : MonoBehaviour
{
    private ColorItem[,] grid;
    public Vector2Int size;
    public Tool tool;

    public GameObject coverItem;
    public Transform drawParent;

    public ColorItem[] colors;
    public GameObject colorItem;
    public Transform colorParent;
    public ColorItem currentColor;

    private CoverCanvasItem[,] cci;

    public TextMeshProUGUI assignmentText, button;
    private const int INCREASEPRDRAW = 20;

    ColorAssignment assignment;

    private int success, inAll;

    public enum Tool
    {
        Draw,
        Erase
    };

    [System.Serializable]
    public struct ColorItem
    {
        public string name;
        public Color c;
    }

    public struct ColorAssignment
    {
        public string name;
        public int val;

        public ColorAssignment(string _name)
        {
            string n = "";

            int r = Random.Range(0, 80);

            if(r >= 0 && r < 10)
            {
                n = "green";
            }
            else if(r >= 10 && r < 20)
            {
                n = "blue";
            }
            else if (r >= 20 && r < 30)
            {
                n = "purple";
            }
            else if (r >= 30 && r < 40)
            {
                n = "dark";
            }
            else if (r >= 40 && r < 50)
            {
                n = "yellow";
            }
            else if (r >= 50 && r < 60)
            {
                n = "red";
            }
            else if (r >= 60 && r < 70)
            {
                n = "grey";
            }
            else if (r >= 70 && r <= 80)
            {
                n = "empty";
            }

            name = n;
            val = Random.Range(5,15) * App.Instance.currentGame.scope;
        }
    }

    private void Start()
    {
        grid = new ColorItem[size.x, size.y];
        cci = new CoverCanvasItem[size.x, size.y];


        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++) //Drawable grid
            {
                CoverCanvasItem item = Instantiate(coverItem, drawParent).GetComponent<CoverCanvasItem>();
                item.location = new Vector2Int(x, y);
                cci[x ,y] = item;
            }
        }

        for (int i = 0; i < colors.Length; i++) //Color palette
        {
            Instantiate(colorItem, colorParent).GetComponent<CoverCanvasColorItem>().Init(colors[i]);
        }

        currentColor = colors[0];

    }

    public void OnEnable()
    {

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++) //Drawable grid
                {
                    if (cci != null)
                    {
                        cci[x, y].ChangeColor();
                    }

                    if(grid != null)
                    {
                        grid[x, y].c = Color.clear;
                    }
                }
            }


        if (App.Instance.currentGame.state == App.Game.State.art)
        {
            assignment = new ColorAssignment("");
            button.text = "Next";
            DisplayAssignment();
        }
        else
        {
            assignmentText.text = "";
            button.text = "Release";
        }

        success = inAll = 0;
    }


    public void UpdateGrid(Vector2Int location)
    {
        grid[location.x, location.y] = currentColor;
    }

    public void UpdateColor(DrawableCanvas.ColorItem c)
    {
        currentColor = c;
    }

    public void OnNext()
    {
        if(App.Instance.currentGame.state == App.Game.State.cover)
        {
            CompileGrid();
            return;
        }

        int green = 0, blue = 0, purple = 0, dark = 0, yellow = 0, red = 0, grey = 0, empty = 0;
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++) //Drawable grid
            {
                if (!string.IsNullOrEmpty(grid[x, y].name))
                {
                    switch (grid[x, y].name)
                    {
                        case "green":
                            green++;
                            break;
                        case "blue":
                            blue++;
                            break;
                        case "purple":
                            purple++;
                            break;
                        case "dark":
                            dark++;
                            break;
                        case "yellow":
                            yellow++;
                            break;
                        case "red":
                            red++;
                            break;
                        case "grey":
                            grey++;
                            break;
                    }
                }
                else
                {
                    empty++;
                }

                cci[x, y].ChangeColor();
            }
        }

        int val = 0;
        switch (assignment.name)
        {
            case "green":
                val = green;
                break;
            case "blue":
                val = blue;
                break;
            case "purple":
                val = purple;
                break;
            case "dark":
                val = dark;
                break;
            case "yellow":
                val = yellow;
                break;
            case "red":
                val = red;
                break;
            case "grey":
                val = grey;
                break;
        }

        if (val >= assignment.val)
        {
            App.Instance.stats.artProgress++;
            success++;
        }


        inAll++;


        App.Instance.currentGame.progress += INCREASEPRDRAW;

        if(App.Instance.currentGame.progress+INCREASEPRDRAW >= 100)
            button.text = "Create sounds";

        if (App.Instance.currentGame.progress >= 100)
        {
            App.Instance.currentGame.graphicQuality = (float)(success + 1f) / (float)(inAll + 1f) * App.Instance.stats.GraphicLevel;
            App.Instance.uiManager.midiMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        else
            DisplayAssignment();
    }

    private void DisplayAssignment()
    {
        assignment = new ColorAssignment("");
        assignmentText.text = "Draw " + assignment.val + " " + assignment.name + " pixels";
    }

    public void CompileGrid()
    {
        Texture2D tex = new Texture2D(size.x, size.y);
        tex.filterMode = FilterMode.Point;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                tex.SetPixel(x, y, grid[x, y].c);
                grid[x, y].c = Color.clear; //Prepare for next image
            }
        }


        tex.Apply(true);

        Sprite s = Sprite.Create(tex,new Rect(0,0,size.x,size.y),Vector2.zero);

        App.Instance.currentGame.cover = s;
        App.Instance.FinishedDevelopingGame();
        App.Instance.uiManager.gameMenuButton.text = "Games";

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                cci[x,y].ChangeColor();
            }
        }

        gameObject.SetActive(false);
    }

}
