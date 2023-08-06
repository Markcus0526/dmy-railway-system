using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLSite.Models.Library
{
    public class LatLonDistance
    {
        private const double EARTH_RADIUS = 6378.137;
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double distance(double lng1, double lat1, double lng2, double lat2, char unit)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            
            return Math.Round(s * 1000, 1);
        }

//         public static double distance(double Long1,
//                       double Lat1, double Long2, double Lat2, char unit)
//         {
//             /*
//                 The Haversine formula according to Dr. Math.
//                 http://mathforum.org/library/drmath/view/51879.html
//                 
//                 dlon = lon2 - lon1
//                 dlat = lat2 - lat1
//                 a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
//                 c = 2 * atan2(sqrt(a), sqrt(1-a)) 
//                 d = R * c
//                 
//                 Where
//                     * dlon is the change in longitude
//                     * dlat is the change in latitude
//                     * c is the great circle distance in Radians.
//                     * R is the radius of a spherical Earth.
//                     * The locations of the two points in 
//                         spherical coordinates (longitude and 
//                         latitude) are lon1,lat1 and lon2, lat2.
//             */
//             double dDistance = Double.MinValue;
//             double dLat1InRad = Lat1 * (Math.PI / 180.0);
//             double dLong1InRad = Long1 * (Math.PI / 180.0);
//             double dLat2InRad = Lat2 * (Math.PI / 180.0);
//             double dLong2InRad = Long2 * (Math.PI / 180.0);
// 
//             double dLongitude = dLong2InRad - dLong1InRad;
//             double dLatitude = dLat2InRad - dLat1InRad;
// 
//             // Intermediate result a.
//             double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
//                        Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
//                        Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);
// 
//             // Intermediate result c (great circle distance in Radians).
//             double c = 2.0 * Math.Asin(Math.Sqrt(a));
// 
//             // Distance.
//             // const Double kEarthRadiusMiles = 3956.0;
//             const Double kEarthRadiusKms = 6376.5;
//             dDistance = kEarthRadiusKms * c;
// 
//             return dDistance;
//         }

//         public static double distance(double lat1, double lon1, double lat2, double lon2, char unit)
//         {
//             double theta = lon1 - lon2;
//             double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
//             dist = Math.Acos(dist);
//             dist = rad2deg(dist);
//             dist = dist * 60 * 1.1515;
//             if (unit == 'K')
//             {
//                 dist = dist * 1.609344;
//             }
//             else if (unit == 'N')
//             {
//                 dist = dist * 0.8684;
//             }
//             //return (Math.Round(dist, 1));
//             return dist;
//         }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        public static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        public static double TestMeterDistance()
        {
            double rst = distance(32.9697, -96.80322, 29.46786, -98.53506, 'M');

            return rst;
        }
    }
}