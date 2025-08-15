using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

var imagesFolder = "C:\\Users\\joao.ramos\\Repository\\MyTrainingGrounds\\AI\\AI.Tutorial\\images\\";

var builder = Host.CreateApplicationBuilder();

builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), "llava:7b"));

var app = builder.Build();

var chatClient = app.Services.GetRequiredService<IChatClient>();

var message = new ChatMessage(ChatRole.User, "Can you extract the text in the label?");
message.Contents.Add(new DataContent(File.ReadAllBytes($"{imagesFolder}/6882325_presunto-fatiado-lourenco-150g.jpeg"), "image/jpeg"));



//
// Normal Response
//

var response = await chatClient.GetResponseAsync(message);
Console.WriteLine(response.Text);

return;

//
// Structured Response
//

var name = "testing";
var message2 = new ChatMessage(ChatRole.User,
    $"""
    Extract Information from this image from camera {name}
    The image from the camera may be blurry, so take that into account when extracting information.
    Pay extra attention to columns of cars and try to accurately count the number of cars.
    And pay extra-special attention to pedestrians, as we want to make sure that we spot them.
    """);
message.Contents.Add(new DataContent(File.ReadAllBytes("images/traffic_cam_1.png"), "image/png"));

var trafficCamResult = await chatClient.GetResponseAsync<TrafficCamResult>(message2);
Console.WriteLine(trafficCamResult.Result with
{
    CameraName = name
});

public record TrafficCamResult
{
    public string CameraName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TrafficStatus Status { get; set; }

    public int NumberOfCars { get; set; }
    public int NumberOfTrucks { get; set; }
    public int NumberOfPedestrians { get; set; }

    public enum TrafficStatus
    {
        Empty,
        Normal,
        Heavy,
        Blocked
    }
}
