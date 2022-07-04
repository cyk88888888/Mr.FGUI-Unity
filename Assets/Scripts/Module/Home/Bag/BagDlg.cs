using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 背包界面
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
    private List<BagItemInfo> _bagData;
    protected override void OnEnter()
    {
        _list = view.GetChild("list_bag").asList;
        _list.onClickItem.Add(__clickItem);
        _list.itemRenderer = RenderListItem;

        _bagData = new();
        int len = 45;
        for (int i = 0; i < len; i++)
        {
            BagItemInfo info = new(Random.Range(0, 10), Random.Range(0, 100));
            _bagData.Add(info);
        }
        _list.numItems = _bagData.Count;

        if (_bagData.Count > 0)
        {
            _list.selectedIndex = Random.Range(0, len);
            _list.ScrollToView(_list.selectedIndex);
            ShowItemDetail(_list.selectedIndex);
        }
        GButton btn_close = view.GetChild("frame").asCom.GetChild("closeButton").asButton;
        btn_close.onClick.Add(() =>{
            Close();
        });
    }

    private void RenderListItem(int index, GObject obj)
    {
        GButton button = (GButton)obj;
        BagItemInfo itemInfo = _bagData[index];
        button.icon = ResUrl.GetItemIcon(itemInfo.id);
        button.title = "" + itemInfo.count;
    }

    private void __clickItem(EventContext context)
    {
        //GButton item = (GButton)context.data;
        //view.GetChild("n11").asLoader.url = item.icon;
        //view.GetChild("n13").text = item.icon;
        ShowItemDetail(_list.selectedIndex);
    }

    private void ShowItemDetail(int index)
    {
        BagItemInfo itemInfo = _bagData[index];
        view.GetChild("n11").asLoader.url = ResUrl.GetItemIcon(itemInfo.id);
        view.GetChild("n13").text = ResUrl.GetItemIcon(itemInfo.id);
    }

    private void TestA()
    {
        Debug.Log("我的方法TestA！！！！！！！！！！！！！！！！");
    }

}

/// <summary>
/// 背包道具信息
/// </summary>
public class BagItemInfo
{
    public int id;
    public int count;
    public BagItemInfo(int _id, int _count)
    {
        id = _id;
        count = _count;
    }
}

