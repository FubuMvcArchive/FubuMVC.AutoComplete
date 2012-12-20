using System;
using FubuCore;
using FubuCore.Formatting;
using FubuMVC.Core.Assets;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.Urls;
using HtmlTags;

namespace FubuMVC.AutoComplete
{
    // All integration tests for this class
    public class AutoCompleteTagBuilder : IElementBuilder 
    {
        private readonly Type _lookupType;
        private string _url;

        public AutoCompleteTagBuilder(Type lookupType)
        {
            if (!lookupType.CanBeCastTo<ILookupProvider>())
            {
                throw new ArgumentOutOfRangeException("lookupType", "lookupType must be assignable to ILookupProvider");
            }

            _lookupType = lookupType;
        }

        public HtmlTag Build(ElementRequest request)
        {
            if (_url.IsEmpty())
            {
                _url = request.Get<IUrlRegistry>().UrlFor(_lookupType);
            }

            request.Get<IAssetRequirements>().Require("fubu.autocomplete.js");

            var label = request.Get<IDisplayFormatter>().GetDisplayForValue(request.Accessor, request.RawValue);
            var hidden = new HiddenTag()
                .Id(request.ElementId)
                .Name(request.ElementId)
                .AddClass("lookup")
                .Attr("value", request.RawValue == null ? string.Empty : request.RawValue.ToString());

            var textboxId = "{0}Value".ToFormat(request.ElementId);
            var textbox = new TextboxTag(textboxId, label)
                .Id(textboxId)
                .Data("lookup-url", _url)
                .Data("value-for", request.ElementId)
                .AddClass("autocomplete");

            hidden.After(textbox);
            
            return hidden;
        }

        public Type LookupType
        {
            get { return _lookupType; }
        }

        protected bool Equals(AutoCompleteTagBuilder other)
        {
            return Equals(_lookupType, other._lookupType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AutoCompleteTagBuilder) obj);
        }

        public override int GetHashCode()
        {
            return (_lookupType != null ? _lookupType.GetHashCode() : 0);
        }
    }
}