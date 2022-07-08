using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;

/// <summary>
/// 本地文件IO工具类
/// author：cyk
/// </summary>
public class FileUT
{
    //窗口置顶
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    /// <summary>
    /// 选择文件夹
    /// </summary>
    public static void OpenDirectoryBrower(Action<string> cb = null)
    {
        OpenDialogDir openDir = new OpenDialogDir();
        openDir.pszDisplayName = new string(new char[2000]);
        openDir.lpszTitle = "文件夹选择";
        openDir.ulFlags = 1;// BIF_NEWDIALOGSTYLE | BIF_EDITBOX;
        openDir.hwndOwner = GetForegroundWindow();

        IntPtr pidl = DllOpenFileDialog.SHBrowseForFolder(openDir);
        char[] path = new char[2000];
        for (int i = 0; i < 2000; i++)
            path[i] = '\0';
        if (DllOpenFileDialog.SHGetPathFromIDList(pidl, path))
        {
            string str = new string(path);
            string DirPath = str.Substring(0, str.IndexOf('\0'));
            cb?.Invoke(DirPath);

        }
    }
    /// <summary>
    /// 选择文件导入
    /// </summary>
    public static void OpenFileBrower(Action<string> cb = null)
    {

        DealFile(true, "选择要导入的地图json文件：", cb);
    }

    /// <summary>
    /// 保存json文件
    /// </summary>
    public static void SaveFileBrower(Action<string> cb = null)
    {
        DealFile(false, "导出当前地图json数据：", cb);
    }


    /// <summary>
    /// 保存png图片文件
    /// </summary>
    public static void SavePngImageBrower(Action<string> cb = null)
    {
        DealFile(false, "保存图片：", cb, ".png");
    }
    //处理文件
    private static void DealFile(bool isOpen, string title, Action<string> cb = null,string extension = ".json")
    {
        OpenDialogFile ofn = new OpenDialogFile();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = extension == ".json" ?  "格式(.json)\0*.json;" : "图片格式(.png;)\0*.png;";//"所有图片格式(.bmp;.jpeg;.jpg;.png;)\0*.bmp;*.jpeg;*.jpg;*.png;";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = Application.dataPath;//默认路径

        ofn.title = title;

        //ofn.defExt = "JPG";//显示文件的类型

        //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        ofn.dlgOwner = GetForegroundWindow(); //这一步将文件选择窗口置顶。

        if (isOpen)
        {//打开文件
            if (DllOpenFileDialog.GetOpenFileName(ofn))
            {
                cb?.Invoke(ofn.file);
            }
        }
        else
        {//保存文件
            if (DllOpenFileDialog.GetSaveFileName(ofn))
            {
                cb?.Invoke(ofn.file);
            }
        }

    }

}

