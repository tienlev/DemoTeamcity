using UnityEditor;
using System.IO;
using System.Linq;
using UnityEditor.Build.Reporting;

/// <summary>
/// 独自のビルドパイプラインを実行するEditor拡張
/// </summary>
public class CustomBuild : EditorWindow
{
    [MenuItem("MyTool/build/dev")]
    private static void DevBuild()
    {
        DoCustomBuildPipeLine();
    }
    
    /// <summary>
    /// CICDのワークフローから実行する時はこちらを呼び出す
    /// </summary>
    private static void DevBuildForCICD()
    {
        DoCustomBuildPipeLine(true);
    }

    /// <summary>
    /// 独自ビルドパイプライン実行
    /// </summary>
    private static void DoCustomBuildPipeLine(bool isCICD = false)
    {
        //保存先のパス取得
        var path = EditorUtility.SaveFolderPanel("Choose Location", "", "");

        //パスが入っていれば選択されたとみなす（キャンセルされたら入ってこない）
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        //ビルドパイプライン設定
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = Path.Combine(path, "Custom Build"),
            target = BuildTarget.WebGL,
            options = BuildOptions.Development
        };

        //Build
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;

        if (isCICD)
        {
            //成否に応じてUnityEditorの終了プロセスを決定する
            EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
        }
    }
}
