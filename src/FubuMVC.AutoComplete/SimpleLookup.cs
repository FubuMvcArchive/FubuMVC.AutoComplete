using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Registration;

namespace FubuMVC.AutoComplete
{
    [Singleton]
    public class SimpleLookup : ILookupProvider
    {
        private readonly IList<LookupItem> _items = new List<LookupItem>();

        public SimpleLookup()
        {
            ItemFilter = (item, query) => item.label.StartsWith(query.term, StringComparison.OrdinalIgnoreCase);
        }

        public Func<LookupItem, AutoCompleteQuery, bool> ItemFilter { get; set; }

        public void Register(LookupItem item)
        {
            _items.Add(item);
        }

        public void Register(string display)
        {
            _items.Add(new LookupItem
            {
                label = display,
                value = Guid.NewGuid().ToString()
            });
        }

        public void Register(string guidString, string display)
        {
            _items.Add(new LookupItem
            {
                label = display,
                value = Guid.Parse(guidString).ToString() // really just want it to be a guid
            });
        }

        public object IdFor(string label)
        {
            var item = _items.Single(x => x.label == label);
            return Guid.Parse(item.value);
        }

        public string LabelFor(object value)
        {
            // I want it to blow up if the value is not found
            return _items.Single(x => x.value == value.ToString()).label;
        }

        public IEnumerable<LookupItem> Lookup(AutoCompleteQuery query)
        {
            Func<LookupItem, bool> filter = item => ItemFilter(item, query);
            return _items.Where(filter).OrderBy(x => x.label);
        }


        public IList<LookupItem> Items
        {
            get { return _items; }
        }

        public string Name
        {
            get { return GetType().Name.Replace("Lookup", ""); }
        }


    }
}