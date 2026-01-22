using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Core
{
    public enum Mode
    {
        Shell,
        Search
    }

    public static class Global
    {
        public static Mode CurrentMode { get; set; } = 0;
    }
}
