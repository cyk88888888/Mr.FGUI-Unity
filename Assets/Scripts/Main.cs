using UnityEngine;
using FairyGUI;
using UnityEngine.SceneManagement;
/// <summary>
/// 主入口文件
/// author：cyk
/// </summary>
public class Main : MonoBehaviour
{
    private Scene curScene;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        UIConfig.defaultFont = "Microsoft YaHei";
        ModuleMgr.inst.Init();
        curScene = SceneManager.GetActiveScene();
        BaseUT.Inst.scaleMode = curScene.name == "Main" ?  new(640, 1280, 1030, 1280) : new(1450, 810, 810, 810);
    }
    // Start is called before the first frame update
    void Start()
    {
        if(curScene.name == "Main")
        {
            SceneMgr.inst.Run("LoadingScene");
        }
        else
        {
            SceneMgr.inst.Run("MapEditorScene");
        }
           
    }
 
}
