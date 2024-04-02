using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopologyIdentification.Properties;

namespace TopologyIdentification
{
    public class Correlation1
    {
        /// <summary>计算皮尔逊积差相关系数</summary>
        /// <param name="dataA">数据样本A.</param>
        /// <param name="dataB">数据样本B.</param>
        /// <returns>返回皮尔逊积差相关系数.</returns>
        public static double Pearson(IEnumerable<double> dataA, IEnumerable<double> dataB)
        {
            int n = 0;
            double r = 0.0;

            double meanA = 0;
            double meanB = 0;
            double varA = 0;
            double varB = 0;

            using (IEnumerator<double> ieA = dataA.GetEnumerator())
            using (IEnumerator<double> ieB = dataB.GetEnumerator())
            {
                while (ieA.MoveNext())
                {
                    if (!ieB.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException("dataB", "数组越界");
                    }

                    double currentA = ieA.Current;
                    double currentB = ieB.Current;

                    double deltaA = currentA - meanA;
                    double scaleDeltaA = deltaA / ++n;

                    double deltaB = currentB - meanB;
                    double scaleDeltaB = deltaB / n;

                    meanA += scaleDeltaA;
                    meanB += scaleDeltaB;

                    varA += scaleDeltaA * deltaA * (n - 1);
                    varB += scaleDeltaB * deltaB * (n - 1);
                    r += (deltaA * deltaB * (n - 1)) / n;
                }

                if (ieB.MoveNext())
                {
                    throw new ArgumentOutOfRangeException("dataA", "数组越界");
                }
            }

            return r / Math.Sqrt(varA * varB);
        }

        /// <summary>计算加权皮尔逊积差相关系数.</summary>
        /// <param name="dataA">数据样本A.</param>
        /// <param name="dataB">数据样本B.</param>
        /// <param name="weights">数据权重.</param>
        /// <returns>加权皮尔逊积差相关系数.</returns>
        public static double WeightedPearson(IEnumerable<double> dataA, IEnumerable<double> dataB, IEnumerable<double> weights)
        {
            int n = 0;

            double meanA = 0;
            double meanB = 0;
            double varA = 0;
            double varB = 0;
            double sumWeight = 0;

            double covariance = 0;

            using (IEnumerator<double> ieA = dataA.GetEnumerator())
            using (IEnumerator<double> ieB = dataB.GetEnumerator())
            using (IEnumerator<double> ieW = weights.GetEnumerator())
            {
                while (ieA.MoveNext())
                {
                    if (!ieB.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException("dataB", "数组越界");
                    }
                    if (!ieW.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException("weights", "数组越界");
                    }
                    ++n;

                    double xi = ieA.Current;
                    double yi = ieB.Current;
                    double wi = ieW.Current;

                    double temp = sumWeight + wi;

                    double deltaX = xi - meanA;
                    double rX = deltaX * wi / temp;
                    meanA += rX;
                    varA += sumWeight * deltaX * rX;

                    double deltaY = yi - meanB;
                    double rY = deltaY * wi / temp;
                    meanB += rY;
                    varB += sumWeight * deltaY * rY;

                    sumWeight = temp;

                    covariance += deltaX * deltaY * (n - 1) * wi / n;
                }
                if (ieB.MoveNext())
                {
                    throw new ArgumentOutOfRangeException("dataB", "数组越界");
                }
                if (ieW.MoveNext())
                {
                    throw new ArgumentOutOfRangeException("weights", "数组越界");
                }
            }
            return covariance / Math.Sqrt(varA * varB);
        }

        /// <summary>计算皮尔逊积差相关矩阵</summary>
        /// <param name="vectors">数据矩阵</param>
        /// <returns>皮尔逊积差相关矩阵.</returns>
        public static Matrix<double> PearsonMatrix(params double[][] vectors)
        {
            var m = Matrix<double>.Build.DenseIdentity(vectors.Length);
            for (int i = 0; i < vectors.Length; i++)
            {
                for (int j = i + 1; j < vectors.Length; j++)
                {
                    var c = Pearson(vectors[i], vectors[j]);
                    m.At(i, j, c);
                    m.At(j, i, c);
                }
            }

            return m;
        }

        /// <summary> 计算皮尔逊积差相关矩阵</summary>
        /// <param name="vectors">数据集合.</param>
        /// <returns>皮尔逊积差相关矩阵.</returns>
        public static Matrix<double> PearsonMatrix(IEnumerable<double[]> vectors)
        {
            return PearsonMatrix(vectors as double[][] ?? vectors.ToArray());
        }

        /// <summary>
        /// 斯皮尔曼等级相关系数
        /// </summary>
        /// <param name="dataA">数据集A.</param>
        /// <param name="dataB">数据集B.</param>
        /// <returns>斯皮尔曼等级相关系数.</returns>
        public static double Spearman(IEnumerable<double> dataA, IEnumerable<double> dataB)
        {
            return Pearson(Rank(dataA), Rank(dataB));
        }

        /// <summary>
        /// 斯皮尔曼等级相关矩阵
        /// Computes the Spearman Ranked Correlation matrix.
        /// </summary>
        /// <param name="vectors">数据集.</param>
        /// <returns>斯皮尔曼等级相关矩阵.</returns>
        public static Matrix<double> SpearmanMatrix(params double[][] vectors)
        {
            return PearsonMatrix(vectors.Select(Rank).ToArray());
        }

        /// <summary>计算斯皮尔曼等级相关矩阵</summary>
        /// <param name="vectors">数据集合.</param>
        /// <returns>斯皮尔曼等级相关矩阵.</returns>
        public static Matrix<double> SpearmanMatrix(IEnumerable<double[]> vectors)
        {
            return PearsonMatrix(vectors.Select(Rank).ToArray());
        }

        static double[] Rank(IEnumerable<double> series)
        {
            if (series == null)
            {
                return new double[0];
            }

            // WARNING: do not try to cast series to an array and use it directly,
            // as we need to sort it (inplace operation)

            var data = series.ToArray();
            return ArrayStatistics.RanksInplace(data, RankDefinition.Average);
        }
    }
}
