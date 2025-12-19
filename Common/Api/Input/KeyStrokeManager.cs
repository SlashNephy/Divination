using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dalamud.Divination.Common.Api.Dalamud;

namespace Dalamud.Divination.Common.Api.Input;

public sealed class KeyStrokeManager : IKeyStrokeManager
{
    private readonly object inputLock = new();

    public void Send(string rawKeys)
    {
        var keys = KeyStroke.ParseVirtualKeys(rawKeys);
        if (keys.Length == 0)
        {
            return;
        }

        var random = new Random();
        Thread.Sleep(random.Next(0, 300));

        var handle = GetGameWindowHandle();
        if (handle == IntPtr.Zero)
        {
            throw new AggregateException("Cannot get game window handle.");
        }

        lock (inputLock)
        {
            foreach (var key in keys)
            {
                Win32Api.SendMessage(handle, Win32Api.WmKeydown, key, 0);
            }

            Thread.Sleep(random.Next(100, 400));

            foreach (var key in ((IEnumerable<byte>)keys).Reverse())
            {
                Win32Api.SendMessage(handle, Win32Api.WmKeyup, key, 0);
            }
        }

        DalamudLog.Log.Debug("SendMessage: {Keys}", keys.ToReadableString());
    }

    private static IntPtr GetGameWindowHandle()
    {
        for (var i = 0; i < 5; i++)
        {
            var handle = Win32Api.GetGameWindowHandle();
            if (handle != IntPtr.Zero)
            {
                return handle;
            }
        }

        return IntPtr.Zero;
    }
}
