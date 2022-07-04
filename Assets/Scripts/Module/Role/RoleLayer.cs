using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 人物信息界面
/// </summary>
public class RoleLayer : UILayer
{
    protected override string PkgName
    {
        get { return "Role"; }
    }

    protected override string CompName
    {
        get { return "RoleLayer"; }
    }

    private GList list_card;
    private GButton btn_back;
    protected override void OnEnter()
    {
        list_card = view.GetChild("list_card").asList;
        btn_back = view.GetChild("btn_back").asButton;
        btn_back.onClick.Add(() =>
        {
            SceneMgr.inst.pop();
        });

        list_card.SetVirtualAndLoop();

        list_card.itemRenderer = RenderListItem;
        list_card.numItems = 5;
        list_card.scrollPane.onScroll.Add(DoSpecialEffect);

        DoSpecialEffect();
    }

    void DoSpecialEffect()
    {
        //change the scale according to the distance to middle
        float midX = list_card.scrollPane.posX + list_card.viewWidth / 2;
        int cnt = list_card.numChildren;
        for (int i = 0; i < cnt; i++)
        {
            GObject obj = list_card.GetChildAt(i);
            float dist = Mathf.Abs(midX - obj.x - obj.width / 2);
            if (dist > obj.width) //no intersection
                obj.SetScale(1, 1);
            else
            {
                float ss = 1 + (1 - dist / obj.width) * 0.24f;
                obj.SetScale(ss, ss);
            }
        }

        view.GetChild("lbl_index").text = "" + ((list_card.GetFirstChildInView() + 1) % list_card.numItems);
    }

    void RenderListItem(int index, GObject obj)
    {
        GButton item = (GButton)obj;
        item.SetPivot(0.5f, 0.5f);
        item.icon = UIPackage.GetItemURL("Role", "n" + (index + 1));
    }

}

