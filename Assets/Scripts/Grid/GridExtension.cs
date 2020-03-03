using System;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridExtension : MonoBehaviour
{
    public Vector2 gridSize;
    
    [SerializeField] private GameObject gridObject;
    
    private Grid grid;
    private GridCell[,] gridCells;

    public GridCell GetCell(Vector2 coordinates)
    {
        try
        {
            return gridCells[(int) coordinates.x, (int) coordinates.y];
        }
        catch
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
        InitGrid();
    }

    private void InitGrid()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        
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