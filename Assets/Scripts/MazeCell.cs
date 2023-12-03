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

    [SerializeField]
    private GameObject LeftWallGraphics;

    [SerializeField]
    private GameObject RightWallGraphics;

    [SerializeField]
    private GameObject FrontWallGraphics;

    [SerializeField]
    private GameObject BackWallGraphics;

    [SerializeField]
    private GameObject UnvisitedBlockGraphics;

    [SerializeField]
    private Material LeftWallNightMaterial;

    [SerializeField]
    private Material RightWallNightMaterial;

    [SerializeField]
    private Material FrontWallNightMaterial;

    [SerializeField]
    private Material BackWallNightMaterial;

    [SerializeField]
    private Material LeftWallDayMaterial;

    [SerializeField]
    private Material RightWallDayMaterial;

    [SerializeField]
    private Material FrontWallDayMaterial;

    [SerializeField]
    private Material BackWallDayMaterial;

    [SerializeField]
    private Material FogMaterial;

    [SerializeField]
    private Material FogNightMaterial;

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

    public void ChangeToNight()
    {
        Renderer LeftWallMeshRenderer = LeftWallGraphics.GetComponent<Renderer>();
        Renderer RightWallMeshRenderer = RightWallGraphics.GetComponent<Renderer>();
        Renderer FrontWallMeshRenderer = FrontWallGraphics.GetComponent<Renderer>();
        Renderer BackWallMeshRenderer  = BackWallGraphics.GetComponent<Renderer>();
        Renderer UnvisitedBlockMeshRenderer = UnvisitedBlockGraphics.GetComponent<Renderer>();
        LeftWallMeshRenderer.material = LeftWallNightMaterial;
        RightWallMeshRenderer.material = RightWallNightMaterial;
        FrontWallMeshRenderer.material = FrontWallNightMaterial;
        BackWallMeshRenderer.material = BackWallNightMaterial;
        UnvisitedBlockMeshRenderer.material = LeftWallNightMaterial;
    }

    public void ChangeToDay()
    {
        Renderer LeftWallMeshRenderer = LeftWallGraphics.GetComponent<Renderer>();
        Renderer RightWallMeshRenderer = RightWallGraphics.GetComponent<Renderer>();
        Renderer FrontWallMeshRenderer = FrontWallGraphics.GetComponent<Renderer>();
        Renderer BackWallMeshRenderer = BackWallGraphics.GetComponent<Renderer>();
        Renderer UnvisitedBlockMeshRenderer = UnvisitedBlockGraphics.GetComponent<Renderer>();
        LeftWallMeshRenderer.material = LeftWallDayMaterial;
        RightWallMeshRenderer.material = RightWallDayMaterial;
        FrontWallMeshRenderer.material = FrontWallDayMaterial;
        BackWallMeshRenderer.material = BackWallDayMaterial;
        UnvisitedBlockMeshRenderer.material = LeftWallDayMaterial;
    }

    public void AddFogDay()
    {
        UnvisitedBlock.SetActive(true);
        Renderer UnvisitedBlockMeshRenderer = UnvisitedBlockGraphics.GetComponent<Renderer>();
        UnvisitedBlockMeshRenderer.material = FogMaterial;
    }

    public void AddFogNight()
    {
        UnvisitedBlock.SetActive(true);
        Renderer UnvisitedBlockMeshRenderer = UnvisitedBlockGraphics.GetComponent<Renderer>();
        UnvisitedBlockMeshRenderer.material = FogNightMaterial;
    }

    public void RemoveFog()
    {
        UnvisitedBlock.SetActive(false);
    }
}
