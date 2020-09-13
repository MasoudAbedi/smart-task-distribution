using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace Failure_prevention
{
    class Program
    {
	\\delays are set
        public static double FDelay = 1.4;
        public static double CDelay = 2.6;
        static void Main(string[] args)
        {
            string[] Save1 = new string[210];
	\\TSWC is set 
            for (int kk = 1; kk < 11; kk++)
            {
		\\the definitions of functions 
                string[] F1_30size = new string[200];
                string[] F1_30time = new string[200];
                string[] F1_50size = new string[200];
                string[] F1_50time = new string[200];
                string[] F1_70size = new string[200];
                string[] F1_70time = new string[200];

                string[] F2_30size = new string[200];
                string[] F2_30time = new string[200];
                string[] F2_50size = new string[200];
                string[] F2_50time = new string[200];
                string[] F2_70size = new string[200];
                string[] F2_70time = new string[200];


                string[] C1_30size = new string[200];
                string[] C1_30time = new string[200];
                string[] C1_50size = new string[200];
                string[] C1_50time = new string[200];
                string[] C1_70size = new string[200];
                string[] C1_70time = new string[200];

                string[] F1_30sizeR = new string[200];
                string[] F1_30timeR = new string[200];
                string[] F1_50sizeR = new string[200];
                string[] F1_50timeR = new string[200];
                string[] F1_70sizeR = new string[200];
                string[] F1_70timeR = new string[200];

                string[] F2_30sizeR = new string[200];
                string[] F2_30timeR = new string[200];
                string[] F2_50sizeR = new string[200];
                string[] F2_50timeR = new string[200];
                string[] F2_70sizeR = new string[200];
                string[] F2_70timeR = new string[200];


                string[] C1_30sizeR = new string[200];
                string[] C1_30timeR = new string[200];
                string[] C1_50sizeR = new string[200];
                string[] C1_50timeR = new string[200];
                string[] C1_70sizeR = new string[200];
                string[] C1_70timeR = new string[200];

		\\real and estimated size and response times are loaded
                Read_DATA_All(Application.StartupPath + "\\In\\F1\\30.txt", F1_30sizeR, F1_30timeR, F1_30size, F1_30time, 1, 1);
                Read_DATA_All(Application.StartupPath + "\\In\\F1\\30.txt", F1_50sizeR, F1_50timeR, F1_50size, F1_50time, 1, 1);
                Read_DATA_All(Application.StartupPath + "\\In\\F1\\30.txt", F1_70sizeR, F1_70timeR, F1_70size, F1_70time, 1, 1);

                Read_DATA_All(Application.StartupPath + "\\In\\F2\\30.txt", F2_30sizeR, F2_30timeR, F2_30size, F2_30time, 1, 1);
                Read_DATA_All(Application.StartupPath + "\\In\\F2\\30.txt", F2_50sizeR, F2_50timeR, F2_50size, F2_50time, 1, 1);
                Read_DATA_All(Application.StartupPath + "\\In\\F2\\30.txt", F2_70sizeR, F2_70timeR, F2_70size, F2_70time, 1, 1);

                Read_DATA_All(Application.StartupPath + "\\In\\C\\30.txt", C1_30sizeR, C1_30timeR, C1_30size, C1_30time, 1, 1);
                Read_DATA_All(Application.StartupPath + "\\In\\C\\50.txt", C1_50sizeR, C1_50timeR, C1_50size, C1_50time, 1, 1);
                Read_DATA_All(Application.StartupPath + "\\In\\C\\70.txt", C1_70sizeR, C1_70timeR, C1_70size, C1_70time, 1, 1);

                //The workloads are randomly set
                string[] randNumberF1 = new string[200];
                string[] randNumberF2 = new string[200];
                string[] randNumberC1 = new string[200];
   
                //definitions of variables that will be used later
                string[] AllC1 = new string[200];
                double SumC1 = 0;
                string[] RandomF1F2 = new string[200];
                Random RandVal = new Random();
                string[] RandomF1F2C1 = new string[200];
                double SumC1_In_F1F2C1 = 0;
                //the workloads are loaded
                Read_DATA_All(Application.StartupPath + "\\RandomUseCPU_F1N" + kk.ToString() + ".txt", randNumberF1, null, null, null, 2, 1);
                Read_DATA_All(Application.StartupPath + "\\RandomUseCPU_F2N" + kk.ToString() + ".txt", randNumberF2, null, null, null, 2, 1);
                Read_DATA_All(Application.StartupPath + "\\RandomUseCPU_C1N" + kk.ToString() + ".txt", randNumberC1, null, null, null, 2, 1);

		\\the processing times are set with regard to the workloads of servers

                double F1 = 0, F2 = 0, C1 = 0, FR1 = 0, FR2 = 0, CR1 = 0;
                int F1Count = 0, F2Count = 0, C1Count = 0;
                double SizeC = 0;
                double[] timeRun = new double[200];
                for (int i = 0; i < randNumberF1.LongLength; i++)
                {
                    switch (randNumberF1[i])
                    {
                        case "1":
                            FR1 = Convert.ToDouble(F1_30timeR[i]);
                            F1 = Convert.ToDouble(F1_30time[i]);
                            break;
                        case "2":
                            FR1 = Convert.ToDouble(F1_50timeR[i]);
                            F1 = Convert.ToDouble(F1_50time[i]);

                            break;
                        case "3":
                            FR1 = Convert.ToDouble(F1_70timeR[i]);
                            F1 = Convert.ToDouble(F1_70time[i]);

                            break;
                    }
                    switch (randNumberF2[i])
                    {
                        case "1":
                            FR2 = Convert.ToDouble(F2_30timeR[i]);
                            F2 = Convert.ToDouble(F2_30time[i]);

                            break;
                        case "2":
                            FR2 = Convert.ToDouble(F2_50timeR[i]);
                            F2 = Convert.ToDouble(F2_50time[i]);

                            break;
                        case "3":
                            FR2 = Convert.ToDouble(F2_70timeR[i]);
                            F2 = Convert.ToDouble(F2_70time[i]);

                            break;
                    }
                    switch (randNumberC1[i])
                    {
                        case "1":
                            CR1 = Convert.ToDouble(C1_30timeR[i]);
                            C1 = Convert.ToDouble(C1_30time[i]);
                            break;
                        case "2":
                            CR1 = Convert.ToDouble(C1_50timeR[i]);
                            C1 = Convert.ToDouble(C1_50time[i]);
                            break;
                        case "3":
                            CR1 = Convert.ToDouble(C1_70timeR[i]);
                            C1 = Convert.ToDouble(C1_70time[i]);


                            break;
                    }
                    F1 += FDelay;
                    F2 += FDelay;
                    C1 += CDelay;
                    FR1 += FDelay;
                    FR2 += FDelay;
                    CR1 += CDelay;
                    //////////////////////////////////////////////////////////////////
                    AllC1[i] = CR1.ToString();
                    SumC1 += Convert.ToDouble(C1_30size[i]);
                    /////////////////////////////////////////////////////////////////
                    {
                        int valRand = RandVal.Next(1, 3);
                        if (valRand == 1)
                            RandomF1F2[i] = F1.ToString();
                        else
                            RandomF1F2[i] = F2.ToString();
                    }
                    /////////////////////////////////////////////////////////////////
                    {
                        int valRand = RandVal.Next(1, 4);
                        switch (valRand)
                        {
                            case 1:
                                RandomF1F2C1[i] = F1.ToString();
                                break;
                            case 2:
                                RandomF1F2C1[i] = F2.ToString();
                                break;
                            case 3:
                                RandomF1F2C1[i] = C1.ToString();
                                SumC1_In_F1F2C1 += Convert.ToDouble(F1_30size[i]);
                                break;
                        }

                    }
                    /////////////////////////////////////////////////////////////////
		\\ Task distribution with regard to predicted response times 
                    if (F1 <= F2 && F1 <= C1)
                    {
                        timeRun[i] = FR1;
                        F1Count++;
                    }
                    else
                        if (F2 <= F1 && F2 <= C1)
                        {
                            timeRun[i] = FR2;
                            F2Count++;
                        }
                        else
                        {
                            timeRun[i] = CR1;
                            C1Count++;
                            SizeC += Convert.ToDouble(C1_30sizeR[i]);
                        }
                }
                int ll = 0;
                Save1[ll++] += kk.ToString() + "   " + "   " + "   " + "   " + "   ";
                Save1[ll++] += "SizeC" + "   " + SizeC.ToString() + "      " + "SumC1_In_F1F2C1   " + SumC1_In_F1F2C1.ToString() + "   ";
                Save1[ll++] += "F1Count   " + F1Count.ToString() + "   " + "   " + "   " + "   ";
                Save1[ll++] += "F2Count   " + F2Count.ToString() + "   " + "   " +  "   " + "   ";
                Save1[ll++] += "C1Count   " + C1Count.ToString() + "   " + "   " + "   " + "   ";
                Save1[ll++] += "   " + "   " + "AllC1" + "   " + "RandomF1F2" + "   " + "RandomF1F2C1" + "   ";
                for (int i = 0; i < randNumberC1.Length; i++)
                    Save1[ll++] += "   " + timeRun[i] + "   " + AllC1[i] + "   " + RandomF1F2[i] + "   " + RandomF1F2C1[i] + "   ";
            }
            for (int i = 0; i <= 205; i++)
                Save(Save1[i], "\\out\\All.txt", 1);
        }
        public static void Read_DATA_All(String PathF, string[] Par1, string[] Par2, string[] Par3, string[] Par4, int mode, int indexFile)
        {

            string path = PathF;
            if (!File.Exists(path))
            {

                using (FileStream fs = File.Create(path))
                {
                    Byte[] info =
                        new UTF8Encoding(true).GetBytes("error in Write/Read NO:531");


                    fs.Write(info, 0, info.Length);
                }
            }
            int index = 0, indexLine = 0;

            string _temp = "";

            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    if (mode == 2)
                    {
                        Par1[index++] = s.ToString();
                        continue;
                    }

                    for (int k = 0; k < s.Length; k++)
                        if (s[k] != '\t')
                            _temp += s[k];
                        else
                        {

                            switch (indexLine)
                            {
                                case 0:
                                    Par1[index] = _temp;
                                    break;
                                case 1:
                                    Par2[index] = _temp;
                                    break;
                                case 2:
                                    Par3[index] = _temp;
                                    break;
                            }
                            _temp = "";
                            indexLine++;
                        }
                    Par4[index] = _temp;
                    _temp = "";
                    index++;
                    indexLine = 0;


                }
            }

        }
        public static void Save(string Data, string Path, int mood)
        {

            string path = Application.StartupPath + Path;
            /*  if (mood == 5)
              {
                  path = Application.StartupPath + "\\Temp"+Convert.ToString(n)+".bat";
                //  File.Delete(path);
                  // File.Create(path);
                //  return;
              }*/
            /*if (mood == 6)
            {
                path = Application.StartupPath + "\\Temp" + Convert.ToString(n) + ".bat";
                mood = 1;
            }*/


            //  sialog1.ShowDialog();
            //  string path = sialog1.FileName;

            // This text is added only once to the file.
            /* if (!File.Exists(path))
             {
                 // Create a file to write to.
                string[] createText = { "" };
                 File.WriteAllLines(path, createText);
             }*/

            // This text is always added, making the file longer over time
            // if it is not deleted.
            //  string appendText = textBox1.Text + Environment.NewLine;
            //  if (mood == 0||mood==2)
            // File.AppendAllText(path, Data);
            if (mood == 2)
            {

                File.Delete(path);
                // File.Create(path);
                return;
            }
            if (1 == mood)
                Data += Environment.NewLine;
            File.AppendAllText(path, Data);

            string[] readText = File.ReadAllLines(path);
            foreach (string s in readText)
            {
                Console.WriteLine(s);
            }
            //  Refresh();

            // Open the file to read from.

            /* string[] readText = File.ReadAllLines(path);

             foreach (string s in readText)
             {
                          
                 Console.WriteLine(s);
                          
             }*/

            //  MessageBox.Show("Chenging Password.", "Admin", MessageBoxButtons.OK);
        }
        public static Random RandNo = new Random();
        public static int RandomGen(int N1, int N2, int N3)
        {


            int N = RandNo.Next(1, 12);
            if (N < N1)
                return 1;
            else
                if (N < N2)
                    return 2;
                else
                    return 3;

        }
    }
}
