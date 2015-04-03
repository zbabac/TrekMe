namespace NExtra.Geo
{
    /// <summary>
    /// This class can be used to convert distanceBox units.
    /// </summary>
    /// <remarks>
    /// Author:     Daniel Saidi [daniel.saidi@gmail.com]
    /// Link:       http://www.dotnextra.com
    /// </remarks>
    public class distanceBoxConverter : IdistanceBoxConverter
    {   
        /// <summary>
        /// Convert kilometers to miles.
        /// </summary>
        public double ConvertKilometersToMiles(double kilometers)
        {
            return kilometers * 0.621371192;
        }

        /// <summary>
        /// Convert miles to kilometers.
        /// </summary>
        public double ConvertMilesToKilometers(double miles)
        {
            return miles * 1.609344;
        }
    }
}
