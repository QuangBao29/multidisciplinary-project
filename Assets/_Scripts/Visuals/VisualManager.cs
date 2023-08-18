using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public class VisualManager : PersistentSingleton<VisualManager>
    {
        public Color BlackColor;
        public Color WhiteColor;
        public Color InactiveButtonColor;
        public Color InactiveTextColor;
        public Color LightPrimaryColor;
        public Color TemperaturePrimaryColor;
        public Color HumidityPrimaryColor;
        public Color LightSecondaryColor;
        public Color TemperatureSecondaryColor;
        public Color HumiditySecondaryColor;
        public Color WaterNowColor;

        public Color GetColor(ColorType colorType)
        {
            switch (colorType)
            {
                case ColorType.Black: return BlackColor;
                case ColorType.White: return WhiteColor;
                case ColorType.InactiveButton: return InactiveButtonColor;
                case ColorType.InactiveText: return InactiveTextColor;
                case ColorType.LightPrimary: return LightPrimaryColor;
                case ColorType.TemperaturePrimary: return TemperaturePrimaryColor;
                case ColorType.HumidityPrimary: return HumidityPrimaryColor;
                case ColorType.LightSecondary: return LightSecondaryColor;
                case ColorType.TemperatureSecondary: return TemperatureSecondaryColor;
                case ColorType.HumiditySecondary: return HumiditySecondaryColor;
                case ColorType.WaterNow: return WaterNowColor;
            }

            return Color.magenta;
        }
    }

    public enum ColorType
    {
        Black,
        White,
        InactiveButton,
        InactiveText,
        LightPrimary,
        TemperaturePrimary,
        HumidityPrimary,
        LightSecondary,
        TemperatureSecondary,
        HumiditySecondary,
        WaterNow,
    }
}