using System;

[Flags]
public enum FontStyles
{
    Bold = 1 << 1,
    Italic = 1 << 2,
    Underline = 1 << 3,
    Strikethrough = 1 << 4,
}