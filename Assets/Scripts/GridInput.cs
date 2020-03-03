using UnityEngine;

public class GridInput : MonoBehaviour
{
    private Grid grid;
    private GridExtension gridExtension;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        gridExtension = GetComponent<GridExtension>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellCoord = grid.WorldToCell(pos);

            GridCell cell = gridExtension.GetCell(cellCoord);

            if (cell)
                cell.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }
}