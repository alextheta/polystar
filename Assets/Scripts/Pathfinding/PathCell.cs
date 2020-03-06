using UnityEngine;

public class PathCell : MonoBehaviour
{
    public int x, y;
    public PathCell previousCell;
    public int gValue;
    public int hValue;
    public int fValue => gValue + hValue;

    private PathfindController pathfindController;

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
            GetComponent<GridCell>().SetColor(cellColor);
        }
    }

    public void Reset()
    {
        Color cellColor = PathfindController.GetCellColorByType(Type);
        GetComponent<GridCell>().SetColor(cellColor);
        previousCell = null;
        gValue = hValue = 0;
    }
}