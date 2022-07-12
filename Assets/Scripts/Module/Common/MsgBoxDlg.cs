using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消息提示弹框
/// </summary>
public class MsgBoxDlg : UIDlg
{
    protected override string PkgName
    {
        get { return "Common"; }
    }

    protected override string CompName
    {
        get { return "MsgBoxDlg"; }
    }

    protected override void Ctor()
    {
        isBgMaskClick = false;
    }

    protected override void OnEnter()
    {
        MsgBoxInfo info = (MsgBoxInfo)__data;
        GTextField txt_msg = view.GetChild("txt_msg").asTextField;
        GButton btn_ok = view.GetChild("btn_ok").asButton;
        btn_ok.onClick.Add(()=> {
            info.onOk?.Invoke();
            Close();
        });
        GButton btn_cancel = view.GetChild("btn_cancel").asButton;
        btn_cancel.onClick.Add(() => {
            info.onCancel?.Invoke();
            Close();
        });

        OnClickBtnClose(()=> {
            info.onCancel?.Invoke();
        });
        txt_msg.text = info.msg;
        btn_ok.visible = info.onOk != null;
        btn_cancel.visible = info.onCancel != null;
    }

}

public class MsgBoxInfo
{
    public string msg;
    public Action onOk;
    public Action onCancel;
    public MsgBoxInfo(string _msg, Action _onOk = null, Action _onCancel = null)
    {
        msg = _msg;
        onOk = _onOk;
        onCancel = _onCancel;
    }
}
