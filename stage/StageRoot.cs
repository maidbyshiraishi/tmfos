using Godot;
using Godot.Collections;
using tmfos.command.stage;
using tmfos.data;
using tmfos.enemy;
using tmfos.hud;
using tmfos.mob;
using tmfos.player;
using tmfos.screen;
using tmfos.system;

namespace tmfos.stage;

/// <summary>
/// ゲームステージの親
/// </summary>
public partial class StageRoot : DialogRoot, IStateful
{
    public static readonly string StagePath = "res://stage/stage_{0:D4}.tscn";
    public static readonly string TutorialPath = "res://stage/tutorial_{0:D4}.tscn";
    public static readonly string LightSourceGroup = "LightSource";
    public static readonly string StatefulGroup = "StatefulGroup";
    public static readonly string StageEntryPointGroup = "StageEntryPointGroup";
    public static readonly string GameNodeGroup = "GameNode";
    public static readonly string PhysicsProcessGroup = "PhysicsProcess";
    public const int StageScenarioNo = 1;
    public const int TutorialScenarioNo = 0;

    /// <summary>
    /// BGMなし
    /// </summary>
    [Export]
    public bool NoBgm { get; set; }

    [Export]
    public AudioStream BgmStream { get; set; }

    [Export]
    public bool IsDarkZone { get; set; } = false;

    [Export]
    public float PlayerLifeTime { get; set; } = 0f;

    [Export]
    public int EnemyAttackCorrection { get; set; } = 0;

    [Export]
    public float PlayerArmorRatio { get; set; } = 1f;

    private Hud _hud;
    private PlayerData _playerData;
    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("%Player");
        _hud = GetNode<Hud>("%HUD");
        Camera2D camera = GetNodeOrNull<Camera2D>("%Camera");

        if (camera is null)
        {
            GD.PrintErr("Player/Cameraが%Cameraで参照できません。");
        }

