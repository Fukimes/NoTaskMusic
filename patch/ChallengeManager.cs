using HarmonyLib;
using Manager;

namespace NoTaskMusic.Patch
{
    /// <summary>
    /// Optionally exposes every challenge as its final phase.
    /// </summary>
    [HarmonyPatch(typeof(ChallengeManager), "GetChallengeDetail", new[] { typeof(int) })]
    public static class ChallengeManagerPatch
    {
        [HarmonyPostfix]
        public static void GetChallengeDetailPostfix(ref ChallengeDetail __result)
        {
            if (!NoTaskMusic.ModPreferences.ForceChallengeFinalPhase)
            {
                return;
            }

            __result.unlockDifficulty = MusicDifficultyID.Basic;
            __result.startLife = 999;
        }
    }
}
