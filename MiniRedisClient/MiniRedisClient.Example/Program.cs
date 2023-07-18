// See https://aka.ms/new-console-template for more information

using MiniRedisClient.Abstractions;
using MiniRedisClient.Core;

IMiniRedisClient client = new RedisClient();

await client.AddAsync("pirveli", "chachava");
await client.AddAsync("meore", "chachava");
await client.AddAsync("mesame", "chachava");

await client.GetAsync<string>("zura");