using UnityEngine;

[RequireComponent(typeof(GridExtension))]
[RequireComponent(typeof(PathfindController))]
public class GridSavingController : MonoBehaviour
{
    private GridExtension gridExtension;
    private PathfindController pathfindController;

    private const string GridSizeKeyX = "GridSizeX";
    private const string GridSizeKeyY = "GridSizeY";

    public void SaveCell(PathCell cell)
    {
        string cellPrefKey = cell.x + " " + cell.y;

        if (cell.Type == PathCell.CellType.Empty)
            PlayerPrefs.DeleteKey(cellPrefKey);
        else
            PlayerPrefs.SetInt(cellPrefKey, (int) cell.Type);
    }

    public void Load()
    {
        for (int i = 0; i < gridExtension.gridSize.x; i++)
        for (int j = 0; j < gridExtension.gridSize.y; j++)
        {
            string cellPrefKey = i + " " + j;
            PathCell.CellType type = PathCell.CellType.Empty;

            if (PlayerPrefs.HasKey(cellPrefKey))
                type = (PathCell.CellType) PlayerPrefs.GetInt(cellPrefKey);

            GridCell gridCell = gridExtension.GetCell(i, j);
            if (gridCell)
            {
                PathCell pathCell = gridCell.GetComponent<PathCell>();
                pathfindController.SetCellType(pathCell, type);
            }
        }
    }

    private void Awake()
    {
        gridExtension = GetComponent<GridExtension>();
        pathfindController = GetComponent<PathfindController>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(GridSizeKeyX) != gridExtension.gridSize.x
            || PlayerPrefs.GetInt(GridSizeKeyY) != gridExtension.gridSize.y)
        {
            PlayerPrefs.DeleteAll();
        }

        PlayerPrefs.SetInt(GridSizeKeyX, gridExtension.gridSize.x);
        PlayerPrefs.SetInt(GridSizeKeyY, gridExtension.gridSize.y);

        Load();
    }
}