using FairyGUI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
/// <summary>
/// ��Ϸ������ؽ���
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
    private GButton btn_clearWalk;
    private GButton btn_clearBolck;
    private GButton btn_clearBolckVert;
    private GButton btn_resizeGrid;
    private GButton btn_exportJson;
    
    private GButton btn_importJson;
    private GTextInput txt_cellSize;
    protected override void OnEnter()
    {
        txt_cellSize = view.GetChild("txt_cellSize").asTextInput;
        txt_cellSize.text = _curCellSize = MapMgr.cellSize.ToString();

        btn_walk = view.GetChild("btn_walk").asButton;
        btn_walk.onClick.Add(_tap_btn_walk);

        btn_block = view.GetChild("btn_block").asButton;
        btn_block.onClick.Add(_tap_btn_block);

        btn_blockVert = view.GetChild("btn_blockVert").asButton;
        btn_blockVert.onClick.Add(_tap_btn_blockVert);

        btn_clearWalk = view.GetChild("btn_clearWalk").asButton;
        btn_clearWalk.onClick.Add(_tap_btn_clearWalk);

        btn_clearBolck = view.GetChild("btn_clearBolck").asButton;
        btn_clearBolck.onClick.Add(_tap_btn_clearBolck);

        btn_clearBolckVert = view.GetChild("btn_clearBolckVert").asButton;
        btn_clearBolckVert.onClick.Add(_tap_btn_clearBolckVert);

        btn_resizeGrid = view.GetChild("btn_resizeGrid").asButton;
        btn_resizeGrid.onClick.Add(_tap_btn_resizeGrid);

        btn_exportJson = view.GetChild("btn_exportJson").asButton;
        btn_exportJson.onClick.Add(_tap_btn_exportJson);

        btn_importJson = view.GetChild("btn_importJson").asButton;
        btn_importJson.onClick.Add(_tap_btn_importJson);
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

    private void _tap_btn_resizeGrid()
    {
        if (_curCellSize == txt_cellSize.text)
        {
            MsgMgr.ShowMsg("���Ӵ�Сδ�䣡����");
            return;
        }
        _curCellSize = txt_cellSize.text;
        Emit(GameEvent.ResizeGrid, new object[] { txt_cellSize.text });
    }

    /** ����json��ͼ����**/
    private void _tap_btn_exportJson()
    {
        MapMgr.inst.ExportJsonData();
    }

    /** ����json��ͼ����**/
    private void _tap_btn_importJson()
    {
        MapMgr.inst.ImportJsonData();
        //FileUT.OpenDirectoryBrower();
    }

}