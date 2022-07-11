using FairyGUI;
using UnityEngine;
/// <summary>
/// 摇杆组件
/// author：cyk
/// </summary>
public class JoystickComp : EventDispatcher
{
    float _InitX;
    float _InitY;
    float _startStageX;
    float _startStageY;
    float _lastStageX;
    float _lastStageY;
    GButton _button;
    GObject _touchArea;
    GObject _thumb;
    GObject _center;
    int touchId;
    GTweener _tweener;

    public EventListener onMove { get; private set; }
    public EventListener onEnd { get; private set; }

    public int radius { get; set; }

    public Vector2 vector = Vector2.zero;//摇杆当前位置
    public JoystickComp(GComponent mainView)
    {
        onMove = new EventListener(this, "onMove");
        onEnd = new EventListener(this, "onEnd");

        _button = mainView.GetChild("joystick").asButton;
        _button.changeStateOnClick = false;
        _thumb = _button.GetChild("thumb");
        _touchArea = mainView.GetChild("joystick_touch");
        _center = mainView.GetChild("joystick_center");

        _InitX = _center.x + _center.width / 2;
        _InitY = _center.y + _center.height / 2;
        touchId = -1;
        radius = 150;

        _touchArea.onTouchBegin.Add(this.OnTouchBegin);
        _touchArea.onTouchMove.Add(this.OnTouchMove);
        _touchArea.onTouchEnd.Add(this.OnTouchEnd);
    }

    public void Trigger(EventContext context)
    {
        OnTouchBegin(context);
    }

    /** 摇杆是否在移动中**/
    public bool isMoving
    {
        get { return vector != Vector2.zero; }
    }

    private void OnTouchBegin(EventContext context)
    {
        if (touchId == -1)//First touch
        {
            InputEvent evt = (InputEvent)context.data;
            touchId = evt.touchId;

            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }
            vector = Vector2.zero;
            Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x, evt.y));
            float bx = pt.x;
            float by = pt.y;
            _button.selected = true;

            if (bx < 0)
                bx = 0;
            else if (bx > _touchArea.width)
                bx = _touchArea.width;

            if (by > GRoot.inst.height)
                by = GRoot.inst.height;
            else if (by < _touchArea.y)
                by = _touchArea.y;

            _lastStageX = bx;
            _lastStageY = by;
            _startStageX = bx;
            _startStageY = by;

            _center.visible = true;
            _center.SetXY(bx - _center.width / 2, by - _center.height / 2);
            _button.SetXY(bx - _button.width / 2, by - _button.height / 2);

            float deltaX = bx - _InitX;
            float deltaY = by - _InitY;
            float degree = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
            _thumb.rotation = degree + 90;
            onMove.Call(degree);
            context.CaptureTouch();
        }
    }

    private void OnTouchEnd(EventContext context)
    {
        InputEvent inputEvt = (InputEvent)context.data;
        if (touchId != -1 && inputEvt.touchId == touchId)
        {
            touchId = -1;
            _thumb.rotation = _thumb.rotation + 180;
            _center.visible = false;
            vector = Vector2.zero;
            _tweener = _button.TweenMove(new Vector2(_InitX - _button.width / 2, _InitY - _button.height / 2), 0.3f).OnComplete(() =>
            {
                _tweener = null;
                _button.selected = false;
                _thumb.rotation = 0;
                _center.visible = true;
                _center.SetXY(_InitX - _center.width / 2, _InitY - _center.height / 2);
            }
            );

            onEnd.Call();
        }
    }

    private void OnTouchMove(EventContext context)
    {
        InputEvent evt = (InputEvent)context.data;
        if (touchId != -1 && evt.touchId == touchId)
        {
            Vector2 pt = GRoot.inst.GlobalToLocal(new Vector2(evt.x, evt.y));

            float rad = Mathf.Atan2(pt.y - _startStageY, pt.x - _startStageX);
            float degree = rad * 180 / Mathf.PI;

            _thumb.rotation = degree + 90;
            Vector2 buttonVec = new();
            float distance = (pt - new Vector2(_startStageX, _startStageY)).magnitude;
            if(distance > radius)
            {
                buttonVec.x = _startStageX + radius * Mathf.Cos(rad); 
                buttonVec.y = _startStageY + radius * Mathf.Sin(rad); 
            }
            else
            {
                buttonVec.x = pt.x;
                buttonVec.y = pt.y;
            }

            //设置摇杆中心点
            buttonVec.x -= _button.width / 2;
            buttonVec.y -= _button.height / 2;
            _button.xy = buttonVec;
            vector.x = pt.x * (pt.x > _startStageX ? 1 : -1);
            vector.y = pt.x * (pt.y > _startStageY ? 1 : -1);

            onMove.Call(degree);
            Debug.Log(vector.normalized);
        }
    }
}