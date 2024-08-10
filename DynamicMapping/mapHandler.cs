using System.Reflection;
using DynamicMapping.Models;
using DynamicMapping.Serializers;

namespace DynamicMapping
{
    internal class mapHandler
    {
        private static readonly Dictionary<Seralizers, Func<I_Seralizer>> SeralizerClasses = new()
        {
            { Seralizers.Json , ()=> new jsonSeralizer() }
        };

        private static readonly Dictionary<Models, Type> contextsTypes = new()
        {
            { Models.Reservation , typeof(Reservation) }
        };


        public static object map(object Data, string sourceType, string targetType, string context)
        {
            if (!Enum.TryParse(sourceType, true, out Seralizers sourceTypeEnum))
            {
                throw new NotSupportedException($"Source type {sourceType} not supported");
            }
            if (!Enum.TryParse(targetType, true, out Seralizers targetTypeEnum))
            {
                throw new NotSupportedException($"Target type {targetType} not supported");
            }
            if (!Enum.TryParse(context, out Models context_type){
                throw new NotSupportedException($"Conext type {context_type} not supported");
            }
            return map(Data, sourceTypeEnum, targetTypeEnum, context_type);
        }

        public static object map(object Data, Seralizers sourceType, Seralizers targetType, Models ContextType)
        {
            if (!contextsTypes.TryGetValue(ContextType, out Type? context))
            {
                throw new NotImplementedException($"context {ContextType} not Implimented");
            }

            Data = Deseralize(Data, sourceType, context);

            I_Seralizer Seralizer = targetSeralizer.Invoke();
            return Seralize(Data, targetType);
        }


        private static object Deseralize(object Data, Seralizers sourceType, Type Context)
        {
            if (sourceType == Seralizers.PredifinedModel)
            {
                return Data;
            }

            string DataString = Data?.ToString();

            if (DataString is null)
            {
                throw new ArgumentException("Input data has evaluated to null");
            }
            if (!SeralizerClasses.TryGetValue(sourceType, out var sourceSeralizer))
            {
                throw new NotImplementedException($"source Type {sourceType} not Implimented");
            }

            I_Seralizer Deseralizer = sourceSeralizer.Invoke();
            object? Output = GenericDeseralize(Deseralizer, DataString, Context);
            if (Output is null)
            {
                throw new Exception("Unable to parse input data");
            }
            return Output;
        }

        private static object Seralize(object Data , Seralizers targetType)
        {
            if(targetType == Seralizers.PredifinedModel)
            {
                return Data;
            }
            if (!SeralizerClasses.TryGetValue(targetType, out var targetSeralizer))
            {
                throw new NotImplementedException($"target Type {targetType} not Implimented");
            }
            object Output = Seralize(Data, targetType);
            if (Output is null)
            {
                throw new Exception("Unable to Researlize output");
            }
            return Output;
        }

        public static string Seralize(object Data, Seralizers targetType)
        {
            if (!SeralizerClasses.TryGetValue(targetType, out var targetSeralizer))
            {
                throw new NotImplementedException($"source Type {targetType} not Implimented");
            }
            I_Seralizer Seralizer = targetSeralizer.Invoke();
            return Seralizer.Serialize(Data);
        }



        private static object? GenericDeseralize(I_Seralizer InputS, string Data, Type Context)
        {

            MethodInfo? genericMethod = InputS.GetType().GetMethod("Deseralize");
            MethodInfo? constructedMethod = genericMethod?.MakeGenericMethod(Context);
            return constructedMethod?.Invoke(InputS, [Data]);
        }

        private static string? Seralize(I_Seralizer InputS, object Data, Type Context)
        {

            MethodInfo? genericMethod = InputS.GetType().GetMethod("Serialize");
            MethodInfo? constructedMethod = genericMethod?.MakeGenericMethod(Context);
            return (string?)constructedMethod?.Invoke(InputS, [Data]);
        }

        public enum Seralizers
        {
            Xml,
            Json,
            PredifinedModel,
        }
        public enum Models
        {
            Reservation
        }
    }
}
