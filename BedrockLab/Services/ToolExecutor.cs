using System.Reflection;
using Amazon.Runtime.Documents;
using BedrockLab.Models;
using BedrockLab.Tools;

namespace BedrockLab.Services;

public class ToolExecutor
{
    public static Document ExecuteTool(string toolName, Document input)
    {
        ILookup<string, BedrockTool> allRegisteredTools = ToolRegistry.GetAllBedrockTools();
        if (!allRegisteredTools.Contains(toolName))
        {
            throw new ArgumentException($"Tool '{toolName}' is not registered.");
        }

        BedrockTool tool = allRegisteredTools[toolName].First();
        var instance = tool.MethodInfo.IsStatic ? null : Activator.CreateInstance(tool.MethodInfo.DeclaringType!);
        object[] parameters = ParseParameters(tool.MethodInfo, input);
        string toolResult = tool.MethodInfo.Invoke(instance, parameters) as string ?? string.Empty;
        return Document.FromObject(new { result = toolResult });
    }

    private static object[] ParseParameters(MethodInfo methodInfo, Document input)
    {
        List<object> result = [];
        ParameterInfo[]? orderedParameters = methodInfo.GetParameters()?.OrderBy(p => p.Position)?.ToArray();
        if (orderedParameters is null)
            return [];
        Dictionary<string, Document> inputData = input.AsDictionary();
        foreach (ParameterInfo param in orderedParameters)
        {
            var paramDescriptionAttr = param.GetCustomAttribute<BedrockToolParamAttribute>();
            string paramName = paramDescriptionAttr?.Name ?? param.Name!;
            if (!inputData.TryGetValue(paramName, out Document value))
            {
                throw new ArgumentException($"Missing required parameter '{paramName}' for tool '{methodInfo.Name}'.");
            }
            object paramValue = GetParamterValue(param.ParameterType, value);
            result.Add(paramValue);
        }
        return result.ToArray();
    }

    private static object GetParamterValue(Type parameterType, Document document)
    {
        return Type.GetTypeCode(parameterType) switch
        {
            TypeCode.Int32 => document.AsInt(),
            TypeCode.String => document.AsString(),
            TypeCode.Boolean => document.AsBool(),
            TypeCode.Double => document.AsDouble(),
            TypeCode.Single => (float)document.AsDouble(),
            TypeCode.Byte => (byte)document.AsInt(),
            TypeCode.Int16 => (short)document.AsInt(),
            TypeCode.Int64 => document.AsLong(),
            TypeCode.Decimal => (decimal)document.AsDouble(),
            TypeCode.DateTime => DateTime.Parse(document.AsString()),
            _ => throw new NotSupportedException($"Parameter type '{parameterType.Name}' is not supported."),
        };
    }
}