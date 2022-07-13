using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消息码管理器
/// author：cyk
/// </summary>
public class MsgMgr
{
    private static MsgMgr _inst;
    public static MsgMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new MsgMgr();
            }
            return _inst;
        }
    }

    /// <summary>
    /// 显示消息提示
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="msgtype"></param>
    /// <param name="onOk"></param>
    /// <param name="onCancel"></param>
    public static void ShowMsg(string msg, MsgType msgtype = MsgType.Normal, Action onOk = null, Action onCancel = null)
    {
        if (msgtype == MsgType.Normal)
        {
            GComponent msgTip = UIPackage.CreateObject("Common", "MsgTip").asCom;
            GTextField txt_msg = msgTip.GetChild("txt_msg").asTextField;
            txt_msg.text = msg;
            if (txt_msg.textWidth > 300) msgTip.width = txt_msg.textWidth + 20;
            SceneMgr.inst.curScene.msg.AddChild(msgTip);
            msgTip.SetXY((msgTip.parent.width - msgTip.width) / 2, (msgTip.parent.height - msgTip.height) / 2 - 200);
            msgTip.TweenMoveY(msgTip.y - 100, 0.3f).SetDelay(0.8f).SetEase(EaseType.QuadOut).OnComplete(() =>
            {
                msgTip.Dispose();
            });
        }
        else if (msgtype == MsgType.MsgBox)
        {
            UILayer.Show("MsgBoxDlg", new MsgBoxInfo(msg, onOk, onCancel));
        }
    }
}
public enum MsgType
{
    Normal,//普通文本消息提示
    MsgBox,//弹窗
}
