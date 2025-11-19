using System.Reflection;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.Documents;
using BedrockLab.Tools;

namespace BedrockLab
{
    internal class ToolsUtility
    {
        private static List<Tool>? _allTools;

        internal static List<Tool> GetAllTools()
        {
            _allTools ??= Scan();
            return _allTools;
        }

        private static List<Tool> Scan()
        {
            List<Tool> result = [];
            MethodInfo[] methods = Assembly.GetExecutingAssembly().GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic))
                .Where(m => m.GetCustomAttributes(typeof(BedrockToolAttribute), false).Length > 0)
                .ToArray();
            foreach (MethodInfo method in methods)
            {
                Tool tool = CreateTool(method);
                result.Add(tool);
            }
            return result;
        }

        private static Tool CreateTool(MethodInfo method)
        {
            BedrockToolAttribute attribute = (BedrockToolAttribute)method.GetCustomAttribute(typeof(BedrockToolAttribute))!;

            Document properties = new();
            List<string> required = [];
            foreach (ParameterInfo param in method.GetParameters())
            {
                string type = param.ParameterType.Name;
                var paramDescriptionAttr = param.GetCustomAttribute<BedrockToolParamAttribute>();
                string paramDescription = paramDescriptionAttr?.Description ?? "";
                string paramName = paramDescriptionAttr?.Name ?? param.Name!;
                bool isRequired = !param.IsOptional;

                properties.Add(paramName, Document.FromObject(new { type, description = paramDescription }));
                if (isRequired)
                {
                    required.Add(paramName);
                }
            }
            Document inputSchemaJson = Document.FromObject(new { type = "object" });
            if (!properties.IsNull())
            {
                inputSchemaJson.Add("properties", properties);
            }
            if (required.Count > 0)
            {
                inputSchemaJson.Add("required", Document.FromObject(required));
            }
            return new Tool()
            {
                ToolSpec = new ToolSpecification
                {
                    Name = attribute.Name,
                    Description = attribute.Description,
                    InputSchema = new() { Json = inputSchemaJson }
                }
            };
        }
    }
}
