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

    private GTextField txt_degree;
    private JoystickComp _joystick;

    protected override void OnEnter()
    {
        txt_degree = view.GetChild("txt_degree").asTextField;
        GButton btn_close = view.GetChild("btn_close").asButton;
        btn_close.onClick.Add(() => { 
            Close();
            Emit(GameEvent.CloseDemo);
        });
        _joystick = new JoystickComp(view);
        _joystick.onMove.Add(__joystickMove);
        _joystick.onEnd.Add(__joystickEnd);
    }

    void __joystickMove(EventContext context)
    {
        float degree = (float)context.data;
        txt_degree.text = "" + degree;
    }

    void __joystickEnd()
    {
        txt_degree.text = "";
    }
}