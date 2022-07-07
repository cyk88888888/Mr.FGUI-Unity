using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// ��ͼjson���ݽṹ
/// author��cyk
/// </summary>
public class MapJsonInfo
{
    public int mapWidth;//��ͼ��
    public int mapHeight;//��ͼ��
    public int gridSize;//���Ӵ�С
    public List<List<int>> walkList;//���ӿ�����״̬�б�1�����ߣ�0�������ߣ�
    public List<int> blockList;//�����ϰ����б�1�ϰ��
    public List<int> blockVertList;//�����ϰ��ﶥ���б�1�ϰ��
    public MapJsonInfo()
    {
        walkList = new List<List<int>>();
        blockList = new List<int>();
        blockVertList = new List<int>();
    }
}

