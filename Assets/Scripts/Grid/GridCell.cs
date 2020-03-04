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

    private GridExtension gridExtension;
    private CellType cellType;
    private SpriteRenderer spriteRenderer;

    public void SetCellType(CellType type)
    {
        cellType = type;
        spriteRenderer.color = gridExtension.GetCellTypeColor(cellType);

        gridExtension.RegisterSpecialCell(this);
    }

    public CellType GetCellType()
    {
        return cellType;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridExtension = FindObjectOfType<GridExtension>();
    }
}