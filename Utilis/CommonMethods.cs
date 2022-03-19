using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NAIKI.Utilis
{
    public class CommonMethods
    {
        public double CalculateMileRadius(string source, string destination)
        {
            try
            {
                var radiusOfEarth = 3958.8;
                var latLongSource = source.Split(',');
                var latLongDestination = destination.Split(',');
                var latSource = Convert.ToDouble(latLongSource[0].Trim());
                var longSource = Convert.ToDouble(latLongSource[1].Trim());
                var latDest = Convert.ToDouble(latLongDestination[0].Trim());
                var longDest= Convert.ToDouble(latLongDestination[1].Trim());
                latSource = latSource * (Math.PI / 180); //degree to radians
                latDest = latDest * (Math.PI / 180);
                var latDifference = latDest - latSource;
                var longDifference = ((longDest - longSource) * (Math.PI / 180));

                double distance = 2 * radiusOfEarth * Math.Asin(Math.Sqrt(Math.Sin(latDifference / 2) * Math.Sin(latDifference / 2)
                    + Math.Cos(latSource) * Math.Cos(latDest) * Math.Sin(longDifference / 2) * Math.Sin(longDifference / 2)));


                return distance;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public string ToShortDateTime(DateTime date)
        {
            return date.ToString(ConfigurationManager.AppSettings["ShortDateFormat"]);
        }
    }
}