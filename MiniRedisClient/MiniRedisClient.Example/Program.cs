// See https://aka.ms/new-console-template for more information

using MiniRedisClient.Abstractions;
using MiniRedisClient.Core;

IMiniRedisClient client = new RedisClient();

// var result = await client.AddAsync("pirveli", "chachava");
// Console.WriteLine(result);
//
// result = await client.AddAsync("meore", "chachava");
// Console.WriteLine(result);
//
// result = await client.AddAsync("mesame", "chachava");
// Console.WriteLine(result);

var x = await client.AddAsync("mease", "zura");
Console.WriteLine(x);

try
{
    var result = await client.GetAsync<string>("mease");
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    var result = await client.GetAsync<string>("pirveli");
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

