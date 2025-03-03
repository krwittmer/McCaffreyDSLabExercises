using McCaffrey.DataScienceLab;

namespace McCaffreyLRL.Tests
{
    [TestClass]
    public class TestBaseFunctions
    {
        [TestMethod]
        public void TestRSquared_WithValidData()
        {
            // Arrange
            double[][] data = new double[][]
            {
                new double[] { 1, 3 },
                new double[] { 2, 5 },
                new double[] { 3, 7 },
                new double[] { 4, 9 }
            };

            double[] coef = new double[] { 1, 2 }; // y = 1 + 2x
            double expectedRSquared = 1.0;
            double tolerance = 1e-6;

            // Act
            double actualRSquared = LRFunctions.RSquared(data, coef);

            // Assert
            Assert.AreEqual(expectedRSquared, actualRSquared, tolerance, "RSquared calculation is incorrect.");
        }

        [TestMethod]
        public void TestRSquared_WithAllYValuesEqual()
        {
            // Arrange
            double[][] data = new double[][]
            {
                new double[] { 1, 5 },
                new double[] { 2, 5 },
                new double[] { 3, 5 },
                new double[] { 4, 5 }
            };

            double[] coef = new double[] { 5, 0 }; // y = 5

            // Act & Assert
            Assert.ThrowsException<Exception>(() => LRFunctions.RSquared(data, coef), "RSquared should throw an exception when all y values are equal.");
        }
    }
}