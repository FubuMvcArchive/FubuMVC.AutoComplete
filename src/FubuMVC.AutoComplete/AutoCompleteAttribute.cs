using System;

namespace FubuMVC.AutoComplete
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoCompleteAttribute : Attribute
    {
        private readonly Type _providerType;

        public AutoCompleteAttribute(Type providerType)
        {
            _providerType = providerType;
        }

        public Type ProviderType
        {
            get { return _providerType; }
        }
    }
}