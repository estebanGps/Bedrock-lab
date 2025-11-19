namespace BedrockLab.Tools;

[AttributeUsage(AttributeTargets.Method)]
public class BedrockToolAttribute(string name, string description) : Attribute
{
    public string Name { get; } = name;
    public string Description { get; } = description;
}

[AttributeUsage(AttributeTargets.Parameter)]
public class BedrockToolParamAttribute(string name, string description) : Attribute
{
    public string Name { get; } = name;
    public string Description { get; } = description;
}