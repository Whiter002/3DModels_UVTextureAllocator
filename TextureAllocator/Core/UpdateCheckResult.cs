using System;
using System.Collections.Generic;
using System.Text;

namespace TextureAllocator.Core;

public record class UpdateCheckResult(bool IsAvailable,string DownloadUrl);
