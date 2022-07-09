using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;

/// <summary>
/// �����ļ�IO������
/// author��cyk
/// </summary>
public class FileUT
{
    //�����ö�
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    /// <summary>
    /// ѡ���ļ���
    /// </summary>
    public static void OpenDirectoryBrower(Action<string> cb = null)
    {
        OpenDialogDir openDir = new OpenDialogDir();
        openDir.pszDisplayName = new string(new char[2000]);
        openDir.lpszTitle = "�ļ���ѡ��";
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
    /// ѡ���ļ�����
    /// </summary>
    public static void OpenFileBrower(Action<string> cb = null)
    {

        DealFile(true, "ѡ��Ҫ����ĵ�ͼjson�ļ���", cb);
    }

    /// <summary>
    /// ����json�ļ�
    /// </summary>
    public static void SaveFileBrower(Action<string> cb = null)
    {
        DealFile(false, "������ǰ��ͼjson���ݣ�", cb);
    }


    /// <summary>
    /// ����pngͼƬ�ļ�
    /// </summary>
    public static void SavePngImageBrower(Action<string> cb = null)
    {
        DealFile(false, "����ͼƬ��", cb, ".png");
    }
    //�����ļ�
    private static void DealFile(bool isOpen, string title, Action<string> cb = null,string extension = ".json")
    {
        OpenDialogFile ofn = new OpenDialogFile();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = extension == ".json" ?  "��ʽ(.json)\0*.json;" : "ͼƬ��ʽ(.png;)\0*.png;";//"����ͼƬ��ʽ(.bmp;.jpeg;.jpg;.png;)\0*.bmp;*.jpeg;*.jpg;*.png;";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = Application.dataPath;//Ĭ��·��

        ofn.title = title;

        //ofn.defExt = "JPG";//��ʾ�ļ�������

        //ע�� һ����Ŀ��һ��Ҫȫѡ ����0x00000008�Ҫȱ��
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        ofn.dlgOwner = GetForegroundWindow(); //��һ�����ļ�ѡ�񴰿��ö���

        if (isOpen)
        {//���ļ�
            if (DllOpenFileDialog.GetOpenFileName(ofn))
            {
                cb?.Invoke(ofn.file);
            }
        }
        else
        {//�����ļ�
            if (DllOpenFileDialog.GetSaveFileName(ofn))
            {
                cb?.Invoke(ofn.file);
            }
        }

    }

}

