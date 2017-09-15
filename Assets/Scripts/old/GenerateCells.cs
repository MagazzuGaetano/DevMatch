using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateCells : MonoBehaviour {

    private RectTransform grid;
    private GridLayoutGroup layout; 
  
    public GameObject[] cells;
    public Padding padding;
    public int variety = 4;

    public static int h;
    public static int w;

    private void Awake()
    {
        grid = GetComponent<RectTransform>();
        layout = GetComponent<GridLayoutGroup>();
    }

    void Start() {
        List<GameObject> choices = new List<GameObject>();

        while(choices.Count < variety)
        {
            var cell = cells[Random.Range(0, cells.Length)];

            if (!choices.Contains(cell))
            {
                choices.Add(cell);
            }
        }

        w = Mathf.RoundToInt((grid.rect.height - layout.padding.vertical) / (layout.cellSize.y + layout.spacing.y));
        h = Mathf.RoundToInt((grid.rect.width - layout.padding.horizontal) / (layout.cellSize.x + layout.spacing.x));

        for (int i = 0; i < h * w; i++)
        {
            var cell = choices[Random.Range(0, variety)];
            Instantiate(cell, transform);
        }
    }

    [System.Serializable]
    public class Padding
    {
        public float Top, Bottom, Left, Right;

        public float vertical
        {
            get
            {
                return Top + Bottom;
            }
        }

        public float horizontal
        {
            get
            {
                return Left + Right;
            }
        }
    }
}
