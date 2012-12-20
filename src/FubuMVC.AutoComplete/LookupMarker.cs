using System;

namespace FubuMVC.AutoComplete
{
    public class LookupMarker
    {
        private readonly Type _lookupProviderType;

        public LookupMarker(Type lookupProviderType)
        {
            if (lookupProviderType == null) throw new ArgumentNullException("lookupProviderType");

            _lookupProviderType = lookupProviderType;
        }

        public Type LookupProviderType
        {
            get { return _lookupProviderType; }
        }

        protected bool Equals(LookupMarker other)
        {
            return Equals(_lookupProviderType, other._lookupProviderType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LookupMarker)obj);
        }

        public override int GetHashCode()
        {
            return (_lookupProviderType != null ? _lookupProviderType.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return string.Format("LookupProviderType: {0}", _lookupProviderType);
        }
    }
}