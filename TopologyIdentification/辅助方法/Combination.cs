using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyIdentification
{
    /// <summary>
    /// 组合.
    /// </summary>
    public class Combination
    {
        /// <summary>
        /// 获得从n个不同元素中任意选取m个元素的组合的所有组合形式的列表
        /// </summary>
        /// <param name="elements">供组合选择的元素</param>
        /// <param name="m">组合中选取的元素个数</param>
        /// <returns>返回一个包含列表的列表，包含的每一个列表就是每一种组合可能</returns>
        public static List<List<T>> GetCombinationList<T>(List<T> elements, int m)
        {
            List<List<T>> result = new List<List<T>>();//存放返回的列表
            List<List<T>> temp = null; //临时存放从下一级递归调用中返回的结果
            List<T> oneList = null; //存放每次选取的第一个元素构成的列表，当只需选取一个元素时，用来存放剩下的元素分别取其中一个构成的列表；
            T oneElment; //每次选取的元素
            List<T> source = new List<T>(elements); //将传递进来的元素列表拷贝出来进行处理，防止后续步骤修改原始列表，造成递归返回后原始列表被修改；
            int n = 0; //待处理的元素个数

            if (elements != null)
            {
                n = elements.Count;
            }
            if (n == m && m != 1)//n=m时只需将剩下的元素作为一个列表全部输出
            {
                result.Add(source);
                return result;
            }
            if (m == 1)  //只选取一个时，将列表中的元素依次列出
            {
                foreach (T el in source)
                {
                    oneList = new List<T>();
                    oneList.Add(el);
                    result.Add(oneList);
                    oneList = null;
                }
                return result;
            }

            for (int i = 0; i <= n - m; i++)
            {
                oneElment = source[0];
                source.RemoveAt(0);
                temp = GetCombinationList(source, m - 1);
                for (int j = 0; j < temp.Count; j++)
                {
                    oneList = new List<T>();
                    oneList.Add(oneElment);
                    oneList.AddRange(temp[j]);
                    result.Add(oneList);
                    oneList = null;
                }
            }


            return result;
        }
    }
}
