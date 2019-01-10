using Discord.Commands;
using System.Threading.Tasks;

namespace HeckBot.Modules
{
    public class PublicModule : ModuleBase<ShardedCommandContext>
    {
        [Command("info")]
        public async Task InfoAsync()
        {
            var msg = $@"Hi {Context.User}! This guild is being served by HeckBot shard number {Context.Client.GetShardFor(Context.Guild).ShardId}";
            await ReplyAsync(msg);
        }

        [Command("help")]
        public async Task HelpAsync()
        {
            var msg = "Below is a list of things I can do:\r\n" +
                "@NerdyGuacamoleBot quests - Returns the current and upcoming quests\r\n" +
				"@NerdyGuacamoleBot shield <time> - Starts a shield timer for <time> hours\r\n" +
				"@NerdyGuacamoleBot shield stop - Stops all shield timers\r\n" +
				"@NerdyGuacamoleBot info - Find out what shard you're connected to\r\n" +
				"@NerdyGuacamoleBot cat - purrrr\r\n" +
				"@NerdyGuacamoleBot BattleKing - ???\r\n";
            await ReplyAsync(msg);
        }

		[Command("BattleKing")]
		public async Task BattleKingAsync()
		{
			var msg = $@"Hi {Context.User}! Can YOU teach BattleKing how to use the modulus operator?";
			await ReplyAsync(msg);
		}
    }
}
