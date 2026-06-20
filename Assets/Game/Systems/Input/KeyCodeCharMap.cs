using System.Collections.Generic;
using UnityEngine;

public static class KeyCodeCharMap
{
    // -----------------------------------------
    //  PRIMARY MAP — UNSHIFTED CHARACTERS
    // -----------------------------------------
    public static readonly Dictionary<KeyCode, char> Unshifted = new()
    {
        // Letters
        { KeyCode.A, 'a' }, { KeyCode.B, 'b' }, { KeyCode.C, 'c' }, { KeyCode.D, 'd' },
        { KeyCode.E, 'e' }, { KeyCode.F, 'f' }, { KeyCode.G, 'g' }, { KeyCode.H, 'h' },
        { KeyCode.I, 'i' }, { KeyCode.J, 'j' }, { KeyCode.K, 'k' }, { KeyCode.L, 'l' },
        { KeyCode.M, 'm' }, { KeyCode.N, 'n' }, { KeyCode.O, 'o' }, { KeyCode.P, 'p' },
        { KeyCode.Q, 'q' }, { KeyCode.R, 'r' }, { KeyCode.S, 's' }, { KeyCode.T, 't' },
        { KeyCode.U, 'u' }, { KeyCode.V, 'v' }, { KeyCode.W, 'w' }, { KeyCode.X, 'x' },
        { KeyCode.Y, 'y' }, { KeyCode.Z, 'z' },

        // Numbers (top row)
        { KeyCode.Alpha0, "0"[0] },
        { KeyCode.Alpha1, "1"[0] },
        { KeyCode.Alpha2, "2"[0] },
        { KeyCode.Alpha3, "3"[0] },
        { KeyCode.Alpha4, "4"[0] },
        { KeyCode.Alpha5, "5"[0] },
        { KeyCode.Alpha6, "6"[0] },
        { KeyCode.Alpha7, "7"[0] },
        { KeyCode.Alpha8, "8"[0] },
        { KeyCode.Alpha9, "9"[0] },

        // Symbols (unshifted)
        { KeyCode.Minus, '-' },
        { KeyCode.Equals, '=' },
        { KeyCode.LeftBracket, '[' },
        { KeyCode.RightBracket, ']' },
        { KeyCode.Backslash, '\\' },
        { KeyCode.Semicolon, ';' },
        { KeyCode.Quote, '\'' },
        { KeyCode.Comma, ',' },
        { KeyCode.Period, '.' },
        { KeyCode.Slash, '/' },
        { KeyCode.BackQuote, '`' },

        // Space
        { KeyCode.Space, ' ' }
    };

    // -----------------------------------------
    //  SHIFTED MAP — SHIFTED CHARACTERS ONLY
    // -----------------------------------------
    public static readonly Dictionary<KeyCode, char> Shifted = new()
    {
        // Letters become uppercase
        { KeyCode.A, 'A' }, { KeyCode.B, 'B' }, { KeyCode.C, 'C' }, { KeyCode.D, 'D' },
        { KeyCode.E, 'E' }, { KeyCode.F, 'F' }, { KeyCode.G, 'G' }, { KeyCode.H, 'H' },
        { KeyCode.I, 'I' }, { KeyCode.J, 'J' }, { KeyCode.K, 'K' }, { KeyCode.L, 'L' },
        { KeyCode.M, 'M' }, { KeyCode.N, 'N' }, { KeyCode.O, 'O' }, { KeyCode.P, 'P' },
        { KeyCode.Q, 'Q' }, { KeyCode.R, 'R' }, { KeyCode.S, 'S' }, { KeyCode.T, 'T' },
        { KeyCode.U, 'U' }, { KeyCode.V, 'V' }, { KeyCode.W, 'W' }, { KeyCode.X, 'X' },
        { KeyCode.Y, 'Y' }, { KeyCode.Z, 'Z' },

        // Numbers (shifted)
        { KeyCode.Alpha1, '!' },
        { KeyCode.Alpha2, '@' },
        { KeyCode.Alpha3, '#' },
        { KeyCode.Alpha4, '$' },
        { KeyCode.Alpha5, '%' },
        { KeyCode.Alpha6, '^' },
        { KeyCode.Alpha7, '&' },
        { KeyCode.Alpha8, '*' },
        { KeyCode.Alpha9, '(' },
        { KeyCode.Alpha0, ')' },

        // Symbols (shifted)
        { KeyCode.Minus, '_' },
        { KeyCode.Equals, '+' },
        { KeyCode.LeftBracket, '{' },
        { KeyCode.RightBracket, '}' },
        { KeyCode.Backslash, '|' },
        { KeyCode.Semicolon, ':' },
        { KeyCode.Quote, '"' },
        { KeyCode.Comma, '<' },
        { KeyCode.Period, '>' },
        { KeyCode.Slash, '?' },
        { KeyCode.BackQuote, '~' }
    };

    // -----------------------------------------
    //  Helper Method
    // -----------------------------------------
    public static bool TryGetChar(KeyCode key, bool shift, out char character)
    {
        if (shift && Shifted.TryGetValue(key, out character))
            return true;

        if (!shift && Unshifted.TryGetValue(key, out character))
            return true;

        character = '\0';
        return false;
    }
}