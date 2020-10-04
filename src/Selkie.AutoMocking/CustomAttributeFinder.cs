using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Diagnostics.CodeAnalysis ;

namespace Selkie.AutoMocking
{
    [ ExcludeFromCodeCoverage ]
    public class CustomAttributeFinder : ICustomAttributeFinder
    {
        public const int MaxStackFrameIteration = 1000 ;

        public IEnumerable < ParameterInfo > Find ( Type type )
        {
            Guard.ArgumentNotNull ( type , nameof ( type ) ) ;

            bool hasFound ;

            var infos = new List < ParameterInfo > ( ) ;

            var count = 0 ;

            do
            {
                var frame = new StackFrame ( count ++ , false ) ;

                var methodInfo = frame.GetMethod ( ) ;

                if ( methodInfo == null ) // did not find AutoDataTestMethodAttribute or arguments
                    break ;

                var attributes = methodInfo.GetCustomAttributes ( type , true ) ;

                hasFound = attributes.Length > 0 ;

                if ( ! hasFound )
                    continue ;

                var index = - 1 ;

                foreach ( var parameterInfo in methodInfo.GetParameters ( ) )
                {
                    if ( ++ index == 0 ) // skip SUT type
                        continue ;

                    var populate = parameterInfo.GetCustomAttributes ( typeof ( PopulateAttribute ) , true ) ;
                    var freeze   = parameterInfo.GetCustomAttributes ( typeof ( FreezeAttribute ) ,   true ) ;
                    var beNull   = parameterInfo.GetCustomAttributes ( typeof ( BeNullAttribute ) ,   true ) ;

                    if ( populate.Length == 0 &&
                         freeze.Length   == 0 &&
                         beNull.Length   == 0 )
                        continue ;

                    var info = new ParameterInfo ( parameterInfo ) ;

                    infos.Add ( info ) ;
                }
            } while ( ! hasFound &&
                      count < MaxStackFrameIteration ) ;

            return infos ;
        }
    }
}