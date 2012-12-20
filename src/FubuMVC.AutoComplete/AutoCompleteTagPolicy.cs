using System;
using System.Linq;
using FubuCore.Reflection;
using FubuCore.Util;
using FubuMVC.Core.UI.Elements;
using HtmlTags.Conventions;

namespace FubuMVC.AutoComplete
{
    public class AutoCompleteTagPolicy : IElementBuilderPolicy
    {
        private readonly Cache<Type, IElementBuilder> _builders =
            new Cache<Type, IElementBuilder>(type => new AutoCompleteTagBuilder(type));

        public AccessorRules Rules { get; set; }

        public bool Matches(ElementRequest subject)
        {
            return Rules.HasAutoComplete(subject.Accessor);
        }

        public ITagBuilder<ElementRequest> BuilderFor(ElementRequest subject)
        {
            if (subject.Accessor.HasAttribute<AutoCompleteAttribute>())
            {
                var att = subject.Accessor.GetAttribute<AutoCompleteAttribute>();
                return _builders[att.ProviderType];
            }

            var rule = Rules.AllRulesFor<LookupMarker>(subject.Accessor).First();
            return _builders[rule.LookupProviderType];
        }
    }

}