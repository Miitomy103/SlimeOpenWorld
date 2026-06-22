using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 作業中・残り・完了タスクを管理する簡易Todoリストのエディタウィンドウ。
/// </summary>
public class TodoList : EditorWindow
{
    [System.Serializable]
    public class TaskContainer : ScriptableObject
    {
        public string currentTask = "";
        public List<string> remainingTasks = new List<string>();
        public List<string> completedTasks = new List<string>();
    }

    private TaskContainer taskData;
    private SerializedObject serializedObjectRef;
    private Vector2 scrollPos;
    private const string assetPath = "Assets/TodoList.asset";

    [MenuItem("Window/Todo List")]
    public static void ShowWindow()
    {
        GetWindow<TodoList>("Todo List");
    }

    private void OnEnable()
    {
        // 既存アセットをロード
        taskData = AssetDatabase.LoadAssetAtPath<TaskContainer>(assetPath);

        // 存在しなければ作成
        if (taskData == null)
        {
            taskData = ScriptableObject.CreateInstance<TaskContainer>();
            AssetDatabase.CreateAsset(taskData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        serializedObjectRef = new SerializedObject(taskData);
    }

    private void OnGUI()
    {
        serializedObjectRef.Update();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.LabelField("タスク管理", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // ==== タスク中 ====
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("タスク中", EditorStyles.boldLabel);

        if (!string.IsNullOrEmpty(taskData.currentTask))
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(taskData.currentTask);

            if (GUILayout.Button("中断", GUILayout.Width(60)))
            {
                taskData.remainingTasks.Add(taskData.currentTask);
                taskData.currentTask = "";
            }

            if (GUILayout.Button("完了", GUILayout.Width(60)))
            {
                taskData.completedTasks.Add(taskData.currentTask);
                taskData.currentTask = "";
            }

            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.LabelField("現在作業中のタスクはありません");
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // ==== 残りのタスク ====
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("残りのタスク", EditorStyles.boldLabel);

        int removeIndex = -1;
        int startIndex = -1;

        for (int i = 0; i < taskData.remainingTasks.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            taskData.remainingTasks[i] = EditorGUILayout.TextField(taskData.remainingTasks[i]);

            if (GUILayout.Button("開始", GUILayout.Width(60)))
            {
                if (!string.IsNullOrEmpty(taskData.currentTask))
                    taskData.remainingTasks.Add(taskData.currentTask);

                taskData.currentTask = taskData.remainingTasks[i];
                startIndex = i;
            }

            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                removeIndex = i;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (startIndex >= 0 && startIndex < taskData.remainingTasks.Count)
            taskData.remainingTasks.RemoveAt(startIndex);

        if (removeIndex >= 0 && removeIndex < taskData.remainingTasks.Count)
            taskData.remainingTasks.RemoveAt(removeIndex);

        if (GUILayout.Button("+ タスクを追加"))
            taskData.remainingTasks.Add("");

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        // ==== 完了タスク ====
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("完了したタスク", EditorStyles.boldLabel);

        removeIndex = -1;

        for (int i = 0; i < taskData.completedTasks.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(taskData.completedTasks[i]);

            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                removeIndex = i;
            }

            EditorGUILayout.EndHorizontal();
        }
        if (removeIndex >= 0 && removeIndex < taskData.completedTasks.Count)
            taskData.completedTasks.RemoveAt(removeIndex);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        serializedObjectRef.ApplyModifiedProperties();

        // ==== 編集があったときだけ保存 ====
        if (serializedObjectRef.hasModifiedProperties || GUI.changed)
        {
            EditorUtility.SetDirty(taskData);
            AssetDatabase.SaveAssets();
        }
    }
}
