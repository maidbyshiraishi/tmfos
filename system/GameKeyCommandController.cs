using Godot;
using Godot.Collections;

namespace tmfos.system;

/// <summary>
/// 格闘ゲームコマンドを検出する
/// </summary>
public partial class GameKeyCommandController : Node
{
    /// <summary>
    /// コマンドの猶予時間
    /// </summary>
    private int _expireTime = 50;

    /// <summary>
    /// キーバッファサイズ
    /// </summary>
    private int _bufferSize = 300;

    // ZZZZ_2468_abZZ_ZZZZ
    private int _zero = 0;
    private int _down = 0b_0000_1000_0000_0000;
    private int _left = 0b_0000_0100_0000_0000;
    private int _right = 0b_0000_0000_1000_0000;
    private int _up = 0b_0000_0000_0100_0000;
    private int _a = 0b_0000_0000_1000_0000;
    private int _b = 0b_0000_0000_0100_0000;

    private int _reverse = 0b_0000_0101_0000_0000;

    /// <summary>
    /// キーバッファ
    /// </summary>
    private Array<int> _buffer = [];

    public override void _Ready()
    {
        base._Ready();
        _ = _buffer.Resize(_bufferSize);
    }

    public override void _Process(double delta)
    {
        _ = CallDeferred(MethodName.AddBuffer);
    }

    private void AddBuffer()
    {
        int now =
          (Input.IsActionPressed("ui_down") ? _down : _zero)
        + (Input.IsActionPressed("ui_left") ? _left : _zero)
        + (Input.IsActionPressed("ui_right") ? _right : _zero)
        + (Input.IsActionPressed("ui_up") ? _up : _zero)
        + (Input.IsActionPressed("ui_accept") ? _a : _zero)
        + (Input.IsActionPressed("b") ? _b : _zero);
        _buffer.Insert(0, now);
        _ = _buffer.Resize(_bufferSize);
    }

    public bool FindCommand(int[] command, int[] mask, int[] tame, bool reverse)
    {
        int clength = command.Length;

        if (_buffer.Count < clength)
        {
            return false;
        }

        int etime = 0;
        int cindex = 0;
        int bindex = 0;
        int blength = _buffer.Count;

        //キーバッファの最後がコマンドの最初でなければ失敗
        int buf = (_buffer[bindex] ^ (reverse ? _reverse : _zero)) & mask[cindex];

        if (buf != command[cindex])
        {
            return false;
        }

        cindex++;
        bindex++;

        // バッファを検索する
        for (; bindex < blength;)
        {
            if (tame[cindex] == 0)
            {
                //通常
                buf = (_buffer[bindex] ^ (reverse ? _reverse : _zero)) & mask[cindex];

                if (buf == command[cindex])
                {
                    cindex++;

                    if (cindex == clength)
                    {
                        _buffer.Clear();
                        return true;
                    }

                    etime = 0;
                    bindex++;
                    continue;
                }
            }
            else
            {
                //タメ
                //現在インデックスからタメ時間＋猶予時間分、連続しているか調べる
                int tlength = bindex + tame[cindex] + _expireTime;

                if (blength < tlength)
                {
                    //タメ時間分のキーバッファがない場合、失敗
                    return false;
                }

                for (int i = bindex; i < tlength; i++)
                {
                    buf = (_buffer[i] ^ (reverse ? _reverse : _zero)) & mask[cindex];

                    if (buf == command[cindex])
                    {
                        continue;
                    }

                    if (i < bindex + tame[cindex])
                    {
                        return false;
                    }

                    cindex++;

                    if (cindex == clength)
                    {
                        _buffer.Clear();
                        return true;
                    }

                    etime = 0;
                    bindex = i + 1;
                }
            }

            etime++;
            bindex++;

            if (_expireTime < etime)
            {
                return false;
            }
        }

        return false;
    }
}
