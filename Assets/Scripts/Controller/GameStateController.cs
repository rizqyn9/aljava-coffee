using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class GameStateController
    {
        public static void UpdateGameState(IGameState _, GameState gameState)
        {
            //Debug.Log($"<color=yellow> req Update :{_} </color>");
            switch (gameState)
            {
                case GameState.INIT:
                    _.OnGameInit();
                    break;
                case GameState.BEFORE_START:
                    _.OnGameBeforeStart();
                    break;
                case GameState.START:
                    _.OnGameStart();
                    break;
                case GameState.CLEARANCE:
                    _.OnGameClearance();
                    break;
                case GameState.PAUSE:
                    _.OnGamePause();
                    break;
                case GameState.FINISH:
                    _.OnGameFinish();
                    break;
                default:
                    _.OnGameIddle();
                    break;
            }
        }
    }
}
