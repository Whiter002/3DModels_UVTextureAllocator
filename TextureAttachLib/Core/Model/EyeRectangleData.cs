using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;
using TextureAttacher.Library.Core.Enums;

namespace TextureAttacher.Library.Core.Model;

public class EyeRectangleData
{
    public string Name { get; set; }
    public bool Invert { get; set; } = false;
    public EyeTextureType Kind { get; set; }
    private RectangleF[]? _eye;
    [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
    public RectangleF[]? Eye { get { return Kind == EyeTextureType.INALL ? _eye : null; } set { _eye = value; } }
    private RectangleF[]? _eyeBackGround;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RectangleF[]? EyeBackGround { get { return Kind == EyeTextureType.BACKGROUND_AND_PUPIL ? _eyeBackGround : null; } set { _eyeBackGround = value; } }
    private RectangleF[]? _eyePupils;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RectangleF[]? EyePupils { get { return Kind == EyeTextureType.BACKGROUND_AND_PUPIL ? _eyePupils : null; } set { _eyePupils = value; } }

}
