using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private StartCell start;

    [SerializeField]
    private EndCell end;

    [SerializeField]
    private MazeCell cell;

    [SerializeField]
    private int MazeWidth;

    [SerializeField]
    private int MazeDepth;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject loadText;

    [SerializeField]
    private GameObject winText;

    [SerializeField]
    private GameObject saveButton;

    [SerializeField]
    private GameObject playerPosition;

    [SerializeField]
    private GameObject enemyPosition;

    private MazeCell[,] MazeGrid;

    private int RandomCorner;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        enemy.SetActive(false);
        MazeGrid = new MazeCell[MazeWidth, MazeDepth];

        for (int i = 0; i < MazeWidth; i++)
        {
            for (int j = 0; j < MazeDepth; j++)
            {
                MazeGrid[i, j] = Instantiate(cell, new Vector3(i, 0, j), Quaternion.identity);
            }
        }

        RandomCorner = Random.Range(0, 3);
        if (RandomCorner == 0)
        {
            Destroy(MazeGrid[0, 0].gameObject);
            Destroy(MazeGrid[MazeWidth - 1, MazeDepth - 1].gameObject);
            MazeGrid[0, 0] = Instantiate(start, new Vector3(0, 0, 0), Quaternion.identity);
            MazeGrid[MazeWidth - 1, MazeDepth - 1] = Instantiate(end, new Vector3(MazeWidth - 1, 0, MazeDepth  - 1), Quaternion.identity);
            end.transform.position = new Vector3(MazeWidth - 1, 0, MazeDepth - 1);
        }
        else if (RandomCorner == 1)
        {
            Destroy(MazeGrid[0, MazeDepth - 1].gameObject);
            Destroy(MazeGrid[MazeWidth - 1, 0].gameObject);
            MazeGrid[0, MazeDepth - 1] = Instantiate(start, new Vector3(0, 0, MazeDepth - 1), Quaternion.identity);
            MazeGrid[MazeWidth - 1, 0] = Instantiate(end, new Vector3(MazeWidth - 1, 0, 0), Quaternion.identity);
            start.transform.position = new Vector3(0, 0, MazeDepth - 1);
            end.transform.position = new Vector3(MazeWidth - 1, 0, 0);
        }
        else if (RandomCorner == 2)
        {
            Destroy(MazeGrid[0, MazeDepth - 1].gameObject);
            Destroy(MazeGrid[MazeWidth - 1, 0].gameObject);
            MazeGrid[MazeWidth - 1, 0] = Instantiate(start, new Vector3(MazeWidth - 1, 0, 0), Quaternion.identity);
            MazeGrid[0, MazeDepth - 1] = Instantiate(end, new Vector3(0, 0, MazeDepth - 1), Quaternion.identity);
            start.transform.position = new Vector3(MazeWidth - 1, 0, 0);
            end.transform.position = new Vector3(0, 0, MazeDepth - 1);
        }
        else if (RandomCorner == 3)
        {
            Destroy(MazeGrid[0, 0].gameObject);
            Destroy(MazeGrid[MazeWidth - 1, MazeDepth - 1].gameObject);
            MazeGrid[MazeWidth - 1, MazeDepth - 1] = Instantiate(start, new Vector3(MazeWidth - 1, 0, MazeDepth - 1), Quaternion.identity);
            MazeGrid[0, 0] = Instantiate(end, new Vector3(0, 0, 0), Quaternion.identity);
            start.transform.position = new Vector3(MazeWidth - 1, 0, MazeDepth - 1);
        }

        yield return GenerateMaze(null, start);

        for (int k = 0; k < MazeWidth; k++)
        {
            for (int l = 0; l < MazeDepth; l++)
            {
                RemoveOverlappingWalls(MazeGrid[k, l]);
            }
        }

        player.transform.position = start.transform.position;

        GameController.gCtrl.SetPlayerPosition(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        enemy.transform.position = MazeGrid[MazeWidth / 2, MazeDepth / 2].transform.position;

        GameController.gCtrl.SetEnemyPosition(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);

        enemy.SetActive(true);

        loadText.SetActive(false);

        saveButton.SetActive(true);

        playerPosition.SetActive(true);

        enemyPosition.SetActive(true);
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < MazeWidth)
        {
            var cellToRight = MazeGrid[x + 1, z];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = MazeGrid[x - 1, z];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < MazeDepth)
        {
            var cellToFront = MazeGrid[x, z + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = MazeGrid[x, z - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        else
        {
            if (previousCell.transform.position.x < currentCell.transform.position.x)
            {
                previousCell.ClearRightWall();
                currentCell.ClearLeftWall();
                return;
            }

            if (previousCell.transform.position.x > currentCell.transform.position.x)
            {
                previousCell.ClearLeftWall();
                currentCell.ClearRightWall();
                return;
            }

            if (previousCell.transform.position.z < currentCell.transform.position.z)
            {
                previousCell.ClearFrontWall();
                currentCell.ClearBackWall();
                return;
            }

            if (previousCell.transform.position.z > currentCell.transform.position.z)
            {
                previousCell.ClearBackWall();
                currentCell.ClearFrontWall();
                return;
            }
        }
    }

    private void RemoveOverlappingWalls(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;
        if (x + 1 < MazeWidth)
        {
            var cellToRight = MazeGrid[x + 1, z];
            if (cellToRight.CheckLeftWall() && currentCell.CheckRightWall())
            {
                cellToRight.ClearLeftWall();
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = MazeGrid[x - 1, z];
            if (cellToLeft.CheckRightWall() && currentCell.CheckLeftWall())
            {
                cellToLeft.ClearRightWall();
            }
        }

        if (z + 1 < MazeDepth)
        {
            var cellToFront = MazeGrid[x, z + 1];
            if (cellToFront.CheckBackWall() && currentCell.CheckFrontWall())
            {
                cellToFront.ClearBackWall();
            }
        }

        if (z - 1 >= 0)
        {
            var cellToBack = MazeGrid[x, z - 1];
            if (cellToBack.CheckFrontWall() && currentCell.CheckBackWall())
            {
                cellToBack.ClearFrontWall();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            player.transform.position = start.transform.position;
            enemy.transform.position = MazeGrid[MazeWidth / 2, MazeDepth / 2].transform.position;
            winText.SetActive(false);
        }

        if (RandomCorner == 0)
        {
            if (player.transform.position.x > MazeWidth - 1.6f && player.transform.position.z > MazeDepth - 1.6f)
            {
                winText.SetActive(true);
            }
        }
        else if (RandomCorner == 1)
        {
            if (player.transform.position.x > MazeWidth - 1.6f && player.transform.position.z < 0.6f)
            {
                winText.SetActive(true);
            }
        }
        else if (RandomCorner == 2)
        {
            if (player.transform.position.x < 0.6f && player.transform.position.z > MazeDepth - 1.6f)
            {
                winText.SetActive(true);
            }
        }
        else if (RandomCorner == 3)
        {
            if (player.transform.position.x < 0.6f && player.transform.position.z < 0.6f)
            {
                winText.SetActive(true);
            }
        }
    }
}
