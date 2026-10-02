# NoTaskMusic

这是一个面向 **.NET Framework 4.7.2 + MelonLoader** 的 MelonLoader mod，按地图节点把课题曲视为普通乐曲，并兼容旧玩家存档：

- `MapTreasureData.Init` 将原始 `MapTaskMusic` 节点转换为 `MusicNew`，保留节点的 `MusicId`。
- `StateUserStore.Init` 在音乐解锁列表载入后，使用每位玩家自己的原始 `UserMap.distance`，补发已经到达但尚未解锁的课题曲。
- 补发按 `mapId + TreasureId` 遍历实际存在的存档记录，距离相等算已到达；同一首歌幂等处理。
- 补发直接写入 `MusicUnlockList`，不会改变当前选曲或设置全局强制换曲状态。
- 普通 `Challenge` 节点保持原有行为。

## 项目结构

- `patch/MapTreasureData.cs`：课题曲节点转换，并记录原始节点标记供补发使用。
- `patch/StateUserStore.cs`：旧存档逐玩家补发逻辑。
- `Main.cs`：MelonLoader 入口和 Harmony 补丁注册。
- `Libs/`：本地编译依赖目录；DLL 不提交到 Git。

## 编译依赖

将与目标游戏版本匹配的下列 DLL 放入 `Libs/`：

```text
0Harmony.dll
AMDaemon.NET.dll
Assembly-CSharp-firstpass.dll
Assembly-CSharp.dll
MelonLoader.dll
UnityEngine.dll
UnityEngine.CoreModule.dll
UnityEngine.IMGUIModule.dll
UnityEngine.InputModule.dll
UnityEngine.TextRenderingModule.dll
```

本工作区中的 `Sinmai-Assist/Libs` 可作为同版本依赖来源。编译前选择 `Release | x64`，还原 `Microsoft.NETFramework.ReferenceAssemblies.net472` 后生成；输出为 `bin\Release\net472\NoTaskMusic.dll`。

将 DLL 放入游戏目录的 `Mods` 文件夹。
