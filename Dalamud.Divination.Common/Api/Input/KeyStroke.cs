using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Dalamud.Divination.Common.Api.Input
{
    public static class KeyStroke
    {
        public static string ToReadableString(this byte[] keys)
        {
            return string.Join(" + ", keys.Select(k => k.ToReadableString())) +
                   $" ({string.Join(", ", keys.Select(k => $"0x{k:X2}"))})";
        }

        public static string ToReadableString(this byte key)
        {
            switch (key)
            {
                case Win32Api.VkBack:
                    return "Backspace";
                case Win32Api.VkTab:
                    return "Tab";
                case Win32Api.VkEnter:
                    return "Enter";
                case Win32Api.VkShift:
                    return "Shift";
                case Win32Api.VkControl:
                    return "Ctrl";
                case Win32Api.VkAlt:
                    return "Alt";
                case Win32Api.VkEscape:
                    return "Esc";
                case Win32Api.VkSpace:
                    return "Space";
                default:
                    switch (key)
                    {
                        case >= Win32Api.Vk0 and <= Win32Api.Vk9:
                            return $"{key - Win32Api.Vk0}";
                        case >= Win32Api.VkA and <= Win32Api.VkZ:
                            return $"{(char) ('A' + key - Win32Api.VkA)}";
                        case >= Win32Api.VkNum0 and <= Win32Api.VkNum9:
                            return $"NUM {key - Win32Api.VkNum0}";
                        case >= Win32Api.VkF1 and <= Win32Api.VkF24:
                            return $"F{key - Win32Api.VkF1 + 1}";
                    }

                    var buf = new StringBuilder(256);
                    Win32Api.ToUnicode(key, 0, new byte[256], buf, 256, 0);
                    var ascii = buf.ToString();

                    return string.IsNullOrWhiteSpace(ascii) ? "?" : ascii;
            }
        }

        public static byte[] ParseVirtualKeys(string value)
        {
            try
            {
                return value.Split(',')
                    .Select(k => k.Trim().Replace("0x", ""))
                    .Select(k => byte.Parse(k, NumberStyles.HexNumber))
                    .ToArray();
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }
    }
}
