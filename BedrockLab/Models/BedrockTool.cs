using System.Reflection;
using Amazon.BedrockRuntime.Model;

namespace BedrockLab.Models;

public class BedrockTool
{
    public required MethodInfo MethodInfo { get; set; }
    public required Tool Tool { get; set; }
}