using UnityEngine;

public class GridCell : MonoBehaviour
{
    public enum CellType
    {
        Empty,
        Solid,
        Start,
        End
    };
    
    public CellType cellType;
}
