using System ;
using System.Collections.Generic ;
using JetBrains.Annotations ;

namespace Selkie.AutoMocking.Interfaces
{
    public interface IParameterInfo
    {
        [ NotNull ] IEnumerable < ICustomAttributeData > CustomAttributes { get ; }

        [ NotNull ] Type ParameterType { get ; }
    }
}