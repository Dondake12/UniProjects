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
            // test DataClass
            DataClass d = new DataClass();
            d.readFromFile(dir, "test1.txt");
            string s = d.showData();
            textBox1.Text = s;
            d.writeToFile(dir, "temp.txt");
            d.normalize(0, 1, "");
            string ss = d.showData();
            textBox1.Text = textBox1.Text +"\r\n\r\n" +ss;
            DataClass d1=null;
            DataClass d2=null;
            d.extractSplit(out d1, out d2, 4, new Random());
            string s1 = d1.showData();
            string s2 = d2.showData();
            textBox1.Text = textBox1.Text + "\r\n\r\n" + s1;
            textBox1.Text = textBox1.Text + "\r\n\r\n" + s2;

        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            // test two of DataClass
            textBox1.Text = "";
            DataClass d = new DataClass();
            d.readFromFile(dir, "test1.txt");
            string s = d.showData();
            textBox1.Text = s;
            textBox1.Text = textBox1.Text + "\r\n\r\n";

            DataClass dd = d.makeExemplar(2, 3, 1);
            string ss = dd.showData();
            textBox1.Text = textBox1.Text + ss;
            textBox1.Text = textBox1.Text + "\r\n\r\n";
        }

        private void btnIris_Click(object sender, EventArgs e)
        {
            // this is code for the student to insert/modify

            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\Cancer\";
            string datafile = "cancer.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Cancer";
            string prefixNameForDataset = "Cancer"; // single word no spaces pls
            int inputs = 9;
            int hidden = int.Parse(hiddens);
            int outputs = 2;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 683;
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass cancerRaw = new DataClass(dir, datafile, new UI(this));
            string s = cancerRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            cancerRaw.normalize(0, 8, "");
            string ss = cancerRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass cancerExemplar = cancerRaw.makeExemplar(inputs, outputs, 1);
            string se = cancerExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            cancerExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
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
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\Abalone\";
            string datafile = "Abalone.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Abalone";
            string prefixNameForDataset = "Abalone"; // single word no spaces pls
            int inputs = 8;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 3;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 4177;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass abaloneRaw = new DataClass(dir, datafile, new UI(this));
            string s = abaloneRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            abaloneRaw.normalize(0, 7, "");
            string ss = abaloneRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass abaloneExemplar = abaloneRaw.makeExemplar(inputs, outputs, 1);
            string se = abaloneExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            abaloneExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
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

            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\Card\";
            string datafile = "card.dat";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Card";
            string prefixNameForDataset = "Card"; // single word no spaces pls
            int inputs = 51;             // Refector this
            int hidden = int.Parse(hiddens);
            int outputs = 2;            // Refactor this
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 690;   // Refactor this
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass cardRaw = new DataClass(dir, datafile, new UI(this));
            string s = cardRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            cardRaw.normalize(0, 50, "");
            string ss = cardRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass cardExemplar = cardRaw.makeExemplar(inputs, outputs, 1);
            string se = cardExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            cardExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
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
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\AbaloneBaby\";
            string datafile = "AbaloneBaby.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "AbaloneBaby";
            string prefixNameForDataset = "AbaloneBaby"; // single word no spaces pls
            int inputs = 8;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 3;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 1392;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass abaloneBabyRaw = new DataClass(dir, datafile, new UI(this));
            string s = abaloneBabyRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            abaloneBabyRaw.normalize(0, 7, "");
            string ss = abaloneBabyRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass abaloneBabyaExemplar = abaloneBabyRaw.makeExemplar(inputs, outputs, 1);
            string se = abaloneBabyaExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            abaloneBabyaExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
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
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\WineData\";
            string datafile = "Wine.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Wine";
            string prefixNameForDataset = "Wine"; // single word no spaces pls
            int inputs = 13;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 3;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc);
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 178;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass wineRaw = new DataClass(dir, datafile, new UI(this));
            string s = wineRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            wineRaw.normalize(0, 12, "");
            string ss = wineRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass wineExemplar = wineRaw.makeExemplar(inputs, outputs, 1);
            string se = wineExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            trainData = new DataClass();
            testData = new DataClass();
            valData = new DataClass();
            DataClass tempData = new DataClass();

            wineExemplar.extractSplit(out trainData, out tempData, sizeOfTrain, rnd1);
            tempData.extractSplit(out testData, out valData, sizeOfTest, rnd1);
            //trainAndTestData.data = trainAndTestData. + trainData.data + testData.data;
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

        private void button11_Click(object sender, EventArgs e)
        {
            // this is code for the run Abalone button
            // the directory below needs a trailing slash
            dir = @"D:\Projects\VisualStudio_Projects\SoftComputing\MarkTute1\V7Data\WineData\";
            string datafile = "Wine.txt";

            string fil = dir + datafile;
            if (!File.Exists(fil))
            {
                string message = fil + " Not found (have you set the file path?)";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "File Not found", buttons);
                return;
            }

            string commonNameForDataset = "Wine";
            string prefixNameForDataset = "Wine"; // single word no spaces pls
            int inputs = 13;             // Modify These
            int hidden = int.Parse(hiddens);
            int outputs = 3;
            double eta = double.Parse(stringETA);
            int epochs = int.Parse(epoc)/2;
            Random rnd1 = new Random(int.Parse(random1)); // data split random number
            Random rnd2 = new Random(int.Parse(random2)); // ANN initialise weights and shuffle data random number
            int sizeOfDataSet = 178;    // Modify These(Get correct value)
            int sizeOfTest = sizeOfDataSet / 3;
            int sizeOfValidation = sizeOfDataSet / 3;
            int sizeOfTrain = sizeOfDataSet - sizeOfTest - sizeOfValidation;

            textBox2.Clear(); // clear previous messages

            DataClass wineRaw = new DataClass(dir, datafile, new UI(this));
            string s = wineRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset);
            textBox1.AppendText(s);
            textBox1.AppendText("\r\n\r\n");

            wineRaw.normalize(0, 12, "");
            string ss = wineRaw.showDataPart(5, inputs + 1, "F4", commonNameForDataset + " Normalised");
            textBox1.AppendText(ss);
            textBox1.AppendText("\r\n\r\n");

            DataClass wineExemplar = wineRaw.makeExemplar(inputs, outputs, 1);
            string se = wineExemplar.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Exemplar Data");
            textBox1.AppendText(se);
            textBox1.AppendText("\r\n\r\n");

            DataClass trainAndTestData = new DataClass();
            valData = new DataClass();

            wineExemplar.extractSplit(out trainAndTestData, out valData, sizeOfTrain+sizeOfTest, rnd1);
            trainAndTestData.writeToFile(dir, "tempTrainAndTest.txt"); // debug
            valData.writeToFile(dir, "tempVal.txt");

            string s1 = trainAndTestData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Training And Testing Data");
            textBox1.AppendText(s1);
            textBox1.AppendText("\r\n\r\n");

            string s2 = valData.showDataPart(5, inputs + outputs, "F4", commonNameForDataset + " Validation Data");
            textBox1.AppendText(s2);
            textBox1.AppendText("\r\n\r\n");

            NeuralNetwork nn = new NeuralNetwork(inputs, hidden, outputs, new UI(this), rnd2);
            nn.InitializeWeights(rnd2);
            textBox1.AppendText("\r\nBeginning training using incremental back-propagation\r\n");
            nn.train(trainAndTestData.data, valData.data, epochs, eta, dir + "nnlog.txt", nnChart, nnProgressBar, checkBoxGraph.Checked);
            textBox1.AppendText("Training complete\r\n");

            double trainandTestingAcc = nn.Accuracy(trainData.data, dir + "trainAndTestingOut.txt");
            string ConfusionTrainAndTesting = nn.showConfusion(dir + "trainAndTestingConfusion.txt");
            double valAcc = nn.Accuracy(valData.data, dir + "valOut.txt");
            string ConfusionVal = nn.showConfusion(dir + "valConfusion.txt");

            // convert accuracy to percents
            trainandTestingAcc = trainandTestingAcc * 100;
            valAcc = valAcc * 100;
            textBox1.AppendText("Training and Testing Accuracy   = " + trainandTestingAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("Validation Accuracy = " + valAcc.ToString("F2") + "\r\n");
            textBox1.AppendText("\r\n\r\n");

            textBox1.AppendText("Training and Testing Confusion Matrix \r\n" + ConfusionTrainAndTesting + "\r\n\r\n");
            textBox1.AppendText("Validation Confusion Matrix \r\n" + ConfusionVal + "\r\n\r\n");

            // finally save weights for the future
            nn.saveANN(dir + prefixNameForDataset + "_Weights.txt");
        }
    }
}
