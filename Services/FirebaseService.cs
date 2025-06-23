using Google.Cloud.Firestore;
using areayvolumen.Models;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace areayvolumen.Services
{
    public class FirebaseService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirebaseService(IConfiguration configuration)
        {
            var projectId = configuration["Firebase:ProjectId"];
            var credentialsPath = configuration["Firebase:CredentialsPath"];
            
            if (string.IsNullOrEmpty(projectId))
            {
                throw new InvalidOperationException("Firebase ProjectId no está configurado en appsettings.json");
            }

            // Si tienes un archivo de credenciales, úsalo
            if (!string.IsNullOrEmpty(credentialsPath) && File.Exists(credentialsPath))
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
            }

            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<CalculationHistory> SaveCalculationAsync(CalculationHistory calculation)
        {
            var collection = _firestoreDb.Collection("calculations");
            
            var document = new Dictionary<string, object>
            {
                { "figure", calculation.Figure },
                { "calculationType", calculation.CalculationType },
                { "parameters", calculation.Parameters },
                { "result", calculation.Result },
                { "timestamp", Timestamp.FromDateTime(calculation.Timestamp.ToUniversalTime()) }
            };

            var docRef = await collection.AddAsync(document);
            calculation.Id = docRef.Id;
            
            return calculation;
        }

        public async Task<List<CalculationHistory>> GetCalculationHistoryAsync()
        {
            var collection = _firestoreDb.Collection("calculations");
            var snapshot = await collection.OrderByDescending("timestamp").GetSnapshotAsync();
            
            var calculations = new List<CalculationHistory>();
            
            foreach (var document in snapshot.Documents)
            {
                var data = document.ToDictionary();
                var calculation = new CalculationHistory
                {
                    Id = document.Id,
                    Figure = data["figure"].ToString(),
                    CalculationType = data["calculationType"].ToString(),
                    Parameters = data["parameters"].ToString(),
                    Result = Convert.ToDouble(data["result"]),
                    Timestamp = ((Timestamp)data["timestamp"]).ToDateTime()
                };
                calculations.Add(calculation);
            }
            
            return calculations;
        }

        public async Task<CalculationHistory> GetCalculationByIdAsync(string id)
        {
            var collection = _firestoreDb.Collection("calculations");
            var document = await collection.Document(id).GetSnapshotAsync();
            
            if (!document.Exists)
                return null;
                
            var data = document.ToDictionary();
            return new CalculationHistory
            {
                Id = document.Id,
                Figure = data["figure"].ToString(),
                CalculationType = data["calculationType"].ToString(),
                Parameters = data["parameters"].ToString(),
                Result = Convert.ToDouble(data["result"]),
                Timestamp = ((Timestamp)data["timestamp"]).ToDateTime()
            };
        }

        public async Task UpdateCalculationAsync(string id, CalculationHistory calculation)
        {
            var docRef = _firestoreDb.Collection("calculations").Document(id);
            var updates = new Dictionary<string, object>
            {
                { "figure", calculation.Figure },
                { "calculationType", calculation.CalculationType },
                { "parameters", calculation.Parameters },
                { "result", calculation.Result }
                // No actualizamos el timestamp para mantener el original
            };
            await docRef.UpdateAsync(updates);
        }

        public async Task DeleteCalculationAsync(string id)
        {
            var docRef = _firestoreDb.Collection("calculations").Document(id);
            await docRef.DeleteAsync();
        }
    }
} 