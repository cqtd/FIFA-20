using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EA.FIFA20.UI
{
	public class MainLanguageCanvas : MonoBehaviour, ILoadingUI
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

		IDisposable[] arrowDisposables;
		
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

			const float scaledUp = 1.25f;
			const float scaleDown = 0.7f;
			const float pointerEnterDurataion = 0.2f;
			const float pointerClickDuration = 0.1f;

			arrowDisposables = new[]
			{

				leftArrow.image.OnPointerEnterAsObservable().Subscribe(e =>
				{
					leftArrow.transform.DOScale(scaledUp, pointerEnterDurataion);
				}),

				leftArrow.image.OnPointerExitAsObservable().Subscribe(e =>
				{
					leftArrow.transform.DOScale(1.0f, pointerEnterDurataion);
				}),

				leftArrow.image.OnPointerClickAsObservable().Subscribe(e =>
				{
					var t1 = leftArrow.transform.DOScale(scaleDown, pointerClickDuration);
					t1.onComplete = () =>
					{
						leftArrow.transform.DOScale(scaledUp, pointerClickDuration);
					};
				}),

				rightArrow.image.OnPointerEnterAsObservable().Subscribe(e =>
				{
					rightArrow.transform.DOScale(scaledUp, pointerEnterDurataion);
				}),

				rightArrow.image.OnPointerExitAsObservable().Subscribe(e =>
				{
					rightArrow.transform.DOScale(1.0f, pointerEnterDurataion);
				}),

				rightArrow.image.OnPointerClickAsObservable().Subscribe(e =>
				{
					var t1 = rightArrow.transform.DOScale(scaleDown, pointerClickDuration);
					t1.onComplete = () =>
					{
						rightArrow.transform.DOScale(scaledUp, pointerClickDuration);
					};
				})
			};
		}

		void EndSelectingLanguage()
		{
			languageSelected = true;
			isSelectingLanguage = false;
			
			languagePanel.interactable = false;
			languagePanel.blocksRaycasts = false;

			foreach (IDisposable disposable in arrowDisposables)
			{
				disposable.Dispose();
			}
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