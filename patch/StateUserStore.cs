using System;
using HarmonyLib;
using MAI2.Util;
using MelonLoader;
using Manager;
using Manager.MaiStudio;
using Net.VO.Mai2;
using Process.UserDataNet.State.UserDataDLState.Exist;

namespace NoTaskMusic.Patch
{
    /// <summary>
    /// Backfills converted task music for each player's own raw map progress.
    /// </summary>
    [HarmonyPatch(typeof(StateUserStore), "Init", new[] { typeof(NetUserData), typeof(UserData) })]
    public static class StateUserStorePatch
    {
        [HarmonyPostfix]
        public static void InitPostfix(NetUserData __0, UserData __1)
        {
            if (__0 == null || __1 == null || __0.MapList == null || __0.MapList.Length == 0
                || __1.MusicUnlockList == null)
            {
                return;
            }

            int unlockedCount = 0;
            foreach (UserMap playerMap in __0.MapList)
            {
                MapData mapData = Singleton<DataManager>.Instance.GetMapData(playerMap.mapId);
                if (mapData == null || mapData.TreasureExDatas == null)
                {
                    continue;
                }

                foreach (MapTreasureExData treasureExData in mapData.TreasureExDatas)
                {
                    if (treasureExData == null || treasureExData.TreasureId == null)
                    {
                        continue;
                    }

                    int treasureId = treasureExData.TreasureId.id;
                    if (!MapTreasureDataPatch.WasMapTaskMusic(treasureId)
                        || treasureExData.Distance < 0
                        || playerMap.distance < (uint)treasureExData.Distance)
                    {
                        continue;
                    }

                    MapTreasureData treasure = Singleton<DataManager>.Instance.GetMapTreasureData(treasureId);
                    if (treasure == null || treasure.MusicId == null)
                    {
                        continue;
                    }

                    int musicId = treasure.MusicId.id;
                    if (musicId <= 0 || Singleton<DataManager>.Instance.GetMusic(musicId) == null
                        || __1.IsUnlockMusic(UserData.MusicUnlock.Base, musicId))
                    {
                        continue;
                    }

                    // Add directly to preserve the player's selected music and avoid global force-change state.
                    __1.MusicUnlockList.Add(musicId);
                    unlockedCount++;
                }
            }

            if (unlockedCount > 0)
            {
                MelonLogger.Msg("[NoTaskMusic] Backfilled " + unlockedCount
                    + " map music reward(s) for user " + __1.Detail.UserID + ".");
            }
        }
    }
}
