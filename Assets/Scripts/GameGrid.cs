using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public int XSize, YSize;
    public float ItemWidth;
    public Transform container;
    public int MinItemsForMatch = 3;
    public float delayBetweenMatches = 0.2f;
    public int variety = 4;
    public AudioSource clip;
    public AudioSource clip2;

    private GameObject[] _choices;
    private GameObject[] _Items;
    private GridItem[,] grid;
    private GridItem CurrentSelectedItem;

    void Start()
    {
        GetItems();
        FillGrid();
        ClearGrid();
        GridItem.OnMouseOverItemHandler += OnMouseOverItem;
    }

    void OnDisable()
    {
        GridItem.OnMouseOverItemHandler -= OnMouseOverItem;
    }

    void OnMouseOverItem(GridItem item)
    {
        clip.Play();

        if (CurrentSelectedItem == item || !GameStatus.CanPlay) return;

        if (CurrentSelectedItem == null)
        {
            CurrentSelectedItem = item;
        }
        else
        {
            float xDIff = Mathf.Abs(item.x - CurrentSelectedItem.x);
            float yDIff = Mathf.Abs(item.y - CurrentSelectedItem.y);

            if (xDIff + yDIff == 1)
            {
                StartCoroutine(TryMatch(CurrentSelectedItem, item));
            }
            else
            {
                print("error");
            }

            CurrentSelectedItem = null;
        }
    }

    void SwapIndices(GridItem a, GridItem b)
    {
        int oldX = a.x;
        int oldY = a.y;

        GridItem temp = grid[a.x, a.y];
        grid[a.x, a.y] = b;
        grid[b.x, b.y] = temp;

        a.OnItemPositionChanged(b.x, b.y);
        b.OnItemPositionChanged(oldX, oldY);
    }

    void ChangeRigidBodyStatus(bool status)
    {
        foreach (GridItem item in grid)
        {
            if (item != null)
            {
                item.GetComponent<Rigidbody2D>().isKinematic = !status;
            }
        }
    }

    void ClearGrid()
    {
        for (int x = 0; x < XSize; x++)
        {
            for (int y = 0; y < YSize; y++)
            {
                MatchInfo m = GetMatchInformation(grid[x, y]);
                if (m.validMatch)
                {
                    Destroy(grid[x, y].gameObject);
                    grid[x, y] = Instantiate(x, y);
                    y--;
                }
            }
        }
    }

    void FillGrid()
    {
        grid = new GridItem[XSize, YSize];

        for (var x = 0; x < XSize; x++)
        {
            for (var y = 0; y < YSize; y++)
            {
                grid[x, y] = Instantiate(x, y);
            }
        }
    }

    void GetItems()
    {
        _choices = Resources.LoadAll<GameObject>("Prefabs");
        _Items = new GameObject[4];

        int counter = 0;
        while (counter < variety)
        {
            _Items[counter] = _choices[Random.Range(0, _choices.Length)];
            counter++;
        }

        for (var k = 0; k < _Items.Length; k++)
        {
            _Items[k].GetComponent<GridItem>().id = k;
        }
    }

    IEnumerator TryMatch(GridItem a, GridItem b)
    {
        yield return StartCoroutine(Swap(a, b));

        MatchInfo A = GetMatchInformation(a);
        MatchInfo B = GetMatchInformation(b);

        if (!A.validMatch && !B.validMatch)
        {
            yield return StartCoroutine(Swap(a, b));
            yield break;
        }

        if (A.validMatch)
        {
            yield return StartCoroutine(DestroyItems(A.match));
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return StartCoroutine(UpdateGridAfterMatch(A));
        }
        else if (B.validMatch)
        {
            yield return StartCoroutine(DestroyItems(B.match));
            yield return new WaitForSeconds(delayBetweenMatches);
            yield return StartCoroutine(UpdateGridAfterMatch(B));
        }
    }

    IEnumerator Swap(GridItem a, GridItem b)
    {
        ChangeRigidBodyStatus(false);
        float movDuration = 0.1f;
        Vector3 aPosition = a.transform.position;
        StartCoroutine(a.transform.Move(b.transform.position, movDuration));
        StartCoroutine(b.transform.Move(aPosition, movDuration));
        yield return new WaitForSeconds(movDuration);
        SwapIndices(a, b);
        ChangeRigidBodyStatus(true);
    }

    IEnumerator DestroyItems(List<GridItem> items)
    {
        foreach (var item in items)
        {
            if (item != null)
            {
                yield return StartCoroutine(item.transform.Scale(Vector3.zero, 0.1f));
                Destroy(item.gameObject);
                yield return new WaitForSeconds(0.05f);
                GameStatus.Score += 50;
                yield return new WaitForSeconds(0.05f);
                clip2.Play();
            }
        }
    }

    IEnumerator UpdateGridAfterMatch(MatchInfo match)
    {

        if (match.matchStartingY == match.matchEndingY)
        {
            for (int x = match.matchStartingX; x <= match.matchEndingX; x++)
            {
                for (int y = match.matchStartingY; y < YSize - 1; y++)
                {
                    GridItem upperIndex = grid[x, y + 1];
                    GridItem current = grid[x, y];
                    grid[x, y] = upperIndex;
                    grid[x, y + 1] = current;
                    grid[x, y].OnItemPositionChanged(grid[x, y].x, grid[x, y].y - 1);
                }
                grid[x, YSize - 1] = Instantiate(x, YSize - 1);
            }
        }
        else if (match.matchStartingX == match.matchEndingX)
        {
            int matchHeight = 1 + (match.matchEndingY - match.matchStartingY);

            for (int y = match.matchStartingY + matchHeight; y <= YSize - 1; y++)
            {
                GridItem lowerIndex = grid[match.matchStartingX, y - matchHeight];
                GridItem current = grid[match.matchStartingX, y];
                grid[match.matchStartingX, y - matchHeight] = current;
                grid[match.matchStartingX, y] = lowerIndex;
            }

            for (int y = 0; y < YSize - matchHeight; y++)
            {
                grid[match.matchStartingX, y].OnItemPositionChanged(match.matchStartingX, y);
            }

            for (int i = 0; i < match.match.Count; i++)
            {
                grid[match.matchStartingX, (YSize - 1) - i] = Instantiate(match.matchStartingX, (YSize - 1) - i);
            }

        }

        for (int x = 0; x < XSize; x++)
        {
            for (int y = 0; y < YSize; y++)
            {
                MatchInfo m = GetMatchInformation(grid[x, y]);
                if (m.validMatch)
                {
                    //yield return new WaitForSeconds(delayBetweenMatches);
                    yield return StartCoroutine(DestroyItems(m.match));
                    yield return new WaitForSeconds(delayBetweenMatches);
                    yield return StartCoroutine(UpdateGridAfterMatch(m));
                }
            }
        }
    }

    GridItem Instantiate(int x, int y)
    {
        GameObject randomItem = _Items[Random.Range(0, _Items.Length)];
        GridItem newItem = ((GameObject)Instantiate(randomItem, new Vector2((x + 1.2f) * ItemWidth, y), Quaternion.identity, container)).GetComponent<GridItem>();
        newItem.OnItemPositionChanged(x, y);
        return newItem;
    }

    List<GridItem> SearchHorizontal(GridItem item)
    {
        List<GridItem> hitems = new List<GridItem> { item };
        int left = item.x - 1;
        int right = item.x + 1;

        while (left >= 0 && grid[left, item.y].id == item.id)
        {
            hitems.Add(grid[left, item.y]);
            left--;
        }

        while (right < XSize && grid[right, item.y].id == item.id)
        {
            hitems.Add(grid[right, item.y]);
            right++;
        }
        return hitems;
    }

    List<GridItem> SearchVertical(GridItem item)
    {
        List<GridItem> vitems = new List<GridItem> { item };
        int top = item.y + 1;
        int botttom = item.y - 1;

        while (botttom >= 0 && grid[item.x, botttom].id == item.id)
        {
            vitems.Add(grid[item.x, botttom]);
            botttom--;
        }

        while (top < YSize && grid[item.x, top].id == item.id)
        {
            vitems.Add(grid[item.x, top]);
            top++;
        }

        return vitems;
    }

    MatchInfo GetMatchInformation(GridItem item)
    {
        MatchInfo m = new MatchInfo();
        m.match = null;
        List<GridItem> hMatch = SearchHorizontal(item);
        List<GridItem> vMatch = SearchVertical(item);

        if (hMatch.Count >= MinItemsForMatch && hMatch.Count >= vMatch.Count)
        {
            m.matchStartingX = GetMinimumX(hMatch);
            m.matchEndingX = GetMaximiumX(hMatch);
            m.matchStartingY = m.matchEndingY = hMatch[0].y;
            m.match = hMatch;
        }
        else if (vMatch.Count >= MinItemsForMatch)
        {
            m.matchStartingY = GetMinimumY(vMatch);
            m.matchEndingY = GetMaximiumY(vMatch);
            m.matchStartingX = m.matchEndingX = vMatch[0].x;
            m.match = vMatch;
        }

        return m;
    }

    int GetMinimumX(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int)Mathf.Min(indices);
    }
    int GetMaximiumX(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].x;
        }
        return (int)Mathf.Max(indices);
    }
    int GetMinimumY(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Min(indices);
    }
    int GetMaximiumY(List<GridItem> items)
    {
        float[] indices = new float[items.Count];
        for (int i = 0; i < indices.Length; i++)
        {
            indices[i] = items[i].y;
        }
        return (int)Mathf.Max(indices);
    }

}
