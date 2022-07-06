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

    public static void ShowMsg(string msg)
    {
        GComponent msgTip = UIPackage.CreateObject("Common", "MsgTip").asCom;
        GTextField txt_msg = msgTip.GetChild("txt_msg").asTextField;
        txt_msg.text = msg;
        if (txt_msg.textWidth > 300) msgTip.width = txt_msg.textWidth + 20;
        SceneMgr.inst.curScene.msg.AddChild(msgTip);
        msgTip.SetXY((msgTip.parent.width - msgTip.width) / 2, (msgTip.parent.height - msgTip.height) / 2 - 200);
        msgTip.TweenMoveY(msgTip.y - 100, 0.3f).SetDelay(0.5f).SetEase(EaseType.QuadOut).OnComplete(() =>
        {
            msgTip.Dispose();
        });
    }

}
