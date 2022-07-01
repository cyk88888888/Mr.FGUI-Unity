using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCfgInfo
{
    public Type targetClass;
    /**模块名称 */
    public string name;
    /**是否进行缓存 */
    public bool cacheEnabled;
    /**需要提前加载的资源列表 */
    public string[] preResList;

    /// <summary>
    /// 模块信息
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_cacheEnabled"></param>
    /// <param name="_preResStr"> 预载资源名称，多个</param>
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
