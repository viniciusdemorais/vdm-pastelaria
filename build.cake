#tool nuget:?package=ReportGenerator&version=5.0.0

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Build")
    .Does(() =>
{
    DotNetCoreBuild("VDM.Pastelaria.sln");
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    if (System.IO.Directory.Exists("testresults"))
        System.IO.Directory.Delete("testresults", true);

    var settings = new DotNetCoreTestSettings
    {
        NoRestore = true,
        ArgumentCustomization  = x => x.Append("/p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:ExcludeByFile=**/Program.cs"),
        ResultsDirectory = "testresults",
        Verbosity = DotNetCoreVerbosity.Quiet
    };

    DotNetCoreTest("VDM.Pastelaria.sln", settings);
});

Task("Coverage")
    .IsDependentOn("Test")
    .Does(() =>
{
    if (System.IO.Directory.Exists("coverageOutput"))
        System.IO.Directory.Delete("coverageOutput", true);

    GlobPattern coverageFiles = "./test/**/*.opencover.xml";

    ReportGenerator(coverageFiles, "./coverageOutput", new ReportGeneratorSettings  { ReportTypes = new []
    {
        ReportGeneratorReportType.TextSummary,
        ReportGeneratorReportType.HtmlInline_AzurePipelines_Dark
    }});
});

RunTarget(target);