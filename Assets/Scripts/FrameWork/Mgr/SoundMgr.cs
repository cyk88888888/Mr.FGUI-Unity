using System.Collections;
using UnityEngine;

/// <summary>
/// 音效管理类
/// author:cyk
/// </summary>
public class SoundMgr
{
    private static SoundMgr _inst;
    public static SoundMgr inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = new SoundMgr();
            }
            return _inst;
        }
    }
}
