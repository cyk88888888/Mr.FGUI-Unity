using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// ����������
/// author: cyk
/// </summary>
public class BaseUT
{
    private static BaseUT _inst;
    public static BaseUT Inst
    {
        get
        {
            if (_inst == null)
                _inst = new BaseUT();
            return _inst;
        }
    }

    public void SetFitSize(GComponent comp)
    {
        comp.MakeFullScreen();
    }

    /// <summary>
    /// ��������ʵ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="className">�������ɰ��������ռ䣬����G.ClassName��</param>
    /// <param name="param"> ���캯������</param>
    /// <returns></returns>
    public T CreateClassByName<T>(string className, object[] param = null)
    {
        Type t = Type.GetType(className);
        T instance = (T)Activator.CreateInstance(t, param);
        return instance;
    }

}
