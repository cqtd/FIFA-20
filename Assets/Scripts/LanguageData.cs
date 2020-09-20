using UnityEngine;

namespace EA.FIFA20.UI
{
	[CreateAssetMenu(menuName = "Create LanguageData", fileName = "LanguageData", order = 30)]
	public class LanguageData : ScriptableObject
	{
		public string languageName;
		public string languageCode;
		public Sprite flag;
	}
}