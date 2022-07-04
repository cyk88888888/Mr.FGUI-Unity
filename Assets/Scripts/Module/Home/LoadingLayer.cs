using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ��Ϸ������ؽ���
/// </summary>
public class LoadingLayer: UILayer
{
    protected override string PkgName
    {
        get { return "Loading"; }
    }

    protected override string CompName
    {
        get { return "LoadingLayer"; }
    }

    private GProgressBar bar;
    private bool isEntering;
    protected override void OnEnter()
    {
        bar = view.GetChild("bar").asProgress;
        Timers.inst.AddUpdate(OnTick);
    }

    private void OnTick(object param)
    {
        bar.value += 0.1;
        if (!isEntering && bar.value >= 100)
        {
            isEntering = true;
            SceneMgr.inst.Run("HomeScene");
            Timers.inst.Remove(OnTick);
        }
    }
}

public class B<T> where T : new()
{
    public static T Get()
    {
        T result = new T();//�����Ϳ���ʵ������Ҳ���Ա���ͨ���� 
        return result;
    }
}
