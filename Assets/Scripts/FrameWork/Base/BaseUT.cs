using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// 基础工具类
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
    /// 根据类名实例化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="className">类名（可包含命名空间，例如G.ClassName）</param>
    /// <param name="param"> 构造函数参数</param>
    /// <returns></returns>
    public T CreateClassByName<T>(string className, object[] param = null)
    {
        Type t = Type.GetType(className);
        T instance = (T)Activator.CreateInstance(t, param);
        return instance;
    }

}
