using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System;
public class InterfaceChanger : AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        if (!path.EndsWith(".cs.meta")) return;

        string scriptPath = path.Substring(0, path.Length - 5);
        if (!File.Exists(scriptPath)) return;

        string content = File.ReadAllText(scriptPath);

        // public class クラス名 : MonoBehaviour を検索
        var match = Regex.Match(content, @"public\s+class\s+(\w+)\s*:\s*MonoBehaviour");
        if (!match.Success) return;

        // キャプチャしたクラス名
        string className = match.Groups[1].Value;

        if (className[0]!= 'I' || !Char.IsUpper(className[1]))
        {
            // クラス名が 'I' で始まらない場合は処理を終了
            return;
        }

        // Monobehaviour を interface に置換
        string newContent = Regex.Replace(
            content,
            @"public\s+class\s+\w+\s*:\s*MonoBehaviour",
            $"public interface {className}"
        );

        // Start メソッドと Update メソッドを削除
        newContent = Regex.Replace(newContent,
            @"void\s+(Start|Update)\s*\([^)]*\)\s*\{[^}]*\}",
            "",
            RegexOptions.Singleline);

        // 行頭のコメントを削除
        newContent = Regex.Replace(newContent, @"^\s*//.*$", "", RegexOptions.Multiline);

        // 連続する空行を1行にまとめる
        newContent = Regex.Replace(newContent, @"\n\s*\n", "\n\n");

        // text の改行コードを Windows 用 (\r\n) に統一
        newContent = newContent.Replace("\r\n", "\n"); // まず \r\n を \n に統一
        newContent = newContent.Replace("\r", "\n");   // 古い Mac 改行 \r を \n に統一
        newContent = newContent.Replace("\n", "\r\n"); // 最後に Windows 改行に変換


        File.WriteAllText(scriptPath, newContent);
        AssetDatabase.Refresh();
    }

}
