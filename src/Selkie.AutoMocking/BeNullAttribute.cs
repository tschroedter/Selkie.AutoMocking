using System ;

namespace Selkie.AutoMocking
{
    [ AttributeUsage ( AttributeTargets.Parameter ) ]
    public sealed class BeNullAttribute : Attribute
    {
    }
}