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
    private GameObject floor;

    [SerializeField]
    private Material FloorNightMaterial;

    [SerializeField]
    private Material FloorDayMaterial;

    private MazeCell[,] MazeGrid;

    private int RandomCorner;

    private bool night = false;

    private bool fog = false;

    private bool audio = false;

    public AudioClip dayAmbience;

    public AudioClip nightAmbience;

    public AudioSource audioSource;

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
                 MazeGrid[i, j].tag = "Wall";
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

        enemy.transform.position = MazeGrid[MazeWidth / 2, MazeDepth / 2].transform.position;

        enemy.SetActive(true);

        loadText.SetActive(false);

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

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Renderer renderer = floor.GetComponent<Renderer>();
            if (night && !fog && audioSource != null)
            {
                renderer.material = FloorDayMaterial;
                for (int i = 0; i < MazeWidth; i++)
                {
                    for (int j = 0; j < MazeDepth; j++)
                    {
                        MazeGrid[i,j].ChangeToDay();
                    }
                }
                audioSource.Stop();
                audioSource.PlayOneShot(dayAmbience);
                night = false;
            }
            else if (!night && !fog && audioSource != null)
            {
                renderer.material = FloorNightMaterial;
                for (int i = 0; i < MazeWidth; i++)
                {
                    for (int j = 0; j < MazeDepth; j++)
                    {
                        MazeGrid[i, j].ChangeToNight();
                    }
                }
                audioSource.Stop();
                audioSource.PlayOneShot(nightAmbience);
                night = true;
            }
            else if (night && fog && audioSource != null)
            {
                renderer.material = FloorDayMaterial;
                for (int i = 0; i < MazeWidth; i++)
                {
                    for (int j = 0; j < MazeDepth; j++)
                    {
                        MazeGrid[i, j].ChangeToDay();
                        MazeGrid[i, j].AddFogDay();
                    }
                }
                audioSource.Stop();
                audioSource.PlayOneShot(dayAmbience);
                night = false;
            }
            else if (!night && fog && audioSource != null)
            {
                renderer.material = FloorDayMaterial;
                for (int i = 0; i < MazeWidth; i++)
                {
                    for (int j = 0; j < MazeDepth; j++)
                    {
                        MazeGrid[i, j].ChangeToNight();
                        MazeGrid[i, j].AddFogNight();
                    }
                }
                audioSource.Stop();
                audioSource.PlayOneShot(nightAmbience);
                night = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (fog && audioSource != null)
            {
                for (int i = 0; i < MazeWidth; i++)
                {
                    for (int j = 0; j < MazeDepth; j++)
                    {
                        MazeGrid[i, j].RemoveFog();
                    }
                }
                audioSource.volume = audioSource.volume * 2f;
                fog = false;
            }
            else if (night && !fog && audioSource != null)
            {
                for (int i = 0; i < MazeWidth; i++)
                {
                    for (int j = 0; j < MazeDepth; j++)
                    {
                        MazeGrid[i, j].AddFogNight();
                    }
                }
                audioSource.volume = audioSource.volume * 0.5f;
                fog = true;
            }
            else if (!night && !fog && audioSource != null)
            {
                for (int i = 0; i < MazeWidth; i++)
                {
                    for (int j = 0; j < MazeDepth; j++)
                    {
                        MazeGrid[i, j].AddFogDay();
                    }
                }
                audioSource.volume = audioSource.volume * 0.5f;
                fog = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (audio)
            {
                audioSource.Stop();
                audio = false;
            }
            else
            {
                if (night && !fog && audioSource != null)
                {
                    audioSource.PlayOneShot(nightAmbience);
                }
                else if (!night && !fog && audioSource != null)
                {
                    audioSource.PlayOneShot(dayAmbience);
                }
                else if (night && fog && audioSource != null)
                {
                    audioSource.PlayOneShot(nightAmbience);
                    audioSource.volume = audioSource.volume * 0.5f;
                }
                else if (!night && fog && audioSource != null)
                {
                    audioSource.PlayOneShot(dayAmbience);
                    audioSource.volume = audioSource.volume * 0.5f;
                }
                audio = true;
            }
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

        if (audioSource != null)
        {
            audioSource.volume = 1 - ((player.transform.position.x + player.transform.position.z) - (enemy.transform.position.x + enemy.transform.position.z)) / 10f;
        }
    }
}
