using System;
using System.Collections.Generic;
using System.Text;

namespace TextureAllocator.Events;

internal delegate void TransitionedMainStatusDelegate(object sender, TransitionedMainStatusEventArgs e);
internal class TransitionedMainStatusEventArgs : EventArgs
{
    public Enums.MainOperatePhase Phase { get; private set; }
    public TransitionedMainStatusEventArgs(Enums.MainOperatePhase phase)
    {
        Phase = phase;
    }
}
