using Orchard;

namespace Lombiq.Antispam.Services
{
    public interface IModelStateStoreService : IDependency
    {
        bool ModelStateIsValid { get; set; }
    }

    public class ModelStateStoreService : IModelStateStoreService
    {
        public bool ModelStateIsValid { get; set; }


        public ModelStateStoreService()
        {
            ModelStateIsValid = true;
        }
    }
}
