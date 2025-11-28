using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VRFest.Scripts.Game.MainMenu
{
    public class RegistrationManager : MonoBehaviour
    {
        
        public TMP_InputField _username;
        public TMP_InputField _age;
        public TMP_Dropdown _pol;
        [SerializeField] private List<GameObject> _panels = new List<GameObject>();
        [SerializeField] private FlaskUploader _uploader;
        
        
        public void Save()
        {
            if (_username.text != "" && _age.text != "")
            {
                PlayerPrefs.SetString("Name", _username.text);
                PlayerPrefs.SetString("Age", _age.text);
                PlayerPrefs.SetInt("Pol", _pol.value);
                _panels[_pol.value].SetActive(true);
                var players = new List<Player>
                {
                    new Player(
                        name: _username.text,
                        gender: _pol.value == 0 ? "Девочка" : "Мальчик",
                        age: int.Parse(_age.text),
                        scores: new List<ScoreEntry>
                        {
                            
                        }
                    ),
                };

                _uploader.SendPlayersToServer(players);
                gameObject.SetActive(false);
            }
        }

    }
}