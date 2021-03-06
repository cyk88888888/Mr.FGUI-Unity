using FairyGUI;
using System;
using System.IO;
using UnityEngine;
/// <summary>
/// 基础工具类
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
    /// 根据类名实例化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="className">类名（可包含命名空间，例如G.ClassName）</param>
    /// <param name="param"> 构造函数参数</param>
    /// <returns></returns>
    public T CreateClassByName<T>(string className, object[] param = null)
    {
        Type t = Type.GetType(className);
        T instance = (T)Activator.CreateInstance(t, param);
        return instance;
    }

    /** 组件截图并保存图像到本地**/
    public static void SaveViewShotToLocal(GObject aObject, Action<string> cb = null, bool needBrower = true, string pngName = "newPng")
    {
        DisplayObject dObject = aObject.displayObject;
        dObject.EnterPaintingMode(1024, null);

        //纹理将在本帧渲染后才能更新，所以访问纹理的代码需要延迟到下一帧执行。
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

            if (needBrower)//自定义保存
            {
                FileUT.SavePngImageBrower((string path) =>
                {
                    SaveImage(path);
                });
            }
            else
            {
                SaveImage(pngName);
            }

            void SaveImage(string path)
            {
                string fullPath = path + ".png";//保存的图片文件数据完整地址
                FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write);
                fileStream.Write(vs, 0, vs.Length);
                fileStream.Dispose();
                fileStream.Close();

                //处理结束后结束绘画模式。id要和Enter方法的对应。
                dObject.LeavePaintingMode(1024);
                cb?.Invoke(fullPath);
                if (needBrower) MsgMgr.ShowMsg("保存成功");
            }
        });
    }

    //// 向量转角度
    //public static float vector_to_angle(Vector2 vector)
    //{
    //    // 将传入的向量归一化
    //    Vector2 dir = vector.normalized;
    //    // 计算出目标角度的弧度
    //    float radian = dir.signAngle(new Vector2(1, 0));
    //    // 把弧度计算成角度
    //    float angle = -radian_to_angle(radian);
    //    // 返回角度
    //    return angle;
    //}

    // 角度转向量   
    public static Vector2 angle_to_vector(float angle)
    {
        float radian = angle_to_radian(angle);
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        Vector2 vec = new Vector2(cos, sin).normalized;
        return vec;
    }

    // 角度转弧度
    public static float angle_to_radian(float angle)
    {
        float radian = Mathf.PI / 180 * angle;
        return radian;
    }

    // 弧度转角度
    public static float radian_to_angle(float radian) {
        float angle = (float)(180 / Math.PI * radian);
        return angle;
    }

}
