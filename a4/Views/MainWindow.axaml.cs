using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using OxyPlot.Avalonia;
using System;
//using OxyPlot.Series;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace a4.Views
{
    public class MainWindow : Window
    {
        Button imgChecked;
        ComboBox funcList;
        Plot plot;

        public static MainWindow Current { get; private set; }
        public string Function { get 
            {
                try {
                    return funcList.SelectedItem.ToString();
                } catch (Exception e)
                {
                    return "";
                }
                
            } 
        }

        public MainWindow()
        {
            Current = this;
            

            ViewModels.MainWindowViewModel ViewModel = new ViewModels.MainWindowViewModel();
            DataContext = ViewModel;

            InitializeComponent();

            imgChecked = this.FindControl<Button>("imgChecked");
            funcList = this.FindControl<ComboBox>("funcList");
            plot = this.FindControl<Plot>("plot");
#if DEBUG
            this.AttachDevTools();
#endif
            ViewModel.PrepareValues();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void changeImgChecked(STATE_CONNECTION i)
        {
            ImageBrush image;
            switch (i){
                case STATE_CONNECTION.CONNECTED:
                    image = (ImageBrush)this.Resources["ok"];
                    imgChecked.Background = image;
                    break;
                case STATE_CONNECTION.DISCONNECTED:
                    image = (ImageBrush)this.Resources["not_ok"];
                    imgChecked.Background = image;
                    break;
                case STATE_CONNECTION.UNDEFINED:
                    image = (ImageBrush)this.Resources["undef"];
                    imgChecked.Background = image;
                    break;
            }
        }

        public void setSelectedFunc(int num)
        {
            funcList.SelectedIndex = 0;
        }

        public void setPlotValues(List<NamedSeries> points)
        {
            plot.Series.Clear();
            foreach(NamedSeries values in points)
            {
                var series = new LineSeries { Title = values.Name, Items = values.Series, DataFieldX = "X", DataFieldY = "Y", StrokeThickness = 2 };
                plot.Series.Add(series);
            }
            plot.InvalidatePlot(true);
            
        }

    }
}
