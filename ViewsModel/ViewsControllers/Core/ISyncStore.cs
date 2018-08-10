namespace Jsa.ViewsModel.ViewsControllers.Core
{
    public interface ISyncStore<TDomainEntity,TStoreEntity>
    {
        TDomainEntity ReadStoreEntity(TStoreEntity storeEntity);
        TStoreEntity WriteStoreEntity(TDomainEntity source, TStoreEntity dist, bool isNew);
    }
}
