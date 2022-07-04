using UnityEngine;
using FairyGUI;

public class TurnPageComp : UIComp
{
    private FairyBook _book;
    private GSlider _slider;

    protected override void Ctor()
    {
        
    }

    protected override void OnEnter()
    {
       
    }

    protected override void OnFirstEnter()
    {
        _book = (FairyBook)view.GetChild("book");
        _book.SetSoftShadowResource("ui://TurnPage/shadow_soft");
        _book.pageRenderer = RenderPage;
        _book.pageCount = 20;
        _book.currentPage = 0;
        _book.ShowCover(FairyBook.CoverType.Front, false);
        _book.onTurnComplete.Add(OnTurnComplete);

        GearBase.disableAllTweenEffect = true;
        view.GetController("bookPos").selectedIndex = 1;
        GearBase.disableAllTweenEffect = false;

        view.GetChild("btnNext").onClick.Add(() =>
        {
            _book.TurnNext();
        });
        view.GetChild("btnPrev").onClick.Add(() =>
        {
            _book.TurnPrevious();
        });

        _slider = view.GetChild("pageSlide").asSlider;
        _slider.max = _book.pageCount - 1;
        _slider.value = 0;
        _slider.onGripTouchEnd.Add(() =>
        {
            _book.TurnTo((int)_slider.value);
        });
    }

    void OnTurnComplete()
    {
        _slider.value = _book.currentPage;

        if (_book.isCoverShowing(FairyBook.CoverType.Front))
            view.GetController("bookPos").selectedIndex = 1;
        else if (_book.isCoverShowing(FairyBook.CoverType.Back))
            view.GetController("bookPos").selectedIndex = 2;
        else
            view.GetController("bookPos").selectedIndex = 0;
    }

    void RenderPage(int index, GComponent page)
    {
        ((BookPage)page).render(index);
    }


}