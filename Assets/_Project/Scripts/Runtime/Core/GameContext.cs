using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>Long-lived game services and content, handed to whatever needs them.</summary>
    public class GameContext
    {
        public GameDatabase Database;
        public SectionVisuals Visuals;
        public Wallet Wallet;
        public CollectionBook Book;
        public AutoSortBoost AutoSort;
        public IAdProvider Ads;
        public IStoreProvider Store;
        public ToolProgress Tools;
        public HelperProgress Helpers;
        /// <summary>The loaded save: venue and room progress that some rules read (helper slots).</summary>
        public SaveData Save;

        public FeelConfig Feel => Database.Feel;

        /// <summary>Current stats of a tool (level 1 stats while still locked).</summary>
        public ToolDefinition.Level ToolStats(ToolType type)
        {
            var tool = Database.ToolFor(type);
            return tool == null ? default : Tools.Stats(tool);
        }

        /// <summary>GDD 10.3: is this helper's Shop slot open yet?</summary>
        public bool HelperSlotOpen(HelperDefinition helper) => VenueProgress.HelperSlotOpen(helper, Save);

        public bool Owns(ToolType type)
        {
            var tool = Database.ToolFor(type);
            return tool != null && Tools.IsOwned(tool);
        }
    }
}
