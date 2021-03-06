﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HeckBot.Models;
using HeckBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HeckBot
{
    class Program
    {
        private const int TOTAL_SHARDS = 4;
        
        private ShieldService _shieldService;

        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            // You specify the amount of shards you'd like to have with the
            // DiscordSocketConfig. Generally, it's recommended to
            // have 1 shard per 1500-2000 guilds your bot is in.
            var config = new DiscordSocketConfig
            {
                TotalShards = TOTAL_SHARDS
            };

            // Dispose when app finishes.
            using (var services = ConfigureServices(config))
            {
                var client = services.GetRequiredService<DiscordShardedClient>();

                // The Sharded Client does not have a Ready event.
                // The ShardReady event is used instead, allowing for individual
                // control per shard.
                client.ShardReady += ReadyAsync;
                client.Log += LogAsync;

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                _shieldService = services.GetRequiredService<ShieldService>();

                // Tokens should be considered secret data, and never hard-coded.
                await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("HECKBOT_TOKEN"));
                await client.StartAsync();

                await Task.Delay(-1);
            }
        }

        private ServiceProvider ConfigureServices(DiscordSocketConfig config)
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(config))
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<PictureService>()
                .AddSingleton<ShieldService>()
                .AddSingleton<DbService>()
                .AddDbContext<HeckBotContext>()
                .BuildServiceProvider();
        }

        private async Task<Task> ReadyAsync(DiscordSocketClient shard)
        {
            await _shieldService.RestoreSavedShields();
            Console.WriteLine($"Shard Number {shard.ShardId} is connected and ready!");
            return Task.CompletedTask;
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }
    }
}
