using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(GridExtension))]
public class PathfindController : MonoBehaviour
{
    public bool allowDiagonalMove;
    public bool showCheckedCells;
    private GridExtension gridExtension;
    private PathCell[,] gridPathCells;
    private HashSet<PathCell> openCells;
    private HashSet<PathCell> closedCells;
    private PathCell startCell;
    private PathCell endCell;

    private const int HValueMultiplier = 10;
    private const int GValueNear = 10;
    private const int GValueFar = 14;
    private Color PathCellColor = Color.yellow;
    private Color CheckedCellColor = Color.gray;

    private static Dictionary<PathCell.CellType, Color> cellColorMap = new Dictionary<PathCell.CellType, Color>()
    {
        {PathCell.CellType.Empty, Color.white},
        {PathCell.CellType.Solid, Color.black},
        {PathCell.CellType.Start, Color.green},
        {PathCell.CellType.End, Color.cyan}
    };

    public static Color GetCellColorByType(PathCell.CellType type)
    {
        if (cellColorMap.TryGetValue(type, out Color color))
            return color;

        return Color.red;
    }

    private PathCell StartCell
    {
        set
        {
            if (startCell)
                startCell.Type = PathCell.CellType.Empty;
            startCell = value;
        }
    }

    private PathCell EndCell
    {
        set
        {
            if (endCell)
                endCell.Type = PathCell.CellType.Empty;
            endCell = value;
        }
    }

    public void SetCellType(PathCell cell, PathCell.CellType type)
    {
        if (type == PathCell.CellType.Start)
            StartCell = cell;
        if (type == PathCell.CellType.End)
            EndCell = cell;

        if (cell.Type == PathCell.CellType.Start && type != PathCell.CellType.Start)
            StartCell = null;
        if (cell.Type == PathCell.CellType.End && type != PathCell.CellType.End)
            EndCell = null;

        cell.Type = type;
    }

    public void Pathfind()
    {
        if (startCell == null)
        {
            Debug.LogError("Start cell is not specified");
            return;
        }

        if (endCell == null)
        {
            Debug.LogError("End cell is not specified");
            return;
        }

        ResetGrid();
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        openCells.Add(startCell);
        bool pathFound = false;

        while (openCells.Count > 0)
        {
            PathCell currentCell = GetLowestValueCell(openCells);

            if (currentCell == endCell)
            {
                UpdateFoundPathCells();
                pathFound = true;
                break;
            }

            openCells.Remove(currentCell);
            closedCells.Add(currentCell);

            ProcessNeighbors(currentCell);
        }
        
        stopwatch.Stop();

        if (pathFound)
            Debug.Log("Path found");
        else
            Debug.Log("Path not found");
        
        Debug.Log("Elapsed time: " + stopwatch.Elapsed);
    }

    public void ResetGrid()
    {
        foreach (PathCell pathCell in gridPathCells)
            pathCell.Reset();

        openCells = new HashSet<PathCell>();
        closedCells = new HashSet<PathCell>();
    }

    private void Awake()
    {
        gridExtension = GetComponent<GridExtension>();
    }

    private void Start()
    {
        gridPathCells = new PathCell[gridExtension.gridSize.x, gridExtension.gridSize.y];

        for (int x = 0; x < gridExtension.gridSize.x; x++)
        for (int y = 0; y < gridExtension.gridSize.y; y++)
        {
            gridPathCells[x, y] = gridExtension.GetCell(x, y).GetComponent<PathCell>();
            gridPathCells[x, y].x = x;
            gridPathCells[x, y].y = y;
        }
    }

    private PathCell GetLowestValueCell(HashSet<PathCell> list)
    {
        PathCell result = list.First();
        foreach (PathCell cell in list)
            if (cell.fValue < result.fValue)
                result = cell;

        return result;
    }

    private void ProcessNeighbors(PathCell cell)
    {
        for (int i = cell.x - 1; i <= cell.x + 1; i++)
        for (int j = cell.y - 1; j <= cell.y + 1; j++)
        {
            if (!allowDiagonalMove && cell.x != i && cell.y != j)
                continue;

            if (gridExtension.GetCell(i, j))
            {
                PathCell neighborCell = gridPathCells[i, j];

                if (showCheckedCells && neighborCell.Type == PathCell.CellType.Empty)
                    neighborCell.GetComponent<GridCell>().SetColor(CheckedCellColor);

                if (neighborCell.Type == PathCell.CellType.Solid
                    || (i == cell.x && j == cell.y)
                    || closedCells.Contains(neighborCell))
                    continue;

                if (openCells.Contains(neighborCell))
                    UpdateOpenCell(neighborCell, cell);
                else
                    AddOpenCell(neighborCell, cell);
            }
        }
    }

    private void AddOpenCell(PathCell cell, PathCell previousCell)
    {
        int localGValue = CalculateHValue(previousCell, cell) > HValueMultiplier ? GValueFar : GValueNear;
        cell.gValue = previousCell.gValue + localGValue;
        cell.hValue = CalculateHValue(cell, endCell);
        cell.previousCell = previousCell;

        openCells.Add(cell);
    }

    private void UpdateOpenCell(PathCell cell, PathCell previousCell)
    {
        int localGValue = CalculateHValue(previousCell, cell) > HValueMultiplier ? GValueFar : GValueNear;
        int totalNewGValue = previousCell.gValue + localGValue;
        if (cell.gValue > totalNewGValue)
        {
            cell.gValue = totalNewGValue;
            cell.previousCell = previousCell;
        }
    }

    private int CalculateHValue(PathCell a, PathCell b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) * HValueMultiplier;
    }

    private void UpdateFoundPathCells()
    {
        PathCell currentCell = endCell;

        while (currentCell != startCell)
        {
            if (currentCell.Type == PathCell.CellType.Empty)
                currentCell.GetComponent<GridCell>().SetColor(PathCellColor);

            currentCell = currentCell.previousCell;
        }
    }
}