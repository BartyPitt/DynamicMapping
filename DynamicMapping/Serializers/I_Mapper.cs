using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMapping.Serializers
{
    internal interface IMapper
    {
        public object? Deserialize(string InputObject);
        public string Serialize(object InputObject);
    }
}
