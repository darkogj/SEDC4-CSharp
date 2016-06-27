using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Truncon.Collections;
namespace DateTimeToReadable
{


    public class Program
    {
        static void Main()
        {
            DateTime date1 = new DateTime(2009, 8, 1, 0, 0, 0);
            DateTime date2 = new DateTime(2009, 8, 1, 5, 1, 5);
            var difference = Domain.ConvertDateTimeDifferenceToHumanReadableFormat(date1, date2);
            Console.WriteLine(difference);

        }

    }

    public class Domain
    {
        public static string ConvertDateTimeDifferenceToHumanReadableFormat(DateTime date1, DateTime date2)
        {
            var totalms = CalculateTimeDifferenceInms(date1, date2);
            var humanReadableDuration = CreateReadableFormatFrommsTimeDifference(totalms);
            return humanReadableDuration;

        }


        public static string CreateReadableFormatFrommsTimeDifference(double ms)
        {

            OrderedDictionary<string, int> durations = ConvertmsToDurations(ms);
            var normalizedDurations = SingularizeDurationNamesIfNeeded(durations);
            var readableString = ConvertDurationsToReadableString(normalizedDurations);

            return readableString;
        }

        public static double CalculateTimeDifferenceInms(DateTime firstDateTime, DateTime secondDateTime)
        {
            var earlierDateTime = (firstDateTime > secondDateTime) ? secondDateTime : firstDateTime; // FindEarlierDateTime
            var latterDateTime = (firstDateTime > secondDateTime) ? firstDateTime : secondDateTime; // FindLatterDateTime

            return CalculatemsTimeDifferenceBetween2DateTimes(earlierDateTime, latterDateTime);

        }


        public static string ConvertDurationsToReadableString(OrderedDictionary<string, int> durations)
        {
            var returnedString = new StringBuilder();
            returnedString.Append("Pred: ");

            foreach (var item in durations)
            {
                var durationName = item.Key;
                var duration = item.Value;

                returnedString.Append($"{duration} {durationName}, ");
            }

            returnedString.Length -= 2;
            return returnedString.ToString();

        }

        public static OrderedDictionary<string, int> SingularizeDurationNamesIfNeeded(OrderedDictionary<string, int> durations)
        {
            var singularNamesForDurations = new OrderedDictionary<string, string>()
            {
                { "Nedeli", "Nedela" },
                { "Denovi", "Den" },
                { "Casa", "Cas" },
                { "Minuti", "Minuta" },
                { "Sekundi", "Sekunda" },
            };
            var normalizedDurations = new OrderedDictionary<string, int>();

            foreach (var item in durations)
            {
                if (item.Value == 1)
                {
                    var singleDuration = singularNamesForDurations[item.Key];
                    normalizedDurations[singleDuration] = item.Value;
                }
                else
                {
                    normalizedDurations[item.Key] = item.Value;
                }
            }

            return normalizedDurations;
        }

        public static OrderedDictionary<string, int> ConvertmsToDurations(double ms)
        {
            OrderedDictionary<string, double> msByDuration = InitializeDurations();
            var calculatedmsForDurations = CalculatemsTimeForDurations(ms, msByDuration);
            var durationsThatHaveValues = RemoveZeroDurations(calculatedmsForDurations);

            return durationsThatHaveValues;
        }

        public static OrderedDictionary<string, int> RemoveZeroDurations(OrderedDictionary<string, int> durations)
        {
            var returnDictionary = new OrderedDictionary<string, int>();
            foreach (var item in durations)
            {
                if (item.Value != 0)
                {
                    returnDictionary[item.Key] = item.Value;
                }
            }
            return returnDictionary;
        }

        public static OrderedDictionary<string, int> CalculatemsTimeForDurations(double ms, OrderedDictionary<string, double> durations)
        {
            var resultDurations = new OrderedDictionary<string, int>();

            double remainderms = ms;
            foreach (var item in durations)
            {
                var durationName = item.Key;
                var durationInms = item.Value;
                var durationResult = Convert.ToInt32(Math.Floor(remainderms / durationInms));
                remainderms = ms % durationInms;
                resultDurations[durationName] = durationResult;
            }

            return resultDurations;
        }

        public static OrderedDictionary<string, double> InitializeDurations()
        {
            return new OrderedDictionary<string, double>()
            {
                {"Nedeli", 604800000 },
                { "Denovi", 86400000 },
                { "Casa", 3600000  },
                { "Minuti", 60000 },
                { "Sekundi", 1000 }
            };
        }



        public static double CalculatemsTimeDifferenceBetween2DateTimes(DateTime earlierDateTime, DateTime latterDateTime)
        {
            return (latterDateTime - earlierDateTime).TotalMilliseconds;
        }
    }


}
