using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANNShell
{
    public class DataClass
    {
        public double[][] data = null;
        public int rows = 0;
        public int columns = 0;
        public int attributes = 0;
        public bool exemplarFormat = false;
        public char[] delimit = {',',' ','\t'};

        public double[] normMin = null; // original minimum of each column
        public double[] normMax = null; // original maximum of each column
        public double[] normMul = null; // original multiplier after subtracting min

        public UserInterface userInterface = null;

        /// <summary>
        /// A form of copy constructor I think its complete
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="ui"></param>
        public DataClass(DataClass dc)
        {
            data = null;
            rows = dc.rows;
            columns = dc.columns;
            attributes = dc.attributes;
            exemplarFormat = dc.exemplarFormat;
            delimit = new char[dc.delimit.Length];

            for (int i = 0; i < dc.delimit.Length; i++) { delimit[i] = dc.delimit[i]; }

            normMin = new double[columns];   
            normMax = new double[columns]; 
            normMul = new double[columns]; 
            for (int col = 0; col < columns; col++ )
            {
                if (dc.normMin != null) normMin[col] = dc.normMin[col];
                if (dc.normMax != null) normMax[col] = dc.normMax[col];
                if (dc.normMul != null) normMul[col] = dc.normMul[col];
            }

            data = new double[dc.data.Length][];
            for (int i=0; i <dc.data.Length; i++)
            {
                data[i] = new double[dc.data[i].Length];
                for (int j = 0; j <dc.data[i].Length; j++)
                {
                    data[i][j] = dc.data[i][j];
                }
            }
        }

    public DataClass(int rowsQ, int cols, UserInterface ui)
        {
            userInterface = ui;
            rows = rowsQ;
            data = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                data[i] = new double[cols];
            }
            columns = cols;
        }

        public DataClass(int rowsQ, UserInterface ui)
        {
            userInterface = ui;
            rows = rowsQ;
            data = new double[rows][];
        }

        public DataClass()
        {
            data = null;
        }

        public DataClass(string dir, string fname, UserInterface ui)
        {
            userInterface = ui;
            readFromFile(dir, fname);
        }

    public bool setRow(int rowNum, double[] rowdata)
        {
            if (rowNum < 0) return false;
            if (rowNum >= rows) return false;
            double[] d = new double[rowdata.Length];
            for (int c = 0; c < rowdata.Length; c++) d[c] = rowdata[c];
            data[rowNum] = d;
            return true;
        }

        /// <summary>
        /// col is the column with the class,  numClasses is the number of classes, startat is the lowest (usually 0 or 1)
        /// </summary>
        /// <param name="col"></param>
        /// <param name="numClasses"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        public DataClass makeExemplar(int col, int numClasses, int startAt )
        {
            DataClass retv = new DataClass(rows, userInterface);
            
            int newCols = columns + numClasses - 1;
            retv.columns = newCols;

            for (int r = 0; r < rows; r++)
            {
                double[] rr = new double[newCols];
                int cc = 0;
                for (int c = 0; c < columns; c++)
                {
                    double d = data[r][c];
                    if (c == col)
                    {
                        for (int j=0; j<numClasses; j++)
                        {
                            rr[cc] = 0;
                            if ((j)==((int)d) - startAt)
                            {
                                rr[cc] = 1;
                            }
                            cc++;
                        }
                    }
                    else
                    {
                        rr[cc] = d;
                        cc++;
                    }
                }
                retv.setRow(r, rr);
            }


            return retv;
        }

        /// <summary>
        /// read a data source from a file
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="fname"></param>
        public void readFromFile(string dir, string fname)
        {
            // first count lines 
            int counter = 0;
            string line;
            System.IO.StreamReader file;

            string ffname = dir + fname;

            if (!File.Exists(ffname))
            {
                //Console.WriteLine("The file '"+ ffname+"' does not exist");
                userInterface.error("The file '"+ ffname+"' does not exist");
                return;
            }
            try
            {
                file = new System.IO.StreamReader(ffname);
            }
            catch (Exception e)
            {
                userInterface.error("The file '"+ ffname+"' did not open "+ e.ToString());
                return;
            }

            while ((line = file.ReadLine()) != null)
            {
                    if (!(line.Trim()=="")) counter++;
            }
            file.Close();

            // now we have a count
            rows = counter;
            data = new double[rows][];
            columns = -1;

            file = new System.IO.StreamReader(ffname);
            counter = 0;
            while ((line = file.ReadLine()) != null)
            {
                if (!(line.Trim() == ""))
                {
                    string[] ss = line.Split(delimit);
                    int cnt = 0;
                    foreach ( string s in ss)
                    {
                        if (s.Trim() != "") cnt++;
                    }
                    data[counter] = new double[cnt];
                    if (columns == -1) columns = cnt;
                    else
                    {
                        if (columns != cnt)
                        {
                            userInterface.error("The file has inconsistent column count '" + ffname + "' row=" + columns.ToString() + " column=" + cnt.ToString());
                            return;
                        }
                    }
                    cnt = 0;
                    foreach ( string s in ss)
                    {
                        if (s.Trim() != "")
                        {
                            double d;
                            try
                            {
                                d = Double.Parse(s);
                            }
                            catch (Exception e)
                            {
                                userInterface.error("The file '" + ffname + "' contains an invalid number " + e.ToString());
                                userInterface.error("The file '" + ffname + "' row="+counter.ToString()+" column="+cnt.ToString());
                                return;
                            }
                            data[counter][cnt] = d;
                            cnt++;
                        }
                    }                       

                    counter++;
                }
            }
            file.Close();
        }


        /// <summary>
        /// splits this data into outputDC and remainderDC numInOutput of them go into into outputdc, the remainder into remainderDC
        /// </summary>
        /// <param name="outputDC"></param>
        /// <param name="remainderDC"></param>
        /// <param name="numInOutput"></param>
        /// <param name="rnd"></param>
        public void extractSplit(out DataClass outputDC, out DataClass remainderDC, int numInOutput, Random rnd)
        {
            
            //if (outputDC == null) outputDC = 
            int totRows = data.Length;
            int numCols = data[0].Length;

            //int trainRows = (int)(totRows * 0.80); // hard-coded 80-20 split
            //int testRows = totRows - trainRows;

            outputDC = new DataClass(numInOutput, userInterface);
            remainderDC = new DataClass(rows - numInOutput, userInterface);

            outputDC.columns = columns;
            remainderDC.columns = columns;

            int[] sequence = new int[totRows]; // create a random sequence of indexes
            for (int i = 0; i < sequence.Length; ++i) sequence[i] = i;
                
            for (int i = 0; i < sequence.Length; i++)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }

          
            int j = 0; // index 

            for (int i=0; i < sequence.Length; i++) // first rows to train data
            {
                if (i < numInOutput)
                {
                    outputDC.setRow(i, data[sequence[i]]);
                }
                else
                {
                    remainderDC.setRow(j, data[sequence[i]]);
                    j++;
                }
            }
        }


        //  normalises a list of colums eg to normalise the first 4 columns use - Normalize(0,3);
        //  column numbers start at zero and are inclusive
        //  stores info to repeat in the file  
        // normalize specified cols by computing (x - mean) / sd for each value
        public void normalize(int firstCol, int lastCol, string fileName)
        {
            // store normalisation parameters (x - mean) / sd for each value
            normMin = new double[columns];
            normMax = new double[columns];
            normMul = new double[columns];

            for (int i = firstCol; i <= lastCol; i++)
            {
                normMul[i] = 1;
                normMin[i] = 0;
                normMax[i] = 0; // not actually needed but may be of interest later
            }

            for (int i = firstCol; i<=lastCol; i++)
            {
                // for column i find min and max 
                double min = data[0][i];
                double max = data[0][i];
                normMin[i] = min;
                normMax[i] = max;
                normMul[i] = 1;

                for (int j = 0; j < rows; j++)
                {
                    double d = data[j][i];
                    if (d < min) { min = d; normMin[i] = min;}
                    if (d > max) { max = d; normMax[i] = max;}
                }

                double mul;
                if (max - min > 0.0001) mul = 1 / (max - min); else mul = 1;
                normMul[i] = mul;

                for (int j = 0; j < rows; j++)
                {
                    double d = data[j][i];
                    d = (d - min) * mul;
                    data[j][i] = d;
                }
            }

            if (fileName != "")
            {
                StreamWriter writer = null;
                writer = new StreamWriter(fileName);

                string normMaxS = NeuralNetwork.vectorToString(normMax, 5);
                string normMinS = NeuralNetwork.vectorToString(normMin, 5);
                string normMulS = NeuralNetwork.vectorToString(normMul, 5);

                writer.WriteLine(normMinS);
                writer.WriteLine(normMaxS);
                writer.WriteLine(normMulS);

                writer.Close();
            }
        }

        //  normalises a list of colums eg to normalise the first 4 columns use - Normalize(0,3);
        //  column numbers start at zero and are inclusive
        // uses stored info to repeat in another file  
        // normalize specified cols by computing (x - mean) / sd for each value
        public void normalizeSame(int firstCol, int lastCol)
        {
            for (int i = firstCol; i <= lastCol; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    double d = data[j][i];
                    d = (d - normMin[i]) * normMul[i];
                    data[j][i] = d;
                }
            }
        }

        //  normalises a list of colums eg to normalise the first 4 columns use - Normalize(0,3);
        //  column numbers start at zero and are inclusive
        // uses stored info to repeat in another file  
        // normalize specified cols by computing (x - mean) / sd for each value
        public void normalizeUsingFile(int firstCol, int lastCol, string fileName)
        {

            //normMin = new double[columns];
            //normMax = new double[columns];
            //normMul = new double[columns];

            string normMaxS;
            string normMinS;
            string normMulS;

            StreamReader reader = null;

            try
            {
                reader = new StreamReader(fileName);
                normMinS = reader.ReadLine();
                normMaxS = reader.ReadLine();
                normMulS = reader.ReadLine();
                reader.Close();
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("An error ocurred while executing the normalisation import: {0}", e.Message), e);
            }

            //string normMaxS = NeuralNetwork.vectorToString(normMax, 5);
            //string normMinS = NeuralNetwork.vectorToString(normMin, 5);
            //string normMulS = NeuralNetwork.vectorToString(normMul, 5);
            normMin = NeuralNetwork.stringToVector(normMinS, columns, userInterface, delimit);
            normMax = NeuralNetwork.stringToVector(normMaxS, columns, userInterface, delimit);
            normMul = NeuralNetwork.stringToVector(normMulS, columns, userInterface, delimit);

            for (int i = firstCol; i <= lastCol; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    double d = data[j][i];
                    d = (d - normMin[i]) * normMul[i];
                    data[j][i] = d;
                }
            }
        }

        public string showData()
        {
            string retv = "Rows="+rows.ToString()+" Columns="+columns.ToString()+"\r\n";
            for (int i = 0; i < rows; ++i)
            {
                for (int c = 0; c < columns; c++)
                {
                    retv = retv + data[i][c].ToString("F4") +" ";  
                }
                retv = retv + "\r\n";
            }
            return retv;
        }

        /// <summary>
        /// Shows the selected first rows and collums of data usually for checking purposes 
        /// fmt is the format usually "F4"
        /// </summary>
        /// <param name="numRows"></param>
        /// <param name="numColumns"></param>
        /// <param name="fmt"></param>
        /// <param name="heading"></param>
        /// <returns></returns>
        public string showDataPart(int numRows, int numColumns, string fmt, string heading)
        {
            string retv = "";
            if (heading.Trim() != "") retv = retv + heading + "\r\n";
            retv = retv + "Rows=" + rows.ToString() + " Columns=" + columns.ToString() + "\r\n";
            retv = retv + "Show Rows=" + numRows.ToString() + " Show Columns=" + numColumns.ToString() + "\r\n";
            for (int i = 0; i < numRows; ++i)
            {
                for (int c = 0; c < numColumns; c++)
                {
                    retv = retv + data[i][c].ToString(fmt) + " ";
                }
                retv = retv + "\r\n";
            }
            return retv;
        }

        public void writeToFile(string dir, string fname)
        {
            string linez;
            string ffname = dir + fname;
            using (StreamWriter writer = new StreamWriter(ffname))
            {
                
                for (int i = 0; i < rows; ++i)
                {
                    linez = "";
                    for (int c = 0; c < columns; c++)
                    {
                        linez = linez + data[i][c].ToString("F4") + " ";
                    }
                    linez = linez + "\r\n";
                    writer.Write(linez);
                }
            }
        }

            /// <summary>
            /// from the original code - not really tested
            /// </summary>
            /// <param name="numRows"></param>
            /// <param name="decimals"></param>
            /// <param name="newLine"></param>
            void ShowMatrixToConsole(int numRows, int decimals, bool newLine)
        {
            for (int i = 0; i < numRows; ++i)
            {
                Console.Write(i.ToString().PadLeft(3) + ": ");
                for (int j = 0; j < data[i].Length; ++j)
                {
                    if (data[i][j] >= 0.0) Console.Write(" "); else Console.Write("-");
                    Console.Write(Math.Abs(data[i][j]).ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine == true) Console.WriteLine("");
        }


    }
}
