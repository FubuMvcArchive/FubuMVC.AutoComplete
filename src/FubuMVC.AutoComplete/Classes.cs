using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore;
using FubuCore.Formatting;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Elements;
using FubuMVC.Core.Urls;
using HtmlTags;
using HtmlTags.Conventions;

namespace FubuMVC.AutoComplete
{
    /*

     */


    public class AutoCompleteTagPolicy : IElementBuilderPolicy
    {
        public AccessorRules Rules { get; set; }

        public bool Matches(ElementRequest subject)
        {
            // Attribute too
            throw new NotImplementedException();
        }

        public ITagBuilder<ElementRequest> BuilderFor(ElementRequest subject)
        {
            throw new NotImplementedException();
        }
    }




    public class AutocompleteTagBuilder<T> : IElementBuilder where T : ILookupProvider
    {
        private string _url;

        public HtmlTag Build(ElementRequest request)
        {
            if (_url.IsEmpty())
            {
                _url = request.Get<IUrlRegistry>().UrlFor<T>(x => x.Lookup(null));
            }

            // TODO -- need to configure this and add in the activator script
            request.Get<IAssetRequirements>().Require("autocomplete");

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
    }

    public static class LookupAccessorRulesExtensions
    {
        public static IAccessorRulesExpression AutoComplete<T>(this IAccessorRulesExpression expression) where T : ILookupProvider
        {
            return expression.Add(new LookupMarker(typeof (T)));
        }
    }

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

    public class AutoCompleteQuery
    {
        public string term { get; set; }
    }

    public interface ILookupProvider
    {
        object IdFor(string label);
        string LabelFor(object value);
        IEnumerable<LookupItem> Lookup(AutoCompleteQuery query);
    }

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

    public class LookupActionSource : IActionSource
    {
        private static readonly string MethodName = ReflectionHelper.GetMethod<ILookupProvider>(x => x.Lookup(null)).Name;

        public static MethodInfo GetLookupMethod(Type type)
        {
            return type.GetMethod(MethodName);
        }

        public IEnumerable<ActionCall> FindActions(Assembly applicationAssembly)
        {
            var types = new TypePool();
            types.AddAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            types.IgnoreExportTypeFailures = true;

            return types.TypesMatching(type => type.IsConcreteTypeOf<ILookupProvider>()).Select(type =>
            {
                var method = GetLookupMethod(type);
                return new ActionCall(type, method);
            });
        }
    }



    public class LookupUrlPolicy : IUrlPolicy
    {
        public bool Matches(ActionCall call)
        {
            return call.HandlerType.CanBeCastTo<ILookupProvider>();
        }

        public IRouteDefinition Build(ActionCall call)
        {
            var pattern = "_lookup/" + call.HandlerType.Name.Replace("LookupProvider", "").ToLower();
            return new RouteDefinition(pattern);
        }
    }

    public class AutoCompleteEndpoints : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            var tagPolicy = new AutoCompleteTagPolicy();
            

            registry.Actions.FindWith(new LookupActionSource());
            registry.Routes.UrlPolicy<LookupUrlPolicy>();

            registry.Import<HtmlConventionRegistry>(x => x.Editors.Add(tagPolicy));

            registry.Configure(graph => {
                var rules = graph.Settings.Get<AccessorRules>();

                var strategy = new AutoCompleteStringifierStrategy(rules);
                var stringifier = graph.Services.DefaultServiceFor<Stringifier>().Value.As<Stringifier>();
                stringifier.AddStrategy(strategy);

                tagPolicy.Rules = rules;
            });
        }
    }

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
            if (_rules == null)
            {
                // go grab it
            }

            // Attribute too
            throw new NotImplementedException();
        }

        public string CreateDisplay(GetStringRequest request)
        {
            throw new NotImplementedException();
        }
    }

}