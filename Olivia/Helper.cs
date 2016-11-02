using UnityEngine;

namespace Olivia
{
    public static class Helper
    {
        public static void LogGameObjectComponents(GameObject gameObject)
        {
            Logger.AppendLine($"LogGameObjectComponents name:{gameObject.name}");
            Logger.AppendLine($"LogGameObjectComponents lossyScale:{gameObject.transform.lossyScale}");
            Logger.AppendLine($"LogGameObjectComponents position:{gameObject.transform.position}");
            Logger.AppendLine($"LogGameObjectComponents localRotation:{gameObject.transform.localRotation}");
            Logger.AppendLine($"LogGameObjectComponents rotation:{gameObject.transform.rotation}");
            var comps = gameObject.GetComponents<Component>();
            foreach (var component in comps)
            {
                Logger.AppendLine($"component type:{component.GetType()}");
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Logger.AppendLine($"child go name:{gameObject.transform.GetChild(i).name}");
            }
        }

        public static GameObject GetChildObjectContainingName(this GameObject gameObject, string name)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).name.Contains(name))
                    return gameObject.transform.GetChild(i).gameObject;
            }

            return null;
        }

        public static GameObject GetParentGameObject(this GameObject go)
        {
            return go.transform.parent.gameObject;
        }

        public static void PopulateTransform(this GameObject gameObject, GameObject otherGameObject)
        {
            gameObject.transform.localPosition = new Vector3(otherGameObject.transform.position.x, otherGameObject.transform.position.y, otherGameObject.transform.position.z);
            gameObject.transform.localScale = new Vector3(otherGameObject.transform.localScale.x, otherGameObject.transform.localScale.y, otherGameObject.transform.localScale.z);
            gameObject.transform.localRotation = new Quaternion(otherGameObject.transform.localRotation.x, otherGameObject.transform.localRotation.y, otherGameObject.transform.localRotation.z, otherGameObject.transform.localRotation.w);
            gameObject.transform.rotation = new Quaternion(otherGameObject.transform.rotation.x, otherGameObject.transform.rotation.y, otherGameObject.transform.rotation.z, otherGameObject.transform.rotation.w);
        }

        public static void ZeroPositionTransform(this GameObject gameObject)
        {
            gameObject.transform.localPosition = Vector3.zero;
        }
    }
}