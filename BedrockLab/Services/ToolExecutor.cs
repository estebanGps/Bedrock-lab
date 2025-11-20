using System.Reflection;
using System.Text.Json;
using Amazon.Runtime.Documents;
using BedrockLab.Models;
using BedrockLab.Tools;

namespace BedrockLab.Services;

public class ToolExecutor
{
    public static async Task<Document> ExecuteTool(string toolName, Document input)
    {
        ILookup<string, BedrockTool> allRegisteredTools = ToolRegistry.GetAllBedrockTools();
        if (!allRegisteredTools.Contains(toolName))
        {
            throw new ArgumentException($"Tool '{toolName}' is not registered.");
        }

        BedrockTool tool = allRegisteredTools[toolName].First();
        var instance = tool.MethodInfo.IsStatic ? null : Activator.CreateInstance(tool.MethodInfo.DeclaringType!);
        object[] parameters = ParseParameters(tool.MethodInfo, input);

        object? invokeResult = tool.MethodInfo.Invoke(instance, parameters);
        if (invokeResult == null)
        {
            return Document.FromObject(new { result = string.Empty });
        }
        // if invokeResult is a Task, await it to get the actual result
        if (IsAwaitable(invokeResult))
        {
            invokeResult = invokeResult.GetType().GetProperty("Result")!.GetValue(invokeResult);
        }

        string toolResult = invokeResult is string strResult
            ? strResult
            : invokeResult.ToString()!;
        return Document.FromObject(new { result = toolResult });
    }

    private static bool IsAwaitable(object invokeResult) =>  invokeResult.GetType().IsGenericType &&
        invokeResult.GetType().GetGenericTypeDefinition() == typeof(Task<>);

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
        if (parameterType == typeof(double) && document.IsDouble())
            return document.AsDouble();
        if (parameterType == typeof(double) && document.IsInt())
            return Convert.ToDouble(document.AsInt()); // the LLM sometimes sends integers for double parameters
        if (parameterType == typeof(int) && document.IsInt())
            return document.AsInt();
        if (parameterType == typeof(string) && document.IsString())
            return document.AsString();
        if (parameterType == typeof(bool) && document.IsBool())
            return document.AsBool();
        if (parameterType == typeof(DateTime) && document.IsString())
            return DateTime.Parse(document.AsString());
        throw new NotSupportedException($"Parameter type '{parameterType.Name}' is not supported.");
    }
}