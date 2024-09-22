using KGS.DataTable.Search;
using KGS.DataTable.Search;
using System.Collections.Generic;

namespace KGS.Repo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(object id);

        void InsertGraph(TEntity entity);

        void InsertCollection(List<TEntity> entityCollection);

        void UpdateCollection(List<TEntity> entityCollection);

        void Update(TEntity entity);

        TEntity Update(TEntity dbEntity, TEntity entity);

        void Delete(object id);

        void Delete(TEntity entity);
        void Delete(List<TEntity> entityCollection);

        void Insert(TEntity entity);

        void ChangeEntityState<T>(T entity, ObjectState state) where T : class;

        void ChangeEntityCollectionState<T>(ICollection<T> entityCollection, ObjectState state) where T : class;

        RepositoryQuery<TEntity> Query();

        void Dispose();

        void SaveChanges();

        PagedListResult<TEntity> Search(SearchQuery<TEntity> searchQuery);

        PagedListResult<TEntity> Search(SearchQuery<TEntity> searchQuery, out int totalCount);
        

    }
}