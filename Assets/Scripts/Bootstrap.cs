using System.Collections;
using UnityEngine;

namespace EA.FIFA20.UI
{
	interface ILoadingUI
	{
		
	}
	
	public class Bootstrap : MonoBehaviour
	{
		[SerializeField] Canvas root;
		ILoadingUI loadingCanvas;
		ILoadingUI languageCanvas;

		void Awake()
		{
			
		}
		
		IEnumerator Start()
		{
			var loading = Resources.LoadAsync<MainLoadingCanvas>("UI/Canvas/Canvas Main Loading");
			while (!loading.isDone) yield return null;

			bool done = false;
			var instance = Instantiate(loading.asset as MainLoadingCanvas, root.transform);
			instance.SetTimer(8f, () => done = true);
			
			while (!done) yield return null;
			
			loading = Resources.LoadAsync<MainLanguageCanvas>("UI/Canvas/Canvas Language");
			while (!loading.isDone) yield return null;
			
			var inst = Instantiate(loading.asset as MainLanguageCanvas, root.transform);
			instance.gameObject.SetActive(false);
		}
	}
}