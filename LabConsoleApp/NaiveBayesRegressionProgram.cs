// See https://aka.ms/new-console-template for more information
using McCaffrey.DataScienceLab;

Console.WriteLine("Hello, World!");

Console.WriteLine("\nBegin naive Bayes regression ");
Console.WriteLine("Predict red wine fixed acidity from volatile acidity, citric acid, etc. ");

// 1. load data
Console.WriteLine("\nLoading train (200) and test (40) data");

string trainFile = "..\\..\\..\\..\\LabData\\wine_train_200.txt";
int[] colsX = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
double[][] trainX = MatLoad(trainFile, colsX, ';', "#");
double[] trainY = MatToVec(MatLoad(trainFile,
    new int[] { 0 }, ';', "#"));

string testFile = "..\\..\\..\\..\\LabData\\wine_test_40.txt";
double[][] testX = MatLoad(testFile, colsX, ';', "#");
double[] testY = MatToVec(MatLoad(testFile, new int[] { 0 }, ';', "#"));
Console.WriteLine("Done ");

Console.WriteLine("\nFirst five train X: ");
for (int i = 0; i < 5; ++i)
    VecShow(trainX[i], 4, 8);

Console.WriteLine("\nFirst five train y: ");
for (int i = 0; i < 5; ++i)
    Console.WriteLine(trainY[i].ToString("F4").
        PadLeft(8));

// 2. create and train model
Console.WriteLine("\nCreating and training" +
    " naive Bayes regression model ");
NaiveBayesRegressor model = new NaiveBayesRegressor();
model.Train(trainX, trainY);
Console.WriteLine("Done ");

// 3. evaluate model
Console.WriteLine("\nEvaluating model ");
double accTrain = model.Accuracy(trainX, trainY, 0.15);
Console.WriteLine("Accuracy train (within 0.15) = " +
    accTrain.ToString("F4"));
double accTest = model.Accuracy(testX, testY, 0.15);
Console.WriteLine("Accuracy test (within 0.15) = " +
    accTest.ToString("F4"));

// 4. use model
double[] x = trainX[0];
Console.WriteLine("\nPredicting for x = ");
VecShow(x, 4, 8);
double predY = model.Predict(x, verbose: true);
Console.WriteLine("\nPredicted y = " +
    predY.ToString("F4"));

Console.WriteLine("\nEnd demo ");
Console.ReadLine();

static double[][] MatLoad(string fn, int[] usecols,
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

static double[] MatToVec(double[][] mat)
{
    int nRows = mat.Length;
    int nCols = mat[0].Length;
    double[] result = new double[nRows * nCols];
    int k = 0;
    for (int i = 0; i < nRows; ++i)
        for (int j = 0; j < nCols; ++j)
            result[k++] = mat[i][j];
    return result;
}

static void VecShow(double[] vec, int dec, int wid)
{
    for (int i = 0; i < vec.Length; ++i)
        Console.Write(vec[i].ToString("F" + dec).
          PadLeft(wid));
    Console.WriteLine("");
}
