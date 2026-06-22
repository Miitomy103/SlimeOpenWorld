using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StateMachine
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StateDictionary))]
    public class StateDictionaryEditor : Editor
    {
        private StateDictionary m_targetObject;
        private Dictionary<string, ComponentState> m_states = new();
        private IState[] m_stateComponents;
        private string m_searchText = "";
        private bool m_isSearchFocused = false;
        private Vector2 m_scrollPosition = Vector2.zero;


        // Static Field
        private static readonly string k_SearchFocusedName = "SearchFocused";
        private static readonly float k_SearchFieldMaxWidth = 300f;
        private static readonly float k_ScrollFieldMaxHeight = 80f;
        private static readonly float k_ScrollFieldMaxWidth = k_SearchFieldMaxWidth;


        /// <summary>
        /// Inspector上での開閉トグル状態とIStateの組。
        /// </summary>
        class ComponentState
        {
            public bool Toggle;
            public readonly IState State;
            public ComponentState(bool toggle, IState state)
            {
                this.Toggle = toggle;
                this.State = state;
            }
            public void SetToggle(bool toggle)
            {
                this.Toggle = toggle;
            }
        }


        /// <summary>
        /// 対象オブジェクトのデフォルトInspectorを描画する。
        /// </summary>
        /// <param name="targetObject"></param>
        private void DrawObjectInspector(Object targetObject)
        {
            var targetSerializedObject = new SerializedObject(targetObject);
            targetSerializedObject.UpdateIfRequiredOrScript();

            SerializedProperty iterator = targetSerializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (iterator.propertyPath == "m_Script")
                {
                    continue;
                }
                EditorGUILayout.PropertyField(iterator, true);
                enterChildren = false;
            }

            targetSerializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// ステートをDictionaryに登録する
        /// </summary>
        private void RegisterState()
        {
            m_targetObject = (StateDictionary)target;
            m_stateComponents = m_targetObject.GetComponents<IState>();

            foreach (var component in m_stateComponents)
            {
                if (m_states.TryAdd(component.GetType().Name, new ComponentState(false, component)))
                {
                    var hideComponent = (Object)component;
                    hideComponent.hideFlags = HideFlags.HideInInspector;
                }
                else
                {
                    DestroyImmediate((Object)component);
                    Debug.LogWarning($"{m_targetObject} : {component.GetType().Name}は既に登録されています。");
                }
            }
        }


        /// <summary>
        /// 検索欄を描画
        /// </summary>
        private void DrawSearchBox()
        {
            GUI.SetNextControlName(k_SearchFocusedName);

            m_searchText = EditorGUILayout.TextField(
                new GUIContent("Search"),
                m_searchText,
                EditorStyles.toolbarSearchField,
                GUILayout.MaxWidth(k_SearchFieldMaxWidth));
        }


        /// <summary>
        /// 検索ボックスにフォーカスがある間、ステート名のトグル一覧を表示する。
        /// </summary>
        private void DrawSearchToggles()
        {
            if (!m_isSearchFocused) { return; }

            using (var scrollView = new EditorGUILayout.ScrollViewScope(m_scrollPosition, EditorStyles.helpBox,
                    GUILayout.MaxHeight(k_ScrollFieldMaxHeight),
                    GUILayout.MaxWidth(k_ScrollFieldMaxWidth)))
            {
                m_scrollPosition = scrollView.scrollPosition;

                foreach (var state in m_states)
                {
                    bool toggle = GUILayout.Toggle(state.Value.Toggle, state.Key, EditorStyles.toolbarButton);

                    if (toggle && toggle != state.Value.Toggle)
                    {
                        ToggleDrawFlag(state.Key);
                    }

                    EditorGUI.FocusTextInControl(k_SearchFocusedName);
                }
            }
        }


        private void ToggleDrawFlag(string flag)
        {
            foreach (var state in m_states)
            {
                state.Value.Toggle = flag == state.Key;
            }
        }


        /// <summary>
        /// 登録のし直しとかに使う
        /// </summary>
        private void OnEnable()
        {
            RegisterState();
        }


        /// <summary>
        /// 実際に描画する処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            m_targetObject = (StateDictionary)target;

            m_isSearchFocused = GUI.GetNameOfFocusedControl() == k_SearchFocusedName;

            DrawSearchBox();

            DrawSearchToggles();

            foreach (var state in m_states)
            {
                if (!state.Value.Toggle)
                {
                    continue;
                }
                DrawObjectInspector((Object)state.Value.State);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }
    }
#endif
}
