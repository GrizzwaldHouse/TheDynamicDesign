using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{

    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _backWall;
    [SerializeField] private GameObject _unvisitedBlock;
    public bool isVisited;
    public void IsVisit()
    {
        isVisited = true;
        _unvisitedBlock.SetActive(false);
    }
    public void clearLeftWall()
    {
        _leftWall.SetActive(false);
    }
    public void clearFrontWall()
    {
        _frontWall.SetActive(false);
    }
    public void clearRightWall()
    {
        _rightWall.SetActive(false);
    }
    public void clearBackWall()
    {
        _backWall.SetActive(false);
    }
}
