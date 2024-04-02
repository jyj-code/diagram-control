using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopologyIdentification
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region 相关系数
            //先生成数据集合data
            var chiSquare = new ChiSquared(5);
            Console.WriteLine(@"2. Generate 1000 samples of the ChiSquare(5) distribution");
            var data = new double[1000];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = chiSquare.Sample();
            }

            //生成数据集合dataB
            var chiSquareB = new ChiSquared(2);
            var dataB = new double[1000];
            for (var i = 0; i < data.Length; i++)
            {
                dataB[i] = chiSquareB.Sample();
            }

            // 5. 计算data和dataB的相关系数
            var r1 = Correlation1.Pearson(data, dataB);
            var r2 = Correlation1.Spearman(data, dataB);
            #endregion

            #region 排列组合
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<List<int>>listResult=  Combination.GetCombinationList<int>(list, 3);
            for(int i=0;i<listResult.Count;i++)
            {
                for(int j=0;j<listResult[i].Count;j++)
                {
                    Console.Write(listResult[i][j]+" ");
                }
                Console.WriteLine();
            }
            #endregion

            #region 逆矩阵运算
            // 定义一个二维矩阵
            var matrix = Matrix<double>.Build.DenseOfArray(new double[,] {
                        { 1, 2, 3 },
                        { 3, 4, 4},
                        { 8, 3, 2 }
                    });
            var matrix2 = Matrix<double>.Build.DenseOfArray(new double[,] {
                        { 1, 2, 3 },
                        { 3, 4, 4},
                        { 8, 3, 2 }
                    });
            // 计算逆矩阵
            var inverse = matrix.Inverse();
            
            //矩阵转置
            var transpose = matrix.Transpose();

            //矩阵相乘
            var multiply = matrix * matrix2;

            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
