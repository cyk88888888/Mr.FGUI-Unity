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

    private GButton btn_walk;
    private GButton btn_block;
    private GButton btn_blockVert;
    protected override void OnEnter()
    {
        btn_walk = view.GetChild("btn_walk").asButton;
        btn_walk.onClick.Add(_tap_btn_walk);

        btn_block = view.GetChild("btn_block").asButton;
        btn_block.onClick.Add(_tap_btn_block);

        btn_blockVert = view.GetChild("btn_blockVert").asButton;
        btn_blockVert.onClick.Add(_tap_btn_blockVert);

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

}
