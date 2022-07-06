using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
/// <summary>
/// �������
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
    /// ��ȡ����
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
    /// �ͷŶ���
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseObject(T obj)
    {
        Contract.Assume(obj != null);

        if (capacity != -1 && count >= capacity) return;//�����洢����

        ResetFunc?.Invoke(obj);
        Buffer.Add(obj);
    }

    /// <summary>
    /// �������
    /// </summary>
    public void Clear()
    {
        Buffer.Clear();
    }
}
