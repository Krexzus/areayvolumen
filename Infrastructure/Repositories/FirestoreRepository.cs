using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace areayvolumen.Infrastructure.Repositories
{
    public class FirestoreRepository<T> : IRepository<T> where T : class
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly string _collectionName;

        public FirestoreRepository(FirestoreDb firestoreDb, string collectionName)
        {
            _firestoreDb = firestoreDb;
            _collectionName = collectionName;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var doc = await _firestoreDb.Collection(_collectionName).Document(id).GetSnapshotAsync();
            return doc.Exists ? doc.ConvertTo<T>() : null;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var snapshot = await _firestoreDb.Collection(_collectionName).GetSnapshotAsync();
            return snapshot.Documents.Select(d => d.ConvertTo<T>());
        }

        public async Task AddAsync(T entity)
        {
            await _firestoreDb.Collection(_collectionName).AddAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity)
        {
            await _firestoreDb.Collection(_collectionName).Document(id).SetAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _firestoreDb.Collection(_collectionName).Document(id).DeleteAsync();
        }
    }
} 