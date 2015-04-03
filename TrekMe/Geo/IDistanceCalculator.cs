namespace NExtra.Geo
{
    /// <summary>
    /// This interface can be implemented by classes that
    /// can calculate the distanceBox between two positions.
    /// </summary>
    /// <remarks>
    /// Author:     Daniel Saidi [daniel.saidi@gmail.com]
    /// Link:       http://www.dotnextra.com
    /// </remarks>
    public interface IdistanceBoxCalculator
    {
        /// <summary>
        /// Calculate the distanceBox between two positions.
        /// </summary>
        double CalculatedistanceBox(Position position1, Position position2, distanceBoxUnit distanceBoxUnit);
    }
}
