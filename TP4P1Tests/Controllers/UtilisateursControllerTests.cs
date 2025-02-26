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
using Moq;

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
        public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurByID(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        //[TestMethod()]
        //public void GetUtilisateurByIDTestOK()
        //{
        //    var result = controller.GetUtilisateurByID(1).Result.Value;
        //    Utilisateur utilisateur = context.Utilisateurs.Where(c => c.UtilisateurId == 1).FirstOrDefault();


        //    Assert.IsInstanceOfType(result, typeof(Utilisateur));
        //    Assert.AreEqual(result, utilisateur);
        //}

        [TestMethod]
        public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurByID(0).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        //[TestMethod()]
        //public void GetUtilisateurByIDTestNonOK()
        //{
        //    var result = controller.GetUtilisateurByID(10000).Result;


        //    Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>));
        //    Assert.IsNull(result.Value);
        //}

        [TestMethod()]
        public void GetUtilisateurByEmail_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByStringAsync("clilleymd@last.fm").Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurByEmail("clilleymd@last.fm").Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        //[TestMethod()]
        //public void GetUtilisateurByEmailTestOK()
        //{
        //    var result = controller.GetUtilisateurByEmail("clilleymd@last.fm").Result.Value;
        //    Utilisateur utilisateur = context.Utilisateurs.Where(c => c.UtilisateurId == 1).FirstOrDefault();


        //    Assert.IsInstanceOfType(result, typeof(Utilisateur));
        //    Assert.AreEqual(result, utilisateur);
        //}

        [TestMethod]
        public void GetUtilisateurByEmail_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.GetUtilisateurByEmail("Durant").Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        //[TestMethod()]
        //public void GetUtilisateurByEmailTestNonOK()
        //{
        //    var result = controller.GetUtilisateurByEmail("durand").Result;


        //    Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>));
        //    Assert.IsNull(result.Value);
        //}

        [TestMethod]
        public void Pututilisateur_ModificationValidated_ModificationOK_AvecMoq()
        {
            // Arrange

            Utilisateur user1 = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };

            Utilisateur user2 = new Utilisateur
            {
                Nom = "POISSONS",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user1);
            var userController = new UtilisateursController(mockRepository.Object);
            user2.UtilisateurId = 1;
            var actionResult = userController.PutUtilisateur(1, user2).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        //[TestMethod()]
        //public void PutUtilisateurTestNoContent()
        //{
        //    Random rnd = new Random();
        //    int chiffre = rnd.Next(1, 1000000000);
        //    // Le mail doit être unique donc 2 possibilités :
        //    // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
        //    // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
        //    Utilisateur userAtester = new Utilisateur()
        //    {
        //        UtilisateurId = 15,
        //        Nom = "MACHIN",
        //        Prenom = "Luc",
        //        Mobile = "0606070809",
        //        Mail = "machin" + chiffre + "@gmail.com",
        //        Pwd = "Toto1234!",
        //        Rue = "Chemin de Bellevue",
        //        CodePostal = "74940",
        //        Ville = "Annecy-le-Vieux",
        //        Pays = "France",
        //        Latitude = null,
        //        Longitude = null
        //    };
        //    // Act
        //    var result = controller.PutUtilisateur(15,userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
        //    // Assert
        //    Assert.IsInstanceOfType(result, typeof(NoContentResult));
        //}

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

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var actionResult = userController.PostUtilisateur(user).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
            user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
            Assert.AreEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
        }


        //[TestMethod()]
        //public void PostUtilisateurTestConforme()
        //{
        //    // Arrange
        //    Random rnd = new Random();
        //    int chiffre = rnd.Next(1, 1000000000);
        //    // Le mail doit être unique donc 2 possibilités :
        //    // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
        //    // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
        //    Utilisateur userAtester = new Utilisateur()
        //    {
        //        Nom = "MACHIN",
        //        Prenom = "Luc",
        //        Mobile = "0606070809",
        //        Mail = "machin" + chiffre + "@gmail.com",
        //        Pwd = "Toto1234!",
        //        Rue = "Chemin de Bellevue",
        //        CodePostal = "74940",
        //        Ville = "Annecy-le-Vieux",
        //        Pays = "France",
        //        Latitude = null,
        //        Longitude = null
        //    };
        //    // Act
        //    var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout
        //    // Assert
        //    Thread.Sleep(1000);
        //    Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault(); // On récupère l'utilisateur créé directement dans la BD grace à son mailunique
        //    // On ne connait pas l'ID de l’utilisateur envoyé car numéro automatique.
        //    // Du coup, on récupère l'ID de celui récupéré et on compare ensuite les 2 users
        //    userAtester.UtilisateurId = userRecupere.UtilisateurId;
        //    Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
        //}

        [TestMethod()]
        public void DeleteUtilisateurTest_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object);
            // Act
            var actionResult = userController.DeleteUtilisateur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        //[TestMethod()]
        //public void DeleteUtilisateurTest()
        //{
        //    Random rnd = new Random();
        //    int chiffre = rnd.Next(1, 1000000000);
        //    // Le mail doit être unique donc 2 possibilités :
        //    // 1. on s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
        //    // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
        //    Utilisateur userAtester = new Utilisateur()
        //    {
        //        Nom = "MACHIN",
        //        Prenom = "Luc",
        //        Mobile = "0606070809",
        //        Mail = "machin" + chiffre + "@gmail.com",
        //        Pwd = "Toto1234!",
        //        Rue = "Chemin de Bellevue",
        //        CodePostal = "74940",
        //        Ville = "Annecy-le-Vieux",
        //        Pays = "France",
        //        Latitude = null,
        //        Longitude = null
        //    };
        //    context.Utilisateurs.Add(userAtester);
        //    context.SaveChanges();
        //    Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper()).FirstOrDefault();
        //    var result = controller.DeleteUtilisateur(userRecupere.UtilisateurId).Result;
        //    Utilisateur utilisateur = context.Utilisateurs.Where(c => c.UtilisateurId == userRecupere.UtilisateurId).FirstOrDefault();

        //    Assert.IsInstanceOfType(result, typeof(NoContentResult));
        //    Assert.IsNull(utilisateur);
        //}
    }
}