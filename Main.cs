using System;
using HarmonyLib;
using MelonLoader;
using NoTaskMusic.Patch;

namespace NoTaskMusic
{
    public static class BuildInfo
    {
        public const string Name = "NoTaskMusic";
        public const string Description = "将地图课题曲节点转换为普通乐曲，兼容旧存档补发，并可修改 Challenge 阶段。";
        public const string Author = "Fukimes";
        public const string Version = "1.0.0";
    }

    public sealed class NoTaskMusicMod : MelonMod
    {
        private const string HarmonyId = "com.fukimes.notaskmusic";
        private bool _initialized;

        public override void OnInitializeMelon()
        {
            if (_initialized)
            {
                return;
            }

            MelonLogger.Msg("[" + BuildInfo.Name + "] Initializing...");
            try
            {
                ModPreferences.Initialize();
            }
            catch (Exception exception)
            {
                MelonLogger.Error("[" + BuildInfo.Name + "] Preferences initialization: FAILED");
                MelonLogger.Error(exception.ToString());
                return;
            }

            bool treasurePatch = ApplyPatch(typeof(MapTreasureDataPatch), "MapTreasureData.Init");
            bool userStorePatch = ApplyPatch(typeof(StateUserStorePatch), "StateUserStore.Init");
            bool challengePatch = ApplyPatch(typeof(ChallengeManagerPatch), "ChallengeManager.GetChallengeDetail");
            MelonLogger.Msg("[" + BuildInfo.Name + "] Patch status: MapTreasureData="
                + Status(treasurePatch) + ", StateUserStore=" + Status(userStorePatch)
                + ", ChallengeManager=" + Status(challengePatch));
            _initialized = treasurePatch || userStorePatch || challengePatch;
        }

        private static bool ApplyPatch(Type patchType, string target)
        {
            try
            {
                HarmonyLib.Harmony.CreateAndPatchAll(patchType, HarmonyId);
                MelonLogger.Msg("[" + BuildInfo.Name + "] Patch " + target + ": OK");
                return true;
            }
            catch (Exception exception)
            {
                MelonLogger.Error("[" + BuildInfo.Name + "] Patch " + target + ": FAILED");
                MelonLogger.Error(exception.ToString());
                return false;
            }
        }

        private static string Status(bool success)
        {
            return success ? "OK" : "FAILED";
        }
    }
}
