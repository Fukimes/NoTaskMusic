using MelonLoader;
using MelonLoader.Preferences;

namespace NoTaskMusic
{
    internal static class ModPreferences
    {
        private const string CategoryName = "NoTaskMusic";
        private static MelonPreferences_Category _category;
        private static MelonPreferences_Entry<bool> _backfillTaskMusicEntry;
        private static MelonPreferences_Entry<bool> _forceChallengeFinalPhaseEntry;

        public static bool BackfillTaskMusic { get; private set; }
        public static bool ForceChallengeFinalPhase { get; private set; }

        public static void Initialize()
        {
            _category = MelonPreferences.CreateCategory(CategoryName, "No Task Music");
            _backfillTaskMusicEntry = _category.CreateEntry(
                "BackfillTaskMusic",
                true,
                "Backfill task music",
                "补发旧存档中已到达但尚未解锁的课题曲。",
                false,
                false,
                null);
            _forceChallengeFinalPhaseEntry = _category.CreateEntry(
                "ForceChallengeFinalPhase",
                false,
                "Force challenge final phase",
                "将完美挑战难度改为 Basic，血量设为 999。",
                false,
                false,
                null);

            BackfillTaskMusic = _backfillTaskMusicEntry.Value;
            ForceChallengeFinalPhase = _forceChallengeFinalPhaseEntry.Value;

            MelonPreferences.Save();
            MelonLogger.Msg("[NoTaskMusic] Preferences loaded: BackfillTaskMusic="
                + BackfillTaskMusic + ", ForceChallengeFinalPhase=" + ForceChallengeFinalPhase);
        }
    }
}
