using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class SwitchHandler : MonoBehaviour
{

    public Transform container;
    public GridLayoutGroup layout;

    private Vector2 startPosition;
    private GameObject selectedCell;
    private GameObject lastSelectedCell;

    private GameObject[,] matrix;

    private void Start()
    {
        matrix = new GameObject[(GenerateCells.w), (GenerateCells.h)];
        FillMatrix();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector3.one);

            if (hit.collider && hit.collider.tag == "Code")
            {
                layout.enabled = false;

                if (!selectedCell)
                {
                    selectedCell = hit.collider.gameObject;
                    startPosition = selectedCell.GetComponent<RectTransform>().position;
                    selectedCell.GetComponent<Image>().enabled = true;
                }
                else
                {
                    lastSelectedCell = selectedCell;

                    selectedCell = hit.collider.gameObject;
                    selectedCell.GetComponent<Image>().enabled = true;

                    Cell Cell1 = selectedCell.GetComponent<Cell>();
                    Cell Cell2 = lastSelectedCell.GetComponent<Cell>();

                    if (checkedNeighbour(Cell1, Cell2))
                    {
                        List<GameObject> _list = checkedNeighbourinTheSameRowCol(Cell1, Cell2);

                        for (int i = 0; i < _list.Count; i++)
                        {
                            print(_list[i].name);
                        }

                        if (_list.Count > 0)
                        {
                            lastSelectedCell.GetComponent<RectTransform>().position = selectedCell.GetComponent<RectTransform>().position;
                            selectedCell.GetComponent<RectTransform>().position = startPosition;

                            var temp = lastSelectedCell;
                            lastSelectedCell = selectedCell;
                            selectedCell = temp;


                        }
                    }

                    lastSelectedCell.GetComponent<Image>().enabled = false;
                    selectedCell.GetComponent<Image>().enabled = false;
                    lastSelectedCell = selectedCell;
                    selectedCell = null;
                }
            }

        }

    }

    void FillMatrix()
    {
        for (var j = 0; j < layout.transform.childCount; j++)
        {
            var cell = getCellByIndex(j, GenerateCells.w, GenerateCells.h);
            layout.transform.GetChild(j).GetComponent<Cell>().col = cell.col;
            layout.transform.GetChild(j).GetComponent<Cell>().row = cell.row;
            matrix[cell.row, cell.col] = layout.transform.GetChild(j).gameObject;
        }
    }

    Cell getCellByIndex(int index, int row, int col)
    {
        var k = 0;
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (k == index)
                {
                    return new Cell(i, j);
                }
                k++;
            }
        }
        return null;
    }

    bool checkedNeighbour(Cell Cell1, Cell Cell2)
    {
        return Cell2.row + 1 == Cell1.row && Cell2.col == Cell1.col ||
               Cell2.col + 1 == Cell1.col && Cell2.row == Cell1.row ||
               Cell2.row - 1 == Cell1.row && Cell2.col == Cell1.col ||
               Cell2.col - 1 == Cell1.col && Cell2.row == Cell1.row;
    }

    List<GameObject> checkedNeighbourinTheSameRowCol(Cell Cell1, Cell Cell2)
    {
        List<GameObject> Similiars = new List<GameObject>();

        if (CheckDirection(Cell1, Cell2) == Directions.Vertical)
        {
            var left = false;
            var right = false;

            Cell leftCell = new Cell(Cell2.col, Cell2.col);
            Cell rightCell = new Cell(Cell2.col, Cell2.col);

            do
            {
                if (leftCell.col > 0)
                {
                    leftCell.col--;
                    left = checkedNeighbour(Cell2, leftCell);

                    if (left)
                    {
                        var cell = matrix[leftCell.row, leftCell.col];

                        if (cell.name == Cell2.name)
                            Similiars.Add(cell);
                    }
                }

                if (rightCell.col < GenerateCells.w)
                {
                    rightCell.col++;
                    right = checkedNeighbour(Cell2, rightCell);

                    if (right)
                    {
                        var cell = matrix[rightCell.row, rightCell.col];

                        if (cell.name == Cell2.name)
                            Similiars.Add(cell);
                    }
                }
            } while (left || right);
        }
        else if (CheckDirection(Cell1, Cell2) == Directions.Horizontal)
        {
            var top = false;
            var bottom = false;

            Cell topCell = new Cell(Cell2.row, Cell2.col);
            Cell bottomCell = new Cell(Cell2.row, Cell2.col);

            do
            {
                if (topCell.row > 0)
                {
                    topCell.row--;
                    top = checkedNeighbour(Cell2, topCell);

                    if (top)
                    {
                        var cell = matrix[topCell.row, topCell.col];

                        if (cell.name == Cell2.name)
                            Similiars.Add(cell);
                    }
                }

                if (bottomCell.row < GenerateCells.h)
                {
                    bottomCell.row++;
                    bottom = checkedNeighbour(Cell2, bottomCell);

                    if (bottom)
                    {
                        var cell = matrix[bottomCell.row, bottomCell.col];

                        if (cell.name == Cell2.name)
                            Similiars.Add(cell);
                    }
                }
            } while (top || bottom);
        }

        return Similiars;
    }

    Directions CheckDirection(Cell Cell1, Cell Cell2)
    {
        if (Cell1.col == Cell2.col) return Directions.Vertical;
        else if (Cell1.row == Cell2.row) return Directions.Horizontal;
        else return Directions.none;
    }

    enum Directions
    {
        Horizontal,
        Vertical,
        none
    }
}
*/