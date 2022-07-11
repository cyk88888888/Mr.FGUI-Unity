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

    private JoystickComp _joystick;

    protected override void OnEnter()
    {
        GButton btn_close = view.GetChild("btn_close").asButton;
        btn_close.onClick.Add(() => { 
            Close();
            Emit(GameEvent.CloseDemo);
        });
        _joystick = new JoystickComp(view);
        MapMgr.inst.joystick = _joystick;
    }

    protected override void OnExit()
    {
        MapMgr.inst.joystick = null;
    }
}