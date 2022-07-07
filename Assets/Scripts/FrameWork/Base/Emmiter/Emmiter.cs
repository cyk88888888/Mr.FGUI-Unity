using UnityEngine;
using System.Collections.Generic;
using System;

public delegate void EventListenerDelegate(EventCallBack evt);

/// <summary>
/// 事件管理
/// CYK
/// </summary>
public class Emmiter
{
    static Dictionary<string, EventListenerDelegate> notifications = new Dictionary<string, EventListenerDelegate>(); // 所有的消息
    public static void On(string type, EventListenerDelegate listener)
    {
        if (listener == null)
        {
            Debug.LogError("registerObserver: listener不能为空");
            return;
        }

        // 将新来的监听者加入调用链，这样只要调用Combine返回的监听者就会调用所有的监听者
        // Debug.Log("NotifacitionCenter: 添加监视" + type);

        notifications.TryGetValue(type, out EventListenerDelegate myListener);
        notifications[type] = (EventListenerDelegate)Delegate.Combine(myListener, listener);
    }
    public static void Off(string type, EventListenerDelegate listener)
    {
        if (listener == null)
        {
            Debug.LogError("removeObserver: listener不能为空");
            return;
        }
        // Debug.Log("NotifacitionCenter: 移除监视" + type);
        notifications[type] = (EventListenerDelegate)Delegate.Remove(notifications[type], listener);
    }
    public static void OffAll()
    {
        notifications.Clear();
    }
    public static void Emit(EventCallBack evt)
    {
        EventListenerDelegate listenerDelegate;
        if (notifications.TryGetValue(evt.Type, out listenerDelegate))
        {
            try
            {
                if (listenerDelegate != null)
                {
                    // 执行调用所有的监听者
                    listenerDelegate(evt);
                }

            }
            catch (Exception e)
            {
                throw new Exception(string.Concat(new string[] { "Error dispatching event", evt.Type.ToString(), ": ", e.Message, " ", e.StackTrace }), e);
            }
        }
    }

}

namespace Global
{
    public class GlobalEmmiter
    {
        public static void Emit(string notificationName)
        {
            Emmiter.Emit(new EventCallBack(notificationName));
        }
        public static void Emit(string notificationName, object[] body)
        {
            Emmiter.Emit(new EventCallBack(notificationName, body));
        }
        public static void Emit(string notificationName, object[] body, object sender)
        {
            Emmiter.Emit(new EventCallBack(notificationName, body, sender));
        }
        public static void OnEmitter(string type, EventListenerDelegate listener)
        {
            Emmiter.On(type, listener);
        }
        public static void UnEmitter(string type, EventListenerDelegate listener)
        {
            Emmiter.Off(type, listener);
        }

    }
}

