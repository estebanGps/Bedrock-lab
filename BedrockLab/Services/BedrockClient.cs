using System.Text;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.Documents;
using BedrockLab.Models;

namespace BedrockLab.Services;

public class BedrockClient
{
    private readonly AmazonBedrockRuntimeClient _bedrockRuntimeClient;

    public BedrockClient(AmazonBedrockRuntimeClient bedrockRuntimeClient)
    {
        _bedrockRuntimeClient = bedrockRuntimeClient;
    }

    public async Task<AiResponse> CallModel(string modelId, string systemPrompt, List<Message> messages)
    {
        int callsCounter = 5; //get this from settings
        long latestTokenCount = 0;
        List<Message> newMessages = [];
        ConverseResponse? response;
        do
        {
            ConverseRequest request = new()
            {
                ModelId = modelId,
                Messages = messages,
                System = [new SystemContentBlock() { Text = systemPrompt }],
                InferenceConfig = new InferenceConfiguration { Temperature = 0.3F, MaxTokens = 2000 },
                ToolConfig = new ToolConfiguration { Tools = ToolRegistry.GetAllTools().ToList() }
            };
            try
            {
                response = await _bedrockRuntimeClient.ConverseAsync(request);
            }
            catch (Exception ex)
            {
                return new AiResponse { Error = ex.Message };
            }
            if (response == null || response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return new AiResponse { Error = "Failed to get a valid response from the model." };
            }
            Message newMessage = response.Output.Message;
            messages.Add(newMessage);
            newMessages.Add(newMessage);
            callsCounter--;

            if (response.Usage.TotalTokens.HasValue)
            {
                latestTokenCount = response.Usage.TotalTokens.Value;
            }

            if (response.StopReason == StopReason.Tool_use)
            {
                Message toolResultMessage = await ExecuteTool(response.Output.Message);
                messages.Add(toolResultMessage);
            }

        } while (response != null && response.StopReason != StopReason.End_turn && callsCounter >= 0);        

        string aiResponseText = ExtractTextFromMessages(newMessages);

        return new AiResponse
        {
            Text = aiResponseText,
            TotalTokenCount = latestTokenCount
        };
    }

    private static async Task<Message> ExecuteTool(Message message)
    {
        List<ContentBlock> toolResults = [];
        foreach (ContentBlock contentBlock in message.Content)
        {
            if (contentBlock.ToolUse is null)
                continue;

            Document toolResult = ToolExecutor.ExecuteTool(contentBlock.ToolUse.Name, contentBlock.ToolUse.Input);

            toolResults.Add(new ContentBlock
            {
                ToolResult = new ToolResultBlock
                {
                    ToolUseId = contentBlock.ToolUse.ToolUseId,
                    Content = [ new ToolResultContentBlock { Json = toolResult } ]
                }
            });
        }
        return new Message() { Role = ConversationRole.User, Content = toolResults };
    }

    private static string ExtractTextFromMessages(List<Message> messagesFromModel)
    {
        StringBuilder result = new();
        foreach (Message message in messagesFromModel)
        {
            foreach (ContentBlock contentBlock in message.Content)
            {
                if (contentBlock.Text is not null)
                {
                    result.AppendLine(contentBlock.Text);
                }
            }
        }
        return result.ToString();
    }
}