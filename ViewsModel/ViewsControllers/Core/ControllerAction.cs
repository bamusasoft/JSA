using System;

namespace Jsa.ViewsModel.ViewsControllers.Core
{
    [Flags]
    public enum ControllerAction:uint
    {
        Cleared = 0x00,
        Saved = 0x02,
        Edited = 0x04,
        Deleted = 0x08,
        Invalid = 0x10,
        
    }
}