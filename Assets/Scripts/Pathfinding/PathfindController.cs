using System;
using System.Collections.Generic;
using UnityEngine;

public class PathfindController : MonoBehaviour
{
    private GridExtension gridExtension;

    private PathCell[,] gridPathCells;

    private List<PathCell> openCells;
    private List<PathCell> closedCells;
    
    private PathCell startCell;
    private PathCell endCell;
    
    private static Dictionary<PathCell.CellType, Color> cellColorMap = new Dictionary<PathCell.CellType, Color>()
    {
        {PathCell.CellType.Empty, Color.white},
        {PathCell.CellType.Solid, Color.gray},
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
        get => startCell;
        set
        {
            if (startCell != null)
                startCell.Type = PathCell.CellType.Empty;
            startCell = value;
        }
    }

    private PathCell EndCell
    {
        get => endCell;
        set
        {
            if (endCell != null)
                endCell.Type = PathCell.CellType.Empty;
            endCell = value;
        }
    }

    public void SetCellType(PathCell cell, PathCell.CellType type)
    {
        switch (type)
        {
            case PathCell.CellType.Start:
                StartCell = cell;
                break;
            case PathCell.CellType.End:
                EndCell = cell;
                break;
        }

        cell.Type = type;
    }

    public void RunPathfind()
    {
        
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
            gridPathCells[x, y] = gridExtension.GetCell(x, y).GetComponent<PathCell>();
    }

    private void ResetGrid()
    {
        foreach (PathCell pathCell in gridPathCells)
            pathCell.Reset();
     
        openCells = new List<PathCell>();
        closedCells = new List<PathCell>();

        startCell.gValue = 0;

        openCells.Add(startCell);
    }
}