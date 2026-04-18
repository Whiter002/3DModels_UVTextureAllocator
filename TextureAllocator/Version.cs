using System;
using System.Collections.Generic;
using System.Text;

namespace TextureAllocator;

internal static class VersionInfo
{
    public static readonly string CurrentVersion = ThisAssembly.AssemblyInformationalVersion.Split("+")[0];
}
