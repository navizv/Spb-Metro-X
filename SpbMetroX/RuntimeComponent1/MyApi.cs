using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Core;

namespace RuntimeComponent1
{
    [AllowForWeb, ComVisible(true)]
    [MarshalingBehavior(MarshalingType.Agile)]
    public sealed class MyApi
    {
        private MyAppApi mainPage;
        CoreDispatcher disp;

        public MyApi(MyAppApi mainPage)
        {
            this.mainPage = mainPage;
            disp = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        public async void setMyText(string fieldName, string val, string color)
        {
            await disp.RunAsync(CoreDispatcherPriority.Normal,
                () => mainPage.setMyText(fieldName, val, color));
        }

        public async void setMyTime(string txt)
        {
            await disp.RunAsync(CoreDispatcherPriority.Normal,
                () => mainPage.setMyTime(txt));
        }
    }

    public interface MyAppApi
    {
        void setMyText(string fieldName, string val, string color);
        void setMyTime(string txt);
    }
}
