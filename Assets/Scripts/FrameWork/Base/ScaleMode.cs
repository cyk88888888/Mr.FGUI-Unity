using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ҳ�������С��߲�������
/// author��cyk
/// </summary>
public class ScaleMode
{
    /**
     * ��ƿ��
     */
    public float designWidth;
     /**
     * ��Ƹ߶�
     */
    public float designHeight;
    /**
     * �����С�߶�
     */
    public float designHeight_min;
     /**
     * ������߶�
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


