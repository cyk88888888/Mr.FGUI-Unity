using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 地图界面
/// </summary>
public class MapComp : UIComp
{
    protected override string PkgName
    {
        get { return "Home"; }
    }

    protected override string CompName
    {
        get { return "MapComp"; }
    }
    private GComponent lineContainer;

    private int _cellSize;
    private readonly Stack<GComponent> _lineCompPool = new Stack<GComponent>();
    protected override void OnFirstEnter()
    {
        //GComponent lineContainer = view.GetChild("lineContainer").asCom;
        //GGraph aa = new GGraph();
        //aa.SetSize(50, 50);
        //aa.DrawRect(300, 300, 2, Color.black, Color.black);
        //aa.SetXY(600, 0);
        //lineContainer.SetSize(aa.x + aa.width, aa.y + aa.height);
        //lineContainer.AddChild(aa);
        view.scrollPane.onScroll.Add(OnScroll);
        _cellSize = 40;
        InitGrid();

    }

    private void InitGrid()
    {
        float mapWidth = 3000;
        float mapHeight = 3000;
        float numCols = Mathf.Floor(mapWidth / _cellSize);
        float numRows = Mathf.Floor(mapHeight / _cellSize);
        Debug.Log("列数：" + numCols);
        Debug.Log("行数：" + numRows);

        lineContainer = view.GetChild("lineContainer").asCom;
        lineContainer.SetSize(mapWidth, mapHeight);
        RemoveAllLine();


        for (int i = 0; i < numCols + 1; i++)//画竖线
        {
            GComponent line = GetLineFromPool();
            lineContainer.AddChild(line);
            line.width = mapHeight;
            line.rotation = 90;
            line.SetXY(i * _cellSize + 2, 0);
            line.visible = view.scrollPane.IsChildInView(line);
        }

        for (int i = 0; i < numRows + 1; i++)//画横线
        {
            GComponent line = GetLineFromPool();
            lineContainer.AddChild(line);
            line.width = mapWidth;
            line.SetXY(0, i * _cellSize);
            line.visible = view.scrollPane.IsChildInView(line);
        }
    }

    private void OnScroll()
    {
        //foreach (var line in lineContainer.GetChildren())
        //{
        //    line.visible = view.scrollPane.IsChildInView(line);
        //}
    }

    private void RemoveAllLine()
    {
        foreach (var item in lineContainer.GetChildren())
        {
            ReturnLineComp((GComponent)item);
        }
    }
    private GComponent GetLineFromPool()
    {
        GComponent line;
        if (_lineCompPool.Count > 0)
            line = _lineCompPool.Pop();
        else
            line = UIPackage.CreateObject("Common", "LineComp").asCom;
        return line;
    }

    private void ReturnLineComp(GComponent line)
    {
        line.rotation = 0;
        _lineCompPool.Push(line);
    }

}


