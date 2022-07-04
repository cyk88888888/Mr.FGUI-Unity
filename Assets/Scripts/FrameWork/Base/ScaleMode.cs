using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 页面最大最小宽高参数设置
/// author：cyk
/// </summary>
public class ScaleMode
{
    /**
     * 设计宽度
     */
    public float designWidth;
     /**
     * 设计高度
     */
    public float designHeight;
    /**
     * 设计最小高度
     */
    public float designHeight_min;
     /**
     * 设计最大高度
     */
    public float designHeight_max;

    
    public ScaleMode(float _designWidth, float _designHeight, float _designHeight_min, float _designHeight_max)
    {
        designWidth = _designWidth;
        designHeight = _designHeight;
        designHeight_min = _designHeight_min;
        designHeight_max = _designHeight_max;
    }
}


