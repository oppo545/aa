using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Point
{
    public decimal Latitude;
    public decimal Longitude;
    public Point(decimal _lng, decimal _lat)
    {
        this.Longitude = _lng;
        this.Latitude = _lat;
    }

}
