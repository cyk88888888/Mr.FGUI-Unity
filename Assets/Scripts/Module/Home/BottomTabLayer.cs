using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 主界面底部按钮
/// </summary>
public class BottomTabLayer : UIMenu
{
    protected override string PkgName
    {
        get { return "Home"; }
    }

    protected override string CompName
    {
        get { return "BottomTabLayer"; }
    }

    private GList list_bottom;
    private int _curSelectIndex;
    private List<object> _layerInfos;

    protected override void Ctor()
    {
        _layerInfos = new List<object>();
        _layerInfos.Add("EquipLayer");
        _layerInfos.Add("ShopLayer");
        _layerInfos.Add("HomeLayer");
        _layerInfos.Add("SkillLayer");
        _layerInfos.Add("SettingLayer");
    }
    protected override void OnEnter()
    {
        list_bottom = view.GetChild("list_bottom").asList;
        list_bottom.onClickItem.Add(OnClickItem);
    }

    protected override void OnFirstEnter()
    {
        _curSelectIndex = list_bottom.selectedIndex = 2;
    }

    private void OnClickItem()
    {
        if (_curSelectIndex == list_bottom.selectedIndex) return;
        _curSelectIndex = list_bottom.selectedIndex;
        object layerInfo = _layerInfos[_curSelectIndex];
        //let layerName = layerInfo.layer;
        Emit("jumpToLayer", new object[] { layerInfo });
    }


}


