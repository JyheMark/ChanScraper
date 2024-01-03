using System.Net.Http.Json;
using ChanScraper.ChanApi;
using ChanScraper.ChanApi.Extensions;
using ChanScraper.ChanApi.Models;
using Thread = ChanScraper.ChanApi.Models.Thread;

Console.Clear();
Console.Write("Enter board: ");
string? boardInput = Console.ReadLine();

Console.Clear();
Console.Write("Enter thread ID: ");
string? threadIdInput = Console.ReadLine();

Console.Clear();
Console.Write("Enter a download location: ");
string? downloadPath = Console.ReadLine();

int threadId = int.Parse(threadIdInput);

var chanClient = new ChanClient(new HttpClient());
HttpResponseMessage postResponse = await chanClient.GetPostsAsync(boardInput, threadId);

if (!postResponse.IsSuccessStatusCode)
    return;

var thread = await postResponse.Content.ReadFromJsonAsync<Thread>();

foreach (Post post in thread.Posts.Where(p => p.HasAttachment()))
{
    HttpResponseMessage imageResponse = await chanClient.GetImageAsync(boardInput, post);

    if (!imageResponse.IsSuccessStatusCode)
        continue;
    
    var imageFileName = $"{post.FileName}--{post.Time}{post.FileExtension}";
    
    Console.WriteLine($"Saving image {imageFileName}");

    byte[] bytes = await imageResponse.Content.ReadAsByteArrayAsync();
    
    await using FileStream write = File.OpenWrite(Path.Join(downloadPath, imageFileName));
    await write.WriteAsync(bytes);
}

Console.WriteLine("Completed");