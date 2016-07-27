using openhab.net.rest.Core;
using openhab.net.rest.Json;
using System;

namespace openhab.net.rest.Items
{
    public class CallItem : OpenhabItem
    {
        internal CallItem(ItemObject original, IElementObserver observer) : base(original, observer)
        {
        }

        
        public bool IsActive => !string.IsNullOrEmpty(OriginNumber) ||
                                !string.IsNullOrEmpty(DestinationNumber);

        string _originNumber;
        public string OriginNumber
        {
            get { return _originNumber; }
            set { _originNumber = value; FromNative(ToNative()); }
        }

        string _destinationNumber;
        public string DestinationNumber
        {
            get { return _originNumber; }
            set { _originNumber = value; FromNative(ToNative()); }
        }

        protected override void Syncronize()
        {
            if (IsInitialized) {
                var call = Value.Split(new[] { "##" }, StringSplitOptions.RemoveEmptyEntries);
                if (call.Length >= 1)  {
                    OriginNumber = call[0];
                }
                if (call.Length >= 2) {
                    DestinationNumber = call[1];
                }
            }
        }

        public override void FromNative(object obj)
        {
            var call = obj as CallInformation;
            if (call != null)
            {
                _originNumber = call.OriginNumber;
                _destinationNumber = call.DestinationNumber;
                Update($"{_originNumber}##{_destinationNumber}");
            }
        }
        
        public override object ToNative()
        {
            return new CallInformation
            {
                OriginNumber = this.OriginNumber,
                DestinationNumber = this.DestinationNumber
            };
        }
    }

    public class CallInformation
    {
        public string OriginNumber { get; set; }
        public string DestinationNumber { get; set; }
    }
}
