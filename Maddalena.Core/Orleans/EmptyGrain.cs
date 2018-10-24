using Orleans;
using Orleans.Core;
using Orleans.Runtime;

namespace Maddalena.Core.Orleans
{
    class EmptyGrain : Grain, IEmptyGrain
    {
        public EmptyGrain()
        {
        }

        public EmptyGrain(IGrainIdentity identity, IGrainRuntime runtime) : base(identity, runtime)
        {
        }
    }
}
