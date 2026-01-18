using Godot;

namespace tmfos.system;

/// <summary>
/// ダブルカセットでBGMの制御を行う
/// </summary>
public partial class MusicPlayer : Node
{
    /// <summary>
    /// MusicPlayerコマンド
    /// </summary>
    public enum Command
    {
        /// <summary>
        /// 何もしない
        /// </summary>
        None = 0,

        /// <summary>
        /// 即再生する
        /// </summary>
        FastPlay,

        /// <summary>
        /// 即ミュートする
        /// </summary>
        FastMute,

        /// <summary>
        /// クロスフェード再生する
        /// </summary>
        Play,

        /// <summary>
        /// 徐々にミュートする
        /// </summary>
        Mute
    }

    /// <summary>
    /// 再生中BGMパス
    /// </summary>
    private string _now = null;

    // 再生予約セット
    private Command _nextCommand;
    private AudioStream _nextStream = null;
    private bool _changing = false;
    private AudioStreamPlayer _deck1;
    private AudioStreamPlayer _deck2;
    private AnimationPlayer _fader;
    private bool _deck1Playing = false;
    private bool _deck2Playing = false;
    private float _deck1Position = 0f;
    private float _deck2Position = 0f;

    public override void _Ready()
    {
        _deck1 = GetNode<AudioStreamPlayer>("Deck1");
        _deck2 = GetNode<AudioStreamPlayer>("Deck2");
        _deck1.VolumeDb = 0;
        _deck2.VolumeDb = 0;
        _fader = GetNode<AnimationPlayer>("Fader");
        _ = _fader.Connect(AnimationMixer.SignalName.AnimationFinished, new(this, MethodName.Finished));
    }

    /// <summary>
    /// MusicPlayerコマンドを実行する
    /// </summary>
    /// <param name="command">MusicPlayerコマンド</param>
    /// <param name="stream">BGMストリーム</param>
    private void ExecCommand(Command command, AudioStream stream = null)
    {
        if (_deck1 is null || _deck2 is null || _fader is null)
        {
            GD.PrintErr("BGMが再生できる状態にありません。");
            return;
        }

        // 即時コマンドの接頭語
        bool fast = false;

        switch (command)
        {
            case Command.Mute:

                _changing = true;
                _now = null;

                if (_fader.HasAnimation("fader/fast_mute"))
                {
                    _fader.Play("fader/mute");
                }

                return;

            case Command.FastMute:

                _changing = true;
                _now = null;

                if (_fader.HasAnimation("fader/fast_mute"))
                {
                    _fader.Play("fader/fast_mute");
                }

                return;

            case Command.FastPlay:

                _changing = true;
                fast = true;
                break;

            case Command.Play:

                _changing = true;
                break;
        }

        string path = stream.ResourcePath;

        // 変更なしなら何もしない
        if (_now == path)
        {
            _changing = false;
            return;
        }

        _now = path;
        string prefix = fast ? "fast_" : "";
        string deck;

        // 再生するデッキを選択する
        if (_deck1.Playing)
        {
            _deck2.Stream = stream;
            deck = $"fader/{prefix}play_deck2";
        }
        else
        {
            _deck1.Stream = stream;
            deck = $"fader/{prefix}play_deck1";
        }

        if (_fader.HasAnimation(deck))
        {
            _fader.Play(deck);
        }
    }

    /// <summary>
    /// BGMの操作を行う
    /// </summary>
    /// <param name="command">MusicPlayerコマンド</param>
    /// <param name="path">BGMパス</param>
    public void Play(Command command, AudioStream stream = null)
    {
        if (command is Command.None || ((command is Command.Play || command is Command.FastPlay) && stream is null))
        {
            return;
        }

        // 作業中なら予約する
        // 最後の予約が優先されて上書きされる
        if (_changing)
        {
            _nextCommand = command;
            _nextStream = stream;
            return;
        }

        ExecCommand(command, stream);
    }

    /// <summary>
    /// 作業が終了したので予約を確認して実行する
    /// </summary>
    /// <param name="animName">終了したアニメーション名</param>
    public void Finished(StringName animName)
    {
        _changing = false;

        if (_nextCommand is Command.None)
        {
            return;
        }

        Command next = _nextCommand;
        _nextCommand = Command.None;
        ExecCommand(next, _nextStream);
    }

    /// <summary>
    /// マスター音量をミュートする
    /// </summary>
    /// <param name="mute">ミュートするか</param>
    public static void SetMasterBusMute(bool mute)
    {
        AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), mute);
    }

    /// <summary>
    /// 再生を一時停止する
    /// </summary>
    /// <param name="paused">一時停止するか</param>
    public void Pause(bool paused)
    {
        if (paused)
        {
            _deck1Position = _deck1.GetPlaybackPosition();
            _deck1Playing = _deck1.Playing;
            _deck1.Playing = false;
            _ = _deck1.GetPlaybackPosition();
            _deck2Position = _deck2.GetPlaybackPosition();
            _deck2Playing = _deck2.Playing;
            _deck2.Playing = false;
            return;
        }

        _deck1.Playing = _deck1Playing;
        _deck1.Seek(_deck1Position);
        _deck2.Playing = _deck2Playing;
        _deck2.Seek(_deck2Position);
    }
}
