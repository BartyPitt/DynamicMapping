using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicMapping
{
    internal class mapHandler
    {
        public static string Map(object Data , string sourceType , string targetType , string context)
        {
            if (!Enum.TryParse(sourceType , out Seralizers sourceTypeEnum))
            {
                throw new NotSupportedException($"Do not support source Type {sourceType}");
            }
            if (!Enum.TryParse(targetType, out Seralizers targetTypeEnum))
            {
                throw new NotSupportedException($"Do not support target Type {targetType}");
            }
            return Map(Data , sourceTypeEnum, targetTypeEnum, context);
        }

        public static string Map(object Data , Seralizers sourceType , Seralizers targetType , string context)
        {

        }


        public enum Seralizers
        {
            Xml,
            Json,
            PredifinedModel,
        }
    }
}
