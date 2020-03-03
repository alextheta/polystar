using System;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridExtension : MonoBehaviour
{
    [SerializeField] private GameObject gridObject;
    [SerializeField] private Vector2 gridSize;

    private Grid grid;
    private GridCell[,] gridCells;

    public GridCell GetCell(Vector2 coordinates)
    {
        try
        {
            return gridCells[(int) coordinates.x, (int) coordinates.y];
        }
        catch (IndexOutOfRangeException exception)
        {
            return null;
        }
    }

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        gridCells = new GridCell[(int) gridSize.x, (int) gridSize.y];
        
        for (int x = 0; x < gridSize.x; x++)
        for (int y = 0; y < gridSize.y; y++)
            gridCells[x, y] = CreateCellObject(x, y);
    }

    private GridCell CreateCellObject(int x, int y)
    {
        GameObject cellObject = Instantiate(gridObject, transform);

        cellObject.transform.position = new Vector3(x * grid.cellSize.x, y * grid.cellSize.y);

        return cellObject.GetComponent<GridCell>();
    }
}