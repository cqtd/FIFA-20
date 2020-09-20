using UnityEngine;

namespace EA.FIFA20.UI
{
	[CreateAssetMenu(menuName = "Create LanguagePack", fileName = "LanguagePack", order = 31)]
	public class LanguagePack : ScriptableObject
	{
		public LanguageData[] releases = new LanguageData[0];
	}
}