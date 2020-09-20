using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EA.FIFA20.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour
    {
        [SerializeField] Button button;

        #if UNITY_EDITOR
        void Reset()
        {
            button = GetComponent<Button>();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        #endif
    }
}