using Loxodon.Framework.Extension;
using UIGen.Demo;

namespace Demo.Script.UI
{
    public class RetryWindowVM:ViewModelCommon
    {
        public ObservableVariable<string> WindowState = new ObservableVariable<string>();

        public void SetRetryWindow(bool win)
        {
            WindowState.Value = win ? RetryWindow_Pages.windowState_win : RetryWindow_Pages.windowState_lose;
        }
        
        public void ReturnMenu()
        {
            
        }

        public void Retry()
        {
            
        }
    }
}