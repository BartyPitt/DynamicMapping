using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMapping.Serializers
{
    internal interface IMapper
    {
        /// <summary>
        /// Takes in a string and turns it into a predefined model representation of the string
        /// </summary>
        /// <param name="InputObject">The string to be converted</param>
        /// <param name="type">The type to be converted to</param>
        /// <returns></returns>
        public object? Deserialize(string InputObject, Type type);
        /// <summary>
        /// Takes in an object and returns an serialized version in the format of the Mapper.
        /// </summary>
        /// <param name="InputObject">The input object to be converted.</param>
        /// <returns></returns>
        public string Serialize(object InputObject);
    }
}
