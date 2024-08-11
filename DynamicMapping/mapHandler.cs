using System.Reflection;
using DynamicMapping.Models;
using DynamicMapping.Serializers;

namespace DynamicMapping
{
    /// <summary>
    /// A class to handle the mapping form one format to another.
    /// </summary>
    public class MapHandler
    {
        /// <summary>
        /// A dictionary mapping the Serializes enum to constructors for the mapping classes.
        /// </summary>
        private static readonly Dictionary<Serializes, Func<IMapper>> MapperConstructors = new()
        {
            { Serializes.Json , ()=> new JsonMapper() },
            { Serializes.Xml , ()=> new XMLMapper() }
        };
        /// <summary>
        /// A mapping of the Models ENUM to the Model types.
        /// </summary>
        private static readonly Dictionary<Models, Type> contextsTypes = new()
        {
            { Models.Reservation , typeof(Reservation) }
        };

        /// <summary>
        /// Converts the Data object which is of type context between the source type to the target type. 
        /// </summary>
        /// <param name="Data">The object to be converted</param>
        /// <param name="sourceType">the format of the Data object</param>
        /// <param name="targetType">the output format desired of the object </param>
        /// <param name="context">The template that the data type follows.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">When the data type is not supported.</exception>
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
        /// <summary>
        /// Converts the Data object which is of type context between the source type to the target type. 
        /// </summary>
        /// <param name="Data">The object to be converted</param>
        /// <param name="sourceType">the format of the Data object</param>
        /// <param name="targetType">the output format desired of the object </param>
        /// <param name="context">The template that the data type follows.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">When there is an implimentaiton of that converter.</exception>
        public static object Map(object Data, Serializes sourceType, Serializes targetType, Models ContextType)
        {
            if (!contextsTypes.TryGetValue(ContextType, out Type? context))
            {
                throw new NotImplementedException($"context {ContextType} not Implemented");
            }

            Data = Deserialize(Data, sourceType, context);
            return Serialize(Data, targetType);
        }

        /// <summary>
        /// Converts the input data to the neutral format (the predefined model)
        /// </summary>
        /// <param name="Data">The Input data to be converted</param>
        /// <param name="sourceType">The type of the input data</param>
        /// <param name="Context">The template that the data object follows</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">When there is a failure to convert the data to a string.</exception>
        /// <exception cref="NotImplementedException"></exception>
        private static object Deserialize(object Data, Serializes sourceType, Type Context)
        {
            if (sourceType == Serializes.PredefinedModel)
            {
                return Data;
            }

            string? DataString = (Data?.ToString()) ?? throw new ArgumentException("Input data cannot be parsed into string");
            if (!MapperConstructors.TryGetValue(sourceType, out var sourceSeralizer))
            {
                throw new NotImplementedException($"source Type {sourceType} not Implemented");
            }

            IMapper Deserializer = sourceSeralizer.Invoke();
            object? Output = Deserializer.Deserialize(DataString ,Context);
            return Output;
        }
        /// <summary>
        /// Converts the input data form a neutral format into the desired output format.
        /// </summary>
        /// <param name="Data">The data in a neutral format</param>
        /// <param name="targetType">The Serializer type</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">The given Serializer has not be implemented</exception>
        /// <exception cref="Exception">If there is an error when reserializing the output</exception>
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
            return Output is null ? throw new Exception("Unable to ReSerialize output") : Output;
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
