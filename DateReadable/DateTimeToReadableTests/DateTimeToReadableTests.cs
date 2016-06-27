using System;
using System.Collections.Generic;
using System.Linq;
using DateTimeToReadable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Truncon.Collections;

namespace DateTimeToReadableTests
{
    [TestClass]
    public class DateTimeToReadableTests
    {
        [TestMethod]
        public void IsItConvertingToHumanReadableFormat()
        {
            DateTime date1 = new DateTime(2009, 8, 1, 0, 0, 0);
            DateTime date2 = new DateTime(2009, 8, 1, 5, 1, 5);
            var difference = Domain.ConvertDateTimeDifferenceToHumanReadableFormat(date1, date2);
            Assert.AreEqual("Pred: 5 Casa, 1 Minuta, 5 Sekundi", difference);
        }

        [TestMethod]
        public void LaterDateTimeComesAsFirstArgument()
        {
            // Arrange
            DateTime date1 = new DateTime(2009, 8, 1, 0, 0, 0);
            DateTime date2 = new DateTime(2009, 8, 1, 0, 1, 0);
            // Act
            var msDifference = Domain.CalculateTimeDifferenceInms(date2, date1);
            // Assert
            Assert.AreEqual(msDifference, TimeSpan.FromMinutes(1).TotalMilliseconds);
            
        }

        [TestMethod]
        public void TimeDifferenceIsCalculatedCorrectly()
        {

            // Arrange
            DateTime date1 = new DateTime(2009, 8, 1, 0, 0, 0);
            DateTime date2 = new DateTime(2009, 8, 1, 1, 0, 0);
            // Act
            var msDifference = Domain.CalculateTimeDifferenceInms(date2, date1);
            // Assert
            Assert.AreEqual(msDifference, TimeSpan.FromHours(1).TotalMilliseconds);
        }

        [TestMethod]
        public void DurationsAreCorrectlyInitialized()
        {
            // Arrange
            var durations = new List<string> {"Nedeli", "Denovi", "Casa", "Minuti", "Sekundi" };
            var durationsFromProgram = Domain.InitializeDurations().Keys.ToList();

            // Assert
            Assert.IsTrue(durations.SequenceEqual(durationsFromProgram));
        }

        [TestMethod]
        public void CalculatesCorrectmsTimeForDurations()
        {
            // Arrange
            var msTime = 10000;
            var durations = Domain.InitializeDurations();
            // Act
            var resultDurations = Domain.CalculatemsTimeForDurations(msTime, durations);
            var resultTimes = resultDurations.Values.ToList();
            // Assert
            Assert.IsTrue(resultTimes.SequenceEqual(new List<int> {0, 0, 0, 0, 10}));
        }

        [TestMethod]
        public void CheckIfZeroDurationsAreRemoved()
        {
            // Arrange
            var durations = new OrderedDictionary<string, int>()
            {
                { "Casa", 1 },
                { "Minuti", 0 }
            };
            // Act
            var withoutZeroDurations = Domain.RemoveZeroDurations(durations);
            var durationNames = withoutZeroDurations.Keys;
            // Assert
            Assert.IsTrue(new List<string> { "Casa" }.SequenceEqual(durationNames));
        }

        [TestMethod]
        public void SingularizesDurationNames()
        {
            // Arrange
            var durations = new OrderedDictionary<string, int>()
            {
                { "Casa", 1 },
                { "Minuti", 43 }
            };
            // Act
            var withoutZeroDurations = Domain.SingularizeDurationNamesIfNeeded(durations);
            var durationNames = withoutZeroDurations.Keys;
            // Assert
            Assert.IsTrue(new List<string> { "Cas", "Minuti" }.SequenceEqual(durationNames));
        }

        [TestMethod]
        public void ConvertsDurationsToReadableString()
        {
            // Arrange
            var durations = new OrderedDictionary<string, int>()
            {
                { "Cas", 1 },
                { "Minuti", 43 },
            };
            // Act
            var result = Domain.ConvertDurationsToReadableString(durations);
            // Assert
            Assert.AreEqual("Pred: 1 Cas, 43 Minuti", result);
        }

    }
}
