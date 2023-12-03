using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject LeftWall;

    [SerializeField]
    private GameObject RightWall;

    [SerializeField]
    private GameObject FrontWall;

    [SerializeField]
    private GameObject BackWall;

    [SerializeField]
    private GameObject UnvisitedBlock;

    public bool IsVisited { get; private set; }

    public void Visit()
    {
        IsVisited = true;
        UnvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        LeftWall.SetActive(false);
    }

    public void ClearRightWall() 
    {
        RightWall.SetActive(false);
    }

    public void ClearFrontWall()
    {
        FrontWall.SetActive(false);
    }

    public void ClearBackWall()
    {
        BackWall.SetActive(false);
    }

    public bool CheckLeftWall()
    {
        return LeftWall.activeSelf;
    }
    public bool CheckRightWall()
    {
        return RightWall.activeSelf;
    }
    public bool CheckFrontWall()
    {
        return FrontWall.activeSelf;
    }
    public bool CheckBackWall()
    {
        return BackWall.activeSelf;
    }
}
