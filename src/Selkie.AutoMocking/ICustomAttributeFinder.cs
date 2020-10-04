using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.AutoMocking
{
    public interface ICustomAttributeFinder
    {
        IEnumerable<ParameterInfo> Find([NotNull] Type type);
    }
}