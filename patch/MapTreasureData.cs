using System;
using System.Collections.Concurrent;
using System.Reflection;
using HarmonyLib;
using Manager.MaiStudio;
using SerializeMapTreasureData = Manager.MaiStudio.Serialize.MapTreasureData;

namespace NoTaskMusic.Patch
{
    /// <summary>
    /// Makes every map task music reward behave like a normal music reward.
    /// </summary>
    [HarmonyPatch(typeof(MapTreasureData), "Init", new[] { typeof(SerializeMapTreasureData) })]
    public static class MapTreasureDataPatch
    {
        private static readonly ConcurrentDictionary<int, byte> ConvertedTreasureIds
            = new ConcurrentDictionary<int, byte>();
        private static readonly MethodInfo TreasureTypeSetter
            = typeof(MapTreasureData).GetProperty("TreasureType", BindingFlags.Instance | BindingFlags.Public)
                .GetSetMethod(true);

        [HarmonyPostfix]
        public static void InitPostfix(MapTreasureData __instance, SerializeMapTreasureData __0)
        {
            if (__instance == null || __0 == null || __0.TreasureType != MapTreasureType.MapTaskMusic)
            {
                return;
            }

            ConvertedTreasureIds[__instance.GetID()] = 0;
            if (TreasureTypeSetter == null)
            {
                return;
            }

            TreasureTypeSetter.Invoke(__instance, new object[] { MapTreasureType.MusicNew });
        }

        public static bool WasMapTaskMusic(int treasureId)
        {
            return ConvertedTreasureIds.ContainsKey(treasureId);
        }
    }
}
