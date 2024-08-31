var builder = DistributedApplication.CreateBuilder(args);

string modelName = "phi"; //"orca-mini", "mistral", "llama2", "codellama", "phi", or "tinyllama"
var ollama = builder.AddContainer("ollama", $"langchain4j/ollama-{modelName}")
       .WithVolume("/root/.ollama")
       .WithBindMount("./ollamaconfig", "/root/")
       .WithHttpEndpoint(port: 7876, targetPort: 11434, name: "OllamaOpenApiEndpointUri")
       ;

EndpointReference ollamaOpenApiEndpointUri = ollama.GetEndpoint("OllamaOpenApiEndpointUri");

var aiApiService =
    builder.AddProject<Projects.Aspire_MakeRagEasy_AiApiService>("aiapiservice")
    .WithEnvironment("OllamaOpenApiEndpointUri", ollamaOpenApiEndpointUri);

builder.AddProject<Projects.Aspire_MakeRagEasy_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(aiApiService)
    ;

builder.AddNpmApp("ollamawebui", "../OllamWebUi", "dev").WithEnvironment("BROWSER", "none").WithExternalHttpEndpoints();

builder.Build().Run();
