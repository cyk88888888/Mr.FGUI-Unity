using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
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
    private GButton btn_resizeGrid;
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

        btn_resizeGrid = view.GetChild("btn_resizeGrid").asButton;
        btn_resizeGrid.onClick.Add(_tap_btn_resizeGrid);

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

}
