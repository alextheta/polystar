using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridInputController : MonoBehaviour
{
    [SerializeField] private ToggleGroup cellTypeToggleGroup;
    private GridExtension gridExtension;
    private PathfindController pathfindController;
    private Camera mainCamera;

    private const int MouseButtonLeft = 0;
    private const int MouseButtonRight = 1;

    private void Awake()
    {
        gridExtension = GetComponent<GridExtension>();
        pathfindController = GetComponent<PathfindController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(MouseButtonLeft) && !EventSystem.current.IsPointerOverGameObject())
            UpdateCellType();

        if (Input.GetMouseButtonDown(MouseButtonRight) && !EventSystem.current.IsPointerOverGameObject())
            ResetCellType();
    }

    private GridCell GetCellOnMouse()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        return gridExtension.GetCellInWorld(mousePosition);
    }

    private void UpdateCellType()
    {
        GridCell cell = GetCellOnMouse();

        if (!cell)
            return;

        Toggle activeToggle = cellTypeToggleGroup.ActiveToggles().First();
        ToggleTypeHolder toggleTypeHolder = activeToggle.GetComponent<ToggleTypeHolder>();
        PathCell pathCell = cell.GetComponent<PathCell>();

        pathfindController.SetCellType(pathCell, toggleTypeHolder.cellType);
    }

    private void ResetCellType()
    {
        GridCell cell = GetCellOnMouse();
        PathCell pathCell = cell.GetComponent<PathCell>();

        pathfindController.SetCellType(pathCell, PathCell.CellType.Empty);
    }
}