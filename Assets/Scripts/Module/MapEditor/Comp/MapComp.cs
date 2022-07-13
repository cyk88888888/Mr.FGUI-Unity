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
    private GComponent grp_setSize;
    private GComponent grp_container;
    private GComponent lineContainer;
    private GComponent gridContainer;
    private GGraph graph_remind;
    private GLoader pet;
    private GGraph bg;
    private GGraph center;
    private int _cellSize;
    private GridType _gridType = GridType.None;//当前格子类型
    private ObjectPool<GComponent> _lineCompPool;
    private ObjectPool<GComponent> _gridCompPool;
    private int lineStroke = 2;//线条粗度
    private float speed = 3;//人物移动速度

    protected override void OnFirstEnter()
    {
        grp_setSize = view.GetChild("grp_setSize").asCom;
        grp_container = view.GetChild("grp_container").asCom;
        graph_remind = grp_container.GetChild("graph_remind").asGraph;
        lineContainer = grp_container.GetChild("lineContainer").asCom;
        gridContainer = grp_container.GetChild("gridContainer").asCom;
        bg = grp_container.GetChild("bg").asGraph;
        pet = grp_container.GetChild("pet").asLoader;
        center = grp_container.GetChild("center").asGraph;
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
        //view.onTouchMove.Add(_onTouchMove);
        view.onClick.Add(_onClick);
        view.displayObject.onMouseWheel.Add(_onMouseWheel);
        _cellSize = MapMgr.inst.cellSize;
        Init();
    }

    protected override void OnEnter()
    {
        OnEmitter(GameEvent.ChangeGridType, OnChangeGridType);//格子类型变化
        OnEmitter(GameEvent.ClearGridType, OnClearGridType);//清除指定格子类型 
        OnEmitter(GameEvent.ImportMapJson, OnImportMapJson);//清除所有线条和格子
        OnEmitter(GameEvent.ResizeGrid, OnResizeGrid);
        OnEmitter(GameEvent.ResizeMap, OnResizeMap);
        OnEmitter(GameEvent.ChangeMap, onChangeMap);
        OnEmitter(GameEvent.ScreenShoot, onScreenShoot);//截图绘画区域
        OnEmitter(GameEvent.RunDemo, OnRunDemo);
        OnEmitter(GameEvent.CloseDemo, OnCloseDemo);
        OnEmitter(GameEvent.ToCenter, OnToCenter);
        OnEmitter(GameEvent.ToOriginalScale, OnToOriginalScale);
        OnEmitter(GameEvent.ClearAllData, OnClearAllData);
    }

    private void Init()
    {
        int mapWidth = MapMgr.inst.mapWidth;
        int mapHeight = MapMgr.inst.mapHeight;
        float numCols = Mathf.Floor(mapWidth / _cellSize);
        float numRows = Mathf.Floor(mapHeight / _cellSize);
        Debug.Log("行数：" + numRows + "，列数：" + numCols);
        OnToOriginalScale(null);
        center.SetXY((MapMgr.inst.mapWidth - center.width) / 2, (MapMgr.inst.mapHeight - center.height) / 2);
        bg.SetSize(mapWidth, mapHeight);
        //bg.url = MapMgr.inst.mapId;//设置背景图todo....
        RemoveAllLine();
        RemoveAllGrid();

        for (int i = 1; i < numCols; i++)//画竖线
        {
            GComponent line = _lineCompPool.GetObject();
            lineContainer.AddChild(line);
            line.width = lineStroke;
            line.height = mapHeight;
            line.SetXY(i * _cellSize, 0);
            line.gameObjectName = "Col_" + i;
        }

        for (int i = 1; i < numRows; i++)//画横线
        {
            GComponent line = _lineCompPool.GetObject();
            lineContainer.AddChild(line);
            line.width = mapWidth;
            line.height = lineStroke;
            line.SetXY(0, i * _cellSize);
            line.gameObjectName = "Row_" + i;
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
        MapMgr.inst.gridTypeDic = new Dictionary<GridType, Dictionary<string, GComponent>>();
    }

    //重置格子大小
    private void OnResizeGrid(EventCallBack evt)
    {
        int cellSize = int.Parse((string)evt.Data[0]);
        if (cellSize == MapMgr.inst.cellSize)
        {
            MsgMgr.ShowMsg("格子大小未变！！！");
            return;
        }
        _cellSize = cellSize;
        MapMgr.inst.cellSize = cellSize;
        Init();
    }

    //重置地图大小
    private void OnResizeMap(EventCallBack evt)
    {
        int mapWidth = int.Parse((string)evt.Data[0]);
        int mapHeight = int.Parse((string)evt.Data[1]);
        if (mapWidth == MapMgr.inst.mapWidth && mapHeight == MapMgr.inst.mapHeight)
        {
            MsgMgr.ShowMsg("地图大小未变！！！");
            return;
        }
        bool isReduce = false;//是否减小地图
        if (mapWidth < MapMgr.inst.mapWidth || mapHeight < MapMgr.inst.mapHeight)//地图变小时，需要检测减小部分是否有已画格子数据，有的话不让改
        {
            isReduce = true;
            bool isCanResizeMap = true;
            foreach (var item in MapMgr.inst.gridTypeDic)
            {
                bool isExistGrid = false;
                foreach (var subItem in item.Value)
                {
                    string[] splitKey = subItem.Key.Split("_");
                    if (subItem.Value.x >= mapWidth || subItem.Value.y >= mapHeight)
                    {
                        isExistGrid = true;
                        break;
                    }
                }
                if (isExistGrid)
                {
                    isCanResizeMap = false;
                    break;
                }
            }

            if (!isCanResizeMap)
            {
                MsgMgr.ShowMsg("地图减少部分包含已画格子数据，请检查！！！");
                GTween.Kill(graph_remind);
                graph_remind.alpha = 1;
                graph_remind.visible = true;
                graph_remind.SetSize(mapWidth, mapHeight);
                graph_remind.TweenFade(0.4f, 0.3f).SetRepeat(3).OnComplete(() =>
                {
                    graph_remind.visible = false;
                    graph_remind.SetSize(0, 0);
                });
                return;
            }
        }

        int oldMapWidth = MapMgr.inst.mapWidth;
        int oldMapHeight = MapMgr.inst.mapHeight;
        int oldNumCols = (int)Mathf.Floor(oldMapWidth / _cellSize);
        int oldNumRows = (int)Mathf.Floor(oldMapHeight / _cellSize);
        MapMgr.inst.mapWidth = mapWidth;
        MapMgr.inst.mapHeight = mapHeight;
        int newNumCols = (int)Mathf.Floor(mapWidth / _cellSize);
        int newNumRows = (int)Mathf.Floor(mapHeight / _cellSize);
        Debug.Log("行数：" + newNumCols + "，列数：" + newNumRows);
        grp_setSize.SetSize(curScale * MapMgr.inst.mapWidth, curScale * MapMgr.inst.mapHeight);
        center.SetXY((MapMgr.inst.mapWidth - center.width) / 2, (MapMgr.inst.mapHeight - center.height) / 2);
        bg.SetSize(mapWidth, mapHeight);

        if (isReduce)//减小地图
        {
            GObject[] _children = lineContainer.GetChildren();
            for (int i = _children.Length - 1; i >= 0; i--)
            {
                GComponent lineItem = (GComponent)_children[i];
                if (lineItem.x >= mapWidth || lineItem.y >= mapHeight)
                {
                    _lineCompPool.ReleaseObject(lineItem);
                }
                else
                {
                    if (lineItem.gameObjectName.Contains("Col_"))
                    {
                        lineItem.width = lineStroke;
                        lineItem.height = mapHeight;
                    }
                    else
                    {
                        lineItem.width = mapWidth;
                        lineItem.height = lineStroke;
                    }
                }
            }
        }
        else
        {
            foreach (var lineItem in lineContainer.GetChildren())
            {
                if (lineItem.gameObjectName.Contains("Col_"))
                {
                    lineItem.width = lineStroke;
                    lineItem.height = mapHeight;
                }
                else
                {
                    lineItem.width = mapWidth;
                    lineItem.height = lineStroke;
                }
            }

            for (int i = oldNumCols; i < newNumCols; i++)//画竖线
            {
                GComponent line = _lineCompPool.GetObject();
                lineContainer.AddChild(line);
                line.width = lineStroke;
                line.height = mapHeight;
                line.SetXY(i * _cellSize, 0);
                line.gameObjectName = "Col_" + i;
            }

            for (int i = oldNumRows; i < newNumRows; i++)//画横线
            {
                GComponent line = _lineCompPool.GetObject();
                lineContainer.AddChild(line);
                line.width = mapWidth;
                line.height = lineStroke;
                line.SetXY(0, i * _cellSize);
                line.gameObjectName = "Row_" + i;
            }
        }
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

    private string _oldGridKey;
    private void _onRightMove(EventContext evt)
    {
        if (_gridType == GridType.None) return;
        InputEvent inputEvt = (InputEvent)evt.data;
        Vector2 inputPos = inputEvt.position;
        Vector2 gridPos = new(Mathf.Floor((inputPos.x + view.scrollPane.posX) / (_cellSize * curScale)), Mathf.Floor((inputPos.y + view.scrollPane.posY) / (_cellSize * curScale)));//所在格子位置
        //if (!MapMgr.inst.gridTypeDic.TryGetValue(_gridType, out Dictionary<string, GComponent> dic))
        //{
        //    MapMgr.inst.gridTypeDic[_gridType] = new Dictionary<string, GComponent>();
        //}
        //Dictionary<string, GComponent> curGridTypeDic = MapMgr.inst.gridTypeDic[_gridType];
        string gridKey = gridPos.x + "_" + gridPos.y;
        if (_oldGridKey == gridKey) return;//当前格子已有
        _oldGridKey = gridKey;
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
        Vector2 gridPos = new Vector2(Mathf.Floor((inputPos.x + view.scrollPane.posX) / (_cellSize * curScale)), Mathf.Floor((inputPos.y + view.scrollPane.posY) / (_cellSize * curScale)));//所在格子位置
        float gridX = gridPos.x * _cellSize;//绘制颜色格子的坐标X
        float gridY = gridPos.y * _cellSize;//绘制颜色格子的坐标Y
        if (gridX >= MapMgr.inst.mapWidth || gridY >= MapMgr.inst.mapHeight) return;
        GetGrid(_gridType, gridPos, gridX, gridY);
    }

    /** 设置地图格子数据 && 添加格子到容器**/
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
        gridComp.gameObjectName = gridKey + "_" + MapMgr.inst.getColorNameByType(gridType) + "_" + (curGridTypeDic.Count + 1);
        gridComp.SetSize(_cellSize, _cellSize);
        gridComp.SetXY(gridX + 1, gridY + 1);
        gridComp.fairyBatching = true;
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
        JuHuaDlg juahua = (JuHuaDlg)UILayer.Show("JuHuaDlg");
        MapJsonInfo mapInfo = (MapJsonInfo)evt.Data[0];
        _cellSize = mapInfo.cellSize;
        Init();
        /** 设置可行走节点**/
        for (int i = 0; i < mapInfo.walkList.Count; i++)
        {
            List<int> lineList = mapInfo.walkList[i];
            for (int j = 0; j < lineList.Count; j++)
            {
                if (lineList[j] == 1)
                {
                    Vector2 gridPos = new(j, i);//所在格子位置
                    float gridX = gridPos.x * _cellSize;//绘制颜色格子的坐标X
                    float gridY = gridPos.y * _cellSize;//绘制颜色格子的坐标Y
                    GetGrid(GridType.Walk, gridPos, gridX, gridY);
                }
            }
        }

        /** 设置障碍物节点**/
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
                Vector2 gridPos = new(item[0], item[1]);//所在格子位置
                float gridX = gridPos.x * _cellSize;//绘制颜色格子的坐标X
                float gridY = gridPos.y * _cellSize;//绘制颜色格子的坐标Y
                GetGrid(gridType, gridPos, gridX, gridY);
            }
        }
        juahua.Close();
    }

    private void onChangeMap(EventCallBack evt)
    {
        MapSelectInfo mapInfo = (MapSelectInfo)evt.Data[0];
        MapMgr.inst.mapId = mapInfo.mapId;
        Init();
    }

    private void OnToCenter(EventCallBack evt)
    {
        //移动镜头
        ScrollPane scrollPane = view.scrollPane;
        scrollPane.SetPosX(center.x * curScale - scrollPane.viewWidth / 2 + center.width / 2 * curScale, true);
        scrollPane.SetPosY(center.y * curScale - scrollPane.viewHeight / 2 + center.height / 2 * curScale, true);
    }

    private float curScale = 1;
    private float scaleDelta = 0.03f;
    private void _onMouseWheel(EventContext context)
    {
        InputEvent inputEvt = context.inputEvent;
        ScrollPane scrollPane = view.scrollPane;
        bool isReduce = inputEvt.mouseWheelDelta > 0;
        if (inputEvt.mouseWheelDelta > 0)//缩小
        {
            if (Mathf.Floor(grp_setSize.width) <= view.viewWidth && Mathf.Floor(grp_setSize.height) <= view.viewHeight) return; ;//已全部可见
            curScale -= scaleDelta;
        }
        else
        {
            if (Mathf.Floor(grp_setSize.width) >= MapMgr.inst.mapWidth && Mathf.Floor(grp_setSize.height) >= MapMgr.inst.mapHeight) return;//已达到原大小
            curScale += scaleDelta;
        }
        //float toViewX = curScale * MapMgr.inst.mapWidth - grp_setSize.width;
        //float toViewY = curScale * MapMgr.inst.mapHeight - grp_setSize.height;
        //Debug.Log(toViewX + "," + toViewY);
        //Debug.Log("inputEvt.xy: " + inputEvt.x + "," + inputEvt.y);
        UpdateContainerSizeXY();

        scrollPane.SetPosX(scrollPane.scrollingPosX * curScale, false);
        scrollPane.SetPosY(scrollPane.scrollingPosY * curScale, false);

        //Debug.Log("resultView: " + scrollPane.scrollingPosX + "," + scrollPane.scrollingPosY);
    }

    private void UpdateContainerSizeXY()
    {
        grp_container.SetScale(curScale, curScale);
        grp_setSize.SetSize(curScale * MapMgr.inst.mapWidth, curScale * MapMgr.inst.mapHeight);
    }

    private void _onTouchMove(EventContext evt)
    {
        //Debug.Log("resultView: " + view.scrollPane.scrollingPosX + "," + view.scrollPane.scrollingPosY);
    }

    private void OnToOriginalScale(EventCallBack evt)
    {
        curScale = 1;
        grp_container.SetScale(curScale, curScale);
        grp_setSize.SetSize(curScale * MapMgr.inst.mapWidth, curScale * MapMgr.inst.mapHeight);
    }

    private void OnClearAllData(EventCallBack evt)
    {
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

    private Dictionary<string, GComponent> _tempwalkGridDic;//当前可行走的所有格子
    private void OnRunDemo(EventCallBack evt)
    {
        MapMgr.inst.gridTypeDic.TryGetValue(GridType.Walk, out Dictionary<string, GComponent> _tempwalkGridDic);
        if (_tempwalkGridDic == null || _tempwalkGridDic.Count == 0)
        {
            MsgMgr.ShowMsg("没有找到可行走的格子！！！");
            return;
        }
        Vector2 firstWalkGridVec = new();
        foreach (var item in _tempwalkGridDic)
        {
            string[] splitKey = item.Key.Split("_");
            firstWalkGridVec.x = int.Parse(splitKey[0]);
            firstWalkGridVec.y = int.Parse(splitKey[1]);
            break;
        }
        pet.visible = true;
        SetPetPosAndRollCamera(firstWalkGridVec.x * _cellSize, firstWalkGridVec.y * _cellSize + _cellSize, true);
        Timers.inst.AddUpdate(OnUpdate);
    }

    private void OnUpdate(object param)
    {
        JoystickComp joystick = MapMgr.inst.joystick;

        if (!joystick.isMoving) return;

        //摇杆坐标
        Vector2 joysticPos = joystick.vector;
        //向量归一化
        Vector2 dir = BaseUT.angle_to_vector(joystick.curDegree);
        //乘速度
        float dir_x = dir.x * speed;
        float dir_y = dir.y * speed;
        //角色方向
        pet.scaleX = joysticPos.x > 0 ? 1 : -1;
        //角色坐标加上方向
        float toX = pet.x + dir_x;
        float toY = pet.y + dir_y;

        SetPetPosAndRollCamera(toX, toY);
    }

    /** 设置角色位置 && 移动镜头**/
    private void SetPetPosAndRollCamera(float toX, float toY, bool needAni = false)
    {
        //判断边界&&设置角色位置
        if (toX < 0) toX = 0;
        if (toX > MapMgr.inst.mapWidth - pet.width) toX = MapMgr.inst.mapWidth - pet.width;
        if (toY < 0) toY = 0;
        if (toY > MapMgr.inst.mapHeight - pet.height) toY = MapMgr.inst.mapHeight - pet.height;
        pet.SetXY(toX, toY);

        //移动镜头
        ScrollPane scrollPane = view.scrollPane;
        scrollPane.SetPosX(pet.x - scrollPane.viewWidth / 2, needAni);
        scrollPane.SetPosY(pet.y - scrollPane.viewHeight / 2, needAni);
    }
}


