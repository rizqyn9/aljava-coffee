using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CapacityMachine : MonoBehaviour
    {
        [Header("Properties")]
        public Image filled;

        [Header("Debug")]
        [SerializeField] RectTransform rectTransform;
        [SerializeField] Machine machine;
        [SerializeField] int maxCapacity;
        [SerializeField] int _stateCapacity = 0;
        [SerializeField] float deltaPerBar;

        public int stateCapacity
        {
            get => _stateCapacity;
            set
            {
                OnUpdateCapacity(_stateCapacity, value);
                _stateCapacity = value;
            }
        }

        private void OnUpdateCapacity(int _old, int _new)
        {
            filled.fillAmount = (float)_new / maxCapacity;
        }

        public void getOne()
        {
            stateCapacity -= 1;
            if(_stateCapacity <= 0)
            {
                OnEmpty();
            }
        }

        public void setFull()
        {
            stateCapacity = maxCapacity;
            LeanTween.alpha(rectTransform, 1, 1f);
        }

        private void OnEmpty()
        {
            LeanTween.alpha(rectTransform, 0, 1f);
            machine.emptyCapacity();
        }

        private void Start()
        {
            LeanTween.alpha(rectTransform, 0, 0);
        }

        private void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
        }

        public void init(Machine _machine)
        {
            machine = _machine;
            maxCapacity = _machine.machineProperties.maxCapacity;
            deltaPerBar = 1 / maxCapacity;
        }
    }
}
