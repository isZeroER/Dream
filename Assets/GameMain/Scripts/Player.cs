using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public bool hasInput = false;

    private void Start()
    {
        health = 3;
        strength = 2;
    }

    public override void HandleMethod()
    {
        base.HandleMethod();
        CheckInput();
    }
    

    public void CheckInput()
    {
        if (hasInput) return;
        //显示所有可走格子
        GridManager.Instance.ShowAround(transform.position);
        
        if (GridManager.Instance.CanWalkTo(transform.position + new Vector3(0, 1, 0)) && Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("WW");
            transform.Translate(0, 1, 0);
            hasInput = true;
        }

        if (GridManager.Instance.CanWalkTo(transform.position + new Vector3(0, -1, 0)) && Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(0, -1, 0);
            hasInput = true;
        }

        if (GridManager.Instance.CanWalkTo(transform.position + new Vector3(-1, 0, 0)) && Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(-1, 0, 0);
            hasInput = true;
        }

        if (GridManager.Instance.CanWalkTo(transform.position + new Vector3(1, 0, 0)) && Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(1, 0, 0);
            hasInput = true;
        }

        if (hasInput)
        {
            GridManager.Instance.ChangeGridInfo(currentGrid, false);
            currentGrid = GridManager.Instance.GetGridByPos(transform.position);
            GridManager.Instance.ChangeGridInfo(currentGrid, true);
        }
    }
}
