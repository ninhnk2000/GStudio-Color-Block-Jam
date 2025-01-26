using System;

public interface ISaferioUIAnimation
{
    public void Show();
    public void Hide();
    public void Show(Action onCompletedAction);
    public void Hide(Action onCompletedAction);
}