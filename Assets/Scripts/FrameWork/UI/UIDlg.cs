using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDlg : UILayer
{
    protected override GComponent GetParent()
    {
        return SceneMgr.inst.curScene.dlg;
    }

    protected override void OnAddToLayer()
    {
        GGraph bgMask = new();
        Color modalLayerColor = new(0x00, 0x00, 0x00, 0.4f);
        bgMask.DrawRect(GRoot.inst.width, GRoot.inst.height, 1, modalLayerColor, modalLayerColor);
        bgMask.onClick.Add(Close);
        AddChildAt(bgMask, 0);
        bgMask.SetXY((bgMask.parent.width - bgMask.width) / 2, (bgMask.parent.height - bgMask.height) / 2);
        OnOpenAnimation();
    }

    protected override void OnOpenAnimation()
    {
        view.SetPivot(0.5f, 0.5f);
        view.TweenScale(new Vector2(1.1f, 1.1f), 0.15f).OnComplete(() =>
        {
            view.TweenScale(new Vector2(1, 1), 0.15f);
        }); ;
    }
}
