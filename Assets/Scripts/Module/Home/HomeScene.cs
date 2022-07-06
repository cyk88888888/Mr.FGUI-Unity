using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ÓÎÏ·Ö÷³¡¾°
/// author£ºcyk
/// </summary>
public class HomeScene : UIScene
{
    private TopInfoLayer _topUsrLayer;
    private BottomTabLayer _bottomLayer;
    protected override void Ctor()
    {
        mainClassLayer = "HomeLayer";
        List<string> classList = new() { "EquipLayer", "ShopLayer", "SkillLayer", "SettingLayer" };
        foreach (var item in classList)
        {
            subLayerMgr.Register(item);
        }
    }

    protected override void OnEnter()
    {
        if (_topUsrLayer == null) _topUsrLayer = UILayer.Show("TopInfoLayer") as TopInfoLayer;
        if (_bottomLayer == null) _bottomLayer = UILayer.Show("BottomTabLayer") as BottomTabLayer;

        OnEmitter("jumpToLayer", OnJumpToLayer);
    }

    private void OnJumpToLayer(EventCallBack prarm)
    {
        if (prarm.Data == null) return;
        Run((string)prarm.Data[0]);
    }
}
