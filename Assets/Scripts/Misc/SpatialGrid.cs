using System.Collections.Generic;
using UnityEngine;

/**
 * spatial grid used for clumping enemies into "buckets", so that we can 
 * more efficiently run "collision" checks
 */
public class SpatialGrid : MonoBehaviour
{
    //how big each cell is
    private float cellSize;

    //dictionary holding all enemies and their position
    private Dictionary<Vector2Int, List<EnemyBaseClass>> grid;

    public void Clear()
    {
        grid.Clear();
    }

    public void Insert(EnemyBaseClass enemy)
    {
        Vector2Int cell = GetCell(enemy.transform.position);

        if (!grid.ContainsKey(cell))
            grid[cell] = new List<EnemyBaseClass>();

        grid[cell].Add(enemy);
    }

    /**
    * returns list of enemies that are in the same or neighboring cells
    * of the enemy calling this
    */
    public List<EnemyBaseClass> GetNeighbors(Vector2 position)
    {
        List<EnemyBaseClass> neighbors = new List<EnemyBaseClass>();
        Vector2Int cell = GetCell(position);

        //check surrounding cells
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int key = new Vector2Int(cell.x + x, cell.y + y);
                if (grid.ContainsKey(key))
                    neighbors.AddRange(grid[key]);
            }
        }

        return neighbors;
    }

    private Vector2Int GetCell(Vector2 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / cellSize),
            Mathf.FloorToInt(position.y / cellSize)
        );
    }

    /**
     * pseudo constructor for the grid
     */
    public void SetGrid(float cellSize)
    {
        this.cellSize = cellSize;
        grid = new Dictionary<Vector2Int, List<EnemyBaseClass>>();
    }
}
