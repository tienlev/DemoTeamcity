using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.Build.Reporting;

public static class Builder
{
    [MenuItem("Builder/Build Android")]
    public static void BuildAndroid()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetBuildScenes();
        buildPlayerOptions.locationPathName = "Builds/Android/AndroidGame.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Console.WriteLine("Build Success!!!");
        }
    }

    private static string[] GetBuildScenes()
    {
        return EditorBuildSettings.scenes.Where(scene => scene.enabled && !string.IsNullOrEmpty(scene.path))
            .Select(scene => scene.path).ToArray();
    }
}