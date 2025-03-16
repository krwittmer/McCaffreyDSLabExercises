using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McCaffreyDSLibrary
{
    public class PoissonRegressor(double alpha)
    {
        public Random rnd = new Random(1);
        public double intercept = 0.0;
        public double[] coeffs;  // allocated in Train()
        public double alpha = alpha;  // wt decay 

        public double Accuracy(double[][] dataX, double[] dataY)
        {
            int numCorrect = 0; int numWrong = 0;
            int n = dataX.Length;
            for (int i = 0; i < n; ++i)
            {
                double[] x = dataX[i];
                int yActual = (int)dataY[i];
                int yPred = (int)Math.Round(this.Predict(x));

                if (yActual == yPred)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (numCorrect * 1.0) / n;
        }

        public double RootMSE(double[][] dataX, double[] dataY)
        {
            double sum = 0.0;
            int n = dataX.Length;
            for (int i = 0; i < n; ++i)
            {
                double[] x = dataX[i];
                double yActual = dataY[i];
                double yPred = this.Predict(x);

                sum += (yActual - yPred) * (yActual - yPred);
            }
            return Math.Sqrt(sum / n);
        }

        private void Shuffle(int[] sequence)
        {
            // Fisher-Yates
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = this.rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
                //sequence[i] = i; // for testing
            }
        } // Shuffle

        public void Train(double[][] trainX, double[] trainY, double lrnRate, int maxEpochs)
        {
            // loop epoch
            //  shuffle
            //  loop each item
            //    compute predicted
            //    update each coefficient

            int freq = maxEpochs / 5;  // when to show progress

            this.coeffs = new double[trainX[0].Length];
            double lo = -0.10; double hi = 0.10;
            for (int i = 0; i < this.coeffs.Length; ++i)
                coeffs[i] = (hi - lo) * this.rnd.NextDouble() + lo;

            int[] indices = new int[trainX.Length];
            for (int i = 0; i < indices.Length; ++i)
                indices[i] = i;

            for (int epoch = 0; epoch < maxEpochs; ++epoch)
            {
                Shuffle(indices);
                for (int i = 0; i < trainX.Length; ++i)
                {
                    int idx = indices[i];
                    double[] x = trainX[idx];
                    double predY = this.Predict(x);
                    double actualY = trainY[idx];

                    // update intercept and coefficients
                    this.intercept -= lrnRate *
                      (predY - actualY) * 1.0;
                    for (int j = 0; j < this.coeffs.Length; ++j)
                    {
                        this.coeffs[j] -= lrnRate *
                          (predY - actualY) * x[j];
                    }

                    //// apply decay
                    //this.intercept *= (1.0 - this.alpha);
                    //for (int j = 0; j < this.coeffs.Length; ++j)
                    //  this.coeffs[j] *= (1.0 - this.alpha);
                }

                // apply decay once per epoch
                this.intercept *= (1.0 - this.alpha);
                for (int j = 0; j < this.coeffs.Length; ++j)
                    this.coeffs[j] *= (1.0 - this.alpha);

                if (epoch % freq == 0)
                {
                    double rmse = this.RootMSE(trainX, trainY);
                    double acc = this.Accuracy(trainX, trainY);
                    string s1 = "epoch = " +
                      epoch.ToString().PadLeft(6);
                    string s2 = " RMSE = " +
                      rmse.ToString("F4");
                    string s3 = " acc = " + acc.ToString("F4");
                    Console.WriteLine(s1 + s2 + s3);
                }
            } // epoch
        } // Train2

        public double Predict(double[] x)
        {
            double sum = 0.0;
            int n = x.Length;  // number predictors
            for (int i = 0; i < n; ++i)
                sum += x[i] * this.coeffs[i];

            sum += this.intercept;  // add the constant
            return Math.Exp(sum);
        }

    } // class PoissonRegressor}

    public class PoissonUtils
    {
        public static double[] MatToVec(double[][] m)
        {
            int rows = m.Length;
            int cols = m[0].Length;
            double[] result = new double[rows * cols];
            int k = 0;
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                {
                    result[k++] = m[i][j];
                }
            return result;
        }

        public static double[][] MatLoad(string fn, int[] usecols,
          char sep, string comment)
        {
            List<double[]> result = new List<double[]>();
            string line = "";
            FileStream ifs = new FileStream(fn, FileMode.Open);
            StreamReader sr = new StreamReader(ifs);
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith(comment) == true)
                    continue;
                string[] tokens = line.Split(sep);
                List<double> lst = new List<double>();
                for (int j = 0; j < usecols.Length; ++j)
                    lst.Add(double.Parse(tokens[usecols[j]]));
                double[] row = lst.ToArray();
                result.Add(row);
            }
            sr.Close(); ifs.Close();
            return result.ToArray();
        }

        public static void VecShow(double[] vec,
          int dec, int wid, bool newLine)
        {
            for (int i = 0; i < vec.Length; ++i)
            {
                double x = vec[i];
                if (Math.Abs(x) < 1.0e-8) x = 0.0;  // hack
                Console.Write(x.ToString("F" +
                  dec).PadLeft(wid));
            }
            if (newLine == true)
                Console.WriteLine("");
        }

    } // class Utils
}
