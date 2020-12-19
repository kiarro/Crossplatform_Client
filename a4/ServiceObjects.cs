using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace a4
{
    public enum STATE_CONNECTION
    {
        CONNECTED = 1,
        DISCONNECTED = 2,
        UNDEFINED = 3
    }

    public struct RequestData
    {
        public string Command { get; set; }
        public string Proc { get; set; }
        public string Data { get; set; }
        public RequestData(string command, string proc, string data)
        {
            Command = command;
            Proc = proc;
            Data = data;
        }
    }
    public struct ResponseData
    {
        public string Error { get; set; }
        public string Result { get; set; }
        public string Axe1Name { get; set; }
        public string Axe2Name { get; set; }
        public NamedSeries[] Values { get; set; }
        public ResponseData(string error, string result, string axe1Name, string axe2Name, NamedSeries[] values)
        {
            Error = error; Result = result;
            Values = values;
            Axe1Name = axe1Name; Axe2Name = axe2Name;
        }
    }
    public struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            this.X = x; this.Y = y;
        }
    }

    public struct NamedSeries
    {
        public Collection<Point> Series { get; set; }
        public string Name { get; set; }
        public NamedSeries(string name, Collection<Point> series)
        {
            Name = name; Series = series;
        }
    }
}
