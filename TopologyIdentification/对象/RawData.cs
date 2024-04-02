using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyIdentification
{
    /// <summary>
    /// 原始数据对象
    /// </summary>
    public class RawData
    {
        /// <summary>
        /// 数据标识
        /// </summary>
        public string DataIdentification { get; set; }

        /// <summary>
        /// 终端名称.
        /// </summary>
        public string TerminalName { get; set; }

        /// <summary>
        /// 测量点.
        /// </summary>
        public string MeasurePoint { get; set; }

        /// <summary>
        /// 电表名称.
        /// </summary>
        public string MeterName { get; set; }

        /// <summary>
        /// 电表地址.
        /// </summary>
        public string MeterAddress { get; set; }

        /// <summary>
        /// 数据时标.
        /// </summary>
        public string DataTimescale { get; set; }

        /// <summary>
        /// 电量数据.
        /// </summary>
        public double Electricity { get; set; }
    }
}
