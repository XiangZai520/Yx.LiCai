using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YxLiCai.Tools
{
    public class KeyValuePair
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

    }

    [XmlRoot(Namespace = "http://msdn.microsoft.com/vsdata/xsd/vsdh.xsd")]
    public class KeyValuePairList
    {
        [XmlElement("KeyValuePair")]
        public List<KeyValuePair> list { get; set; }
    }
}
