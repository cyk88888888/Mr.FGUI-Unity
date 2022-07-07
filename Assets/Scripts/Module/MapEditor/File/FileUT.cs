using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.IO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    #region Config Field
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
    #endregion


    #region Win32API WRAP
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    static extern bool GetOpenFileName([In, Out] LocalDialog dialog);  //����������Ʊ���ΪGetOpenFileName
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    static extern bool GetSaveFileName([In, Out] LocalDialog dialog);  //����������Ʊ���ΪGetSaveFileName
    #endregion
}

public class LocalDialog
{
    //����ָ��ϵͳ����       ���ļ��Ի���
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOFN([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }

    //����ָ��ϵͳ����        ���Ϊ�Ի���
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
    public static bool GetSFN([In, Out] OpenFileName ofn)
    {
        return GetSaveFileName(ofn);
    }

    //�����ö�
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();
}

public class FileUT
{
    public static void OpenFileDownLoadFunction()
    {
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);

        ofn.filter = "��ʽ(.json)\0*.json;";//"����ͼƬ��ʽ(.bmp;.jpeg;.jpg;.png;)\0*.bmp;*.jpeg;*.jpg;*.png;";

        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;

        ofn.initialDir = Application.dataPath;//Ĭ��·��

        ofn.title = "ѡ��Ҫ�����json�ļ���";

        //ofn.defExt = "JPG";//��ʾ�ļ�������

        //ע�� һ����Ŀ��һ��Ҫȫѡ ����0x00000008�Ҫȱ��
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        ofn.dlgOwner = LocalDialog.GetForegroundWindow(); //��һ�����ļ�ѡ�񴰿��ö���

        if (LocalDialog.GetOpenFileName(ofn))
        {
            string json = File.ReadAllText(ofn.file);
            Debug.Log(json);
        }

    }

}

