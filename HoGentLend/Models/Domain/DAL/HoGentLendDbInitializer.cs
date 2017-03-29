using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class HoGentLendDbInitializer : System.Data.Entity.DropCreateDatabaseAlways<HoGentLendContext>
    {
        protected override void Seed(HoGentLendContext context)
        {
            try
            {
                // Config
                DateTime _10u30 = new DateTime(2016, 4, 13).AddHours(10).AddMinutes(30);
                DateTime _17u00 = new DateTime(2016, 4, 13).AddHours(17);

                Config c = new Config
                {
                    LendingPeriod = 1,
                    Indiendag = "vrijdag",
                    Ophaaldag = "maandag",
                    Indientijd = _10u30,
                    Ophaaltijd = _17u00
                };

                context.Configs.Add(c);

                // Hier zetten we database context initializatie
                Groep d1 = new Groep { Name = "Kleuteronderwijs", IsLeerGebied = false };
                Groep d2 = new Groep { Name = "Lager onderwijs", IsLeerGebied = false };
                Groep d3 = new Groep { Name = "Secundair onderwijs", IsLeerGebied = false };
                Groep d4 = new Groep { Name = "Hoger onderwijs", IsLeerGebied = false };


                List<Groep> van3tot12 = new List<Groep>();
                van3tot12.Add(d1);
                van3tot12.Add(d2);

                List<Groep> van6tot12 = new List<Groep>();
                van6tot12.Add(d2);

                List<Groep> van12tot21 = new List<Groep>();
                van12tot21.Add(d3);
                van12tot21.Add(d4);





                List<Groep> leergebiedenSet1 = new List<Groep>();
                leergebiedenSet1.Add(new Groep { Name = "Aardrijkskunde", IsLeerGebied = true });
                leergebiedenSet1.Add(new Groep { Name = "Geografie", IsLeerGebied = true });

                List<Groep> leergebiedenSet2 = new List<Groep>();
                leergebiedenSet2.Add(new Groep { Name = "Wiskunde", IsLeerGebied = true });

                List<Groep> leergebiedenSet3 = new List<Groep>();
                leergebiedenSet3.Add(new Groep { Name = "LO", IsLeerGebied = true });

                List<Groep> leergebiedenSet4 = new List<Groep>();
                leergebiedenSet4.Add(new Groep { Name = "Biologie", IsLeerGebied = true });

                List<Groep> leergebiedenSet5 = new List<Groep>();
                leergebiedenSet5.Add(new Groep { Name = "Kunst", IsLeerGebied = true });

                List<Groep> leergebiedenSet6 = new List<Groep>();
                leergebiedenSet6.Add(new Groep { Name = "Spel", IsLeerGebied = true });















                Firma f1 = new Firma { Name = "Goaty Enterprise", Email = "info@goatyenterprise.be" };

                Materiaal m1 = new Materiaal
                {
                    Name = "Wereldbol Caran D'Ache Supra 80",
                    //Description = "Alle landen van de wereld in één handomdraai.",
                    //ArticleCode = "abc123",
                    //Price = 12.85,
                    Amount = 10,
                    //AmountNotAvailable = 10,
                    IsLendable = true,
                    //Location = "GSCHB4.021",
                    //Firma = f1,
                    Doelgroepen = van3tot12,
                    Leergebieden = leergebiedenSet1,
                    //PhotoBytes = new WebClient().DownloadData("https://www.dezwerver.nl/media/cache/e3/88/e38825e2d8175a72d9d346193f983983.jpg")
                };

                Materiaal m2 = new Materiaal
                {
                    Name = "Rekenmachine",
                    Description = "Reken er op los met deze grafische rekenmachine.",
                    ArticleCode = "abc456",
                    Price = 19.99,
                    Amount = 4,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = van12tot21,
                    Leergebieden = leergebiedenSet2,
                    PhotoBytes = new WebClient().DownloadData("http://www.epacking.eu/Docs/Images/Groups/1/Product_2012329991k222k87k491_rekenmachine.jpg")
                };

                Materiaal m3 = new Materiaal
                {
                    Name = "Kleurpotloden",
                    Description = "Alle kleuren van de regenboog.",
                    ArticleCode = "abc789",
                    Price = 29.99,
                    Amount = 10,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = van6tot12,
                    Leergebieden = leergebiedenSet5,
                };

                Materiaal m4 = new Materiaal
                {
                    Name = "Voetbal",
                    Description = "Voetballen voor in het lager onderwijs.",
                    ArticleCode = "abc147",
                    Price = 25.99,
                    Amount = 15,
                    AmountNotAvailable = 3,
                    IsLendable = false,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = van6tot12,
                    Leergebieden = leergebiedenSet3,
                    PhotoBytes = new WebClient().DownloadData("http://hobby.blogo.nl/files/2007/11/hoe-voetbal-surprise-maken-680x703.jpg"),
                };

                Materiaal m5 = new Materiaal
                {
                    Name = "Basketbal",
                    Description = "De NBA Allstar biedt de perfecte oplossing op het vlak van duurzaamheid en spelprestaties. Zowel geschikt voor indoor als outdoor. Uitstekende grip!",
                    ArticleCode = "abc258",
                    Price = 25.99,
                    Amount = 12,
                    AmountNotAvailable = 3,
                    IsLendable = true,
                    Location = "GSCHB 4.021",
                    Firma = f1,
                    Doelgroepen = van12tot21,
                    Leergebieden = leergebiedenSet3,
                    PhotoBytes = null,
                };

                Materiaal m6 = new Materiaal
                {
                    Name = "Dobbelsteen-schatkist-162delig",
                    Description = "Een koffertje met verschillende soorten dobbelstenen: blanco, met cjfers, ...",
                    ArticleCode = "MH1447",
                    Price = 35.00,
                    Amount = 1,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GLEDE 1.011",
                    Firma = f1,
                    Doelgroepen = van6tot12,
                    Leergebieden = leergebiedenSet6,
                    PhotoBytes = new WebClient().DownloadData("http://www.baert.com/images/products/MH1447-03.jpg"),
                };

                Materiaal m7 = new Materiaal
                {
                    Name = "Mini-loco-spelbord - 4 tot 8 jaar",
                    Description = "Spelbord: klapt open met een rode becijferde kant en een doorzichtige kan + 12 blokjes met de getallen van 1 tot en met 12.",
                    ArticleCode = "NC2038",
                    Price = 15.90,
                    Amount = 6,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GLEDE 1.011",
                    Firma = f1,
                    Doelgroepen = van3tot12,
                    Leergebieden = leergebiedenSet6,
                    PhotoBytes = new WebClient().DownloadData("https://s.s-bol.com/imgbase0/imagebase3/large/FC/4/9/6/6/1001004004476694.jpg"),
                };

                Materiaal m8 = new Materiaal
                {
                    Name = "Student Dissectie Set",
                    Description = "Student Dissectie Set van professionele kwaliteit. De kit is zeer compleet en is ontworpen voor studenten.",
                    ArticleCode = "WTC911",
                    Price = 17.95,
                    Amount = 12,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "Campus Vesalius 4.020",
                    Firma = f1,
                    Doelgroepen = van12tot21,
                    Leergebieden = leergebiedenSet4,
                    PhotoBytes = new WebClient().DownloadData("http://www.nursexl.be/media/catalog/product/cache/24/image/250x250/17f82f742ffe127f42dca9de82fb58b1/v/k/vk-1.jpg"),
                };

                Materiaal m9 = new Materiaal
                {
                    Name = "Acer H5380BD",
                    Description = "Fed decent video content (like Blu-ray), the H5380BD puts out an extremely watchable image. And its input lag is low—faster than most TVs and high-end projectors.",
                    ArticleCode = "3EPNO60",
                    Price = 495.00,
                    Amount = 12,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GLEDE 1.011",
                    Firma = f1,
                    Doelgroepen = van12tot21,
                    Leergebieden = leergebiedenSet1,
                    PhotoBytes = new WebClient().DownloadData("https://thewirecutter.com/wp-content/uploads/2016/01/01w-500-projector-acer-h5380bd-630-420x280.jpg"),
                };

                Materiaal m10 = new Materiaal
                {
                    Name = "Kleurpotloden Caran D'Ache Supra 80",
                    Description = "* Superieure kwaliteit aquarel kleurpotloden. * Met zachte potlood stift. * Excellente lichtechtheid. * Gemaakt uit FSC gecertificeerd hout, verpakt in een luxe koffer.",
                    ArticleCode = "7610186044809",
                    Price = 269.00,
                    Amount = 22,
                    AmountNotAvailable = 0,
                    IsLendable = true,
                    Location = "GLEDE 1.011",
                    Firma = f1,
                    Doelgroepen = van3tot12,
                    Leergebieden = leergebiedenSet5,
                    PhotoBytes = new WebClient().DownloadData("https://s.s-bol.com/imgbase0/imagebase3/large/FC/8/5/4/0/9200000046170458.jpg"),
                };

                context.Materialen.Add(m1);
                context.Materialen.Add(m2);
                context.Materialen.Add(m3);
                context.Materialen.Add(m4);
                context.Materialen.Add(m5);
                context.Materialen.Add(m6);
                context.Materialen.Add(m7);
                context.Materialen.Add(m8);
                context.Materialen.Add(m9);
                context.Materialen.Add(m10);


                Gebruiker g1 = new Student("Offline", "Student", "offline.student@hogent.be");
                Gebruiker g2 = new Lector("Offline", "Lector", "offline.lector@hogent.be");
                Gebruiker g3 = new Lector("Jan", "Pers", "tstpers456");
                Gebruiker g4 = new Lector("Hoofd", "Beheerder", "hoofdbeheerder@hogent.be")
                {
                    IsHoofdBeheerder = true,
                    IsBeheerder = true
                };

                g3.IsBeheerder = true;

                context.Users.Add(new ApplicationUser()
                {
                    Id = "95bebdd6-39b7-4ea5-a3fb-996af68af2aa",
                    Email = "offline.student@hogent.be",
                    EmailConfirmed = false,
                    SecurityStamp = "6a296cb5-8ebd-45e9-b539-0be1526bccb3",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "student",
                    PasswordHash = "ABoaWXEiAci5aH8AifWD7ugcOa8TrTarAGGdff7BJ7zyJFeGATxR71fmBnuwzvPAxw=="
                });
                context.Users.Add(new ApplicationUser()
                {
                    Id = "9c544638-8722-4542-a7e2-5b82cd6c1592",
                    Email = "offline.lector@hogent.be",
                    EmailConfirmed = false,
                    SecurityStamp = "715aa298-8ae8-4861-b34d-79fc9e95d3c3",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "lector",
                    PasswordHash = "AJYvUYUnpvspai3ll9CtyglpHk+9MwU6huE7PpWIWYLpFoNRKd8knknkzfLaqQthLw=="
                });

                context.Gebruikers.Add(g1);
                context.Gebruikers.Add(g2);
                context.Gebruikers.Add(g3);
                context.Gebruikers.Add(g4);

                DateTime _11Juli2016 = new DateTime(2016, 7, 11);
                DateTime _15Juli2016 = new DateTime(2016, 7, 15);

                DateTime _18Juli2016 = new DateTime(2016, 7, 18);
                DateTime _22Juli2016 = new DateTime(2016, 7, 22);

                DateTime _13Juni2016 = new DateTime(2016, 6, 13);
                DateTime _17Juni2016 = new DateTime(2016, 6, 17);

                DateTime _20Juni2016 = new DateTime(2016, 6, 20);
                DateTime _24Juni2016 = new DateTime(2016, 6, 24);

                Reservatie r1 = new Reservatie(g1, _11Juli2016, _15Juli2016);
                r1.ReservatieLijnen = new List<ReservatieLijn>();
                r1.ReservatieLijnen.Add(new ReservatieLijn(4, _11Juli2016, _15Juli2016, m5, r1));
                r1.ReservatieLijnen.Add(new ReservatieLijn(2, _11Juli2016, _15Juli2016, m1, r1));
                r1.ReservatieLijnen.Add(new ReservatieLijn(3, _11Juli2016, _15Juli2016, m2, r1));
                

                Reservatie r3 = new Reservatie(g1, _13Juni2016, _17Juni2016)
                {
                    Opgehaald = true
                };
                r3.ReservatieLijnen = new List<ReservatieLijn>();
                r3.ReservatieLijnen.Add(new ReservatieLijn(2, _13Juni2016, _17Juni2016, m1, r3));
                r3.ReservatieLijnen.Add(new ReservatieLijn(3, _13Juni2016, _17Juni2016, m2, r3));
                r3.ReservatieLijnen.Add(new ReservatieLijn(4, _13Juni2016, _17Juni2016, m3, r3));

                Reservatie r2 = new Reservatie(g1, _18Juli2016, _22Juli2016);
                r2.ReservatieLijnen = new List<ReservatieLijn>();
                r2.ReservatieLijnen.Add(new ReservatieLijn(2, _18Juli2016, _22Juli2016, m4, r2));
                r2.ReservatieLijnen.Add(new ReservatieLijn(3, _18Juli2016, _22Juli2016, m5, r2));
                r2.ReservatieLijnen.Add(new ReservatieLijn(4, _18Juli2016, _22Juli2016, m3, r2));

                Reservatie r4 = new Reservatie(g2, _18Juli2016, _22Juli2016);
                r4.ReservatieLijnen = new List<ReservatieLijn>();
                r4.ReservatieLijnen.Add(new ReservatieLijn(12, _18Juli2016, _22Juli2016, m4, r4));
                r4.ReservatieLijnen.Add(new ReservatieLijn(7, _18Juli2016, _22Juli2016, m5, r4));

                Reservatie r5 = new Reservatie(g2, _20Juni2016, _24Juni2016);
                r5.ReservatieLijnen = new List<ReservatieLijn>();
                r5.ReservatieLijnen.Add(new ReservatieLijn(1, _20Juni2016, _24Juni2016, m9, r5));
                r5.ReservatieLijnen.Add(new ReservatieLijn(2, _20Juni2016, _24Juni2016, m5, r5));

                context.Reservaties.Add(r1);
                context.Reservaties.Add(r2);
                context.Reservaties.Add(r3);
                context.Reservaties.Add(r4);
                context.Reservaties.Add(r5);

                context.SaveChanges();
                //base.Seed(context);
            }
            catch (DbEntityValidationException e)
            {
                string s = "Fout creatie database ";
                foreach (var eve in e.EntityValidationErrors)
                {
                    s += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.GetValidationResult());
                    foreach (var ve in eve.ValidationErrors)
                    {
                        s += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(s);
            }
        }
    }
}