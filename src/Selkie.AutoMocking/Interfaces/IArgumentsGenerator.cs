﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace Selkie.AutoMocking.Interfaces
{
    public interface IArgumentsGenerator
    {
        [NotNull]
        object[] Create([NotNull] IEnumerable<IParameterInfo> parameterInfos);
    }
}