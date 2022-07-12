using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDlg : UILayer
{
    protected bool isBgMaskClick = true;//背景灰色是否可点击
    protected bool needOpenAnimation = true;//打开弹窗时是否需要动画
    protected override GComponent GetParent()
    {
        return SceneMgr.inst.curScene.dlg;
    }

    protected override void OnAddToLayer()
    {
        GGraph bgMask = new();
        Color modalLayerColor = new(0x00, 0x00, 0x00, 0.4f);
        bgMask.DrawRect(GRoot.inst.width, GRoot.inst.height, 1, modalLayerColor, modalLayerColor);
        if(isBgMaskClick) bgMask.onClick.Add(Close);
        AddChildAt(bgMask, 0);
        bgMask.SetXY((bgMask.parent.width - bgMask.width) / 2, (bgMask.parent.height - bgMask.height) / 2);
        GComponent frame = view.GetChild("frame")?.asCom;
        GButton btn_close = frame?.GetChild("closeButton")?.asButton;
        btn_close?.onClick?.Add(() =>
        {
            _btnCloseCb?.Invoke();
            Close();
        });
        if(needOpenAnimation) OnOpenAnimation();
    }

    protected override void OnOpenAnimation()
    {
        view.SetPivot(0.5f, 0.5f);
        view.TweenScale(new Vector2(1.1f, 1.1f), 0.15f).OnComplete(() =>
        {
            view.TweenScale(new Vector2(1, 1), 0.15f);
        }); 
    }

    private Action _btnCloseCb;
    public void OnClickBtnClose(Action cb)
    {
        _btnCloseCb = cb;
    }
}
