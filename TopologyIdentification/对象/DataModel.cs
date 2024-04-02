using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyIdentification
{
    /// <summary>
    /// 基础数据.
    /// </summary>
    public class DataALL
    {
        //导入数据集合.
        public List<RawData> RawDatas = new List<RawData>();

        //计算用电序列
        public double[] Data;
        /// <summary>
        /// 表箱数据.
        /// </summary>
        public double[,] BranchData;

        /// <summary>
        /// 用户数据.
        /// </summary>
        public double[,] UserData;
    }

    /// <summary>
    /// 电表数据.
    /// </summary>
    public class DataMeter
    {
        /// <summary>
        /// 采集个数，一般是96点.
        /// </summary>
        public int PonitCount;

        /// <summary>
        /// 角色：分支表、用户表
        /// </summary>
        public string RoleType;

        /// <summary>
        /// 父节点.
        /// </summary>
        public DataMeter FatherDataMeter;

        /// <summary>
        /// 节点地址.
        /// </summary>
        public string Address;

        /// <summary>
        /// 在矩阵中的位置.
        /// </summary>
        public int RowIndex;

        /// <summary>
        /// 排序后的位置.
        /// </summary>
        public int Sequence;
    }
}
