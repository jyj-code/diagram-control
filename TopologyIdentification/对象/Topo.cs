using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopologyIdentification
{
    public class Topo
    {
        /// <summary>
        /// Gets or sets 节点地址.
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// Gets or sets 层级.
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// Gets or sets 父节点Tei.
        /// </summary>
        public string FatherTei { get; set; }

        /// <summary>
        /// Gets or sets 本节点Tei.
        /// </summary>
        public string Tei { get; set; }

        /// <summary>
        /// Gets or sets 类型.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets 代理变更使用（0：总表；1：下挂）.
        /// </summary>
        public string InfoType { get; set; }

        /// <summary>
        /// Gets or sets 代理变更下挂时间.
        /// </summary>
        public string Psubmissiontime { get; set; }

        public string Name { get; set; }

        public string Right { get; set; }

        /// <summary>
        /// 总表、一级分支、二级分支、……表箱
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Gets 显示信息.
        /// </summary>
        public string Description
        {
            get 
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Tei:{Tei}");
                stringBuilder.AppendLine($"PCO_TEI:{FatherTei}");
                stringBuilder.AppendLine($"节点地址：{NodeAddress}");
                stringBuilder.AppendLine($"名称：{Name}");
                stringBuilder.AppendLine($"角色类型：{Type}");
                stringBuilder.AppendLine($"识别结果：{Right}");
                return stringBuilder.ToString(); 
            }
        }


    }
}
