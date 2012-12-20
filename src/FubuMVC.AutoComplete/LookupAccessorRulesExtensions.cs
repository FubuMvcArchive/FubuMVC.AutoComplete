using System.Linq;
using FubuCore.Reflection;
using FubuMVC.Core.Registration;

namespace FubuMVC.AutoComplete
{
    public static class LookupAccessorRulesExtensions
    {
        /// <summary>
        /// Creates an auto complete rule on an accessor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IAccessorRulesExpression AutoComplete<T>(this IAccessorRulesExpression expression) where T : ILookupProvider
        {
            return expression.Add(new LookupMarker(typeof (T)));
        }

        public static bool HasAutoComplete(this AccessorRules rules, Accessor accessor)
        {
            return accessor.HasAttribute<AutoCompleteAttribute>() ||
                   rules.AllRulesFor<LookupMarker>(accessor).Any();  
        }
    }
}