using FairyGUI;
using System.Collections.Generic;
/// <summary>
/// 地图编辑器主界面
/// </summary>
public class MapEditorLayer : UILayer
{
    protected override string PkgName
    {
        get { return "MapEditor"; }
    }

    protected override string CompName
    {
        get { return "MapEditorLayer"; }
    }

    private GList list_map;
    private GTextInput txt_cellSize;
    private GTextInput txt_mapWidth;
    private GTextInput txt_mapHeight;
    private List<MapSelectInfo> _mapData;
    private int _lastSelctIdx = 0;
    protected override void OnEnter()
    {
        txt_cellSize = view.GetChild("txt_cellSize").asTextInput;
        txt_cellSize.text = MapMgr.inst.cellSize.ToString();

        txt_mapWidth = view.GetChild("txt_mapWidth").asTextInput;
        txt_mapWidth.text = MapMgr.inst.mapWidth.ToString();

        txt_mapHeight = view.GetChild("txt_mapHeight").asTextInput;
        txt_mapHeight.text = MapMgr.inst.mapHeight.ToString();

        GButton btn_walk = view.GetChild("btn_walk").asButton;
        btn_walk.onClick.Add(_tap_btn_walk);

        GButton btn_block = view.GetChild("btn_block").asButton;
        btn_block.onClick.Add(_tap_btn_block);

        GButton btn_blockVert = view.GetChild("btn_blockVert").asButton;
        btn_blockVert.onClick.Add(_tap_btn_blockVert);

        GButton btn_water = view.GetChild("btn_water").asButton;
        btn_water.onClick.Add(_tap_btn_water);

        GButton btn_clearWalk = view.GetChild("btn_clearWalk").asButton;
        btn_clearWalk.onClick.Add(_tap_btn_clearWalk);

        GButton btn_clearBolck = view.GetChild("btn_clearBolck").asButton;
        btn_clearBolck.onClick.Add(_tap_btn_clearBolck);

        GButton btn_clearBolckVert = view.GetChild("btn_clearBolckVert").asButton;
        btn_clearBolckVert.onClick.Add(_tap_btn_clearBolckVert);

        GButton btn_clearWater = view.GetChild("btn_clearWater").asButton;
        btn_clearWater.onClick.Add(_tap_btn_clearWater);

        GButton btn_resizeGrid = view.GetChild("btn_resizeGrid").asButton;
        btn_resizeGrid.onClick.Add(_tap_btn_resizeGrid);

        GButton btn_resizeMap = view.GetChild("btn_resizeMap").asButton;
        btn_resizeMap.onClick.Add(_tap_btn_resizeMap);

        GButton btn_toCenter = view.GetChild("btn_toCenter").asButton;
        btn_toCenter.onClick.Add(_tap_btn_toCenter);

        GButton btn_originalScale = view.GetChild("btn_originalScale").asButton;
        btn_originalScale.onClick.Add(_tap_btn_originalScale);

        GButton btn_exportJson = view.GetChild("btn_exportJson").asButton;
        btn_exportJson.onClick.Add(_tap_btn_exportJson);

        GButton btn_importJson = view.GetChild("btn_importJson").asButton;
        btn_importJson.onClick.Add(_tap_btn_importJson);

        GButton btn_clearAll = view.GetChild("btn_clearAll").asButton;
        btn_clearAll.onClick.Add(_tap_btn_clearAll);

        GButton btn_runDemo = view.GetChild("btn_runDemo").asButton;
        btn_runDemo.onClick.Add(_tap_btn_runDemo);

        list_map = view.GetChild("list_map").asList;
        list_map.onClickItem.Add(OnClickMapItem);
        list_map.itemRenderer = RenderListMapItem;
        _mapData = MapMgr.inst.mapDataList;
        list_map.numItems = _mapData.Count;
        list_map.selectedIndex = 0;

        OnEmitter(GameEvent.ImportMapJson, OnImportMapJson);//清除所有线条和格子
    }

    private void _tap_btn_walk()
    {
        Emit(GameEvent.ChangeGridType, new object[] { GridType.Walk });
    }

    private void _tap_btn_block()
    {
        Emit(GameEvent.ChangeGridType, new object[] { GridType.Block });
    }

    private void _tap_btn_blockVert()
    {
        Emit(GameEvent.ChangeGridType, new object[] { GridType.BlockVerts });
    }

    private void _tap_btn_water()
    {
        Emit(GameEvent.ChangeGridType, new object[] { GridType.Water });
    }

    private void _tap_btn_clearWalk()
    {
        Emit(GameEvent.ClearGridType, new object[] { GridType.Walk });
    }
    private void _tap_btn_clearBolck()
    {
        Emit(GameEvent.ClearGridType, new object[] { GridType.Block });
    }
    private void _tap_btn_clearBolckVert()
    {
        Emit(GameEvent.ClearGridType, new object[] { GridType.BlockVerts });
    }
    private void _tap_btn_clearWater()
    {
        Emit(GameEvent.ClearGridType, new object[] { GridType.Water });
    }
    private void _tap_btn_resizeGrid()
    {
        Emit(GameEvent.ResizeGrid, new object[] { txt_cellSize.text });
    }

    private void _tap_btn_resizeMap()
    {
        Emit(GameEvent.ResizeMap, new object[] { txt_mapWidth.text, txt_mapHeight.text });
    }

    private void _tap_btn_toCenter()
    {
        Emit(GameEvent.ToCenter);
    }

    private void _tap_btn_originalScale()
    {
        Emit(GameEvent.ToOriginalScale);
    }

    /** 清除所有数据**/
    private void _tap_btn_clearAll()
    {
        Emit(GameEvent.ClearAllData);
    }

    private void OnImportMapJson(EventCallBack evt)
    {
        txt_mapWidth.text = MapMgr.inst.mapWidth.ToString();
        txt_mapHeight.text = MapMgr.inst.mapHeight.ToString();
        txt_cellSize.text = MapMgr.inst.cellSize.ToString();
    }

    private void OnClickMapItem(EventContext context)
    {
        bool isEmptyMap = true;
        foreach (var item in MapMgr.inst.gridTypeDic)
        {
            if (item.Value.Count > 0)
            {
                isEmptyMap = false;
                break;
            }
        }
        if (!isEmptyMap)
        {
            MsgMgr.ShowMsg("当前有未保存的地图数据，确定切换地图吗？", MsgType.MsgBox, ()=> {
                onSure();
            },()=> {
                list_map.selectedIndex = _lastSelctIdx;
            });
            return;
        }
        else
        {
            onSure();
        }
        void onSure()
        {
            _lastSelctIdx = list_map.selectedIndex;
            MapSelectInfo itemInfo = _mapData[list_map.selectedIndex];
            Emit(GameEvent.ChangeMap, new object[] { itemInfo });
        }
       
    }

    private void RenderListMapItem(int index, GObject obj)
    {
        GButton button = (GButton)obj;
        MapSelectInfo itemInfo = _mapData[index];
        button.title = itemInfo.mapName;
    }

    /** 导出json地图数据**/
    private void _tap_btn_exportJson()
    {
        MapMgr.inst.ExportJsonData();
    }

    /** 导入json地图数据**/
    private void _tap_btn_importJson()
    {
        MapMgr.inst.ImportJsonData();
    }

    /** 截图绘画区域**/
    private void _tap_btn_screenShot()
    {
        Emit(GameEvent.ScreenShoot);
    }

    /** 测试运行demo**/
    private void _tap_btn_runDemo()
    {
        Show("JoystickLayer");
        Emit(GameEvent.RunDemo);
    }

}
