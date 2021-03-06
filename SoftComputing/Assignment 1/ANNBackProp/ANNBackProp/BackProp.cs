using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace ANNShell
{
    public class NeuralNetwork
    {
        private Random rnd;
        public bool functionAproximator = false;

        private int numInput;
        private int numHidden;
        private int numOutput;

        private double[] inputs;

        private double[][] ihWeights; // input-hidden
        private double[] hBiases;
        private double[] hOutputs;

        private double[][] hoWeights; // hidden-output
        private double[] oBiases;

        private double[] outputs;

        // back-prop specific arrays (these could be local to method UpdateWeights)
        private double[] oGrads; // output gradients for back-propagation
        private double[] hGrads; // hidden gradients for back-propagation

        // back-prop momentum specific arrays (could be local to method Train)
        private double[][] ihPrevWeightsDelta;  // for momentum with back-propagation
        private double[] hPrevBiasesDelta;
        private double[][] hoPrevWeightsDelta;
        private double[] oPrevBiasesDelta;

        public double momentum = 0.0;
        public double weightDecay = 0.0; // 0.0001

        public int[,] confusionMatrix = null; // warning written, printed/shown and overwritten used as a global temporary
        public int confusionMatrixCnt = 0;

        public UserInterface userInterface = null;
        public char[] delimit = { ',', ' ', '\t' };

        public NeuralNetwork(int numInput, int numHidden, int numOutput, UserInterface ui, Random rndQ)
        {
            userInterface = ui;
            rnd = rndQ; // for InitializeWeights() and Shuffle()

            this.numInput = numInput;
            this.numHidden = numHidden;
            this.numOutput = numOutput;

            this.inputs = new double[numInput];

            this.ihWeights = MakeMatrix(numInput, numHidden);
            this.hBiases = new double[numHidden];
            this.hOutputs = new double[numHidden];

            this.hoWeights = MakeMatrix(numHidden, numOutput);
            this.oBiases = new double[numOutput];

            this.outputs = new double[numOutput];

            // back-prop related arrays below
            this.hGrads = new double[numHidden];
            this.oGrads = new double[numOutput];

            this.ihPrevWeightsDelta = MakeMatrix(numInput, numHidden);
            this.hPrevBiasesDelta = new double[numHidden];
            this.hoPrevWeightsDelta = MakeMatrix(numHidden, numOutput);
            this.oPrevBiasesDelta = new double[numOutput];
        } // ctor

        private static double[][] MakeMatrix(int rows, int cols) // helper for ctor
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
                result[r] = new double[cols];
            return result;
        }

        public override string ToString() // yikes
        {
            string s = "";
            s += "===============================\n";
            s += "numInput = " + numInput + " numHidden = " + numHidden + " numOutput = " + numOutput + "\n\n";

            s += "inputs: \n";
            for (int i = 0; i < inputs.Length; ++i)
                s += inputs[i].ToString("F2") + " ";
            s += "\n\n";

            s += "ihWeights: \n";
            for (int i = 0; i < ihWeights.Length; ++i)
            {
                for (int j = 0; j < ihWeights[i].Length; ++j)
                {
                    s += ihWeights[i][j].ToString("F4") + " ";
                }
                s += "\n";
            }
            s += "\n";

            s += "hBiases: \n";
            for (int i = 0; i < hBiases.Length; ++i)
                s += hBiases[i].ToString("F4") + " ";
            s += "\n\n";

            s += "hOutputs: \n";
            for (int i = 0; i < hOutputs.Length; ++i)
                s += hOutputs[i].ToString("F4") + " ";
            s += "\n\n";

            s += "hoWeights: \n";
            for (int i = 0; i < hoWeights.Length; ++i)
            {
                for (int j = 0; j < hoWeights[i].Length; ++j)
                {
                    s += hoWeights[i][j].ToString("F4") + " ";
                }
                s += "\n";
            }
            s += "\n";

            s += "oBiases: \n";
            for (int i = 0; i < oBiases.Length; ++i)
                s += oBiases[i].ToString("F4") + " ";
            s += "\n\n";

            s += "hGrads: \n";
            for (int i = 0; i < hGrads.Length; ++i)
                s += hGrads[i].ToString("F4") + " ";
            s += "\n\n";

            s += "oGrads: \n";
            for (int i = 0; i < oGrads.Length; ++i)
                s += oGrads[i].ToString("F4") + " ";
            s += "\n\n";

            s += "ihPrevWeightsDelta: \n";
            for (int i = 0; i < ihPrevWeightsDelta.Length; ++i)
            {
                for (int j = 0; j < ihPrevWeightsDelta[i].Length; ++j)
                {
                    s += ihPrevWeightsDelta[i][j].ToString("F4") + " ";
                }
                s += "\n";
            }
            s += "\n";

            s += "hPrevBiasesDelta: \n";
            for (int i = 0; i < hPrevBiasesDelta.Length; ++i)
                s += hPrevBiasesDelta[i].ToString("F4") + " ";
            s += "\n\n";

            s += "hoPrevWeightsDelta: \n";
            for (int i = 0; i < hoPrevWeightsDelta.Length; ++i)
            {
                for (int j = 0; j < hoPrevWeightsDelta[i].Length; ++j)
                {
                    s += hoPrevWeightsDelta[i][j].ToString("F4") + " ";
                }
                s += "\n";
            }
            s += "\n";

            s += "oPrevBiasesDelta: \n";
            for (int i = 0; i < oPrevBiasesDelta.Length; ++i)
                s += oPrevBiasesDelta[i].ToString("F4") + " ";
            s += "\n\n";

            s += "outputs: \n";
            for (int i = 0; i < outputs.Length; ++i)
                s += outputs[i].ToString("F2") + " ";
            s += "\n\n";

            s += "===============================\n";
            return s;
        }

        // ----------------------------------------------------------------------------------------

        public void SetWeights(double[] weights)
        {
            // copy weights and biases in weights[] array to i-h weights, i-h biases, h-o weights, h-o biases
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            if (weights.Length != numWeights)
                throw new Exception("Bad weights array length: ");

            int k = 0; // points into weights param

            for (int i = 0; i < numInput; ++i)
                for (int j = 0; j < numHidden; ++j)
                    ihWeights[i][j] = weights[k++];
            for (int i = 0; i < numHidden; ++i)
                hBiases[i] = weights[k++];
            for (int i = 0; i < numHidden; ++i)
                for (int j = 0; j < numOutput; ++j)
                    hoWeights[i][j] = weights[k++];
            for (int i = 0; i < numOutput; ++i)
                oBiases[i] = weights[k++];
        }

        public void InitializeWeights(Random rnd)
        {
            // initialize weights and biases to small random values
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            double[] initialWeights = new double[numWeights];
            double lo = -0.1;
            double hi = 0.1;
            for (int i = 0; i < initialWeights.Length; ++i)
                initialWeights[i] = (hi - lo) * rnd.NextDouble() + lo;
            this.SetWeights(initialWeights);
        }

        public double[] GetWeights()
        {
            // returns the current set of wweights, presumably after training
            int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
            double[] result = new double[numWeights];
            int k = 0;
            for (int i = 0; i < ihWeights.Length; ++i)
                for (int j = 0; j < ihWeights[0].Length; ++j)
                    result[k++] = ihWeights[i][j];
            for (int i = 0; i < hBiases.Length; ++i)
                result[k++] = hBiases[i];
            for (int i = 0; i < hoWeights.Length; ++i)
                for (int j = 0; j < hoWeights[0].Length; ++j)
                    result[k++] = hoWeights[i][j];
            for (int i = 0; i < oBiases.Length; ++i)
                result[k++] = oBiases[i];
            return result;
        }

        // ----------------------------------------------------------------------------------------

        private double[] ComputeOutputs(double[] xValues)
        {
            if (xValues.Length != numInput)
                throw new Exception("Bad xValues array length");

            double[] hSums = new double[numHidden]; // hidden nodes sums scratch array
            double[] oSums = new double[numOutput]; // output nodes sums

            for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
                this.inputs[i] = xValues[i];

            for (int j = 0; j < numHidden; ++j)  // compute i-h sum of weights * inputs
                for (int i = 0; i < numInput; ++i)
                    hSums[j] += this.inputs[i] * this.ihWeights[i][j]; // note +=

            for (int i = 0; i < numHidden; ++i)  // add biases to input-to-hidden sums
                hSums[i] += this.hBiases[i];

            for (int i = 0; i < numHidden; ++i)   // apply activation
                this.hOutputs[i] = HyperTanFunction(hSums[i]); // hard-coded

            for (int j = 0; j < numOutput; ++j)   // compute h-o sum of weights * hOutputs
                for (int i = 0; i < numHidden; ++i)
                    oSums[j] += hOutputs[i] * hoWeights[i][j];

            for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
                oSums[i] += oBiases[i];

            double[] softOut; // says softout but its not all we need if function aprox
            if (!functionAproximator)
            {
                softOut = Softmax(oSums); // softmax activation does all outputs at once for efficiency
                Array.Copy(softOut, outputs, softOut.Length);
            }
            else
            {
                //softOut = msse(oSums);
                //Array.Copy(softOut, outputs, softOut.Length);
            }
            double[] retResult = new double[numOutput]; // could define a GetOutputs method instead
            Array.Copy(this.outputs, retResult, retResult.Length);
            return retResult;
        } // ComputeOutputs

        private static double HyperTanFunction(double x)
        {
            if (x < -20.0) return -1.0; // approximation is correct to 30 decimals
            else if (x > 20.0) return 1.0;
            else return Math.Tanh(x);
        }

        private static double[] Softmax(double[] oSums)
        {
            // determine max output sum
            // does all output nodes at once so scale doesn't have to be re-computed each time
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
                if (oSums[i] > max) max = oSums[i];

            // determine scaling factor -- sum of exp(each val - max)
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                scale += Math.Exp(oSums[i] - max);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i] - max) / scale;

            return result; // now scaled so that xi sum to 1.0
        }


        //private static double[] msse(double[] oSums)
        //{
        //    // determine msse

        //    double[] result = new double[oSums.Length];
        //    for (int i = 0; i < oSums.Length; ++i)
        //        result[i] = Math.Exp(oSums[i] - max) / scale;

        //    return result; // now scaled so that xi sum to 1.0
        //}

        // ----------------------------------------------------------------------------------------

        private void UpdateWeights(double[] tValues, double learnRate)
        {
            // update the weights and biases using back-propagation, with target values, eta (learning rate),
            // alpha (momentum).
            // assumes that SetWeights and ComputeOutputs have been called and so all the internal arrays
            // and matrices have values (other than 0.0)
            if (tValues.Length != numOutput)
                throw new Exception("target values not same Length as output in UpdateWeights");

            // 1. compute output gradients
            for (int i = 0; i < oGrads.Length; ++i)
            {
                // derivative of softmax = (1 - y) * y (same as log-sigmoid)
                double derivative = (1 - outputs[i]) * outputs[i];
                // 'mean squared error version' includes (1-y)(y) derivative
                oGrads[i] = derivative * (tValues[i] - outputs[i]);
            }

            // 2. compute hidden gradients
            for (int i = 0; i < hGrads.Length; ++i)
            {
                // derivative of tanh = (1 - y) * (1 + y)
                double derivative = (1 - hOutputs[i]) * (1 + hOutputs[i]);
                double sum = 0.0;
                for (int j = 0; j < numOutput; ++j) // each hidden delta is the sum of numOutput terms
                {
                    double x = oGrads[j] * hoWeights[i][j];
                    sum += x;
                }
                hGrads[i] = derivative * sum;
            }

            // 3a. update hidden weights (gradients must be computed right-to-left but weights
            // can be updated in any order)
            for (int i = 0; i < ihWeights.Length; ++i) // 0..2 (3)
            {
                for (int j = 0; j < ihWeights[0].Length; ++j) // 0..3 (4)
                {
                    double delta = learnRate * hGrads[j] * inputs[i]; // compute the new delta
                    ihWeights[i][j] += delta; // update. note we use '+' instead of '-'. this can be very tricky.
                                              // now add momentum using previous delta. on first pass old value will be 0.0 but that's OK.
                    if (momentum > 0) ihWeights[i][j] += momentum * ihPrevWeightsDelta[i][j];
                    if (weightDecay > 0) ihWeights[i][j] -= (weightDecay * ihWeights[i][j]); // weight decay
                    ihPrevWeightsDelta[i][j] = delta; // don't forget to save the delta for momentum 
                }
            }

            // 3b. update hidden biases
            for (int i = 0; i < hBiases.Length; ++i)
            {
                double delta = learnRate * hGrads[i] * 1.0; // t1.0 is constant input for bias; could leave out
                hBiases[i] += delta;
                if (momentum > 0) hBiases[i] += momentum * hPrevBiasesDelta[i]; // momentum
                if (weightDecay > 0) hBiases[i] -= (weightDecay * hBiases[i]); // weight decay
                hPrevBiasesDelta[i] = delta; // don't forget to save the delta
            }

            // 4. update hidden-output weights
            for (int i = 0; i < hoWeights.Length; ++i)
            {
                for (int j = 0; j < hoWeights[0].Length; ++j)
                {
                    // see above: hOutputs are inputs to the nn outputs
                    double delta = learnRate * oGrads[j] * hOutputs[i];
                    hoWeights[i][j] += delta;
                    if (momentum > 0) hoWeights[i][j] += momentum * hoPrevWeightsDelta[i][j]; // momentum
                    if (weightDecay > 0) hoWeights[i][j] -= (weightDecay * hoWeights[i][j]); // weight decay
                    hoPrevWeightsDelta[i][j] = delta; // save
                }
            }

            // 4b. update output biases
            for (int i = 0; i < oBiases.Length; ++i)
            {
                double delta = learnRate * oGrads[i] * 1.0;
                oBiases[i] += delta;
                if (momentum > 0) oBiases[i] += momentum * oPrevBiasesDelta[i]; // momentum
                if (weightDecay > 0) oBiases[i] -= (weightDecay * oBiases[i]); // weight decay
                oPrevBiasesDelta[i] = delta; // save
            }
        } // UpdateWeights

        // ----------------------------------------------------------------------------------------
        public void momentumAndDecay(double momentumQ, double weightDecayQ)
        {
            momentum = momentumQ;
            weightDecay = weightDecayQ;
        }

        /// <summary>        
        /// ///  The main training routine             
        ///  train a back-prop style NN classifier using learning rate and momentum
        ///  weight decay reduces the magnitude of a weight value over time unless that value
        ///  is constantly increased
        /// </summary>
        /// <param name="trainData"></param>
        /// <param name="testData"></param>
        /// <param name="maxEpochs"></param>
        /// <param name="learnRate"></param>
        /// <param name="logFileName"></param>
        /// <param name="chart"></param>
        /// <param name="progressBar"></param>
        /// <param name="graph"></param>
        public void train(double[][] trainData, double[][] testData, int maxEpochs, double learnRate, string logFileName, Chart chart, ProgressBar progressBar, bool graph)
        {
            // train a back-prop style NN classifier using learning rate and momentum
            // weight decay reduces the magnitude of a weight value over time unless that value
            // is constantly increased
            int epoch = 0;
            double[] xValues = new double[numInput]; // inputs
            double[] tValues = new double[numOutput]; // target values

            if (!functionAproximator)
            {
                // clear chart
                chart.Series["Training"].Points.Clear();
                chart.Series["Testing"].Points.Clear();
            }

            int[] sequence = new int[trainData.Length];
            for (int i = 0; i < sequence.Length; i++) sequence[i] = i;

            int percentageComplete = 0;

            double trainAccuracy = 0; // *100 to convert to percent
            double testAccuracy = 0;


            using (StreamWriter writer = new StreamWriter(logFileName))
            {

                while (epoch < maxEpochs)
                {
                    percentageComplete = (int)((double)epoch / (double)maxEpochs * 100);
                    progressBar.Value = percentageComplete;
                    if (epoch % 10 == 0) { Application.DoEvents(); } // release windows

                    //double mse = MeanSquaredError(trainData);
                    //if (mse < 0.020) break; // consider passing value in as parameter

                    Shuffle(sequence); // visit each training data in random order
                    for (int i = 0; i < trainData.Length; i++)
                    {
                        int idx = sequence[i];
                        Array.Copy(trainData[idx], xValues, numInput);
                        Array.Copy(trainData[idx], numInput, tValues, 0, numOutput);
                        ComputeOutputs(xValues); // copy xValues in, compute outputs (store them internally)
                        UpdateWeights(tValues, learnRate); // find better weights
                    } // each training tuple

                    if (!functionAproximator)
                    {
                        trainAccuracy = Accuracy(trainData, "") * 100; // *100 to convert to percent
                        testAccuracy = Accuracy(testData, "") * 100;
                    }

                    double trainMse = MeanSquaredError(trainData);
                    double testMse = MeanSquaredError(testData);
                    string linez = epoch.ToString() + " ";
                    linez = linez + trainMse.ToString() + " " + testMse.ToString() + " ";
                    if (!functionAproximator)
                    {
                        linez = linez + trainAccuracy.ToString("F2") + "% " + testAccuracy.ToString("F2") + "% ";
                    }
                    linez = linez + "\r\n";
                    writer.Write(linez);

                    if (!functionAproximator && graph)
                    {
                        //Add another point to the chart
                        chart.Series["Training"].Points.AddY(trainAccuracy);
                        chart.Series["Testing"].Points.AddY(testAccuracy);
                    }
                    epoch++;
                }
            }
            progressBar.Value = 100;
        } // Train

        private void Shuffle(int[] sequence)
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }
        }

        private double MeanSquaredError(double[][] trainData) // used as a training stopping condition
        {
            // average squared error per training tuple
            double sumSquaredError = 0.0;
            double[] xValues = new double[numInput]; // first numInput values in trainData
            double[] tValues = new double[numOutput]; // last numOutput values

            // walk thru each training case. looks like (6.9 3.2 5.7 2.3) (0 0 1)
            for (int i = 0; i < trainData.Length; ++i)
            {
                Array.Copy(trainData[i], xValues, numInput);
                Array.Copy(trainData[i], numInput, tValues, 0, numOutput); // get target values
                double[] yValues = this.ComputeOutputs(xValues); // compute output using current weights
                for (int j = 0; j < numOutput; j++)
                {
                    double err = tValues[j] - yValues[j];
                    sumSquaredError += err * err;
                }
            }

            return sumSquaredError / numOutput; // is this an error in the code should it be sumSquaredError/numOutput RC 5/9/2016
        }

        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the confusion matrix as a string or to a file
        /// to ignore the file make filename ""
        /// note the con
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string showConfusion(string fileName)
        {
            string retv = "";

            for (int y = 0; y < numOutput; y++)
            {
                for (int x = 0; x < numOutput; x++)
                {
                    string temp = confusionMatrix[x, y].ToString("D");
                    temp = temp.PadLeft(4);
                    retv = retv + temp + " ";
                    if (x != numOutput - 1) retv = retv + "| ";
                }
                retv = retv + "\r\n";
                if (y != numOutput - 1) retv = retv + (new string('-', numOutput * 7)) + "\r\n";
            }

            if (fileName.Trim() != "")
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(retv);
                }
            }

            return retv;
        }

        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the confusion matrix as a string or to a file
        /// to ignore the file make filename ""
        /// note the con
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string showConfusionPercent(string fileName)
        {
            string retv = "";

            double[,] confusionMatrixP = new double[numOutput, numOutput];


            for (int y = 0; y < numOutput; y++)
            {
                for (int x = 0; x < numOutput; x++)
                {
                    confusionMatrixP[x, y] = (double)confusionMatrix[x, y] / (double)confusionMatrixCnt * 100.0;
                    string temp = confusionMatrixP[x, y].ToString("F2");
                    temp = temp.PadLeft(6);
                    retv = retv + temp + "% ";
                    if (x != numOutput - 1) retv = retv + "| ";
                }
                retv = retv + "\r\n";
                if (y != numOutput - 1) retv = retv + (new string('-', numOutput * 9)) + "\r\n";
            }

            if (fileName.Trim() != "")
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(retv);
                }
            }

            return retv;
        }


        // ----------------------------------------------------------------------------------------
        public double Accuracy(double[][] dataSet, string optionalOutFileName)
        {
            // percentage correct using winner-takes all
            StreamWriter writer = null;
            if (optionalOutFileName.Trim() != "")
            {
                writer = new StreamWriter(optionalOutFileName);
            }
            int numCorrect = 0;
            int numWrong = 0;
            double[] xValues = new double[numInput]; // inputs
            double[] tValues = new double[numOutput]; // targets
            double[] yValues; // computed Y

            confusionMatrix = new int[numOutput, numOutput];
            confusionMatrixCnt = 0;

            for (int i = 0; i < dataSet.Length; ++i)
            {
                Array.Copy(dataSet[i], xValues, numInput); // parse test data into x-values and t-values
                Array.Copy(dataSet[i], numInput, tValues, 0, numOutput);
                yValues = this.ComputeOutputs(xValues);
                int maxIndexOut = MaxIndex(yValues); // which cell in yValues has largest value?
                int maxIndexExpected = MaxIndex(tValues); // which cell in yValues has largest value?

                if (maxIndexOut == maxIndexExpected) numCorrect++;
                else numWrong++;

                confusionMatrix[maxIndexExpected, maxIndexOut] = confusionMatrix[maxIndexExpected, maxIndexOut] + 1;
                confusionMatrixCnt++;

                //if (tValues[maxIndex] == 1.0) // ugly. consider AreEqual(double x, double y)
                //    ++numCorrect;
                //else
                //    ++numWrong;
                if (optionalOutFileName.Trim() != "")
                {
                    string linez = "";
                    for (int j = 0; j < numOutput; j++) { linez = linez + tValues[j] + " "; } // expected first
                    linez = linez + maxIndexExpected + " ";
                    for (int k = 0; k < numOutput; k++) { linez = linez + yValues[k] + " "; } // actual second
                    linez = linez + maxIndexOut;
                    linez = linez + "\r\n";
                    writer.Write(linez);
                }
            }

            if (optionalOutFileName.Trim() != "") writer.Close();
            return (numCorrect * 1.0) / (numCorrect + numWrong); // ugly 2 - check for divide by zero note *100 for percent
        }

        // ----------------------------------------------------------------------------------------

        public double rightOrWrong(double[][] dataSet, string OutFileName)
        {
            // percentage correct using winner-takes all
            bool result;

            StreamWriter writer = null;
            if (OutFileName.Trim() != "")
            {
                writer = new StreamWriter(OutFileName);
            }
            int numCorrect = 0;
            int numWrong = 0;
            double[] xValues = new double[numInput]; // inputs
            double[] tValues = new double[numOutput]; // targets
            double[] yValues; // computed Y

            confusionMatrix = new int[numOutput, numOutput];
            confusionMatrixCnt = 0;

            for (int i = 0; i < dataSet.Length; ++i)
            {
                Array.Copy(dataSet[i], xValues, numInput); // parse test data into x-values and t-values
                Array.Copy(dataSet[i], numInput, tValues, 0, numOutput);
                yValues = this.ComputeOutputs(xValues);
                int maxIndexOut = MaxIndex(yValues); // which cell in yValues has largest value?
                int maxIndexExpected = MaxIndex(tValues); // which cell in yValues has largest value?

                if (maxIndexOut == maxIndexExpected) { numCorrect++; result = true; }
                else { numWrong++; result = false; }

                if (OutFileName.Trim() != "")
                {
                    string linez = "";
                    for (int j = 0; j < dataSet[i].Length; j++)
                    {
                        linez = linez + dataSet[i][j].ToString("F6") + " ";

                    }
                    linez = linez + "(" + maxIndexOut + " , " + maxIndexExpected + ") ";
                    if (result) linez = linez + " Correct\r\n"; else linez = linez + " Wrong  \r\n";

                    writer.Write(linez);
                }
            }

            if (OutFileName.Trim() != "") writer.Close();
            return (numCorrect * 1.0) / (numCorrect + numWrong); // ugly 2 - check for divide by zero note *100 for percent
        }

        // ----------------------------------------------------------------------------------------


        /// <summary>
        /// for function aproximators process the data 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="optionalOutFileName"></param>
        public void processDataset(double[][] dataSet, string outFileName)
        {
            // percentage correct using winner-takes all
            StreamWriter writer = null;
            writer = new StreamWriter(outFileName);
            double[] xValues = new double[numInput]; // inputs
            double[] tValues = new double[numOutput]; // targets
            double[] yValues; // computed Y

            for (int i = 0; i < dataSet.Length; ++i)
            {
                Array.Copy(dataSet[i], xValues, numInput); // parse test data into x-values and t-values
                Array.Copy(dataSet[i], numInput, tValues, 0, numOutput);
                yValues = this.ComputeOutputs(xValues);

                string linez = "";
                for (int j = 0; j < numInput; j++) { linez = linez + xValues[j] + " "; } // expected first
                for (int j = 0; j < numOutput; j++) { linez = linez + tValues[j] + " "; } // expected first
                for (int k = 0; k < numOutput; k++) { linez = linez + yValues[k] + " "; } // actual second
                linez = linez + "\r\n";
                writer.Write(linez);
            }

            writer.Close();
            return;
        }


        private static int MaxIndex(double[] vector) // helper for Accuracy()
        {
            // index of largest value
            int bigIndex = 0;
            double biggestVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > biggestVal)
                {
                    biggestVal = vector[i]; bigIndex = i;
                }
            }
            return bigIndex;
        }

        static public string vectorToString(double[] vector, int decimals)
        {
            string retv = "";
            for (int i = 0; i < vector.Length; i++)
            {
                retv = retv + (vector[i].ToString("F" + decimals) + " ");
            }
            return retv;
        }

        static public void appendVectorToFile(StreamWriter sw, double[] vector)
        {
            string s = vectorToString(vector, 5);
            sw.WriteLine(s);
        }

        public void saveANN(string fileName)
        {
            //double[] weights = GetWeights();

            StreamWriter writer = null;
            writer = new StreamWriter(fileName);

            // save setup info 
            writer.WriteLine(numInput.ToString() + " " + numHidden.ToString() + " " + numOutput.ToString());

            //Save hidden layer
            for (int i = 0; i < numInput; i++) appendVectorToFile(writer, ihWeights[i]);
            appendVectorToFile(writer, hBiases);

            //save output layer
            for (int i = 0; i < numHidden; i++) appendVectorToFile(writer, hoWeights[i]);
            appendVectorToFile(writer, oBiases);

            writer.Close();
        }

        public static double[] stringToVector(string str, int itemCount, UserInterface userInterfaceZ, char[] delimiters)
        {
            double[] retv = new double[itemCount];
            str = str.Trim();
            if (str == "") return retv;
            string[] ss = str.Split(delimiters);
            int cnt = 0;
            double d = 0;
            foreach (string s in ss)
            {
                try
                {
                    d = Double.Parse(s);
                }
                catch (Exception e)
                {
                    userInterfaceZ.error("The line '" + str + "' contains an invalid number " + e.ToString());
                    return retv;
                }
                retv[cnt] = d;
                cnt++;
            }
            return retv;
        }

        public void loadANN(string fileName)
        {
            //double[] weights = GetWeights();

            StreamReader reader = null;
            try
            {
                reader = new StreamReader(fileName);

                // save setup info 
                string setupInfo = reader.ReadLine();
                //writer.WriteLine(numInput.ToString() + " " + numHidden.ToString() + " " + numOutput.ToString());
                double[] d = stringToVector(setupInfo, 3, userInterface, delimit);

                int numInputCheck = (int)d[0];
                int numHiddenCheck = (int)d[1];
                int numOutputCheck = (int)d[2];

                if (numInputCheck != numInput ||
                    numHiddenCheck != numHidden ||
                    numOutputCheck != numOutput)
                {
                    userInterface.error("This network has different number of nodes to the one on the file - loadANN aborted !");
                    reader.Close();
                    return;
                }
                string line = "";
                //double[] ihWeights

                for (int i = 0; i < numInput; i++)
                {
                    line = reader.ReadLine();
                    ihWeights[i] = stringToVector(line, numHidden, userInterface, delimit);
                    //appendVectorToFile(writer, ihWeights[i]);
                }
                line = reader.ReadLine();
                hBiases = stringToVector(line, numHidden, userInterface, delimit);
                //appendVectorToFile(writer, hBiases);

                //save output layer
                for (int i = 0; i < numHidden; i++)
                {
                    line = reader.ReadLine();
                    hoWeights[i] = stringToVector(line, numOutput, userInterface, delimit);
                    //appendVectorToFile(writer, hoWeights[i]);
                }
                line = reader.ReadLine();
                oBiases = stringToVector(line, numOutput, userInterface, delimit);
                //appendVectorToFile(writer, oBiases);

                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("An error ocurred while executing the data import: {0}", e.Message), e);
            }
        }

    } // NeuralNetwork

}