using openhab.net.rest.Core;
using openhab.net.rest.Json;

namespace openhab.net.rest.Items
{
    public class ColorItem : OpenhabItem
    {
        internal ColorItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }

        #region RGB
        float _r, _g, _b;
        /// <summary>
        /// 0 - 255
        /// </summary>
        public int Red
        {
            get { return (int)(_r * 255f); }
            set { _r = value / 255f; ConvertToHSV(); }
        }
        /// <summary>
        /// 0 - 255
        /// </summary>
        public int Green
        {
            get { return (int)(_g * 255f); }
            set { _g = value / 255f; ConvertToHSV(); }
        }
        /// <summary>
        /// 0 - 255
        /// </summary>
        public int Blue
        {
            get { return (int)(_b * 255f); }
            set { _b = value / 255f; ConvertToHSV(); }
        }
        #endregion

        #region HSV
        float _h, _s, _v;
        /// <summary>
        /// 0 - 360
        /// </summary>
        public float Hue
        {
            get { return _h; }
            set { _h = value; ConvertToRGB();  }
        }
        /// <summary>
        /// 0 - 100
        /// </summary>
        public float Saturation
        {
            get { return _s * 100; }
            set { _s = value / 100f; ConvertToRGB(); }
        }
        /// <summary>
        /// 0 - 100
        /// </summary>
        public float Brightness
        {
            get { return _v * 100; }
            set { _v = value / 100f; ConvertToRGB(); }
        }
        #endregion


        void ConvertToRGB()
        {
            ColorConversion.HSVtoRGB(_h, _s, _v, out _r, out _g, out _b);
            FromNative(ToNative());
        }

        void ConvertToHSV()
        {
            ColorConversion.RGBtoHSV(_r, _g, _b, out _h, out _s, out _v);
            FromNative(ToNative());
        }


        protected override void Syncronize()
        {
            if (IsInitialized) {
                var hsb = Value.Split(',');
                if (hsb.Length == 3) {
                    float s, v;                    
                    bool b1 = ValueParser.TryParse(hsb[0], out _h);
                    bool b2 = ValueParser.TryParse(hsb[1], out s);
                    bool b3 = ValueParser.TryParse(hsb[2], out v);
                    if (b1 && b2 && b3) {
                        _s = s / 100f;
                        _v = v / 100f;
                        ColorConversion.HSVtoRGB(_h, _s, _v, out _r, out _g, out _b);
                    }
                }
            }
        }

        public override void FromNative(object obj)
        {
            var color = obj as ColorRGB;
            if (color != null) {
                _r = color.R / 255f;
                _g = color.G / 255f;
                _b = color.B / 255f;
                ColorConversion.RGBtoHSV(_r, _g, _b, out _h, out _s, out _v);
                
                Update($"{ToFormat(_h)},{ToFormat(_s * 100f)},{ToFormat(_v * 100f)}");
            }
        }

        string ToFormat(float f) => f.ToString().Replace(',', '.');

        public override object ToNative() => new ColorRGB { R = Red, G = Green, B = Blue };
    }

    public class ColorRGB
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}
