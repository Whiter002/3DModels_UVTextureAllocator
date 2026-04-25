using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareUpdator;

internal class ResultCounter
{
    internal int SkippedCount { get; private set; } = 0;
    internal int SuccessedCount {  get; private set; } = 0;
    internal int FailedCount { get; private set; } = 0;

    internal int Total() => SkippedCount + SuccessedCount + FailedCount;
    internal void Skip() => SkippedCount++;
    internal void Sccess() => SuccessedCount++;
    internal void Fail() => FailedCount++;


}
