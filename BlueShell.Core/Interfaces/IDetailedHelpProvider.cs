using System;
using System.Collections.Generic;
using System.Text;
using BlueShell.Core.Models;

namespace BlueShell.Core.Interfaces
{
    public interface IDetailedHelpProvider
    {
        CommandHelp GetDetailedHelp();
    }
}
