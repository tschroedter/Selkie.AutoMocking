using Microsoft.VisualStudio.TestTools.UnitTesting ;

namespace Selkie.AutoMocking
{
    public sealed class AutoDataTestClassAttribute
        : TestClassAttribute
    {
        public override TestMethodAttribute GetTestMethodAttribute ( TestMethodAttribute testMethodAttribute )
        {
            return testMethodAttribute is AutoDataTestMethodAttribute
                       ? testMethodAttribute
                       : new AutoDataTestMethodAttribute ( base.GetTestMethodAttribute ( testMethodAttribute ) ) ;
        }
    }
}