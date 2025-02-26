using Microsoft.VisualStudio.TestTools.UnitTesting;
using TP4P1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TP4P1.Models.EntityFramework;
using Npgsql.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TP4P1.Models.Repository;
using TP4P1.Models.DataManager;

namespace TP4P1.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        public UtilisateursController controller { get; set; }
        public FilmRatingsDBContext context { get; set; }

        private IDataRepository<Utilisateur> dataRepository;

        public UtilisateursControllerTests()
        {
            var builder = new DbContextOptionsBuilder<FilmRatingsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=FilmsRatingsDB; uid=postgres; password=postgres;");
            context = new FilmRatingsDBContext(builder.Options);
            dataRepository = new UtilisateurManager(context);
            controller = new UtilisateursController(dataRepository);
        }

        [TestMethod()]
        public void UtilisateursControllerTest()
        {
            Assert.IsNotNull(controller);
        }

        [TestMethod()]
        public void GetUtilisateursTest()
        {
            var result = controller.GetUtilisateurs().Result.Value.ToList();
            List<Utilisateur> utilisateurs = context.Utilisateurs.ToList();

            Assert.IsInstanceOfType(result, typeof(List<Utilisateur>));
            CollectionAssert.AreEqual(result.ToList(), utilisateurs);
        }

        [TestMethod()]
        public void GetUtilisateurByIDTestOK()
        {
            var result = controller.GetUtilisateurByID(1).Result.Value;
            Utilisateur utilisateur = context.Utilisateurs.Where(c => c.UtilisateurId == 1).FirstOrDefault();


            Assert.IsInstanceOfType(result, typeof(Utilisateur));
            Assert.AreEqual(result, utilisateur);
        }

        [TestMethod()]
        public void GetUtilisateurByIDTestNonOK()
        {
            var result = controller.GetUtilisateurByID(10000).Result;


            Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>));
            Assert.IsNull(result.Value);
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTestOK()
        {
            var result = controller.GetUtilisateurByEmail("clilleymd@last.fm").Result.Value;
            Utilisateur utilisateur = context.Utilisateurs.Where(c => c.UtilisateurId == 1).FirstOrDefault();


            Assert.IsInstanceOfType(result, typeof(Utilisateur));
            Assert.AreEqual(result, utilisateur);
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTestNonOK()
        {
            var result = controller.GetUtilisateurByEmail("durand").Result;


            Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>));
            Assert.IsNull(result.Value);
        }

        [TestMethod()]
        public void PutUtilisateurTestNoContent()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                UtilisateurId = 15,
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var result = controller.PutUtilisateur(15,userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod()]
        public void PutUtilisateurTestBadRequest()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                UtilisateurId = 15,
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var result = controller.PutUtilisateur(1000000000, userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PutUtilisateurTestNotFound()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                UtilisateurId = 1000000000,
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var result = controller.PutUtilisateur(1000000000, userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void PostUtilisateurTestConforme()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
            // Assert
            Thread.Sleep(1000);
            Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault(); // On récupère l'utilisateur créé directement dans la BD grace à son mailunique
            // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            userAtester.UtilisateurId = userRecupere.UtilisateurId;
            Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
        }

        [TestMethod()]
        public void PostUtilisateurTestNonConforme()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre,
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
            // Assert
            Thread.Sleep(1000);
            Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault(); // On récupère l'utilisateur créé directement dans la BD grace à son mailunique
            // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
            // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
            userAtester.UtilisateurId = userRecupere.UtilisateurId;
            Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>));
            Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
        }

        [TestMethod()]
        public void DeleteUtilisateurTest()
        {
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            context.Utilisateurs.Add(userAtester);
            context.SaveChanges();
            Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault();
            var result = controller.DeleteUtilisateur(userRecupere.UtilisateurId).Result;
            Utilisateur utilisateur = context.Utilisateurs.Where(c => c.UtilisateurId == userRecupere.UtilisateurId).FirstOrDefault();

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.IsNull(utilisateur);
        }
    }
}