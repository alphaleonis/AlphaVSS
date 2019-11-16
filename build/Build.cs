using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.VSWhere;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using Nuke.DocFX;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using Nuke.Common.CI.AzurePipelines;
using System.Threading;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[MSBuildVerbosityMapping]
class AlphaVssBuild : NukeBuild
{
   public static int Main() => Execute<AlphaVssBuild>(x => x.Compile);
   

   [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
   readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

   [Parameter]
   readonly string FeedUri;

   [Parameter]
   readonly string NuGetApiKey = "VSTS";

   [Solution] readonly Solution Solution;
   [GitRepository] readonly GitRepository GitRepository;
   [GitVersion] readonly GitVersion GitVersion;

   string RequiredMSBuildVersion = "[16.4,)";

   AbsolutePath SourceDirectory => RootDirectory / "src";
   AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
   AbsolutePath NuSpecDirectory => RootDirectory / "build" / "nuget";
   AbsolutePath DocFxFile => RootDirectory / "docs" / "docfx.json";
   AbsolutePath PackageArtifactsDirectory => ArtifactsDirectory / "package";
   AbsolutePath DocFxArtifactsDirectory => ArtifactsDirectory / "docs";
   AbsolutePath DocFxZipFilePath => ArtifactsDirectory / "docs.zip";

   string MSBuildToolPath;

   protected override void OnBuildInitialized()
   {
      base.OnBuildInitialized();
      var result =
         VSWhereTasks.VSWhere(s => s
            .SetVersion(RequiredMSBuildVersion)
            .EnableLatest()
            .EnablePrerelease()
            .EnableUTF8()
            .SetLogOutput(Verbosity == Verbosity.Verbose)
            .SetProperty("InstallationPath")
            .SetFormat(VSWhereFormat.value)
      ).Output.EnsureOnlyStd().FirstOrDefault().Text;

      var vsInstance = result.FirstOrDefault().NotNull($"Unable to find VS version {RequiredMSBuildVersion}");
      MSBuildToolPath = Path.Combine(result, "MSBuild\\Current\\Bin\\MSBuild.exe");
      Logger.Normal($"Using MSBuild at \"{MSBuildToolPath}\"");
      MSBuildLogger = CustomMSBuildLogger;
      NuGetLogger = CustomNuGetLogger;
      if (IsServerBuild)
      { 
         AzurePipelines.Instance.UpdateBuildNumber($"AlphaVSS-{GitVersion.SemVer}")
            ;
      }
   }

   internal static void CustomMSBuildLogger(OutputType type, string output)
   {
      if (type == OutputType.Err || output.IndexOf(": error", StringComparison.Ordinal) != -1)
         Logger.Error(output);
      else if (output.IndexOf(": warning", StringComparison.Ordinal) != -1)
         Logger.Warn(output);
      else
         Logger.Normal(output);

   }

   internal static void CustomNuGetLogger(OutputType type, string output)
   {
      if (type == OutputType.Err || output.StartsWith("ERROR:", StringComparison.OrdinalIgnoreCase))
         Logger.Error(output);
      else if (output.StartsWith("WARNING:", StringComparison.OrdinalIgnoreCase))
         Logger.Warn(output);
      else
         Logger.Normal(output);
   }

   Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
           SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
           EnsureCleanDirectory(ArtifactsDirectory);
        });

   Target Restore => _ => _
       .Executes(() =>
       {
          MSBuild(_ => _
               .SetToolPath(MSBuildToolPath)
               .SetTargetPath(Solution)
               .SetTargets("Restore"));
       });

   Target Compile => _ => _
       .DependsOn(Restore)
       .Executes(() =>
       {
          BuildProject("AlphaVSS.Common", Configuration, "AnyCPU");

          BuildPlatformProject("core31");
          BuildPlatformProject("net45");

          BuildProject("AlphaShadow", Configuration, "AnyCPU");


          void BuildPlatformProject(string projectConfigurationPrefix)
          {
             projectConfigurationPrefix = Configuration == Configuration.Debug ? $"{projectConfigurationPrefix}d" : projectConfigurationPrefix;
             BuildProject("AlphaVSS.Platform", projectConfigurationPrefix, "Win32");
             BuildProject("AlphaVSS.Platform", projectConfigurationPrefix, "x64");
          }

          void BuildProject(string projectName, string configuration, string platform)
          {
             var project = Solution.AllProjects.FirstOrDefault(p => p.Name == projectName).NotNull($"Unable to find project named {projectName} in solution {Solution.Name}");

             MSBuild(_ => _
                  .SetToolPath(MSBuildToolPath)
                  .SetTargetPath(project)
                  .SetTargetPlatform((MSBuildTargetPlatform)platform)
                  .SetConfiguration(configuration)
                  .SetTargets("Build")
                  .SetAssemblyVersion(GitVersion.AssemblySemVer)
                  .SetFileVersion(GitVersion.AssemblySemFileVer)
                  .SetInformationalVersion(GitVersion.InformationalVersion)
                  .AddProperty("BuildProjectReferences", false)
                  .AddProperty("AlphaVss_VersionMajor", 1)
                  .SetInformationalVersion(GitVersion.InformationalVersion)
                  .SetMaxCpuCount(Environment.ProcessorCount)
                  .SetNodeReuse(IsLocalBuild));
          }
       });

   Target DocMetadata => _ => _
      .DependsOn(Compile)
      .Executes(() =>
      {
         DocFXTasks.DocFXMetadata(s => s
            .SetProjects(DocFxFile)
            .SetLogLevel(DocFXLogLevel.Verbose)            
         );
      });

   Target DocBuild => _ => _
      .DependsOn(DocMetadata)
      .Executes(() =>
      {
         DocFXTasks.DocFXBuild(s => s
            .SetConfigFile(DocFxFile)
            .SetLogLevel(DocFXLogLevel.Verbose)
            );         
      });

   Target DocPack => _ => _
      .DependsOn(DocBuild)
      .Executes(() =>
      {
         CompressionTasks.CompressZip(DocFxArtifactsDirectory, DocFxZipFilePath);
      });

   Target ServeDocs => _ => _
      .DependsOn(DocBuild)
      .OnlyWhenStatic(() => IsLocalBuild)
      .Executes(() =>
      {
         DocFXTasks.DocFXServe(s => s
            .SetFolder(ArtifactsDirectory / "docs")
         );
      });

   Target Build => _ => _
      .DependsOn(Clean, Compile);

   Target Pack => _ => _
      .DependsOn(Build)
      .Executes(() =>
      {
         var version = GitVersion.NuGetVersion;
         if (IsLocalBuild)
            version += DateTime.UtcNow.ToString("yyMMddHHmmss");

         foreach (var nuspec in GlobFiles(NuSpecDirectory, "*.nuspec"))
         {
            NuGetPack(s => s
               .SetMSBuildPath(MSBuildToolPath)
               .SetOutputDirectory(ArtifactsDirectory)
               .AddProperty("branch", GitVersion.BranchName)
               .AddProperty("commit", GitVersion.Sha)
               .SetVersion(version)
               .SetTargetPath(nuspec)
            );
         }
      });

   Target Push => _ => _
      .DependsOn(Pack)
      .Requires(() => FeedUri)
      .Executes(() =>
      {
         foreach (var file in GlobFiles(ArtifactsDirectory, "*.nupkg"))
         {
            NuGetPush(s => s
               .SetApiKey(NuGetApiKey)
               .SetSource(FeedUri)
               .SetTargetPath(file));
         }
      });

   Target UploadArtifacts => _ => _
      .DependsOn(Clean, Pack, DocPack)
      .OnlyWhenStatic(() => IsServerBuild)
      .Executes(() =>
      {
         foreach (var file in GlobFiles(ArtifactsDirectory, "*.nupkg"))
         {
            AzurePipelines.Instance.UploadArtifacts("Package", Path.GetFileName(file), file);
         }
         Thread.Sleep(2000);
         AzurePipelines.Instance.UploadArtifacts("docs", "docs", DocFxZipFilePath);         
      });

   Target DistBuild => _ => _
      .DependsOn(Pack, UploadArtifacts);
}
