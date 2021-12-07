using WebdriverFramework.Framework.Util;
using WebdriverFramework.Framework.WebDriver;

namespace TestEnviroment.BrowserStart
{
    public class BrowserStart
    {
        private readonly Checker checker;
        private readonly Logger _logger;

        public BrowserStart()
        {
            checker = new Checker(Logger.Instance, Browser.Instance);
            _logger = Logger.Instance;
        }       

        public void StartBrowserOnStartPage()
        {
            Browser.Instance.Quit();
            Browser.Instance.InitDriver();
            Browser.Instance.NavigateToStartPage();
            Browser.Instance.WindowMaximise();
        }

        //This Method is used for Cars Models task
        public void NavigateToMainPage()
        {            
            Browser.Instance.NavigateToStartPage();            
        }
    }
}