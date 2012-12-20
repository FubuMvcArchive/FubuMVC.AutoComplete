using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore;
using FubuCore.Binding;
using FubuCore.Formatting;
using FubuCore.Reflection;
using FubuCore.Util;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Resources.Conneg;

namespace FubuMVC.AutoComplete
{

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
}