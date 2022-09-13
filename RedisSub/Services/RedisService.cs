// using System.Text.Json;
// using RedisSub.Dto;
// using RedisSub.Entities;
// using RedisSub.Repositories;
// using StackExchange.Redis;
//
// namespace RedisSub.Services;
//
// public class RedisService : IRedisService
// {
//     
//     private readonly IRedisSettings _redisSettings;
//     private readonly ISubscriber _subscriber;
//     private readonly LogRepository _logRepository;
//     public RedisService(IRedisSettings redisSettings, LogRepository logRepository)
//     {
//         _redisSettings = redisSettings;
//         _logRepository = logRepository;
//         var connection = ConnectionMultiplexer.Connect(_redisSettings.ConnectionString);
//         _subscriber = connection.GetSubscriber();
//     }
//
//     public async Task Subscribe()
//     {
//         await _subscriber.SubscribeAsync(_redisSettings.ChannelName, (channel, msg) =>
//         {
//             Console.WriteLine("message came");
//             var dto = JsonSerializer.Deserialize<CustomerForLogDto>(msg!);
//             var log = new Log
//             {
//                 Id = Guid.NewGuid().ToString(),
//                 LogMessage = $"Customer was {dto?.Action}.  {dto}"
//             };
//             _logRepository.AddAsync(log);
//
//         });
//     }
//     
// }