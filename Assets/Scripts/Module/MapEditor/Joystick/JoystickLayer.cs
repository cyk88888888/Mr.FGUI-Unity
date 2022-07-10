using UnityEngine;
using FairyGUI;

/// <summary>
/// 摇杆界面
/// author：cyk
/// </summary>
public class JoystickLayer : UILayer
{
    protected override string PkgName
    {
        get { return "MapEditor"; }
    }

    protected override string CompName
    {
        get { return "JoystickLayer"; }
    }

    GTextField _text;
    JoystickComp _joystick;

    protected override void OnEnter()
    {
        _text = view.GetChild("n9").asTextField;
        GButton btn_close = view.GetChild("btn_close").asButton;
        btn_close.onClick.Add(() => { Close(); });
        _joystick = new JoystickComp(view);
        _joystick.onMove.Add(__joystickMove);
        _joystick.onEnd.Add(__joystickEnd);
    }

    void __joystickMove(EventContext context)
    {
        float degree = (float)context.data;
        _text.text = "" + degree;
    }

    void __joystickEnd()
    {
        _text.text = "";
    }
}