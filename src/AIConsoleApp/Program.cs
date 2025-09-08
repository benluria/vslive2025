/*
Run this model in C#.

> dotnet add package OpenAI
*/
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using Microsoft.Extensions.Configuration;

// To authenticate with the model you will need to generate a personal access token (PAT) in your GitHub settings. 
// Create your PAT token by following instructions here: https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens
// Secure token retrieval using .NET Secret Manager
var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var credential = configuration["GitHubToken"] 
    ?? throw new InvalidOperationException(
        "GitHub token not configured. Run: dotnet user-secrets set GitHubToken YOUR_TOKEN");

var openAIOptions = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.github.ai/inference")
};

var client = new ChatClient("openai/gpt-4o-mini", new ApiKeyCredential(credential), openAIOptions);


DisplayWelcomeMessage();

while (true)
{
    Console.Write("\nYour question: ");
    var userInput = Console.ReadLine();

    if (string.IsNullOrEmpty(userInput) || userInput.ToLower() == "exit")
        break;

    List<ChatMessage> messages = new List<ChatMessage>()
    {
        new SystemChatMessage("You are a helpful C# and .NET programming expert. Provide clear, concise answers with code examples when appropriate."),
        new UserChatMessage(userInput),
    };

    var requestOptions = new ChatCompletionOptions()
    {
        Temperature = 0.7f,
        MaxOutputTokenCount = 1000,
    };

    try
    {
        Console.WriteLine("\nAI Response:");
        Console.WriteLine("------------");
        var response = client.CompleteChat(messages, requestOptions);
        Console.WriteLine(response.Value.Content[0].Text);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static void DisplayWelcomeMessage()
{
    Console.WriteLine("VSLIVE! 2025 - AI Chat Console (Secure Version)");
    Console.WriteLine("===============================================");
    Console.WriteLine("Features:");
    Console.WriteLine("- Secure token management with .NET Secret Manager");
    Console.WriteLine("- Integration with GitHub Models API");
    Console.WriteLine("- Interactive chat interface");
    Console.WriteLine();
    Console.WriteLine("Ask me anything about C# and .NET! (type 'exit' to quit)");
}

Console.WriteLine("\nThanks for using the AI Chat Console!");