using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using BedrockLab;
using BedrockLab.Models;
using BedrockLab.Services;

const string MODEL_ID = "openai.gpt-oss-20b-1:0";

BedrockClient bedrockClient = new(new AmazonBedrockRuntimeClient(new AmazonBedrockRuntimeConfig { RegionEndpoint = RegionEndpoint.USWest2 }));

List<Message> messages = [];

string userInput = GetUserInput();
while (!string.IsNullOrWhiteSpace(userInput))
{
    messages.Add(new Message() { Role = ConversationRole.User, Content = [new() { Text = userInput }] });

    AiResponse aiResponse = await bedrockClient.CallModel(MODEL_ID, SystemPrompts.SystemPrompt, messages);

    if (!string.IsNullOrEmpty(aiResponse.Error))
    {
        Console.Error.WriteLine(aiResponse.Error);
        break;
    }

    Console.WriteLine("AI Response: " + aiResponse.Text);
    userInput = GetUserInput();
}

static string GetUserInput()
{
    Console.WriteLine("Enter your message:");
    return Console.ReadLine() ?? string.Empty;
}