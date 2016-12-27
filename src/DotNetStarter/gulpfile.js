/// <binding AfterBuild="" BeforeBuild="" ProjectOpened="" Clean="clean:packages" />

// global variables
var nugetServers = [
    "http://nuget.com/api/odata"
];

var directory = "./_packages-nuget/";
var pushedFolder = directory;
var nugetPath = 'C:/_Web/Utils/nuget.exe';

// dependencies
var gulp = require('gulp'),
    nuget = require('gulp-nuget'), // https://www.npmjs.com/package/gulp-nuget
    msbuild = require("gulp-msbuild"), // https://www.npmjs.com/package/gulp-msbuild
    mstest = require("gulp-mstest"),
    del = require("del"),
    mergeStream = require('ordered-merge-stream'),
    assemblyInfo = require('gulp-dotnet-assembly-info'), /// https://www.npmjs.com/package/gulp-dotnet-assembly-info 
    fs = require("fs"); // read files;

// gets path to solution relative to gulpjs file
var solutionDir = process.cwd() + '\\..\\..\\';
var nugetVersion = "";
var nugetProps = "";

function cleanPackages() {
    return del([directory + '*.nupkg', "./TestResults/**/*"]);
}

gulp.task('default', ['nuget:push'], cleanPackages);

gulp.task('clean:packages', cleanPackages);

gulp.task('msbuild:test', ['msbuild:release'], function () {
    var testPath = "..//../**/bin/[Rr]elease/*[Tt]est*.dll";

    return gulp.src(testPath).pipe(mstest({
        outputEachResult: true,
        quitOnFailed: true,
        errorStackTrace: false,
        errorMessage: true
    }));
});

gulp.task("msbuild:getversion", ["msbuild:release"], function () {
    var assemblyInfoFileContents = fs.readFileSync('./Properties/AssemblyInfo.cs', "utf8");
    var assembly = assemblyInfo.getAssemblyMetadata(assemblyInfoFileContents);
    var title = assembly["AssemblyTitle"];
    nugetVersion = assembly["AssemblyInformationalVersion"];

    var currentYear = new Date().getFullYear();
    nugetProps = "year=" + currentYear;
    nugetProps += ";title=" + title;

    console.log("version = " + nugetVersion + " props = " + nugetProps);
});

gulp.task("msbuild:release", ["clean:packages"], function () {
    var options = {
        targets: ['Clean', 'Rebuild'],
        stdout: true,
        properties: { Configuration: 'Release', SolutionDir: solutionDir },
        toolsVersion: 14.0
    };

    return gulp.src('../../*.sln').pipe(msbuild(options));
});

gulp.task('nuget:pack', ['msbuild:test', 'msbuild:getversion'], function () {    
    var options = {
        nuget: nugetPath,
        version: nugetVersion,
        outputDirectory: directory,
        properties: 'configuration=release;' + nugetProps,
        symbols: true
    };

    return gulp.src('./*.nuspec').pipe(nuget.pack(options));
});

gulp.task('nuget:push', ["nuget:pack"], function () {
    var src = directory + '*.nupkg';
    var tasks = [];
    nugetServers.forEach(function (server) {
        var options = {
            nuget: nugetPath,
            source: server,
            timeout: '300'
        };

        tasks.push(
            gulp.src(src)
            .pipe(nuget.push(options))
        );
    });

    tasks.push(gulp.src(src).pipe(gulp.dest(pushedFolder)));

    return mergeStream(tasks);
});