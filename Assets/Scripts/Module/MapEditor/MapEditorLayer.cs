using FairyGUI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// 游戏登入加载界面
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

    private string _curCellSize;
    private GButton btn_walk;
    private GButton btn_block;
    private GButton btn_blockVert;
    private GButton btn_water;
    private GButton btn_clearWalk;
    private GButton btn_clearBolck;
    private GButton btn_clearBolckVert;
    private GButton btn_clearWater;
    private GButton btn_resizeGrid;
    private GButton btn_exportJson;

    private GList list_map;
    private GButton btn_importJson;
    private GTextInput txt_cellSize;
    private GTextField txt_mapSize;

    private List<MapSelectInfo> _mapData;
    protected override void OnEnter()
    {
        txt_cellSize = view.GetChild("txt_cellSize").asTextInput;
        txt_cellSize.text = _curCellSize = MapMgr.inst.cellSize.ToString();

        txt_mapSize = view.GetChild("txt_mapSize").asTextField;
        txt_mapSize.text = MapMgr.inst.mapWidth + "," + MapMgr.inst.mapHeight;

        btn_walk = view.GetChild("btn_walk").asButton;
        btn_walk.onClick.Add(_tap_btn_walk);

        btn_block = view.GetChild("btn_block").asButton;
        btn_block.onClick.Add(_tap_btn_block);

        btn_blockVert = view.GetChild("btn_blockVert").asButton;
        btn_blockVert.onClick.Add(_tap_btn_blockVert);

        btn_water = view.GetChild("btn_water").asButton;
        btn_water.onClick.Add(_tap_btn_water);

        btn_clearWalk = view.GetChild("btn_clearWalk").asButton;
        btn_clearWalk.onClick.Add(_tap_btn_clearWalk);

        btn_clearBolck = view.GetChild("btn_clearBolck").asButton;
        btn_clearBolck.onClick.Add(_tap_btn_clearBolck);

        btn_clearBolckVert = view.GetChild("btn_clearBolckVert").asButton;
        btn_clearBolckVert.onClick.Add(_tap_btn_clearBolckVert);

        btn_clearWater = view.GetChild("btn_clearWater").asButton;
        btn_clearWater.onClick.Add(_tap_btn_clearWater);

        btn_resizeGrid = view.GetChild("btn_resizeGrid").asButton;
        btn_resizeGrid.onClick.Add(_tap_btn_resizeGrid);

        btn_exportJson = view.GetChild("btn_exportJson").asButton;
        btn_exportJson.onClick.Add(_tap_btn_exportJson);

        btn_importJson = view.GetChild("btn_importJson").asButton;
        btn_importJson.onClick.Add(_tap_btn_importJson);

    
        list_map = view.GetChild("list_map").asList;
        list_map.onClickItem.Add(OnClickMapItem);
        list_map.itemRenderer = RenderListMapItem;
        _mapData = MapMgr.inst.mapDataList;
        list_map.numItems = _mapData.Count;
        list_map.selectedIndex = 0;
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
        if (_curCellSize == txt_cellSize.text)
        {
            MsgMgr.ShowMsg("格子大小未变！！！");
            return;
        }
        _curCellSize = txt_cellSize.text;
        Emit(GameEvent.ResizeGrid, new object[] { txt_cellSize.text });
    }


    private void OnClickMapItem(EventContext context)
    {
        MapSelectInfo itemInfo = _mapData[list_map.selectedIndex];
        Emit(GameEvent.ChangeMap, new object[] { itemInfo });
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

}
