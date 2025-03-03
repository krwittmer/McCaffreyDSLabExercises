using McCaffrey.DataScienceLab;

namespace McCaffreyLRL.Tests
{
    [TestClass]
    public class LRFunctionsTests
    {
        [TestMethod]
        public void RSquared_AllYValuesEqual_ThrowsException()
        {
            // Arrange
            double[][] data = new double[][]
            {
                new double[] { 1, 2, 3, 4 },
                new double[] { 1, 2, 3, 4 },
                new double[] { 1, 2, 3, 4 }
            };
            double[] coef = new double[] { 1, 2, 3, 4 };

            // Act & Assert
            Assert.ThrowsException<System.Exception>(() => LRFunctions.RSquared(data, coef));
        }

        [TestMethod]
        public void RSquared_CalculatesCorrectly()
        {
            // Arrange
            double[][] data = new double[][]
            {
                new double[] { 1, 2, 3, 4 },
                new double[] { 2, 3, 4, 5 },
                new double[] { 3, 4, 5, 6 }
            };
            double[] coef = new double[] { 1, 2, 3, 4 };

            // Act
            double result = LRFunctions.RSquared(data, coef);

            // Assert
            Assert.AreEqual(0.0, result, 0.0001);
        }

        [TestMethod]
        public void Income_CalculatesCorrectly()
        {
            // Arrange
            double x1 = 1;
            double x2 = 2;
            double x3 = 3;
            double[] coef = new double[] { 1, 2, 3, 4 };

            // Act
            double result = LRFunctions.Income(x1, x2, x3, coef);

            // Assert
            Assert.AreEqual(18.0, result, 0.0001);
        }

        [TestMethod]
        public void Design_AddsLeadingColumnOfOnes()
        {
            // Arrange
            double[][] data = new double[][]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 },
                new double[] { 7, 8, 9 }
            };

            // Act
            double[][] result = LRFunctions.Design(data);

            // Assert
            Assert.AreEqual(1.0, result[0][0], 0.0001);
            Assert.AreEqual(1.0, result[1][0], 0.0001);
            Assert.AreEqual(1.0, result[2][0], 0.0001);
        }

        [TestMethod]
        public void Design_AddsDataColumns()
        {
            // Arrange
            double[][] data = new double[][]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 },
                new double[] { 7, 8, 9 }
            };

            // Act
            double[][] result = LRFunctions.Design(data);

            // Assert
            Assert.AreEqual(2.0, result[0][1], 0.0001);
            Assert.AreEqual(3.0, result[0][2], 0.0001);
            Assert.AreEqual(5.0, result[1][1], 0.0001);
            Assert.AreEqual(6.0, result[1][2], 0.0001);
            Assert.AreEqual(8.0, result[2][1], 0.0001);
            Assert.AreEqual(9.0, result[2][2], 0.0001);
        }

        [TestMethod]
        public void Solve_CalculatesCoefficients()
        {
            // Arrange
            double[][] design = new double[][]
            {
                new double[] { 1, 2, 3 },
                new double[] { 4, 5, 6 },
                new double[] { 7, 8, 9 }
            };

            // Act
            double[] result = LRFunctions.Solve(design);

            // Assert
            Assert.AreEqual(1.0, result[0], 0.0001);
            Assert.AreEqual(2.0, result[1], 0.0001);
            Assert.AreEqual(3.0, result[2], 0.0001);
        }
    }
}
