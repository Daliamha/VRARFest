using System.Collections.Generic;
using BaCon;
using UnityEngine;
using R3;
using TMPro;
using UnityEngine.UI;
using VRFest.Scripts.Game.Gameplay;

namespace VRFest.Scripts.Game.MainMenu
{

    public class MainMenuEntryPoint : MonoBehaviour
    {
        private Subject<GameplayEnterParams> _exitSceneSignalSubj;
        [SerializeField] private GameObject _registrationPanel;
        [SerializeField] private Button _openMenu;
        [SerializeField] private TextMeshProUGUI _nameAndAgeText;
        [SerializeField] private TextMeshProUGUI _genderText;
        [SerializeField] private List<GameObject> _panels = new();
        
        public Observable<GameplayEnterParams> Run(DIContainer mainMenuContainer, MainMenuEnterParams enterParams)
        {
            var exitSignalSubj = new Subject<GameplayEnterParams>();

            foreach (var panel in _panels)
            {
                panel.SetActive(false);
            }
            
            if (!PlayerPrefs.HasKey("State"))
            {
                _registrationPanel.SetActive(true);
            }
            else
            {
                _panels[PlayerPrefs.GetInt("Pol")].SetActive(true);
            }
            _openMenu.onClick.AddListener(() =>
            {
                _nameAndAgeText.text = PlayerPrefs.GetString("Name") + "   "
                                                                     + PlayerPrefs.GetString("Age");
                _genderText.text = PlayerPrefs.GetInt("Gender") == 0 ? "Мальчик" : "Девочка";
            });
            
            Bind(exitSignalSubj);
            return exitSignalSubj;
        }
        
        public void HanleGoToGameplayButtonClicked(GameObject button)
        {
            bool isEducation = button.name.Contains("Education");
            var enterParams = new GameplayEnterParams(button.name, isEducation);
            _exitSceneSignalSubj.OnNext(enterParams);
        }

        public void BindExitButton()
        {
            Debug.Log("Bind ExitButton");
            Application.Quit();
        }
        
        private void Bind(Subject<GameplayEnterParams> exitSceneSignalSubj)
        {
            _exitSceneSignalSubj = exitSceneSignalSubj;
        }
    }
}