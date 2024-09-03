using Google.Cloud.Firestore;
using YourNamespace.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using System;

namespace YourNamespace.Controllers
{
    public class NotesController : Controller
    {
        private readonly FirestoreDb _firestoreDb;

        public NotesController(IConfiguration configuration)
        {
            string projectId = configuration["Firebase:ProjectId"];
            string credPath = configuration["Firebase:CredentialsPath"];

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credPath);
            
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var notes = new List<Note>();
                Query query = _firestoreDb.Collection("notes");
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    var note = documentSnapshot.ConvertTo<Note>();
                    note.Id = documentSnapshot.Id;
                    notes.Add(note);
                }
                return View(notes);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in Index: {ex}");
                return StatusCode(500, "An error occurred while fetching notes.");
            }
        }

        // ... other methods ...
    }
}