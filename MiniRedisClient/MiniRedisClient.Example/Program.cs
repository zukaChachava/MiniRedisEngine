// See https://aka.ms/new-console-template for more information

using MiniRedisClient.Abstractions;
using MiniRedisClient.Core;

IMiniRedisClient client = new RedisClient();

await client.AddAsync("zura", "chachava");