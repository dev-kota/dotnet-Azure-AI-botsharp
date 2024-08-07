namespace BotSharp.Plugin.KnowledgeBase.Services;

public partial class KnowledgeService
{
    public async Task<StringIdPagedItems<KnowledgeCollectionData>> GetKnowledgeCollectionData(string collectionName, KnowledgeFilter filter)
    {
        try
        {
            var db = GetVectorDb();
            return await db.GetCollectionData(collectionName, filter);
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error when getting knowledge collectio data. {ex.Message}\r\n{ex.InnerException}");
            return new StringIdPagedItems<KnowledgeCollectionData>();
        }
    }
}
