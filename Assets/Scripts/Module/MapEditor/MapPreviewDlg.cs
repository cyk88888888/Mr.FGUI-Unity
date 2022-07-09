using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// µÿÕº‘§¿¿
/// author£∫cyk
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
        holder = view.GetChild("previewComp").asCom.GetChild("holder").asGraph;
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
        float delta = 0.05f;
        if (inputEvt.mouseWheelDelta > 0)
        {
            holder.SetScale(holder.scaleX - delta, holder.scaleY - delta);
        }
        else
        {
            holder.SetScale(holder.scaleX + delta, holder.scaleY + delta);
        }
        holder.SetSize(_renderTextureSize.x * holder.scaleX, _renderTextureSize.y * holder.scaleX);
    }

    protected override void OnExit()
    {
        _image.Dispose();
    }
}
