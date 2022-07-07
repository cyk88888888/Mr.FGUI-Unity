using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ��ͼ����
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
    private GridType _gridType = GridType.None;//��ǰ��������
    private ObjectPool<GComponent> _lineCompPool;
    private ObjectPool<GComponent> _gridCompPool;

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
        _cellSize = MapMgr.inst.cellSize;
        InitGrid();
    }

    protected override void OnEnter()
    {
        OnEmitter(GameEvent.ChangeGridType, OnChangeGridType);//�������ͱ仯
        OnEmitter(GameEvent.ClearGridType, OnClearGridType);//�������ͱ仯 
        OnEmitter(GameEvent.ResizeGrid, OnResizeGrid);

    }

    private void InitGrid()
    {
        int mapWidth = MapMgr.inst.mapWidth;
        int mapHeight = MapMgr.inst.mapHeight;
        float numCols = Mathf.Floor(mapWidth / _cellSize);
        float numRows = Mathf.Floor(mapHeight / _cellSize);
        Debug.Log("������" + numCols);
        Debug.Log("������" + numRows);
        GGraph bg = view.GetChild("bg").asGraph;
        bg.SetSize(mapWidth, mapHeight);


        lineContainer.SetSize(mapWidth, mapHeight);
        RemoveAllLine();
        RemoveAllGrid();

        int lineStroke = 2;//�����ֶ�
        for (int i = 1; i < numCols; i++)//������
        {
            GComponent line = _lineCompPool.GetObject();
            lineContainer.AddChild(line);
            line.width = lineStroke;
            line.height = mapHeight;
            line.SetXY(i * _cellSize, 0);
            line.visible = view.scrollPane.IsChildInView(line);
            line.gameObjectName = "Col_" + i;
        }

        for (int i = 1; i < numRows; i++)//������
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

    //�Ƴ���������
    private void RemoveAllLine()
    {
        foreach (var item in lineContainer.GetChildren())
        {
            _lineCompPool.ReleaseObject((GComponent)item);
        }
    }

    //�Ƴ����и���
    private void RemoveAllGrid()
    {
        foreach (var item in gridContainer.GetChildren())
        {
            _gridCompPool.ReleaseObject((GComponent)item);
        }
        MapMgr.inst.gridTypeDic = new Dictionary<GridType, Dictionary<string, GComponent>>();
    }

    //���ø��Ӵ�С
    private void OnResizeGrid(EventCallBack evt)
    {
        int cellSize = int.Parse((string)evt.Data[0]);
        _cellSize = cellSize;
        MapMgr.inst.cellSize = cellSize;
        InitGrid();
    }

    //���ĸ���������ʽ
    private void OnChangeGridType(EventCallBack evt)
    {
        GridType gridType = (GridType)evt.Data[0];
        _gridType = gridType;
    }

    private void _onRightDown(EventContext evt)
    {
        evt.CaptureTouch();//�������һ��Ҫ���ã����򲻺�����rightMove
        if (_gridType == GridType.None)
        {
            MsgMgr.ShowMsg("���ȵ�����ø������ͣ�����");
            return;
        }
    }
    private void _onRightMove(EventContext evt)
    {
        if (_gridType == GridType.None) return;
        InputEvent inputEvt = (InputEvent)evt.data;
        Vector2 inputPos = inputEvt.position;
        Vector2 gridPos = new Vector2(Mathf.Floor((inputPos.x + view.scrollPane.posX) / _cellSize), Mathf.Floor((inputPos.y + view.scrollPane.posY) / _cellSize));//���ڸ���λ��
        float gridX = gridPos.x * _cellSize;
        float gridY = gridPos.y * _cellSize;
        if (!MapMgr.inst.gridTypeDic.TryGetValue(_gridType, out Dictionary<string, GComponent> dic))
        {
            MapMgr.inst.gridTypeDic[_gridType] = new Dictionary<string, GComponent>();
        }
        Dictionary<string, GComponent> curGridTypeDic = MapMgr.inst.gridTypeDic[_gridType];
        string gridKey = gridPos.x + "_" + gridPos.y;
        if (curGridTypeDic.TryGetValue(gridKey, out GComponent gridComp)) return;//��ǰ��������

        AddOrRmGrid(evt);
    }
    private void _onRightUp(EventContext evt) { }

    private void _onClick(EventContext evt)
    {
        if (_gridType == GridType.None)
        {
            MsgMgr.ShowMsg("���ȵ�����ø������ͣ�����");
            return;
        }

        AddOrRmGrid(evt);
    }

    private void AddOrRmGrid(EventContext evt)
    {
        if (_gridType == GridType.None) return;
        InputEvent inputEvt = (InputEvent)evt.data;
        Vector2 inputPos = inputEvt.position;
        Vector2 gridPos = new Vector2(Mathf.Floor((inputPos.x + view.scrollPane.posX) / _cellSize), Mathf.Floor((inputPos.y + view.scrollPane.posY) / _cellSize));//���ڸ���λ��
        float gridX = gridPos.x * _cellSize;//������ɫ���ӵ�����X
        float gridY = gridPos.y * _cellSize;//������ɫ���ӵ�����Y

        if (!MapMgr.inst.gridTypeDic.TryGetValue(_gridType, out Dictionary<string, GComponent> dic))
        {
            MapMgr.inst.gridTypeDic[_gridType] = new Dictionary<string, GComponent>();
        }
        Dictionary<string, GComponent> curGridTypeDic = MapMgr.inst.gridTypeDic[_gridType];
        string gridKey = gridPos.x + "_" + gridPos.y;
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
        if (!MapMgr.inst.gridTypeDic.TryGetValue(gridType, out Dictionary<string, GComponent> dic))
        {
            return;
        }
        Dictionary<string, GComponent> curGridTypeDic = MapMgr.inst.gridTypeDic[gridType];
        foreach (var item in curGridTypeDic)
        {
            _gridCompPool.ReleaseObject(item.Value);
        }
        curGridTypeDic.Clear();
    }
}


