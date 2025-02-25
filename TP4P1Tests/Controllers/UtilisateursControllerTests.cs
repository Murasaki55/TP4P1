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

namespace TP4P1.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        public UtilisateursController controller { get; set; }
        public UtilisateursControllerTests()
        {
            var builder = new DbContextOptionsBuilder<FilmRatingsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=FilmsRatingsDB; uid=postgres; password=postgres;");
            FilmRatingsDBContext context = new FilmRatingsDBContext(builder.Options);
            controller = new UtilisateursController(context);
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
            List<Utilisateur> UtilisateurList = new List<Utilisateur>() { new Utilisateur(1,"Calida","Lilley","0653930778","clilleymd@last.fm","Toto12345678!","Impasse des bergeronnettes", "74200", "Allinges", "France", (float)46.344795, (float)6.4885845, new DateTime(2025,02,25), []), new Utilisateur(2, "Gwendolin", "Dominguez", "0724970555", "gdominguez0@washingtonpost.com", "Toto12345678!", "Chemin de gom", "73420", "Voglans", "France", (float)45.622154, (float)5.8853216, new DateTime(2025-02-25), []), new Utilisateur(3, "Randolph", "Richings", "0630271158", "rrichings1@naver.com", "Toto12345678!", "Route des charmottes d'en bas", "74890", "Bons-en-Chablais", "France", (float)46.25732, (float)6.367676, new DateTime(2025-02-25), []) };

            CollectionAssert.AreEqual(result.Where(s => s.UtilisateurId <= 3).ToList(), UtilisateurList);
        }

        [TestMethod()]
        public void GetUtilisateurByIDTest()
        {

        }

        [TestMethod()]
        public void GetUtilisateurByEmailTest()
        {

        }

        [TestMethod()]
        public void PutUtilisateurTest()
        {

        }

        [TestMethod()]
        public void PostUtilisateurTest()
        {

        }

        [TestMethod()]
        public void DeleteUtilisateurTest()
        {

        }
    }
}