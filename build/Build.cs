using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.Xunit;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using static Nuke.Common.Tools.Xunit.XunitTasks;

[GitHubActions(
    "continuous",
    GitHubActionsImage.WindowsServer2022,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(GitHubWorkflow) },
    AutoGenerate = false
)]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Output directory for artifacts generated while packing and publishing.")]
    public readonly AbsolutePath WorkDirectory = AbsolutePath.Create(GitHubActions.Instance?.Workspace ?? RootDirectory); 

    
    [Solution]
    readonly Solution Solution;
    
    [GitVersion(NoCache=false, NoFetch=true)]
    readonly GitVersion GitVersion;
    
    
    AbsolutePath PackagesDirectory =>  WorkDirectory / "artifacts";

   
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() => {
            MSBuild(settings => SetMSBuildDefaults(settings).SetTargets("Clean"));
        });

    Target Restore => _ => _
        .After(Clean)
        .Executes(() => {
            NuGetRestore(settings => settings
                .SetTargetPath(Solution)
                .SetMSBuildPath(((AbsolutePath)MSBuildPath)?.Parent)
            );
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() => {
            MSBuild(settings => SetMSBuildDefaults(settings).SetTargets("Build").DisableRestore());
        });

    Target Test => _ => _
        .After(Compile)
        .Executes(() => {
            DotNetTest(settings => settings.SetConfiguration(Configuration).EnableNoBuild());
        });
    
    Target Pack => _ => _
        .After(Compile)
        .Produces(PackagesDirectory / "*.nupkg")
        .Produces(PackagesDirectory / "*.snupkg")
        .Executes(() => {
            DotNetPack(settings => settings
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .SetProperty("PackageVersion", GitVersion.NuGetVersion)
                .SetOutputDirectory(PackagesDirectory)
            );
        });

    Target GitHubWorkflow => _ => _
        .DependsOn(Compile)
        .DependsOn(Test)
        .DependsOn(Pack);
    
    MSBuildSettings SetMSBuildDefaults(MSBuildSettings s)
        => s.SetProcessToolPath(MSBuildPath)
            .SetTargetPath(Solution)
            .SetConfiguration(Configuration)
            .SetAssemblyVersion(GitVersion.AssemblySemVer)
            .SetFileVersion(GitVersion.AssemblySemFileVer)
            .SetInformationalVersion(GitVersion.InformationalVersion)
            .SetMaxCpuCount(Environment.ProcessorCount)
            .SetNodeReuse(IsLocalBuild);
}