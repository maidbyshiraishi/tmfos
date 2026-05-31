using System;
using System.Threading;

namespace maid_by_shiraishi.system;

/// <summary>
/// 二重起動防止
/// </summary>
public static class GameMutex
{
    private static readonly Mutex _mutex = new(false, "maid_by_shiraishi");

    [STAThread]
    public static bool IsExecuted()
    {
        return _mutex.WaitOne(0, false);
    }
}
