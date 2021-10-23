using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CapacityMachine : MonoBehaviour
    {
        [Header("Properties")]
        public Image filled;

        [Header("Debug")]
        [SerializeField] int maxCapacity;
        [SerializeField] Machine machine;
        [SerializeField] float deltaPerBar;
        [SerializeField] int _stateCapacity;
        public int stateCapacity
        {
            get => _stateCapacity;
            set
            {
                updateUI(_stateCapacity > value);
                _stateCapacity = value;
                if (_stateCapacity == 0) OnEmpty();
            }
        }

        private void updateUI(bool isDecrease)
        {
            float v = isDecrease ? (filled.fillAmount -= deltaPerBar) : (filled.fillAmount += deltaPerBar);
        }

        private void OnEmpty()
        {
            throw new NotImplementedException();
        }

        public void setFull()
        {
            filled.fillAmount = 1;
        }

        public void setMin()
        {
            stateCapacity = -1;
        }

        private void Start() => hide();

        public void init(Machine _machine)
        {
            machine = _machine;
            maxCapacity = _machine.MachineData.maxCapacity;
            deltaPerBar = 1 / maxCapacity;
        }

        public void hide() => gameObject.LeanAlpha(0, 0);
        public void show() => gameObject.LeanAlpha(0, .4f);
    }
}
