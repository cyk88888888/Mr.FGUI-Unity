using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// ��ͼԤ��
/// author��cyk
/// </summary>
public class MapPreviewDlg : UIDlg
{
    protected override string PkgName
    {
        get { return "MapEditor"; }
    }

    protected override string CompName
    {
        get { return "MapPreviewDlg"; }
    }

    protected override void Ctor()
    {
        needOpenAnimation = false;
    }
    private GGraph holder;
    private Image _image;
    private Vector2 _renderTextureSize;
    protected override void OnEnter()
    {
        GButton btn_close = view.GetChild("btn_close").asButton;
        btn_close.onClick.Add(() => { Close(); });
        GComponent previewComp = view.GetChild("previewComp").asCom;
        holder = previewComp.GetChild("holder").asGraph;
        
        //RenderTexture renderTexture = (RenderTexture)__data;
        Texture2D renderTexture = (Texture2D)__data;
        _renderTextureSize = new(renderTexture.width, renderTexture.height);
        holder.SetSize(renderTexture.width, renderTexture.height);
        _image = new Image();
        holder.SetNativeObject(_image);
        _image.texture = new NTexture(renderTexture);
        _image.blendMode = BlendMode.Off;
        
        view.displayObject.onMouseWheel.Add(_onMouseWheel);
    }

    private void _onMouseWheel(EventContext context)
    {
        InputEvent inputEvt = (InputEvent)context.data;
        float scaleDelta = 0.05f;
        if (inputEvt.mouseWheelDelta > 0)
        {
            holder.SetScale(holder.scaleX - scaleDelta, holder.scaleY - scaleDelta); 
        }
        else
        {
            holder.SetScale(holder.scaleX + scaleDelta, holder.scaleY + scaleDelta);
        }
        holder.SetSize(_renderTextureSize.x * holder.scaleX, _renderTextureSize.y * holder.scaleX);
        Debug.Log("holder.width: " + holder.width + ", holder.height: " + holder.height);
        holder.x = holder.width < GRoot.inst.width ? (GRoot.inst.width - holder.width) / 2 : 0;
        holder.y = holder.height < GRoot.inst.height ? (GRoot.inst.height - holder.height) / 2 : 0;
    }

    protected override void OnExit()
    {
        _image.Dispose();
    }
}
