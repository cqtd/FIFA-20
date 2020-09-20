using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EA.FIFA20.UI
{
	public class MainLanguageCanvas : MonoBehaviour
	{
		[SerializeField] CanvasGroup languagePanel = default;
		[SerializeField] CanvasGroup eula = default;
		
		[SerializeField] float tweenDuraction  = 0.4f;
		[SerializeField] float eulaDuration = 7.0f;

		[SerializeField] Button leftArrow = default;
		[SerializeField] Button rightArrow = default;
		[SerializeField] Image flagImage = default;
		[SerializeField] TextMeshProUGUI localize = default;

		LanguageData[] loadedLanguages;
		bool languageSelected;
		bool isSelectingLanguage;

		int index = -1;
		
		void Awake()
		{
			languagePanel.alpha = 0;
			eula.alpha = 0;

			languagePanel.interactable = false;
			languagePanel.blocksRaycasts = false;
			
			eula.interactable = false;
			eula.blocksRaycasts = false;
		}

		IEnumerator Start()
		{
			yield return LoadLanguageData();

			bool next = false;
			var tweener = languagePanel.DOFade(1.0f, tweenDuraction);
			tweener.onComplete = () =>
			{
				next = true;
			};
			while (!next) yield return null;
			
			BeginSelectingLanguage();

			while (!languageSelected) yield return null;

			next = false;
			tweener = languagePanel.DOFade(0.0f, tweenDuraction);
			tweener.onComplete = () =>
			{
				next = true;
			};
			while (!next) yield return null;
			
			languagePanel.gameObject.SetActive(false);

			next = false;
			tweener = eula.DOFade(1.0f, tweenDuraction);
			tweener.onComplete = () =>
			{
				next = true;
			};
			while (!next) yield return null;
			yield return new WaitForSeconds(eulaDuration);
			
			next = false;
			tweener = eula.DOFade(0.0f, tweenDuraction);
			tweener.onComplete = () =>
			{
				next = true;
			};
			while (!next) yield return null;
		}

		IEnumerator LoadLanguageData()
		{
			ResourceRequest load = Resources.LoadAsync<LanguagePack>("Localization/LanguagePack");
			while (!load.isDone) yield return null;

			loadedLanguages = ((LanguagePack) load.asset).releases;
			index = 0;
			
			SetLanguage();
		}

		void BeginSelectingLanguage()
		{
			languagePanel.interactable = true;
			languagePanel.blocksRaycasts = true;
			
			leftArrow.onClick.AddListener(OnLeft);
			rightArrow.onClick.AddListener(OnRight);

			isSelectingLanguage = true;
		}

		void EndSelectingLanguage()
		{
			languageSelected = true;
			isSelectingLanguage = false;
			
			languagePanel.interactable = false;
			languagePanel.blocksRaycasts = false;
		}

		void SetLanguage()
		{
			var data = loadedLanguages[this.index];
			flagImage.sprite = data.flag;
			localize.SetText(data.languageName);
		}

		void OnLeft()
		{
			index--;
			index += loadedLanguages.Length;
			index %= loadedLanguages.Length;
			
			SetLanguage();
		}

		void OnRight()
		{
			index++;
			index %= loadedLanguages.Length;
			
			SetLanguage();
		}

		void Select()
		{
			EndSelectingLanguage();
		}

		void Update()
		{
			if (!isSelectingLanguage) return;

			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				OnLeft();
			}

			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				OnRight();
			}

			if (Input.GetKeyDown(KeyCode.Return))
			{
				Select();
			}
		}
	}
}