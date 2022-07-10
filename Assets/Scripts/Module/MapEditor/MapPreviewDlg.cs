using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
/// <summary>
/// 地图预览
/// author：cyk
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
        curScale = GRoot.inst.height / renderTexture.height;
        _image = new Image();
        holder.SetNativeObject(_image);
        _image.texture = new NTexture(renderTexture);
        _image.blendMode = BlendMode.Off;
        UpdateHoldSizeXY();
        view.displayObject.onMouseWheel.Add(_onMouseWheel);
    }

    private float curScale;
    private float scaleDelta = 0.01f;
    private void _onMouseWheel(EventContext context)
    {
        InputEvent inputEvt = (InputEvent)context.data;
        if (inputEvt.mouseWheelDelta > 0)
        {
            if (Mathf.Floor(holder.width) <= GRoot.inst.width && Mathf.Floor(holder.height) <= GRoot.inst.height) return; ;//已全部可见
            curScale -= scaleDelta;
        }
        else
        {
            if (Mathf.Floor(holder.width) >= _renderTextureSize.x && Mathf.Floor(holder.height) >= _renderTextureSize.y) return;//已达到原大小
            curScale += scaleDelta;
        }

        UpdateHoldSizeXY();
    }

    private void UpdateHoldSizeXY()
    {
        holder.SetSize(_renderTextureSize.x * curScale, _renderTextureSize.y * curScale);
        holder.x = holder.width < GRoot.inst.width ? (GRoot.inst.width - holder.width) / 2 : 0;
        holder.y = holder.height < GRoot.inst.height ? (GRoot.inst.height - holder.height) / 2 : 0;
        Debug.Log("holder.width: " + holder.width + ", holder.height: " + holder.height + ", curScale: " + curScale);
    }

    protected override void OnExit()
    {
        _image.Dispose();
    }
}
