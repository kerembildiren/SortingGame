using System.IO;
using System.Linq;
using System.Text;
using SortingGame.Core;
using SortingGame.Data;
using UnityEditor;
using UnityEngine;

namespace SortingGame.EditorTools
{
    /// <summary>
    /// Writes Docs/ECONOMY.md from the content assets: income per room, Shop prices and when the reference player
    /// (<see cref="EconomyModel.ReferenceTimeline"/>) can afford each purchase. Regenerate after any price or content change:
    /// menu "Sorting Game/Reports/Write Economy Report" or tools/unity.sh economy.
    /// </summary>
    public static class EconomyReport
    {
        const string OutputPath = "Docs/ECONOMY.md";

        [MenuItem("Sorting Game/Reports/Write Economy Report")]
        public static void Write()
        {
            var database = AssetDatabase.LoadAssetAtPath<GameDatabase>(ProjectSetup.DatabasePath);
            if (database == null)
            {
                Debug.LogError("[EconomyReport] No GameDatabase. Run 'Sorting Game/Setup/Run Full Setup'.");
                return;
            }
            var path = Path.Combine(Application.dataPath, "..", OutputPath);
            File.WriteAllText(path, Build(database).Replace("\r\n", "\n"));
            Debug.Log($"[EconomyReport] Wrote {OutputPath}.");
        }

        public static string Build(GameDatabase database)
        {
            _english = null; // the table may have changed since the last report
            var text = new StringBuilder();
            var rooms = EconomyModel.Ladder(database);
            var income = EconomyModel.TotalIncome(database);
            var sinks = EconomyModel.TotalSinks(database);

            text.AppendLine("# Economy report");
            text.AppendLine();
            text.AppendLine("Generated from the content assets by `EconomyReport` (menu `Sorting Game/Reports/Write Economy Report`");
            text.AppendLine("or `tools/unity.sh economy`). Do not edit by hand; change the numbers in `ContentBuilder` and regenerate.");
            text.AppendLine();

            text.AppendLine("## Income: coins a room pays when everything is shelved");
            text.AppendLine();
            text.AppendLine("| Venue | Room | Items | Coins | Coins so far |");
            text.AppendLine("|---|---|---:|---:|---:|");
            foreach (var room in rooms)
                text.AppendLine($"| {Name(room.Venue.DisplayNameKey)} | {Name(room.Section.DisplayNameKey)} | {room.Items} | {room.Coins} | {room.CoinsBefore + room.Coins} |");
            text.AppendLine();
            text.AppendLine($"Whole content: **{income} coins**. Coin values per item: "
                            + string.Join(", ", database.Categories.Select(c => $"{Name(c.DisplayNameKey)} {c.BaseCoinValue}"))
                            + "; rare items "
                            + string.Join(", ", database.Items.Where(i => i.IsRare).Select(i => $"{Name(i.DisplayNameKey)} {i.CoinValue}")) + ".");
            text.AppendLine();

            text.AppendLine("## Shop prices");
            text.AppendLine();
            text.AppendLine("| What | Level costs | Total | Opens |");
            text.AppendLine("|---|---|---:|---|");
            foreach (var tool in database.Tools)
            {
                var gate = tool.RequiresMaxed != null ? $"needs {Name(tool.RequiresMaxed.DisplayNameKey)} at max level" : "from the start";
                text.AppendLine($"| {Name(tool.DisplayNameKey)} (tool) | {string.Join(" / ", tool.Levels.Select(l => l.Cost))} | {EconomyModel.TotalCost(tool)} | {gate} |");
            }
            foreach (var helper in database.Helpers)
            {
                var gate = helper.RequiredVenue == null ? "from the start"
                    : helper.RequiredVenuePercent >= 100 ? $"{Name(helper.RequiredVenue.DisplayNameKey)} fully restored"
                    : $"{helper.RequiredVenuePercent}% of {Name(helper.RequiredVenue.DisplayNameKey)}";
                text.AppendLine($"| {Name(helper.DisplayNameKey)} (helper) | {string.Join(" / ", helper.Levels.Select(l => l.Cost))} | {EconomyModel.TotalCost(helper)} | {gate} |");
            }
            text.AppendLine();
            text.AppendLine($"Everything in the Shop: **{sinks} coins**, {sinks / (float)Mathf.Max(1, income):0.0} times what the content pays.");
            text.AppendLine("What the content does not pay for is left for later venues (GDD 5.3, 11.5).");
            text.AppendLine();

            text.AppendLine("## Reference player");
            text.AppendLine();
            text.AppendLine("Plays the rooms in order, earns evenly through each room and always saves for the cheapest thing the Shop");
            text.AppendLine("offers at that moment. Real players choose differently; this is a yardstick, not a prediction.");
            text.AppendLine();
            text.AppendLine("| # | Purchase | Cost | Bought in | At |");
            text.AppendLine("|---:|---|---:|---|---:|");
            var number = 0;
            foreach (var purchase in EconomyModel.ReferenceTimeline(database))
            {
                number++;
                var where = purchase.Reached ? Name(purchase.Room.DisplayNameKey) : "not within this content";
                var at = purchase.Reached ? $"{purchase.RoomPercent}%" : "";
                text.AppendLine($"| {number} | {Name(purchase.Id)} level {purchase.Level} | {purchase.Cost} | {where} | {at} |");
            }
            text.AppendLine();
            text.AppendLine("Not in this model: Auto Sort (ads or real money, never coins) and how fast a player actually sorts.");
            text.AppendLine("Coins buy tools and helpers only: venues and locked rooms open by progress.");
            return text.ToString();
        }

        /// <summary>English names, straight from the string table file (the report does not depend on a running game).</summary>
        static string Name(string key)
        {
            _english ??= Loc.Parse(File.ReadAllText(Path.Combine(Application.dataPath, "_Project/Localization/en.txt")));
            return _english.TryGetValue(key, out var value) ? value : key;
        }

        static System.Collections.Generic.Dictionary<string, string> _english;
    }
}
