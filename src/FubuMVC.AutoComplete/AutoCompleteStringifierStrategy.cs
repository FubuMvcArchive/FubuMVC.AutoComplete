using System;
using FubuCore.Formatting;
using FubuCore.Reflection;

namespace FubuMVC.AutoComplete
{
    // TODO -- NOT DONE.
    public class AutoCompleteStringifierStrategy : StringifierStrategy
    {
        private readonly AccessorRules _rules;

        public AutoCompleteStringifierStrategy(AccessorRules rules)
        {
            _rules = rules;

            Matches = AccessorMatches;
            StringFunction = CreateDisplay;
        }

        public bool AccessorMatches(GetStringRequest request)
        {
            return _rules.HasAutoComplete(new SingleProperty(request.Property));
        }

        public string CreateDisplay(GetStringRequest request)
        {
            throw new NotImplementedException();
        }
    }
}