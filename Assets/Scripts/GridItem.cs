using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour {

    [HideInInspector]
    public int id;

    public int x {
        get;
        private set;
    }

    public int y {
        get;
        private set;
    }
    
    public void OnItemPositionChanged(int NewX, int NewY)
    {
        this.x = NewX;
        this.y = NewY;
        this.gameObject.name = string.Format("Sprite [{0},{1}]", x, y);
    }

    public void OnMouseDown()
    {
        if (OnMouseOverItemHandler != null)
        {
            OnMouseOverItemHandler(this);
        }
    }

    public delegate void OnMouseOverItem (GridItem item);
    public static event OnMouseOverItem OnMouseOverItemHandler;
}
