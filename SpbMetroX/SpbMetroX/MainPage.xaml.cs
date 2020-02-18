using RuntimeComponent1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpbMetroX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, MyAppApi
    {
        public MainPage()
        {
            this.InitializeComponent();
            MyApi api = new MyApi(this);
            webView1.AddWebAllowedObject("API", api);
            webView1.Source = new Uri("ms-appx-web:///Assets/default.html");
            resetFields();
            clearButton.Click += async (o, i) =>
            {
                await webView1.InvokeScriptAsync("reset_selection", null);
                resetFields();
            };
            buildPopup();
            toField.Tapped += ToField_Tapped;
            fromField.Tapped += ToField_Tapped;
            Window.Current.SizeChanged += (a, b) => { centerPopup(); };
        }
        private async void buildPopup()
        {
            var l = Package.Current.InstalledLocation;
            var f = await l.GetFolderAsync("Assets");
            var fl = await f.GetFileAsync("spb.csv");
            Stream stream = await fl.OpenStreamForReadAsync();
            StreamReader sr = new StreamReader(stream);
            List<Station> ls = new List<Station>(13);
            while (!sr.EndOfStream)
            {
                string[] lines = sr.ReadLine().Split(';');
                if (lines[0] == "line")
                    continue;
                if (lines[0] == "intersect")
                    break;
                ls.Add(new Station(lines[0], lines[1]));
            }
            ls.Sort();
            foreach (Station st in ls)
            {
                stationsList.Items.Add(st);
            }
            stationsList.Tapped += async (a, b) =>
            {
                Station st = (Station)stationsList.SelectedItem;
                string str = direction == "toField" ? "to" : "from";
                await webView1.InvokeScriptAsync("select_station_field", new string[] { str, st.id });
                stationsPopup.IsOpen = false;
            };
        }
        private string direction;
        private void ToField_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)e.OriginalSource;
            direction = tb.Name;
            centerPopup();
            stationsPopup.IsOpen = true;
        }

        private void centerPopup()
        {
            stationsPopup.HorizontalOffset = (Window.Current.Bounds.Width - stationsList.Width) / 2;
            stationsPopup.VerticalOffset = 10;
            stationsList.Height = Window.Current.Bounds.Height - stationsPopup.VerticalOffset;
        }

        public void resetFields()
        {
            fromField.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            toField.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            timeField.Foreground = new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            fromField.Text = "Откуда";
            toField.Text = "Куда";
            timeField.Text = "Время";
        }

        public void setMyText(string fieldName, string val, string color)
        {
            TextBlock tb = fieldName == "toField" ? toField : fromField;
            tb.Text = val;
            if (color == "red")
                tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            else if (color == "green")
                tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 128, 0));
            else if (color == "blue")
                tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            else if (color == "orange")
                tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 165, 0));
            else if (color == "darkmagenta")
                tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 139, 0, 139));

        }

        public void setMyTime(string txt)
        {
            timeField.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            timeField.Text = txt;
        }
    }
}
