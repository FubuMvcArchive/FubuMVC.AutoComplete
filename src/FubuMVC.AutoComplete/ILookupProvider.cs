using System.Collections.Generic;

namespace FubuMVC.AutoComplete
{
    public interface ILookupProvider
    {
        object IdFor(string label);
        string LabelFor(object value);
        IEnumerable<LookupItem> Lookup(AutoCompleteQuery query);
    }
}