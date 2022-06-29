using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基础工具类
/// author: cyk
/// </summary>
public class BaseUT
{
    private static BaseUT _inst;
    public static BaseUT inst
    {
        get
        {
            if (_inst == null)
                _inst = new BaseUT();
            return _inst;
        }
    }
}
