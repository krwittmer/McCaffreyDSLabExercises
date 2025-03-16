using McCaffreyDSLibrary;

namespace McCaffreyLabsConsoleApp
{
    internal class PoissonRegressionProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nPoisson regression using C# ");

            // 1. load data from file to memory
            Console.WriteLine("\nLoading train and test data ");
            string trainFile =
              "..\\..\\..\\..\\LabData\\train_poisson_200.txt";
            double[][] trainX =
              PoissonUtils.MatLoad(trainFile,
              new int[] { 0, 1, 2, 3 }, ',', "#");
            double[] trainY =
              PoissonUtils.MatToVec(PoissonUtils.MatLoad(trainFile,
              new int[] { 4 }, ',', "#"));

            string testFile =
              "..\\..\\..\\..\\LabData\\test_poisson_40.txt";
            double[][] testX =
              PoissonUtils.MatLoad(testFile,
              new int[] { 0, 1, 2, 3 }, ',', "#");
            double[] testY =
              PoissonUtils.MatToVec(PoissonUtils.MatLoad(testFile,
              new int[] { 4 }, ',', "#"));
            Console.WriteLine("Done ");

            Console.WriteLine("\nFirst three train X inputs: ");
            for (int i = 0; i < 3; ++i)
                PoissonUtils.VecShow(trainX[i], 4, 9, true);

            Console.WriteLine("\nFirst three train y targets: ");
            for (int i = 0; i < 3; ++i)
                Console.WriteLine(trainY[i].ToString("F0"));

            // 2. create model
            Console.WriteLine("\nCreating and training" +
              " Poisson model");
            double alpha = 1.0e-5;  // weight decay
            PoissonRegressor model = new PoissonRegressor(alpha);

            // 3. train model
            double lrnRate = 0.001;
            int maxEpochs = 150;
            Console.WriteLine("\nlearn rate = " +
              lrnRate.ToString("F4"));
            Console.WriteLine("maxEpochs = " + maxEpochs);
            Console.WriteLine("weight decay alpha = " +
              alpha.ToString("F6"));
            Console.WriteLine("Start ");
            model.Train(trainX, trainY, lrnRate, maxEpochs);
            Console.WriteLine("Done");

            Console.WriteLine("\nModel constant and " +
              " coefficients: ");
            Console.WriteLine(model.intercept.
              ToString("F4").PadLeft(9));
            PoissonUtils.VecShow(model.coeffs, 4, 9, true);

            // 4. evaluate model
            Console.WriteLine("\nComputing model accuracy ");
            double accTrain =
              model.Accuracy(trainX, trainY);
            Console.WriteLine("\nAccuracy on train = "
              + accTrain.ToString("F4"));

            double accTest =
              model.Accuracy(testX, testY);
            Console.WriteLine("Accuracy on test = " +
              accTest.ToString("F4"));

            // 5. use model
            double[] x = trainX[0];
            Console.WriteLine("\nPredicting for x = ");
            PoissonUtils.VecShow(x, 4, 9, true);
            double y = model.Predict(x);
            Console.WriteLine("\ny = " + y.ToString("F4"));
            int yInt = (int)Math.Round(y);
            Console.WriteLine("y = " + yInt);

            Console.WriteLine("\nEnd demo ");
            Console.ReadLine();
        } // Main

    } // Program
}
