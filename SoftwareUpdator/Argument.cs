using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareUpdator;

internal class Argument
{
    [Option('s', "-source-Zip", Required = true, HelpText = "The source zip file for the update.")]
    public string UpdateZipSource { get; set; }
    [Option('d', "-destination", Required = true, HelpText = "The destination path where the update should be extracted.")]
    public string UpdateExtractDestination { get; set; }
    [Option('m', "-mirror", Required = false, HelpText = "Whether to mirror the directory structure when extracting.")]
    public bool? Mirror { get; set; }
    [Option('p',"-PowershellCommand",Required=false,HelpText="The PowershellCommand for run after Update")]
    public string? PowerShellCommand { get; set; }
}

