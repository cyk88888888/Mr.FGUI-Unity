using FairyGUI;
using UnityEngine;

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
            float buttonX;
            float buttonY;
            float distance = (pt - new Vector2(_startStageX, _startStageY)).magnitude;
            if(distance > radius)
            {
                float maxX = _startStageX + radius * Mathf.Cos(rad);
                float maxY = _startStageY + radius * Mathf.Sin(rad);
                buttonX = maxX;
                buttonY = maxY;
            }
            else
            {
                buttonX = pt.x;
                buttonY = pt.y;
            }

            _button.SetXY(buttonX - _button.width / 2, buttonY - _button.height / 2);
            onMove.Call(degree);
        }
    }
}