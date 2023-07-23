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
var item = await client.AddAsync("mease", "zura");
Console.WriteLine(item);

try
{
    var result = await client.GetAsync<string>("mease");
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


await client.UpdateAsync("mease", "Updated Data");

try{
    System.Console.WriteLine(await client.GetAsync<string>("mease"));
}
catch(Exception ex){
    System.Console.WriteLine(ex.Message);
}


await client.RemoveAsync("mease");

try{
    System.Console.WriteLine(await client.GetAsync<string>("mease"));
}
catch(Exception ex){
    System.Console.WriteLine(ex.Message);
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

