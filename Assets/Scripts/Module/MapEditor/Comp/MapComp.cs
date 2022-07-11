using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.IO;
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
    private GComponent grp_container;
    private GComponent lineContainer;
    private GComponent gridContainer;
    private GLoader pet;
    private int _cellSize;
    private GridType _gridType = GridType.None;//��ǰ��������
    private ObjectPool<GComponent> _lineCompPool;
    private ObjectPool<GComponent> _gridCompPool;

    private float speed = 3;

    protected override void OnFirstEnter()
    {
        grp_container = view.GetChild("grp_container").asCom;
        lineContainer = grp_container.GetChild("lineContainer").asCom;
        gridContainer = grp_container.GetChild("gridContainer").asCom;
        pet = grp_container.GetChild("pet").asLoader;
        _lineCompPool = new(
            () => { return UIPackage.CreateObject("MapEditor", "LineComp").asCom; },
            (GComponent obj) => { obj.RemoveFromParent(); }
        );

        _gridCompPool = new(
            () => { return UIPackage.CreateObject("MapEditor", "GridComp").asCom; },
            (GComponent obj) => { obj.RemoveFromParent(); }
        );


        view.onRightDown.Add(_onRightDown);
        view.onRightMove.Add(_onRightMove);
        view.onRightUp.Add(_onRightUp);
        view.onClick.Add(_onClick);
        _cellSize = MapMgr.inst.cellSize;
        Init();

    }

    protected override void OnEnter()
    {
        OnEmitter(GameEvent.ChangeGridType, OnChangeGridType);//�������ͱ仯
        OnEmitter(GameEvent.ClearGridType, OnClearGridType);//���ָ���������� 
        OnEmitter(GameEvent.ImportMapJson, OnImportMapJson);//������������͸���
        OnEmitter(GameEvent.ResizeGrid, OnResizeGrid);
        OnEmitter(GameEvent.ChangeMap, onChangeMap);
        OnEmitter(GameEvent.ScreenShoot, onScreenShoot);//��ͼ�滭����
        OnEmitter(GameEvent.RunDemo, OnRunDemo);
        OnEmitter(GameEvent.CloseDemo, OnCloseDemo);
    }

    private void Init(bool needCreate = true)
    {
        if (!needCreate) return;
        int mapWidth = MapMgr.inst.mapWidth;
        int mapHeight = MapMgr.inst.mapHeight;
        float numCols = Mathf.Floor(mapWidth / _cellSize);
        float numRows = Mathf.Floor(mapHeight / _cellSize);
        Debug.Log("������" + numRows + "��������" + numCols);
        GGraph bg = grp_container.GetChild("bg").asGraph;
        bg.SetSize(mapWidth, mapHeight);
        grp_container.SetSize(mapWidth, mapHeight);
        //bg.url = MapMgr.inst.mapId;//���ñ���ͼtodo....
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
            line.gameObjectName = "Col_" + i;
        }

        for (int i = 1; i < numRows; i++)//������
        {
            GComponent line = _lineCompPool.GetObject();
            lineContainer.AddChild(line);
            line.width = mapWidth;
            line.height = lineStroke;
            line.SetXY(0, i * _cellSize);
            line.gameObjectName = "Row_" + i;
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
        if (cellSize == MapMgr.inst.cellSize)
        {
            MsgMgr.ShowMsg("���Ӵ�Сδ�䣡����");
            return;
        }
        _cellSize = cellSize;
        MapMgr.inst.cellSize = cellSize;
        Init();
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

        GetGrid(_gridType, gridPos, gridX, gridY);
    }

    private void GetGrid(GridType gridType, Vector2 gridPos, float gridX, float gridY)
    {
        if (!MapMgr.inst.gridTypeDic.TryGetValue(gridType, out Dictionary<string, GComponent> dic))
        {
            MapMgr.inst.gridTypeDic[gridType] = new Dictionary<string, GComponent>();
        }
        Dictionary<string, GComponent> curGridTypeDic = MapMgr.inst.gridTypeDic[gridType];
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
        loader.url = MapMgr.inst.getGridUrlByType(gridType);
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

    private void OnImportMapJson(EventCallBack evt)
    {
        MapJsonInfo mapInfo = (MapJsonInfo)evt.Data[0];
        bool needRemoveLine = mapInfo.mapWidth != MapMgr.inst.mapWidth || mapInfo.mapHeight != MapMgr.inst.mapHeight || _cellSize != MapMgr.inst.cellSize;
        _cellSize = mapInfo.cellSize;
        Init(needRemoveLine);
        if (!needRemoveLine) RemoveAllGrid();
        /** ���ÿ����߽ڵ�**/
        for (int i = 0; i < mapInfo.walkList.Count; i++)
        {
            List<int> lineList = mapInfo.walkList[i];
            for (int j = 0; j < lineList.Count; j++)
            {
                if (lineList[j] == 1)
                {
                    Vector2 gridPos = new Vector2(j, i);//���ڸ���λ��
                    float gridX = gridPos.x * _cellSize;//������ɫ���ӵ�����X
                    float gridY = gridPos.y * _cellSize;//������ɫ���ӵ�����Y
                    GetGrid(GridType.Walk, gridPos, gridX, gridY);
                }
            }
        }

        /** �����ϰ���ڵ�**/
        AddBlockByType(GridType.Block);
        AddBlockByType(GridType.BlockVerts);
        AddBlockByType(GridType.Water);
        void AddBlockByType(GridType gridType)
        {
            List<List<int>> blockList = new();
            if (gridType == GridType.Block) blockList = mapInfo.blockList;
            else if (gridType == GridType.BlockVerts) blockList = mapInfo.blockVertList;
            else if (gridType == GridType.Water) blockList = mapInfo.waterList;
            foreach (var item in blockList)
            {
                Vector2 gridPos = new Vector2(item[0], item[1]);//���ڸ���λ��
                float gridX = gridPos.x * _cellSize;//������ɫ���ӵ�����X
                float gridY = gridPos.y * _cellSize;//������ɫ���ӵ�����Y
                GetGrid(gridType, gridPos, gridX, gridY);
            }
        }
    }

    private void onChangeMap(EventCallBack evt)
    {
        MapSelectInfo mapInfo = (MapSelectInfo)evt.Data[0];
        MapMgr.inst.mapId = mapInfo.mapId;
        Init();
    }

    private void onScreenShoot(EventCallBack evt)
    {
        MapMgr.inst.ShowMapPreviewDlg(grp_container);
    }

    private void OnCloseDemo(EventCallBack evt)
    {
        pet.visible = false;
        Timers.inst.Remove(OnUpdate);
    }

    private void OnRunDemo(EventCallBack evt)
    {
        pet.visible = true;
        pet.SetXY(0, 0);
        view.scrollPane.SetPosX(0, false);
        view.scrollPane.SetPosY(0, false);
        Timers.inst.AddUpdate(OnUpdate);
    }

    private void OnUpdate(object param)
    {
        JoystickComp joystick = MapMgr.inst.joystick;

        if (!joystick.isMoving) return;

        //ҡ������
        Vector2 joysticPos = joystick.vector;
        //������һ��
        Vector2 dir = BaseUT.angle_to_vector(joystick.curDegree);
        //���ٶ�
        float dir_x = dir.x * speed;
        float dir_y = dir.y * speed;
        //��ɫ����
        pet.scaleX = joysticPos.x > 0 ? 1 : -1;
        //��ɫ������Ϸ���
        float toX = pet.x + dir_x;
        float toY = pet.y + dir_y;
        if (toX < 0) toX = 0;
        if (toX > MapMgr.inst.mapWidth - pet.width) toX = MapMgr.inst.mapWidth - pet.width;
        if (toY < 0) toY = 0;
        if (toY > MapMgr.inst.mapHeight - pet.height) toY = MapMgr.inst.mapHeight - pet.height;
        pet.SetXY(toX, toY);

        //�ƶ���ͷ
        ScrollPane scrollPane = view.scrollPane;
        //if (scrollPane.scrollingPosX < scrollPane.contentWidth - scrollPane.viewWidth) scrollPane.SetPosX(pet.x, false);
        //if (scrollPane.scrollingPosY < scrollPane.contentHeight - scrollPane.viewHeight) scrollPane.SetPosY(pet.y, false);
        scrollPane.SetPosX(pet.x - scrollPane.viewWidth / 2, false);
        scrollPane.SetPosY(pet.y - scrollPane.viewHeight / 2, false);

    }
}


