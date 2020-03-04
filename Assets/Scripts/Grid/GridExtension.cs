using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridExtension : MonoBehaviour
{
    public Vector2 gridSize;
    public Grid grid;

    [SerializeField] private GameObject gridObject;

    private GridCell[,] gridCells;
    private GridCell startCell;
    private GridCell endCell;

    private Dictionary<GridCell.CellType, Color> cellColorMap = new Dictionary<GridCell.CellType, Color>()
    {
        {GridCell.CellType.Empty, Color.white},
        {GridCell.CellType.Solid, Color.gray},
        {GridCell.CellType.Start, Color.green},
        {GridCell.CellType.End, Color.cyan}
    };
    
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

    public Color GetCellTypeColor(GridCell.CellType type)
    {
        if (cellColorMap.TryGetValue(type, out Color color))
            return color;

        return Color.red;
    }

    public void RegisterSpecialCell(GridCell cell)
    {
        switch (cell.GetCellType())
        {
            case GridCell.CellType.Start:
                if (!ReferenceEquals(startCell, null))
                    startCell.SetCellType(GridCell.CellType.Empty);
                startCell = cell;
                break;
            case GridCell.CellType.End:
                if (!ReferenceEquals(endCell, null))
                    endCell.SetCellType(GridCell.CellType.Empty);
                endCell = cell;
                break;
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