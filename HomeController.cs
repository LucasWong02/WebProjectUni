using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        object obj;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Person> persons = new List<Person>();

            using(MySqlConnection con=new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Persons", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Person person = new Person();
                    person.FirstName = reader["FirstName"].ToString();
                    person.LastName = reader["LastName"].ToString();
                    person.Mail = reader["Mail"].ToString();
                    person.Password = reader["Password"].ToString();
                    person.Puesto = reader["Puesto"].ToString();

                    persons.Add(person);
                }
                reader.Close();
            }

            return View();
        }

        [HttpGet]
        public ActionResult Dashboard(Dash dash)
        {
            return View(dash);
        }

        public IActionResult Msnger()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserProfile(Person per)
        {
            return View(per);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult IndexAdmin()
        {
            return View();
        }

        public IActionResult Usuarios_Aplic()
        {
            return View();
        }

        public IActionResult RegistroJob()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(Person per)
        {
            List<Job> jobs = new List<Job>();
            Dash dash = new Dash();
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Persons(FirstName, LastName, Mail, Password, Puesto) values('" + per.FirstName + "', '" + per.LastName + "', '" + per.Mail + "', '" + per.Password + "', 'Usuario')", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Close();


                MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM Jobs", con);
                MySqlDataReader reader2 = cmd2.ExecuteReader();

                while (reader2.Read())
                {
                    Job job = new Job();
                    job.Company = reader2.GetString("Company");
                    job.Descripcion = reader2["Descripcion"].ToString();
                    job.JobTitle = reader2["JobTitle"].ToString();
                    job.Place = reader2["Place"].ToString();
                    job.Money = reader2["Money"].ToString();
                    job.position = reader2["position"].ToString();
                    job.SumDescription = reader2["SumDescription"].ToString();

                    jobs.Add(job);
                }
                reader2.Close();
                dash.Persona = per;
                dash.Trabajos = jobs;
            }
            dash.Persona = per;

            return View("Dashboard", dash);
        }

        [HttpPost]
        public ActionResult AddCompany(Person per)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Persons(FirstName, LastName, Mail, Password, Puesto, Company) VALUES('" + per.FirstName + "', '" + per.LastName + "', '" + per.Mail + "', '" + per.Password + "', 'Empresa','" + per.Empresa + "')", con);
                cmd.ExecuteReader();

            }
            return View("IndexAdmin");
        }

        [HttpPost]
        public ActionResult AddJob(Job job)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Jobs (Company, Descripcion, JobTitle, Place, Money, position, SumDescription) VALUES('" + job.Company + "','" + job.Descripcion + "','" + job.JobTitle + "','" + job.Place + "','" + job.Money + "','" + job.position + "','" + job.SumDescription + "')", con);
                cmd.ExecuteReader();

            }
            return View("IndexAdmin");
        }

        [HttpPost]
        public ActionResult AddApplication(Person per, string comp)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Applications (Company, UMail, UName) VALUES('"+comp+ "','" + per.Mail + "','" + per.FirstName+" "+per.LastName + "')", con);
                cmd.ExecuteReader();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Verify(Person per)
        {
            List<Job> jobs = new List<Job>();
            List<Person> aplicantes = new List<Person>();
            Dash dash = new Dash();
            Dash_Empresa dashEmpresa = new Dash_Empresa();
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Persons WHERE Mail='" + per.Mail + "' and Password='" + per.Password + "'", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    per.Puesto = reader.GetString("Puesto");
                    per.FirstName = reader.GetString("FirstName");
                    per.LastName = reader.GetString("LastName");

                    reader.Close();

                    MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM Jobs", con);
                    MySqlDataReader reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        Job job = new Job();
                        job.Company = reader2.GetString("Company");
                        job.Descripcion = reader2["Descripcion"].ToString();
                        job.JobTitle = reader2["JobTitle"].ToString();
                        job.Place = reader2["Place"].ToString();
                        job.Money = reader2["Money"].ToString();
                        job.position = reader2["position"].ToString();
                        job.SumDescription = reader2["SumDescription"].ToString();

                        jobs.Add(job);
                    }
                    reader2.Close();
                    dash.Persona = per;
                    dash.Trabajos = jobs;

                    if (per.Puesto == "Usuario")
                    {
                        return View("Dashboard", dash);
                    }
                    else if (per.Puesto == "Empresa")
                    {
                        MySqlCommand cmd3 = new MySqlCommand("SELECT * FROM Persons", con);
                        MySqlDataReader reader3 = cmd3.ExecuteReader();

                        while (reader3.Read())
                        {
                            per.Mail = reader3.GetString("Mail");
                            //per.EmpresaAplic = reader3.GetString("EmpresaAplic");
                            per.Puesto = reader3.GetString("Puesto");
                            per.FirstName = reader3.GetString("FirstName");
                            per.LastName = reader3.GetString("LastName");

                            aplicantes.Add(per);
                        }
                        reader2.Close();
                        con.Close();
                        return View("Dashboard_Empresa");
                    }
                    else
                    {
                        return View("IndexAdmin");
                    }
                }
                else
                {
                    con.Close();
                    return View("Index");
                }
            }
        }


        [HttpPost]
        public ActionResult EditUserDescription(Person per)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE Persons SET Descripcion ='"+per.Descripcion+"' WHERE Mail ='"+per.Mail+"'; ", con);
                cmd.ExecuteReader();
            }
            return View("UserProfile", per);
        }

        [HttpPost]
        public ActionResult DeleteDB(Person per)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=test;port=3306;password=3681286a"))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE", con);
                cmd.ExecuteNonQuery();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Index2()
        {
            return View();
        }

        public IActionResult UserView()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
