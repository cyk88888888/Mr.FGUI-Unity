using System.Collections;
using UnityEngine;

/// <summary>
/// 场景管理类
/// author:cyk
/// </summary>
public class SceneMgr
{
    private static SceneMgr _inst;
    public static SceneMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new SceneMgr();
            }
            return _inst;
        }
    }
}
