using LearnSmartCoding.CosmosDb.Linq.API.Core;

namespace LearnSmartCoding.CosmosDb.Linq.API.Model
{
    public class AttachmentsUpdateRequest
    {
        public List<Attachment> AttachmentsToAdd { get; set; }
        public List<string> AttachmentsToDelete { get; set; }
    }

    public class SubtaskUpdateRequest
    {
        public string Status { get; set; }
    }

}
