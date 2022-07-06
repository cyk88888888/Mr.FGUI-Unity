using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
/// <summary>
/// 对象池类
/// author: cyk
/// </summary>
public class ObjectPool<T>
{
    private readonly ConcurrentBag<T> Buffer;
    private readonly Func<T> CreateFunc;
    private readonly Action<T> ResetFunc;

    public int capacity { get; private set; }
    public int count { get { return Buffer.Count; } }
    public ObjectPool(Func<T> _createFunc, Action<T> _resetFunc, int _capacity = -1)
    {
        Contract.Assume(_createFunc != null);

        Buffer = new();
        CreateFunc = _createFunc;
        ResetFunc = _resetFunc;
        capacity = _capacity;
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    /// <returns></returns>
    public T GetObject()
    {
        if(!Buffer.TryTake(out T obj))
        {
            return CreateFunc();
        }
        else
        {
            return obj;
        }
    }

    /// <summary>
    /// 释放对象
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseObject(T obj)
    {
        Contract.Assume(obj != null);

        if (capacity != -1 && count >= capacity) return;//超出存储数量

        ResetFunc?.Invoke(obj);
        Buffer.Add(obj);
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    public void Clear()
    {
        Buffer.Clear();
    }
}
