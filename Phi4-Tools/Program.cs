

using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;

using OllamaSharp.Models.Chat;
using System;
using System.ComponentModel;

var ip = "http://185.86.157.226:11434/";
#pragma warning disable SKEXP0070
IKernelBuilder builderllama = Kernel.CreateBuilder()
    .AddOllamaChatCompletion("phi4-mini:3.8b-fp16", new Uri(ip));
Kernel kernelLlama = builderllama.Build();
//kernelLlama.Plugins.AddFromType<WetterPlugin>();
kernelLlama.Plugins.AddFromFunctions("GetWetter");
//kernelLlama.Plugins.AddFromObject(new WetterPlugin());
#pragma warning disable SKEXP0001

PromptExecutionSettings settings01 = new()
{
        FunctionChoiceBehavior = FunctionChoiceBehavior.Required()
};
var result01 = await kernelLlama.InvokePromptAsync(
    "<|system|>You are a helpful assistant with using the provided tools.<|tool|>" +
    "[{\"name\": \"GetWetter\", \"description\": \"Weather details of today for a city as text.\"," +
    " \"parameters\": {\"city\": {\"description\": \"city\", \"type\": \"str\", \"default\": \"London\"}}}]<|/tool|><|end|><|user|>" +
    "Was ziehe ich heute draussen an in Wien?<|end|><|assistant|>",
    new KernelArguments(settings01)
);
Console.WriteLine(result01);
Console.WriteLine("------------------------------------------------------------------");







[KernelFunction("GetWetter"), Description("Weather details of today for a city as text")]
async Task<string> GetWetter([Description("city")] string Ort)
{
    var i = 0;

    return $"Das Wetter in Wien ist aktuell 10° bei Schneefall und Sonnenschein ";
}