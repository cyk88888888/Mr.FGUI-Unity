using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
/// <summary>
/// ����������
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
    public ScaleMode scaleMode;
    public Vector2 SetFitSize(GComponent comp)
    {
        float designHeight = GRoot.inst.height < scaleMode.designHeight_max ? GRoot.inst.height : scaleMode.designHeight_max;
        comp.SetSize(scaleMode.designWidth, designHeight);
        return new Vector2(scaleMode.designWidth, designHeight);
    }

    /// <summary>
    /// ��������ʵ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="className">�������ɰ��������ռ䣬����G.ClassName��</param>
    /// <param name="param"> ���캯������</param>
    /// <returns></returns>
    public T CreateClassByName<T>(string className, object[] param = null)
    {
        Type t = Type.GetType(className);
        T instance = (T)Activator.CreateInstance(t, param);
        return instance;
    }

    /** �����ͼ������ͼ�񵽱���**/
    public void SaveViewShotToLocal(GObject aObject)
    {
        DisplayObject dObject = aObject.displayObject;
        dObject.EnterPaintingMode(1024, null);

        //�����ڱ�֡��Ⱦ����ܸ��£����Է�������Ĵ�����Ҫ�ӳٵ���һִ֡�С�
        Timers.inst.CallLater((object param) =>
        {

            RenderTexture renderTexture = (RenderTexture)dObject.paintingGraphics.texture.nativeTexture;

            int width = renderTexture.width;
            int height = renderTexture.height;
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture2D.Apply();
            byte[] vs = texture2D.EncodeToPNG();

            FileUT.SavePngImageBrower((string path) =>
            {
                string fullPath = path + ".png";//�����json�ļ�����������ַ
                FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(vs, 0, vs.Length);
                fileStream.Dispose();
                fileStream.Close();

                //�������������滭ģʽ��idҪ��Enter�����Ķ�Ӧ��
                dObject.LeavePaintingMode(1024);

                MsgMgr.ShowMsg("����ɹ�");
            });
        });
    }

}
