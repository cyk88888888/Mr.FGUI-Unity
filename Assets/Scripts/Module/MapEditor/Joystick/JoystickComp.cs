using FairyGUI;
using UnityEngine;
/// <summary>
/// 摇杆组件
/// author：cyk
/// </summary>
public class JoystickComp : EventDispatcher
{
    private float _InitX;
    private float _InitY;
    private float _startStageX;
    private float _startStageY;
    private GObject _touchArea;
    private GObject _thumb;
    private GObject _center;
    private int touchId;
    private GTweener _tweener;

    public EventListener onMove { get; private set; }
    public EventListener onEnd { get; private set; }

    public int radius { get; set; }

    public Vector2 vector = Vector2.zero;//摇杆当前位置
    public float curDegree;
    public JoystickComp(GComponent mainView)
    {
        onMove = new EventListener(this, "onMove");
        onEnd = new EventListener(this, "onEnd");

        _thumb = mainView.GetChild("thumb");
        _touchArea = mainView.GetChild("joystick_touch");
        _center = mainView.GetChild("joystick_center");

        _InitX = _center.x;
        _InitY = _center.y;
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
            if (bx < 0)
                bx = 0;
            else if (bx > _touchArea.width)
                bx = _touchArea.width;

            if (by > GRoot.inst.height)
                by = GRoot.inst.height;
            else if (by < _touchArea.y)
                by = _touchArea.y;

            _startStageX = bx;
            _startStageY = by;

            _thumb.visible = _center.visible = true;
            
            _center.SetXY(bx, by);
            _thumb.SetXY(bx, by);

            float deltaX = bx - _InitX;
            float deltaY = by - _InitY;
            float degree = curDegree = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
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
            _tweener = _thumb.TweenMove(new Vector2(_InitX, _InitY), 0.3f).OnComplete(() =>
            {
                _tweener = null;
                _thumb.visible = false;
                _thumb.rotation = 0;
                _center.visible = true;
                _center.SetXY(_InitX, _InitY);
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
            float degree = curDegree = rad * 180 / Mathf.PI;

            _thumb.rotation = degree + 90;
            Vector2 buttonVec = new();
            float distance = (pt - new Vector2(_startStageX, _startStageY)).magnitude;
            if (distance > radius)
            {
                buttonVec.x = _startStageX + radius * Mathf.Cos(rad);
                buttonVec.y = _startStageY + radius * Mathf.Sin(rad);
            }
            else
            {
                buttonVec.x = pt.x;
                buttonVec.y = pt.y;
            }

            _thumb.xy = buttonVec;
            vector.x = pt.x * (pt.x > _startStageX ? 1 : -1);
            vector.y = pt.x * (pt.y > _startStageY ? 1 : -1);

            onMove.Call(degree);
        }
    }
}