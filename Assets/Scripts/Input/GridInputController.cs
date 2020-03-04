using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridInputController : MonoBehaviour 
{
    [SerializeField] private ToggleGroup cellTypeToggleGroup;
    private GridExtension gridExtension;
    private Camera mainCamera;

    private const int MouseButtonLeft = 0;
    private const int MouseButtonRight = 1;

    private void Awake()
    {
        gridExtension = GetComponent<GridExtension>();
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
        Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cellCoordinates = gridExtension.grid.WorldToCell(pos);

        return gridExtension.GetCell(cellCoordinates);
    }

    private void UpdateCellType()
    {
        GridCell cell = GetCellOnMouse();

        if (!cell)
            return;

        Toggle activeToggle = cellTypeToggleGroup.ActiveToggles().First();
        ToggleTypeHolder toggleTypeHolder = activeToggle.GetComponent<ToggleTypeHolder>();

        cell.SetCellType(toggleTypeHolder.cellType);
    }

    private void ResetCellType()
    {
        GridCell cell = GetCellOnMouse();
        cell.SetCellType(GridCell.CellType.Empty);
    }
}