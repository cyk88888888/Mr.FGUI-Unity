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
    private GComponent gridContainer;

    private int _cellSize;
    private GridType _gridType = GridType.None;//当前格子类型
    private ObjectPool<GComponent> _lineCompPool;
    private ObjectPool<GComponent> _gridCompPool;
    private Dictionary<GridType, Dictionary<string, GComponent>> _gridTypeDic;
    protected override void OnFirstEnter()
    {
        lineContainer = view.GetChild("lineContainer").asCom;
        gridContainer = view.GetChild("gridContainer").asCom;

        _lineCompPool = new(
            () => { return UIPackage.CreateObject("MapEditor", "LineComp").asCom; },
            (GComponent obj) => { obj.RemoveFromParent(); }
        );

        _gridCompPool = new(
            () => { return UIPackage.CreateObject("MapEditor", "GridComp").asCom; },
            (GComponent obj) => { obj.RemoveFromParent(); }
        );

        view.scrollPane.onScroll.Add(OnScroll);

        view.onRightDown.Add(_onRightDown);
        view.onRightMove.Add(_onRightMove);
        view.onRightUp.Add(_onRightUp);
        view.onClick.Add(_onClick);
        //view.onRightClick.Add(_onRightClick);
        _cellSize = 40;
        InitGrid();
    }

    protected override void OnEnter()
    {
        OnEmitter(GameEvent.ChangeGridType, OnChangeGridType);//格子类型变化
        OnEmitter(GameEvent.ClearGridType, OnClearGridType);//格子类型变化 
        OnEmitter(GameEvent.ResizeGrid, OnResizeGrid);

    }

    private void InitGrid()
    {
        float mapWidth = 3000;
        float mapHeight = 3000;
        float numCols = Mathf.Floor(mapWidth / _cellSize);
        float numRows = Mathf.Floor(mapHeight / _cellSize);
        Debug.Log("列数：" + numCols);
        Debug.Log("行数：" + numRows);
        GGraph bg = view.GetChild("bg").asGraph;
        bg.SetSize(mapWidth, mapHeight);


        lineContainer.SetSize(mapWidth, mapHeight);
        RemoveAllLine();
        RemoveAllGrid();

        int lineStroke = 2;//线条粗度
        for (int i = 1; i < numCols; i++)//画竖线
        {
            GComponent line = _lineCompPool.GetObject();
            lineContainer.AddChild(line);
            line.width = lineStroke;
            line.height = mapHeight;
            line.SetXY(i * _cellSize, 0);
            line.visible = view.scrollPane.IsChildInView(line);
            line.gameObjectName = "Col_" + i;
        }

        for (int i = 1; i < numRows; i++)//画横线
        {
            GComponent line = _lineCompPool.GetObject();
            lineContainer.AddChild(line);
            line.width = mapWidth;
            line.height = lineStroke;
            line.SetXY(0, i * _cellSize);
            line.visible = view.scrollPane.IsChildInView(line);
            line.gameObjectName = "Row_" + i;
        }
    }

    private void OnScroll()
    {
        foreach (var line in lineContainer.GetChildren())
        {
            line.visible = view.scrollPane.IsChildInView(line);
        }
    }

    //移除所有线条
    private void RemoveAllLine()
    {
        foreach (var item in lineContainer.GetChildren())
        {
            _lineCompPool.ReleaseObject((GComponent)item);
        }
    }

    //移除所有格子
    private void RemoveAllGrid()
    {
        foreach (var item in gridContainer.GetChildren())
        {
            _gridCompPool.ReleaseObject((GComponent)item);
        }
        _gridTypeDic = new Dictionary<GridType, Dictionary<string, GComponent>>();
    }

    //重置格子大小
    private void OnResizeGrid(EventCallBack evt)
    {
        int cellSize = int.Parse((string)evt.Data[0]);
        _cellSize = cellSize;
        InitGrid();
    }

    //更改格子类型样式
    private void OnChangeGridType(EventCallBack evt)
    {
        GridType gridType = (GridType)evt.Data[0];
        _gridType = gridType;
    }

    private void _onRightDown(EventContext evt)
    {
        evt.CaptureTouch();//这个方法一定要调用，否则不糊出发rightMove
        if (_gridType == GridType.None)
        {
            MsgMgr.ShowMsg("请先点击设置格子类型！！！");
            return;
        }
    }
    private void _onRightMove(EventContext evt)
    {
        if (_gridType == GridType.None) return;
        InputEvent inputEvt = (InputEvent)evt.data;
        Vector2 inputPos = inputEvt.position;
        float gridX = Mathf.Floor((inputPos.x + view.scrollPane.posX) / _cellSize) * _cellSize;
        float gridY = Mathf.Floor((inputPos.y + view.scrollPane.posY) / _cellSize) * _cellSize;
        if (!_gridTypeDic.TryGetValue(_gridType, out Dictionary<string, GComponent> dic))
        {
            _gridTypeDic[_gridType] = new Dictionary<string, GComponent>();
        }
        Dictionary<string, GComponent> curGridTypeDic = _gridTypeDic[_gridType];
        string gridKey = gridX + "_" + gridY;
        if (curGridTypeDic.TryGetValue(gridKey, out GComponent gridComp))//当前格子已有
        {
            return;
        }
        AddOrRmGrid(evt);
    }
    private void _onRightUp(EventContext evt) { }
    
    private void _onClick(EventContext evt)
    {
        if (_gridType == GridType.None)
        {
            MsgMgr.ShowMsg("请先点击设置格子类型！！！");
            return;
        }
  
        AddOrRmGrid(evt);
    }

    private void AddOrRmGrid(EventContext evt)
    {
        if (_gridType == GridType.None) return;
        InputEvent inputEvt = (InputEvent)evt.data;
        Vector2 inputPos = inputEvt.position;
        float gridX = Mathf.Floor((inputPos.x + view.scrollPane.posX) / _cellSize) * _cellSize;
        float gridY = Mathf.Floor((inputPos.y + view.scrollPane.posY) / _cellSize) * _cellSize;

        if (!_gridTypeDic.TryGetValue(_gridType, out Dictionary<string, GComponent> dic))
        {
            _gridTypeDic[_gridType] = new Dictionary<string, GComponent>();
        }
        Dictionary<string, GComponent> curGridTypeDic = _gridTypeDic[_gridType];
        string gridKey = gridX + "_" + gridY;
        GComponent gridComp;
        if (curGridTypeDic.TryGetValue(gridKey, out gridComp))
        {
            _gridCompPool.ReleaseObject(gridComp);
            curGridTypeDic.Remove(gridKey);
            return;
        }

        gridComp = _gridCompPool.GetObject();
        GLoader loader = gridComp.GetChild("icon").asLoader;
        loader.url = MapMgr.inst.getGridUrlByType(_gridType);
        gridComp.SetSize(_cellSize, _cellSize);
        gridComp.SetXY(gridX + 1, gridY + 1);
        gridContainer.AddChild(gridComp);
        curGridTypeDic.Add(gridKey, gridComp);
    }

    private void OnClearGridType(EventCallBack evt)
    {
        GridType gridType = (GridType)evt.Data[0];
        if (!_gridTypeDic.TryGetValue(gridType, out Dictionary<string, GComponent> dic))
        {
            return;
        }
        Dictionary<string, GComponent> curGridTypeDic = _gridTypeDic[gridType];
        foreach (var item in curGridTypeDic)
        {
            _gridCompPool.ReleaseObject(item.Value);
        }
        curGridTypeDic.Clear();
    }
}


