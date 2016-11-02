using System;
using UnityEngine;
using Wizard;
using Object = UnityEngine.Object;

namespace Olivia
{
    public class Loader
    {
        public static void Load()
        {
            try
            {
                var uiRoot = ToolboxGame.GameManager.m_GameManagerObj;
                var existingComponents = uiRoot.GetComponents(typeof(Component));
                
                foreach (var existingComponent in existingComponents)
                {
                    var componentName = existingComponent.GetType();
                    
                    if (componentName.Name.Contains("Bootstrap"))
                    {
                        Logger.AppendLine($"Removing {componentName}");
                        var comp = uiRoot.GetComponent(componentName);
                        Object.DestroyObject(comp);
                    }
                }

                var newType = DynamicTypeGenerator.GetNewType(typeof(Bootstrap));
                uiRoot.AddComponent(newType);
            }
            catch (Exception e)
            {
                Logger.AppendLine(e.ToString());
            }
        }

        public static void Unload()
        {
            var uiRoot = ToolboxGame.GameManager.m_GameManagerObj;
            var existingComponents = uiRoot.GetComponents(typeof(Component));

            foreach (var existingComponent in existingComponents)
            {
                var componentName = existingComponent.GetType();

                if (componentName.Name.Contains("Bootstrap"))
                {
                    Logger.AppendLine($"Removing {componentName}");
                    var comp = uiRoot.GetComponent(componentName);
                    Object.DestroyObject(comp);
                }
            }

        }
    }
}