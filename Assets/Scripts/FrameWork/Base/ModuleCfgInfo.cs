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
    public string[] preResList;

    /// <summary>
    /// ģ����Ϣ
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_cacheEnabled"></param>
    /// <param name="_preResStr"> Ԥ����Դ���ƣ����</param>
    public ModuleCfgInfo(string _name, bool _cacheEnabled, string[] _preResList)
    {
        name = _name;
        targetClass = Type.GetType(_name);
        cacheEnabled = _cacheEnabled;
        preResList = _preResList;
    }
}

public class PkgInfo
{
    public string pkgName;
    public string compName;

    public PkgInfo(string _pkgName, string _compName)
    {
        pkgName = _pkgName;
        compName = _compName;
    }
}
