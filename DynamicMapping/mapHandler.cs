using System.Reflection;
using DynamicMapping.Models;
using DynamicMapping.Serializers;

namespace DynamicMapping
{
    internal class MapHandler
    {
        private static readonly Dictionary<Serializes, Func<IMapper>> MapperConstructors = new()
        {
            { Serializes.Json , ()=> new JsonMapper() },
            { Serializes.Xml , ()=> new XMLMapper() }
        };

        private static readonly Dictionary<Models, Type> contextsTypes = new()
        {
            { Models.Reservation , typeof(Reservation) }
        };


        public static object Map(object Data, string sourceType, string targetType, string context)
        {
            if (!Enum.TryParse(sourceType, true, out Serializes sourceTypeEnum))
            {
                throw new NotSupportedException($"Source type {sourceType} not supported");
            }
            if (!Enum.TryParse(targetType, true, out Serializes targetTypeEnum))
            {
                throw new NotSupportedException($"Target type {targetType} not supported");
            }
            if (!Enum.TryParse(context, out Models context_type)){
                throw new NotSupportedException($"Context type {context_type} not supported");
            }
            return Map(Data, sourceTypeEnum, targetTypeEnum, context_type);
        }

        public static object Map(object Data, Serializes sourceType, Serializes targetType, Models ContextType)
        {
            if (!contextsTypes.TryGetValue(ContextType, out Type? context))
            {
                throw new NotImplementedException($"context {ContextType} not Implimented");
            }

            Data = Deserialize(Data, sourceType, context);
            return Serialize(Data, targetType);
        }


        private static object Deserialize(object Data, Serializes sourceType, Type Context)
        {
            if (sourceType == Serializes.PredefinedModel)
            {
                return Data;
            }

            string? DataString = (Data?.ToString()) ?? throw new ArgumentException("Input data has evaluated to null");
            if (!MapperConstructors.TryGetValue(sourceType, out var sourceSeralizer))
            {
                throw new NotImplementedException($"source Type {sourceType} not Implimented");
            }

            IMapper Deserializer = sourceSeralizer.Invoke();
            object? Output = Deserializer.Deserialize(DataString ,Context);
            return Output;
        }

        private static object Serialize(object Data , Serializes targetType)
        {
            if(targetType == Serializes.PredefinedModel)
            {
                return Data;
            }
            if (!MapperConstructors.TryGetValue(targetType, out var targetSerializer))
            {
                throw new NotImplementedException($"target Type {targetType} not Implemented");
            }
            IMapper Serializer = targetSerializer.Invoke();
            object? Output = Serializer.Serialize(Data);
            return Output is null ? throw new Exception("Unable to Researlize output") : Output;
        }

        public enum Serializes
        {
            Xml,
            Json,
            PredefinedModel,
        }
        public enum Models
        {
            Reservation
        }
    }
}
