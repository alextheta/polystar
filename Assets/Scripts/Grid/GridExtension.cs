using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridExtension : MonoBehaviour
{
    public Vector2Int gridSize;
    public Vector2 cellSize => grid.cellSize;

    [SerializeField] private GameObject cellObject;

    private Grid grid;
    private GridCell[,] gridCells;
    
    public GridCell GetCell(int x, int y)
    {
        try
        {
            return gridCells[x, y];
        }
        catch
        {
            return null;
        }
    }

    public GridCell GetCellInWorld(Vector2 position)
    {
        Vector3Int cellCoordinates = grid.WorldToCell(position);
        return GetCell(cellCoordinates.x, cellCoordinates.y);
    }

    private void Awake()
    {
        grid = GetComponent<Grid>();
        InitGrid();
    }
    
    private void InitGrid()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        gridCells = new GridCell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        for (int y = 0; y < gridSize.y; y++)
            gridCells[x, y] = CreateCellObject(x, y);
    }

    private GridCell CreateCellObject(int x, int y)
    {
        GameObject cellObject = Instantiate(this.cellObject, transform);

        cellObject.transform.position = new Vector3(x * grid.cellSize.x, y * grid.cellSize.y);

        return cellObject.GetComponent<GridCell>();
    }
}