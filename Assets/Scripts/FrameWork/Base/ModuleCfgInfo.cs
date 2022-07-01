using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCfgInfo
{
    public Type targetClass;
    /**ģ������ */
    public string name;
    /**�Ƿ���л��� */
    public bool cacheEnabled;
    /**��Ҫ��ǰ���ص���Դ�б� */
    public List<string> preResList;

    public ModuleCfgInfo(string _name, bool _cacheEnabled, List<string> _preResList)
    {
        name = _name;
        targetClass = Type.GetType(_name);
        cacheEnabled = _cacheEnabled;
        preResList = _preResList;
    }
}
