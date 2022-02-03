using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using a4.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using OxyPlot;
using DynamicData;

namespace a4.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static string ServerAddress
        {
            get { return Client.Address; }
            set
            {
                Client.Address = value;
                a4.Views.MainWindow.Current.changeImgChecked(STATE_CONNECTION.UNDEFINED);
            }
        }
        public static string ServerPort
        {
            get { return Client.Port; }
            set
            {
                Client.Port = value;
                a4.Views.MainWindow.Current.changeImgChecked(STATE_CONNECTION.UNDEFINED);
            }
        }
        public static string DataStr { get; set; }
        public static string OutputString { get; set; }
        public string SelectedFunction
        {
            get => SelectedFunc;
            set
            {
                SelectedFunc = value;
                if (!(value is null))
                    getFuncInfo(value);
            }
        }
        public List<NamedSeries> Points
        {
            set => a4.Views.MainWindow.Current.setPlotValues(value);
        }

        public MainWindowViewModel()
        {
            DataStr = "";
            TLog = "Start logging\n";

        }

        public void PrepareValues()
        {
            Points = new List<NamedSeries>(new NamedSeries[] {
                new NamedSeries("Series1", new Collection<Point>(new List<Point>(new Point[] { new Point(1, -1), new Point(2, 2), new Point(3, 1) }))),
                new NamedSeries("Series1", new Collection<Point>(new List<Point>(new Point[] { new Point(1, 1), new Point(2, 2.2), new Point(3, -2) })))
            });
        }

        public void oncSend()
        {
            List<NamedSeries> _Points = new List<NamedSeries>(); ;
            ResponseData rd;
            try
            {
                string file = Views.MainWindow.Current.Function;
                if (file == "")
                {
                    ResultText = "Function not chose";
                    return;
                }
                string res = SendAndReceive(new RequestData("eval", file, DataStr));
                rd = JsonSerializer.Deserialize<ResponseData>(res);
                res = "";
                res += rd.Result;
                if (rd.Error != "") // error
                {
                    res += "\n\nError:\n" + rd.Error + "\n";
                }
                ResultText = res;

                if (!(rd.Values is null))
                {
                    foreach (NamedSeries ns in rd.Values)
                    {
                        _Points.Add(ns);
                    }
                    Points = _Points;
                }

                Axe1Name = rd.Axe1Name;
                Axe2Name = rd.Axe2Name;

            }
            catch (Exception e)
            {
                String __R = e.StackTrace;
                ResultText = "Server Error. Response has wrong format.";
            }
        }

        public void oncCheck()
        {
            SendAndReceive(new RequestData("check", null, null));
            ResultText = "";
        }

        public void oncPrList()
        {
            string response = SendAndReceive(new RequestData("search", null, null));
            ResponseData responseData = JsonSerializer.Deserialize<ResponseData>(response);
            if (responseData.Error != "")
            {
                FuncInfo = String.Format("Error: {0}", responseData.Error);
            }
            else
            {
                try
                {
                    string[] funcs = JsonSerializer.Deserialize<string[]>(responseData.Result);
                    AvailableFunc = funcs;
                }
                catch (Exception e)
                {
                    AvailableFunc = null;
                }
            }
            //SelectedFunction = funcs[1];
            Views.MainWindow.Current.setSelectedFunc(0);
        }

        public void getFuncInfo(string func)
        {
            try
            {
                string info = SendAndReceive(new RequestData("info", func, null));
                ResponseData rd = JsonSerializer.Deserialize<ResponseData>(info);
                if (rd.Error != "") // error
                {
                    ResultText += "\n\nError:\n" + rd.Error + "\n";
                }
                else
                {
                    FuncInfo = rd.Result;
                }
            }
            catch (Exception e)
            {
                FuncInfo = "";
                AvailableFunc = new string[0] { };
                ResultText = "Server Error";
            }
        }

        public string SendAndReceive(RequestData msg)
        {
            string smsg = JsonSerializer.Serialize(msg);
            TLog += DateTime.Now.ToString("HH:mm:ss.fff") + " => " + smsg + "\n";
            string res = Client.Send(smsg);
            res = res.Remove(res.Length - 2);
            TLog += DateTime.Now.ToString("HH:mm:ss.fff") + " <= " + res + "\n";
            if (res == "::E") { a4.Views.MainWindow.Current.changeImgChecked(STATE_CONNECTION.DISCONNECTED); }
            else { a4.Views.MainWindow.Current.changeImgChecked(STATE_CONNECTION.CONNECTED); }
            return res;
        }
    }
}
