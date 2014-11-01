using System.Linq;

namespace Guide.Data.UnitOfWork
{
    public partial class UnitOfWork
    {
        public bool DbInitialized {
            get
            {
                return this.Countries.All.Any();
            }
        }
    }
}
