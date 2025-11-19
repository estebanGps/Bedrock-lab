using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using BedrockLab;
using BedrockLab.Models;
using BedrockLab.Services;

BedrockClient bedrockClient = new(
    new AmazonBedrockRuntimeClient(new AmazonBedrockRuntimeConfig
    {
        RegionEndpoint = RegionEndpoint.USWest2
    }),
new ToolExecutor());

List<Message> messages = [ new Message(){ Role = ConversationRole.User, Content = [new() { Text = "How many assets do I have?"} ]}];

AiResponse aiResponse = await bedrockClient.CallModel("openai.gpt-oss-20b-1:0", SystemPrompts.SystemPrompt, messages);

if (!string.IsNullOrEmpty(aiResponse.Error))
{
    Console.Error.WriteLine(aiResponse.Error);
    return;
}

Console.WriteLine("AI Response:" + aiResponse.Text);