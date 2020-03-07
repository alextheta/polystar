using UnityEngine;

public class PathCell : MonoBehaviour
{
    public int x, y;
    public PathCell previousCell;
    public int gValue;
    public int hValue;
    public int fValue => gValue + hValue;

    private PathfindController pathfindController;
    public GridCell gridCell;

    public enum CellType
    {
        Empty,
        Solid,
        Start,
        End
    };
    
    private CellType cellType;

    public CellType Type
    {
        get => cellType;
        set
        {
            cellType = value;
            Color cellColor = PathfindController.GetCellColorByType(value);
            gridCell.SetColor(cellColor);
        }
    }

    public void Reset()
    {
        Color cellColor = PathfindController.GetCellColorByType(Type);
        gridCell.SetColor(cellColor);
        previousCell = null;
        gValue = hValue = 0;
    }

    private void Awake()
    {
        gridCell = GetComponent<GridCell>();
    }
}