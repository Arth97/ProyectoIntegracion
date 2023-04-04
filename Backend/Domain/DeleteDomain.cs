using Repository;

namespace Domain
{
    public class DeleteDomain
    {
        private readonly DataRepository dataRepository;
        public DeleteDomain (DataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }
        public string DeleteDatabase()
        {
            string result = dataRepository.DeleteDatabase();

            return result;
        }
    }
}
