/// <summary>
/// 事件回调类
/// CYK
/// </summary>
public class EventCallBack
{
    protected object[] arguments;  //参数
    protected string type;  //事件类型
    protected object sender;    //发送者
                        
    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public object[] Params
    {
        get { return arguments; }
        set { arguments = value; }
    }

    public object Sender
    {
        get { return sender; }
        set { sender = value; }
    }
    // 常用函数
    public override string ToString()
    {
        return type + " [ " + ((sender == null) ? "null" : sender.ToString()) + " ] ";
    }
    public EventCallBack(string type)
    {
        Type = type;
    }
    public EventCallBack(string type, object[] args)
    {
        Type = type;
        arguments = args;
    }
    public EventCallBack(string type, object[] args, object sender)
    {
        Type = type;
        arguments = args;
        Sender = sender;
    }
    public EventCallBack Clone()
    {
        return new EventCallBack(type, arguments, sender);
    }
}
