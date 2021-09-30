using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RulesController : Singleton<RulesController>, IGameState
    {
        [Header("Debug")]
        [SerializeField] GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
            }
        }

        public void OnGameStateChanged() => GameState = MainController.Instance.GameState;
    }
}