        camera.Enabled = false;
        base._Ready();
        camera.Enabled = true;
    }

    public override void _ExitTree()
    {
        GetTree().CallGroup(GameNodeGroup, "FinalizeNode");
    }

    protected override void InitializeNode()
    {
        GameData gdata = GetNode<GameData>("/root/GameData");
        gdata.Restore();
        SetPlayerLifeTime(gdata);
        _playerData = gdata.GetPlayerData();
        GetTree().CallGroup(GameNodeGroup, "InitializeNode");
        StateLoad();
        SetStartGateway();
        EnableLight();
        PlayBgm();
        UpdateHud();
    }

    protected override void FinalizeNode()
    {
        GetTree().CallGroup(GameNodeGroup, "FinalizeNode");
    }

    private void SetPlayerLifeTime(GameData gdata)
    {
        // デフォルト値を設定する
        _player.LifeTime = PlayerLifeTime;

        // 保存されている場合は以前の値を復元する
        StageData stageData = gdata.GetStageData();
        int stageNo = stageData.StageNo;

        if (stageNo == stageData.TakeOverStageNo && 0.05f <= PlayerLifeTime)
        {
            _player.LifeTime = stageData.TakeOverPlayerLifeTime;
        }
    }

    protected void PlayBgm()
    {
        if (NoBgm)
        {
            GetNode<MusicPlayer>("/root/MusicPlayer").Play(MusicPlayer.Command.Mute);
            return;
        }

        if (BgmStream is null)
        {
            return;
        }

        GetNode<MusicPlayer>("/root/MusicPlayer").Play(MusicPlayer.Command.FastPlay, BgmStream);
    }

    private void EnableLight()
    {
        if (!IsDarkZone)
        {
            return;
        }

        GetNode<CanvasModulate>("DarkZone").Show();
        GetTree().CallGroup(LightSourceGroup, "EnableLight");
    }

    /// <summary>
    /// ステージ状態の保存を行う。
    /// 画面切り替え前、セーブ前に行われる
    /// </summary>
    public void StateSave()
    {
        GetTree().CallGroup(StatefulGroup, "StateSave");
    }

    public void StateLoad()
    {
        GetTree().CallGroup(StatefulGroup, "StateLoad");
    }

    private StageEntryPoint FindStageEntryPoint(int doorNo)
    {
        Array<Node> group = GetTree().GetNodesInGroup(StageEntryPointGroup);

        foreach (Node n in group)
        {
            if (n is StageEntryPoint stateEntryPoint && stateEntryPoint.DoorNo == doorNo)
            {
                return stateEntryPoint;
            }
        }

        return null;
    }

    protected void SetStartGateway()
    {
        StageData stageData = GetNode<GameData>("/root/GameData").GetStageData();
        int doorNo = stageData.DoorNo;
        StageEntryPoint stateEntryPoint = FindStageEntryPoint(doorNo);

        if (stateEntryPoint is null)
        {
            GD.PrintErr($"移動先のゲートウェイ{doorNo}が見つかりません。");
            return;
        }

        Player player = GetNode<Player>("%Player");
        stateEntryPoint.SetPlayerStartPosition(player);
    }

    public void EnterGateway(int destStageNo, int destDoorNo, string fadeout, string fadein)
    {
        GetTree().CallGroup(StatefulGroup, "StateSave");
        StageData stageData = GetNode<GameData>("/root/GameData").GetStageData();
        stageData.StageNo = destStageNo;
        stageData.DoorNo = destDoorNo;
        GetNode<GameData>("/root/GameData").Backup();
        GetNode<DialogLayer>("/root/DialogLayer").OpenGame(StartGameType.TakeoverStage, GameData.DefaultSlotNo, fadeout, fadein);
    }

    public void Miss()
    {
        _playerData.Remain--;
        UpdateHud();

        if (0 < _playerData.Remain)
        {
            _playerData.Life = PlayerData.InitialLife;
            GetNode<DialogLayer>("/root/DialogLayer").OpenGame(StartGameType.Restart, GameData.DefaultSlotNo, "fadeout_1", "fadein_1");
        }
        else
        {
            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://screen/game_over_dialog.tscn");
        }
    }

    public static string GetResourcePath(StageData stageData)
    {
        return string.Format(stageData.ScenarioNo == TutorialScenarioNo ? TutorialPath : StagePath, stageData.StageNo);
    }

    public void AddScore(int score)
    {
        _playerData.Score += score;
        _playerData.ExtendScore += score;
        UpdateHud();

        // エクステンド
        if (_playerData.ExtendScore >= PlayerData.ExtendScoreThreshold)
        {
            _playerData.ExtendScore -= PlayerData.ExtendScoreThreshold;
            AddRemain();
        }
    }

    public void AddRemain()
    {
        _playerData.Remain++;
        GetNode<SePlayer>("/root/SePlayer").Play("player_one_up");
        UpdateHud();
    }

    public void SpawnNode(Node node, Node spawner, Vector2 position, Vector2 direction, double lifeTime)
    {
        if (node is not ISpawnedNode ispawn)
        {
            return;
        }

        if (spawner is ISpawner ispawner)
        {
            ispawner.SetSpawned();
            ispawn.SetSpawner(ispawner);
        }

        ispawn.SetNodeInfo(position, direction);
        ispawn.SetLifeTime(lifeTime);
        _ = CallDeferred(MethodName.DeferredSpawnNode, node);
    }

    private void DeferredSpawnNode(Node node)
    {
        GetNode<Node>("Spawned").AddChild(node);
        Lib.InitializeNodeAll(node);
    }

    public void UpdateHud()
    {
        _hud.UpdateHudRemain(_playerData.Remain);
        _hud.UpdateHudLife(_playerData.Life);
        _hud.UpdateHudTime(_player.GetLifeTimeLeft());
        _hud.UpdateHudScore(_playerData.Score);
    }
}
