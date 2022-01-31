using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANNShell
{
    public partial class Form1 : Form
    {
        string dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\";

        DataClass trainData = new DataClass();
        DataClass testData = new DataClass();
        DataClass valData = new DataClass();

        string stringETA;
        string hiddens;
        string epoc;

        string random1;
        string random2;

        public Form1()
        {
            InitializeComponent();
        }

        class UI : UserInterface
        {
            public Form1 form;

            public UI(Form1 formQ)
            {
                form = formQ;
            }

            public override void error(string s)
            {
                form.textBox2.Text = form.textBox2.Text + "ERROR>>" + s + "\r\n";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show("ERROR>>" + s, "Error", buttons);
            }
            public override void clear(string s)
            {
                form.textBox2.Text = "";
            }
            public override void warning(string s)
            {
                form.textBox2.Text = form.textBox2.Text + "WARNING>>" + s + "\r\n";
            }
            public override void note(string s)
            {
                form.textBox2.Text = form.textBox2.Text + "NOTE>>" + s + "\r\n";
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\FaceDataZip\";
            string datafile = "FaceDataBoth_edit.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "FaceDataBoth";
            string prefixNameForDataset = "FaceDataBoth"; // single word no spaces pls
            int inputs = 49;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 6977;    // Modify These(Get correct value)
            int sizeOfTest = (sizeOfDataSet*2) / 5;
            int sizeOfValidation = sizeOfDataSet / 5;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass faceDataBothRaw = new DataClass(dir, datafile, new UI(this));
            string s = faceDataBothRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            faceDataBothRaw.normalize(0, 48, "");
            string ss = faceDataBothRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass faceDataBothExemplar = faceDataBothRaw.makeExemplar(inputs, outputs, 1);
            string se = faceDataBothExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            faceDataBothExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTrain.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            // this is code for the student to insert/modify

            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\task2_2020-Data\";
            string datafile = "task2_all.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Task2_All";
            string prefixNameForDataset = "Task2_All"; // single word no spaces pls
            int inputs = 2;             // Refector this
            int hidden = int.Parse(hiddens);
            int outputs = 2;            // Refactor this
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 9384;   // Refactor this
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass task2ARaw = new DataClass(dir, datafile, new UI(this));
            string s = task2ARaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            task2ARaw.normalize(0, 1, "");
            string ss = task2ARaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass task2AExemplar = task2ARaw.makeExemplar(inputs, outputs, 1);
            string se = task2AExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            task2AExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTest.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }

        private void btnIris_Click(object sender, EventArgs e)
        {
            // this is code for the student to insert/modify

            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\Dermatology\";
            string datafile = "dermatology.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Dermatology";
            string prefixNameForDataset = "Dermatology"; // single word no spaces pls
            int inputs = 34;
            int hidden = int.Parse(hiddens);
            int outputs = 6;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 366;
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass dermatologyRaw = new DataClass(dir, datafile, new UI(this));
            string s = dermatologyRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            dermatologyRaw.normalize(0, 33, "");
            string ss = dermatologyRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass dermatologyExemplar = dermatologyRaw.makeExemplar(inputs, outputs, 1);
            string se = dermatologyExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            dermatologyExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTest.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }

        private void nnProgressBar_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // this is code for the run iris button
            // the directory below requires a trailing slash
            
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\Iris\";
            string datafile = "IrisDataOriginalNum.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found",buttons);
                return;
            }

            string commonNameForDataset = "Iris";
            string prefixNameForDataset = "Iris"; // single word no spaces pls
            int inputs = 4;
            int hidden = int.Parse(hiddens);
            int outputs = 3;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 150;
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            // iris button
            //nnChart.Series["Training"].Points.Clear();
            //nnChart.Series["Testing"].Points.Clear();

            textBox2.Clear(); // clear previous messages

            DataClass irisRaw = new DataClass(dir, datafile, new UI(this));
            string s = irisRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            irisRaw.normalize(0, 3, "");
            string ss = irisRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset+" Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass irisExemplar = irisRaw.makeExemplar(inputs, outputs, 1);
            string se = irisExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            irisExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTest.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset+"_Weights.txt");

        }

        private void FaceAssignment_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // test of save and restore weights 
            Random rnd2 = new Random(40);
            int inputs = 4;
            int hidden = 5;
            int outputs = 3;

            NeuralNetwork nnn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nnn.InitializeWeights(rnd2);
            nnn.loadANN(dir + "irisWeights.txt");
   
            nnn.saveANN(dir + "irisWeights2.txt");

            double trainAcc = nnn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nnn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nnn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nnn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nnn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nnn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

        }

        private void buttonTimesTables_Click(object sender, EventArgs e)
        {
            // this is code for the times tables button - stull under development 

            MessageBox.Show("Option still being constructed");
            return; 
 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\task2_2020-Data\";
            string datafile = "task2_2020c.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Task2C";
            string prefixNameForDataset = "Task2C"; // single word no spaces pls
            int inputs = 2;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 6562;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass task2CRaw = new DataClass(dir, datafile, new UI(this));
            string s = task2CRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            task2CRaw.normalize(0, 1, "");
            string ss = task2CRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass task2CExemplar = task2CRaw.makeExemplar(inputs, outputs, 1);
            string se = task2CExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            task2CExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTest.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");

        }

        private void Run4000_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\Data4000e10\";
            string datafile = "Data4000e10.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Data4000e10";
            string prefixNameForDataset = "Data4000e10"; // single word no spaces pls
            int inputs = 2;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 4000;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            // iris button
            //nnChart.Series["Training"].Points.Clear();
            //nnChart.Series["Testing"].Points.Clear();

            textBox2.Clear(); // clear previous messages

            DataClass data4000e10Raw = new DataClass(dir, datafile, new UI(this));
            string s = data4000e10Raw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            data4000e10Raw.normalize(0, 1, "");
            string ss = data4000e10Raw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass data4000e10Exemplar = data4000e10Raw.makeExemplar(inputs, outputs, 1);
            string se = data4000e10Exemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            data4000e10Exemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTest.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");

        }

        private void checkBoxGraph_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            stringETA = textBoxETA.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            hiddens = textBoxHidden.Text;
        }

        private void textBoxOutput_TextChanged(object sender, EventArgs e)
        {
            epoc = textBoxEpocs.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            random1 = textBoxHidden1.Text;
        }

        private void textBoxHidden2_TextChanged(object sender, EventArgs e)
        {
            random2 = textBoxHidden2.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // this is code for the student to insert/modify

            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\task2_2020-Data\";
            string datafile = "task2_2020a.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Task2a";
            string prefixNameForDataset = "Task2a"; // single word no spaces pls
            int inputs = 2;             // Refector this
            int hidden = int.Parse(hiddens);
            int outputs = 2;            // Refactor this
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 713;   // Refactor this
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass task2ARaw = new DataClass(dir, datafile, new UI(this));
            string s = task2ARaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            task2ARaw.normalize(0, 1, "");
            string ss = task2ARaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass task2AExemplar = task2ARaw.makeExemplar(inputs, outputs, 1);
            string se = task2AExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            task2AExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTest.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\task2_2020-Data\";
            string datafile = "task2_2020b.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Task2b";
            string prefixNameForDataset = "Task2b"; // single word no spaces pls
            int inputs = 2;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 2109;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass task2BRaw = new DataClass(dir, datafile, new UI(this));
            string s = task2BRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            task2BRaw.normalize(0, 1, "");
            string ss = task2BRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass task2BExemplar = task2BRaw.makeExemplar(inputs, outputs, 1);
            string se = task2BExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            task2BExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTest.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\task3\";
            string datafile = "Task3a2020.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Task3a";
            string prefixNameForDataset = "Task3a"; // single word no spaces pls
            int inputs = 2;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 1000;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass task3ARaw = new DataClass(dir, datafile, new UI(this));
            string s = task3ARaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            //task3ARaw.normalize(0, 1, "");
            //string ss = task3Raw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            //textBox1.AppendText(ss);
            //textBox1.AppendText("\r\n\r\n");

            DataClass task3AExemplar = task3ARaw.makeExemplar(inputs, outputs, 1);
            string se = task3AExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            task3AExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTrain.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            nn.rightOrWrong(trainData.data, dir + "trainResult.txt"); // put errors in trainResult.txt
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            nn.rightOrWrong(testData.data, dir + "testResult.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");
            nn.rightOrWrong(valData.data, dir + "valResult.txt");
            double allAcc = nn.Accuracy(task3AExemplar.data, dir + "allOut.txt");
            string ConfusionAll = nn.showConfusion(dir + "allConfusion.txt");
            nn.rightOrWrong(task3AExemplar.data, dir + "rightOrWrong.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            allAcc = allAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("All Accuracy        = " + allAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");
            textBox1.AppendText("All Confusion Matrix \r\n" + ConfusionAll + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\FaceDataZip\";
            string datafile = "FaceDataBoth_edit.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "FaceDataBoth";
            string prefixNameForDataset = "FaceDataBoth"; // single word no spaces pls
            int inputs = 49;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 6977;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass faceDataBothRaw = new DataClass(dir, datafile, new UI(this));
            string s = faceDataBothRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            faceDataBothRaw.normalize(0, 48, "");
            string ss = faceDataBothRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass faceDataBothExemplar = faceDataBothRaw.makeExemplar(inputs, outputs, 1);
            string se = faceDataBothExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            faceDataBothExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTrain.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\Assignment 1\V7Data\task3\";
            string datafile = "Task3b2020.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Task3B";
            string prefixNameForDataset = "Task3B"; // single word no spaces pls
            int inputs = 5;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 1222;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass task3BRaw = new DataClass(dir, datafile, new UI(this));
            string s = task3BRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            //task3BRaw.normalize(0, 4, "");
            //string ss = task3Raw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            //textBox1.AppendText(ss);
            //textBox1.AppendText("\r\n\r\n");

            DataClass task3BExemplar = task3BRaw.makeExemplar(inputs, outputs, 1);
            string se = task3BExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            task3BExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            trainData.writeToFile(dir, "tempTrain.txt"); // debug
            testData.writeToFile(dir, "tempTrain.txt");
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = testData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Testing Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            string s3 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s3);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainData.data, testData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainAcc = nn.Accuracy(trainData.data, dir + "trainOut.txt");
            string ConfusionTrain = nn.showConfusion(dir + "trainConfusion.txt");
            nn.rightOrWrong(trainData.data, dir + "trainResult.txt"); // put errors in trainResult.txt
            double testAcc = nn.Accuracy(testData.data, dir + "testOut.txt");
            string ConfusionTest = nn.showConfusion(dir + "testConfusion.txt");
            nn.rightOrWrong(testData.data, dir + "testResult.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");
            nn.rightOrWrong(valData.data, dir + "valResult.txt");
            double allAcc = nn.Accuracy(task3BExemplar.data, dir + "allOut.txt");
            string ConfusionAll = nn.showConfusion(dir + "allConfusion.txt");
            nn.rightOrWrong(task3BExemplar.data, dir + "rightOrWrong.txt");

            // convert accuracy to percents
            trainAcc = trainAcc * 100;
            testAcc = testAcc * 100;
            valAcc = valAcc * 100;
            allAcc = allAcc * 100;
            textBox1.AppendText("Training Accuracy   = " + trainAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Testing Accuracy    = " + testAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("All Accuracy = " + allAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training Confusion Matrix \r\n" + ConfusionTrain + "\r\n\r\n");
            textBox1.AppendText("Testing Confusion Matrix \r\n" + ConfusionTest + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");
            textBox1.AppendText("All Confusion Matrix \r\n" + ConfusionAll + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }
    }
}
