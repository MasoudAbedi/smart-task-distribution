using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace model
{
    public class Program
    {
	\\The daly of communication between the users and servers are set 
          public static double FDelay = 1.4;
          public static double CDelay = 2.6;
     //   public static double FDelay = 0.0;
       // public static double CDelay = 0.0;
          public static int N;
        public static  void Main(string[] args)
        {
	\\This part limits the number of tasks in the broker to be between 2 and 10.
            for (int T = 2; T < 11; T++)
            {
	\\The real and estimated response times and sizes are read.
                string[] F_Parameters = new string[200];
                string[] C_Parameters = new string[200];

                string[] F_ParametersR = new string[200];
                string[] C_ParametersR = new string[200];

                double[,] FParameters = new double[200, 2];
                double[,] CParameters = new double[200, 2];

                double[,] FParametersR = new double[200, 2];
                double[,] CParametersR = new double[200, 2];
                Read_DATA_All(Application.StartupPath + "//F-Data.txt", F_Parameters, 1, 1);
                Read_DATA_All(Application.StartupPath + "//C-Data.txt", C_Parameters, 1, 1);

                Read_DATA_All(Application.StartupPath + "//F-DataR.txt", F_ParametersR, 1, 1);
                Read_DATA_All(Application.StartupPath + "//C-DataR.txt", C_ParametersR, 1, 1);

                convert2double(F_ParametersR, ref FParametersR);
                convert2double(C_ParametersR, ref CParametersR);

                convert2double(F_Parameters, ref FParameters);
                convert2double(C_Parameters, ref CParameters);

                N = T;
                double[,] F = new double[N, 2];
                double[,] C = new double[N, 2];

                double[,] FR = new double[N, 2];
                double[,] CR = new double[N, 2];
		\\ this part of the code divides the tasks into a number between 2 and 10, in order to be ready for distribution between the servers.

                for (int i = 0; i < (FParameters.Length / 2) - 0; i += N)
                {
                    {
                        int index = 0;
                        for (int j = i; j < (i + N); j++)
                        {
                            if (j == 200)
                                break;
                            F[index, 0] = (FParameters[j, 0] + CParameters[j, 0]) / 2;
                            F[index, 1] = FParameters[j, 1];

                            C[index, 0] = (CParameters[j, 0] + FParameters[j, 0]) / 2;
                            C[index, 1] = CParameters[j, 1];


                            FR[index, 0] = FParametersR[j, 0];
                            FR[index, 1] = FParametersR[j, 1];

                            CR[index, 0] = CParametersR[j, 0];
                            CR[index++, 1] = CParametersR[j, 1];
                        }
                    }
		\\ In this part the available tasks in the broker are sorted with regard to their estimated sizes.
                    SortBy(ref F, ref C, N, 0, ref FR, ref CR);
		\\ the best distribution possibility is selected here, some tasks will be sent to the cloud, and some others will be sent to the fog servers.
                    int select = SelectSortBySize(ref F, ref C, N);
		\\ After finding the best distribution the tasks will be ordered with regard to their predicted processing times.
                    SortByTime(ref C, 0, select, N, 1, ref CR);
                    SortByTime(ref F, select, N, N, 1, ref FR);
		\\ The response times of both fog and cloud servers are calculated and the higher one is selected as the totall response time.
                    {
                        double sunF = 0;
                        double sunC = 0;
                        for (int k = 0; k < select; k++)
                            sunC += CR[k, 1];
                        sunC += CDelay;
                        for (int k = select; k < N; k++)
                            sunF += FR[k, 1];
                        sunF += FDelay;
                        Save(sunF.ToString() + ' ' + sunC.ToString(),0);
                        if (sunC > sunF)
                            Save('\t' + sunC.ToString(),0);
                        else
                            Save('\t' + sunF.ToString(), 0);

                        //The Internet traffic is calculated as follows:
                        double sunCNew = 0;
                        for (int k = 0; k < select; k++)
                            sunCNew += CR[k, 0];
                        Save('\t' + sunCNew.ToString(), 0);
                        ////////////////////////////////////////////////
                        Save("",1);
                    }

                }
                Save("",1);
                Save("", 1);
                for (int i = 0; i < (FParameters.Length / 2) - 0; i += N)
                {
                    {
                        int index = 0;
                        for (int j = i; j < i + N; j++)
                        {
                            if (j == 200)
                                break;
                            F[index, 0] = FParameters[j, 0];
                            F[index, 1] = (CParameters[j, 1] + FParameters[j, 1]) / 2;

                            C[index, 0] = CParameters[j, 0];
                            C[index, 1] = (CParameters[j, 1] + FParameters[j, 1]) / 2;
                            ////////////////////////////////////////////////////////////
                            FR[index, 0] = FParametersR[j, 0];
                            FR[index, 1] = FParametersR[j, 1];

                            CR[index, 0] = CParametersR[j, 0];
                            CR[index++, 1] = CParametersR[j, 1];

                        }
                    }
                    //    Console.WriteLine((SUM(FR, 0, N, N) + FDelay).ToString());
                    {
			\\Calculation of internet traffic if all the data were sent to the cloud
                        double sunF = 0;
                        double sunC = 0;
                        int select = N;
                        for (int k = 0; k < select; k++)
                            sunC += CR[k, 0];
                     
                        for (int k = select; k < N; k++)
                            sunF += FR[k, 0];
                  
                        Save(sunC.ToString(),0);
          
                        Save("",1);
                    }
                }
                Save("",1);
                Save("", 1);
                for (int i = 0; i < (CParameters.Length / 2) - 0; i += N)
                {
                    {
                        int index = 0;
                        for (int j = i; j < i + N; j++)
                        {
                            if (j == 200)
                                break;
                            F[index, 0] = FParameters[j, 0];
                            F[index, 1] = (CParameters[j, 1] + FParameters[j, 1]) / 2;

                            C[index, 0] = CParameters[j, 0];
                            C[index, 1] = (CParameters[j, 1] + FParameters[j, 1]) / 2;

                            ////////////////////////////////////////////////////////////
                            FR[index, 0] = FParametersR[j, 0];
                            FR[index, 1] = FParametersR[j, 1];

                            CR[index, 0] = CParametersR[j, 0];
                            CR[index++, 1] = CParametersR[j, 1];
                        }
                    }
		\\ Calculation of response time for cloud-based and fog-based approaches
                    Save((SUM(CR, 0, N, N) + CDelay).ToString() + " " + (SUM(FR, 0, N, N) + FDelay).ToString(),1);
                    //  Console.WriteLine((SUM(FR, 0, N, N) + FDelay).ToString());
                }
            }
            Console.ReadKey();
        }

        public static int SelectSortBySize(ref double[,] F, ref double[,] C, int N)
        {
            double[] selectP = new double[N];
            double[] FF = new double[N];
            double[] CC = new double[N];
            int select=0;
            double C_Sum;
            double F_Sum=0;
            if (N == 1)
            {
                C_Sum = C[0,1] + CDelay;
                F_Sum = F[0, 1] + FDelay;
         /*       if (C_Sum > F_Sum)
                    Console.Write('\t' + "0" + '\t' + F_Sum.ToString());
                else
                    Console.Write('\t' + C[0, 0].ToString() + '\t' + C_Sum.ToString());
                Console.Write("\r\n");*/
                if (C_Sum > F_Sum)
                {
                    return 0;
                }
                return 1;
            }
            if (N > 1)
            {
                
                for (int i = 1; i < N; i++)
                {
                    C_Sum = SUM(C, 0, i, N) + CDelay;
                    F_Sum = SUM(F, i, N, N) + FDelay;
                    FF[i] = F_Sum;
                    CC[i] = C_Sum;
                    selectP[i] = Math.Abs(C_Sum - F_Sum);
                    
                }
                double min=selectP[1];
                select = 1;
                for(int i=2;i<N;i++)
                    if (min > selectP[i])
                    {
                        min = selectP[i];
                        select = i;
                    }

            }

        /*    Console.Write(FF[select].ToString() + '\t' + CC[select].ToString());
            if (CC[select] > FF[select])
                Console.Write('\t' + CC[select].ToString());
            else
                Console.Write('\t' + FF[select].ToString());
            Console.Write("\r\n");*/
            return select;
        }
        public static double SUM(double[,] Arr, int Start, int End, int N)
        {
            double Sum=0;
            for (int i = Start; i < End&& i<N; i++)
                Sum += Arr[i, 1];
            return Sum;
        }
        public static double SUMSize(double[,] Arr, int Start, int End, int N)
        {
            double Sum = 0;
            for (int i = Start; i < End && i < N; i++)
                Sum += Arr[i, 0];
            return Sum;
        }
        public static void SortBy(ref double[,] FParameters, ref double[,] CParameters, int N,int sizeOrTime,ref double[,] FParametersR, ref double[,] CParametersR)
        {
            double temp=0;
            for(int i=0;i<N;i++)
                for(int j=0;j<N;j++)
                    if (FParameters[i, sizeOrTime] < FParameters[j, sizeOrTime])
                    {
                        temp = FParameters[i, 0];
                        FParameters[i, 0] = FParameters[j, 0];
                        FParameters[j, 0] = temp;

                        temp = FParameters[i, 1];
                        FParameters[i, 1] = FParameters[j, 1];
                        FParameters[j, 1] = temp;

                        temp = CParameters[i, 0];
                        CParameters[i, 0] = CParameters[j, 0];
                        CParameters[j, 0] = temp;

                        temp = CParameters[i, 1];
                        CParameters[i, 1] = CParameters[j, 1];
                        CParameters[j, 1] = temp;

                        ///////////////////////////////////////
                        temp = FParametersR[i, 0];
                        FParametersR[i, 0] = FParametersR[j, 0];
                        FParametersR[j, 0] = temp;

                        temp = FParametersR[i, 1];
                        FParametersR[i, 1] = FParametersR[j, 1];
                        FParametersR[j, 1] = temp;

                        temp = CParametersR[i, 0];
                        CParametersR[i, 0] = CParametersR[j, 0];
                        CParametersR[j, 0] = temp;

                        temp = CParametersR[i, 1];
                        CParametersR[i, 1] = CParametersR[j, 1];
                        CParametersR[j, 1] = temp;

                    }

        }
        public static void SortByTime(ref double[,] Parameters, int strat, int end, int N, int sizeOrTime, ref double[,] ParametersR)
        {
            double temp = 0;
            for (int i = strat;i<end && i < N; i++)
                for (int j = strat; j<end&& j < N; j++)
                    if (Parameters[i, sizeOrTime] < Parameters[j, sizeOrTime])
                    {

                        temp = Parameters[i, 0];
                        Parameters[i, 0] = Parameters[j, 0];
                        Parameters[j, 0] = temp;

                        temp = Parameters[i, 1];
                        Parameters[i, 1] = Parameters[j, 1];
                        Parameters[j, 1] = temp;
                        //////////////////////////////////////
                        temp = ParametersR[i, 0];
                        ParametersR[i, 0] = ParametersR[j, 0];
                        ParametersR[j, 0] = temp;

                        temp = ParametersR[i, 1];
                        ParametersR[i, 1] = ParametersR[j, 1];
                        ParametersR[j, 1] = temp;

                    }

        }
        public static void convert2double(string[] Parameters, ref double[,] doubleParameters)
        {
            string temp = "";
            int index=0;
            for (int i = 0; i < Parameters.Length; i++)
            {
                index = 0;
                int j = 0;
                for (; j < Parameters[i].Length; j++)
                    if (Parameters[i][j] != '\t')
                        temp += Parameters[i][j];
                    else
                    {
                        doubleParameters[i, index++] = Convert.ToDouble(temp);
                        temp = "";
                    }
                if(Parameters[i].Length==j)
                {
                    doubleParameters[i, index++] = Convert.ToDouble(temp);
                    temp = "";
                }
            }
                
        }
        public static void Read_DATA_All(String PathF, String[] Parameters, int mode, int indexFile)
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
            int index = 0;

            // string _temp = "";

            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    // for (int k = 0; k < s.Length; k++)
                    //   if (s[k] != '\t')
                    Parameters[index++] = s;
                    // else
                    //   index++;

                }
            }

        }
        public static void Save(string Data, int mood)
        {

            string path = Application.StartupPath + "\\MyData" + N.ToString() + ".xls";
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
    }
}
