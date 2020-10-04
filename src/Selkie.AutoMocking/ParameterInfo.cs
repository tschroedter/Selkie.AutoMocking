using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.AutoMocking.Interfaces;

namespace Selkie.AutoMocking
{
    public class ParameterInfo : IParameterInfo
    {
        public ParameterInfo([NotNull] System.Reflection.ParameterInfo parameterInfo)
        {
            Guard.ArgumentNotNull(parameterInfo,
                                  nameof(parameterInfo));

            var parameterInfo1 = parameterInfo;

            CustomAttributes = parameterInfo1.CustomAttributes.Select(x => new CustomAttributeData(x));
            ParameterType = parameterInfo1.ParameterType;
        }

        private ParameterInfo()
        {
            CustomAttributes = Array.Empty<ICustomAttributeData>();
            ParameterType = typeof(object);
        }

        public IEnumerable<ICustomAttributeData> CustomAttributes { get; }

        public Type ParameterType { get; }
    }
}