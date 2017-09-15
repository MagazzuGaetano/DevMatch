using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell:MonoBehaviour
{
    public int row;
    public int col;

    public Cell(int row, int col)
    {
        this.row = row;
        this.col = col;
    }
}
