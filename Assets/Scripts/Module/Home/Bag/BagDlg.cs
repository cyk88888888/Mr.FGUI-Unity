using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ±³°ü½çÃæ
/// </summary>
public class BagDlg : UIDlg
{
    protected override string PkgName
    {
        get { return "Home"; }
    }

    protected override string CompName
    {
        get { return "BagDlg"; }
    }
    private GList _list;
    protected override void OnEnter()
    {
        _list = view.GetChild("list_bag").asList;
        _list.onClickItem.Add(__clickItem);
        _list.itemRenderer = RenderListItem;
        _list.numItems = 45;

        GButton btn_close = view.GetChild("frame").asCom.GetChild("closeButton").asButton;
        btn_close.onClick.Add(() => { 
            Close();
        });
    }

    private void RenderListItem(int index, GObject obj)
    {
        GButton button = (GButton)obj;
        button.icon = ResUrl.GetItemIcon(Random.Range(0, 10));
        button.title = "" + Random.Range(0, 100);
    }

    private void __clickItem(EventContext context)
    {
        GButton item = (GButton)context.data;
        view.GetChild("n11").asLoader.url = item.icon;
        view.GetChild("n13").text = item.icon;
    }

}


