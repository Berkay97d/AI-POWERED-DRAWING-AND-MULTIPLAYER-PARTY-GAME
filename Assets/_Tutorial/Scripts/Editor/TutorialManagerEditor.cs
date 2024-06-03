using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace _Tutorial.Editor
{
    [CustomEditor(typeof(TutorialManager))]
    public class TutorialManagerEditor : UnityEditor.Editor
    {
        private static Type[] ms_AllTutorialTypes;
        
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            OnAddTutorialButtonsGUI();
        }


        private void OnAddTutorialButtonsGUI()
        {
            ms_AllTutorialTypes ??= GetAllTutorialTypes();

            foreach (var tutorialType in ms_AllTutorialTypes)
            {
                OnAddTutorialButtonGUI(tutorialType);
            }
        }
        
        private void OnAddTutorialButtonGUI(Type type)
        {
            var title = $"Add {type.Name}";
            
            if (!GUILayout.Button(title)) return;
            
            Undo.RecordObject(target, title);
            
            var tutorials = GetTutorials();
            var tutorial = (Tutorial) Activator.CreateInstance(type);
            
            tutorials.Add(tutorial);
        }
        
        private List<Tutorial> GetTutorials()
        {
            return (List<Tutorial>) typeof(TutorialManager)
                .GetField("tutorials", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(target);
        }
        
        
        private static Type[] GetAllTutorialTypes()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && typeof(Tutorial).IsAssignableFrom(type))
                .ToArray();
        }
    }
}