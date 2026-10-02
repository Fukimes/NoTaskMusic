# NoTaskMusic

这是一个面向 **.NET Framework 4.7.2 + MelonLoader** 的 MelonLoader mod，按地图节点把课题曲视为普通乐曲，并兼容旧玩家存档：

- `MapTreasureData.Init` 将原始 `MapTaskMusic` 节点转换为 `MusicNew`，保留节点的 `MusicId`。
- `StateUserStore.Init` 在音乐解锁列表载入后，使用每位玩家自己的原始 `UserMap.distance`，补发已经到达但尚未解锁的课题曲。
- `ChallengeManager.GetChallengeDetail` 可选地将 Challenge 阶段固定为 `unlockDifficulty=Basic`、`startLife=999`。
- 补发按 `mapId + TreasureId` 遍历实际存在的存档记录，距离相等算已到达；同一首歌幂等处理。
- 补发直接写入 `MusicUnlockList`，不会改变当前选曲或设置全局强制换曲状态。
- `ForceChallengeFinalPhase` 关闭时，普通 `Challenge` 节点保持原有行为。

## MelonPreferences 配置

首次启动会创建 `NoTaskMusic` 分类，`BackfillTaskMusic` 默认为 `true`，`ForceChallengeFinalPhase` 默认为 `false`：

```toml
[NoTaskMusic]
BackfillTaskMusic = true
ForceChallengeFinalPhase = false
```

- `BackfillTaskMusic=true`：为旧玩家补发已到达但尚未解锁的课题曲。
- `ForceChallengeFinalPhase=true`：让 `ChallengeManager.GetChallengeDetail` 返回的 `unlockDifficulty` 始终为 `Basic`，`startLife` 始终为 `300`。

## 项目结构

- `patch/MapTreasureData.cs`：课题曲节点转换，并记录原始节点标记供补发使用。
- `patch/ChallengeManager.cs`：可选的 Challenge 最终阶段补丁。
- `patch/StateUserStore.cs`：旧存档逐玩家补发逻辑。
- `Preferences.cs`：MelonPreferences 配置定义和读取。
- `Main.cs`：MelonLoader 入口和 Harmony 补丁注册。
- `Libs/`：本地编译依赖目录；DLL 不提交到 Git。

## 编译依赖

请在编译前手动将下面这些 DLL 放入项目根目录的 `Libs/` 文件夹。它们应来自目标游戏安装目录和对应的 MelonLoader 安装，版本必须与目标游戏匹配：

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
## 编译

1. 使用 Visual Studio 打开 `NoTaskMusic.sln`，先按上面的列表准备 `Libs/` 中的 DLL。
2. 选择 `Release | x64`，还原 NuGet 包 `Microsoft.NETFramework.ReferenceAssemblies.net472` 后生成。
3. 输出文件为 `bin\Release\net472\NoTaskMusic.dll`。
4. 将 DLL 放到游戏目录的 `Mods` 文件夹。