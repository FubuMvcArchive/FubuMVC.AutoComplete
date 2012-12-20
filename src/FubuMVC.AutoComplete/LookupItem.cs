namespace FubuMVC.AutoComplete
{
    public class LookupItem
    {
        public string label { get; set; }
        public string value { get; set; }

        protected bool Equals(LookupItem other)
        {
            return string.Equals(label, other.label) && string.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LookupItem)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((label != null ? label.GetHashCode() : 0) * 397) ^ (value != null ? value.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("label: {0}, value: {1}", label, value);
        }
    }
}