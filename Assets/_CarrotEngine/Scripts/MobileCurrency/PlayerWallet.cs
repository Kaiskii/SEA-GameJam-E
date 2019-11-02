using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotEngine.Mobile
{
    public class PlayerWallet
    {
        private Dictionary<CurrencyType, float> playerWallet = new Dictionary<CurrencyType, float>();

        PlayerWallet()
        {
            foreach(CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
            {
                // Initializing wallet to 0
                playerWallet.Add(type, 0);
            }
        }

        public delegate void CurrencyChange(CurrencyType type, float oldValue, float newValue);
        public event CurrencyChange OnCurrencyChange;

        public float GetCurrency(CurrencyType type)
        {
            return playerWallet[type];
        }

        public void ReduceCurrency(CurrencyType type, float value)
        {
            UpdateCurrency(type, value * -1);
        }

        public void AddCurrency(CurrencyType type, float value)
        {
            UpdateCurrency(type, value);
        }

        void UpdateCurrency(CurrencyType type, float change)
        {
            float oldValue = GetCurrency(type);
            float newValue = oldValue + change;

            OnCurrencyChange(type, oldValue, newValue);
        }


    }
}