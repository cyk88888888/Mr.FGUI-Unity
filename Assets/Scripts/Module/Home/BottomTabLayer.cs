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
        Debug.Log("list_bottom.selectedIndex: " + list_bottom.selectedIndex);
    }


}


