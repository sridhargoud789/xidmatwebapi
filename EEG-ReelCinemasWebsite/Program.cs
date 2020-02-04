using EEG_ReelCinemasWebsite;
using EEG_ReelCinemasWebsite.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RCWebKeyArt
{
    class Program
    {
        public static string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        public static string FileArchive = ConfigurationManager.AppSettings["FileArchive"].ToString();

        public static oDataCinemas objCinemas = new oDataCinemas();
        public static ODataFilmGenres objDataFilmGenres = new ODataFilmGenres();
        public static ODataSession objDataSessions = new ODataSession();
        public static oDataMovies objMovieObject = new oDataMovies();

        public static TotalExperiences objTotalExperiences = new TotalExperiences();
        static void Main(string[] args)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            #region 1.Cinema
            var Cinemas_odata = GetCinemas(ConfigurationManager.AppSettings["ConnectTokenKey"].ToString());


            var oCinemasData = JsonConvert.DeserializeObject<oDataCinemas>(Cinemas_odata.ToString(), settings);

            if (oCinemasData.Value != null && oCinemasData.Value.Count > 0)
            {
                objCinemas = oCinemasData;
            }
            #endregion

            #region 2. FilmGenres
            var Genres_odata = GetGenres(ConfigurationManager.AppSettings["ConnectTokenKey"].ToString());


            var oGenresData = JsonConvert.DeserializeObject<ODataFilmGenres>(Genres_odata.ToString(), settings);

            if (oGenresData.Value != null && oGenresData.Value.Count > 0)
            {
                objDataFilmGenres = oGenresData;
            }
            #endregion

            #region 3. Sessions
            var Sessions_odata = GetSessions(ConfigurationManager.AppSettings["ConnectTokenKey"].ToString());


            var oSessionData = JsonConvert.DeserializeObject<ODataSession>(Sessions_odata.ToString(), settings);

            if (oSessionData.Value != null && oSessionData.Value.Count > 0)
            {
                objDataSessions = oSessionData;
            }
            #endregion

            #region 4. Films
            var Films_odata = GetFilms(ConfigurationManager.AppSettings["ConnectTokenKey"].ToString());

            var oMovieData = JsonConvert.DeserializeObject<oDataMovies>(Films_odata.ToString(), settings);

            if (oMovieData.value != null && oMovieData.value.Count > 0)
            {
                //string str = MovieIDsBySessionDate();
                //var selectedTable = oMovieData.value.AsEnumerable()
                //  .Where(i => str.Contains(i.ID)).ToList();

                objMovieObject = oMovieData;
            }
            #endregion


            ConvertToMovieDataClass();


        }

        private static object GetGenres(string connectapitoken)
        {
            object result = null;

            string OdataUrl = ConfigurationManager.AppSettings["OdataUrl"].ToString();

            string url = OdataUrl + "/FilmGenres?$format=json";


            result = WebRequest.CreateWebRequest(url, connectapitoken);

            return result;
        }

        private static object GetCinemas(string connectapitoken)
        {
            object result = null;

            string OdataUrl = ConfigurationManager.AppSettings["OdataUrl"].ToString();

            string url = OdataUrl + "/Cinemas?$format=json";

            result = WebRequest.CreateWebRequest(url, connectapitoken);

            return result;
        }

        public static object GetFilms(string connectapitoken)
        {
            object result = null;


            string OdataUrl = ConfigurationManager.AppSettings["OdataUrl"].ToString();

            string url = OdataUrl + "/Films?$format=json";

            result = WebRequest.CreateWebRequest(url, connectapitoken);

            return result;
        }
        public static object GetSessions(string connectapitoken)
        {
            object result = null;


            string OdataUrl = ConfigurationManager.AppSettings["OdataUrl"].ToString();

            string url = OdataUrl + "/Sessions?$format=json";

            result = WebRequest.CreateWebRequest(url, connectapitoken);

            return result;
        }

        #region Private

        private static string MovieIDsBySessionDate()
        {
            string movieIds = "";
            var selectedTable = objDataSessions.Value.AsEnumerable()
                .Where(i => i.Showtime >= DateTime.Now).ToList();
            foreach (var item in selectedTable)
            {
                movieIds += item.ScheduledFilmId + ",";
            }
            movieIds = movieIds.TrimEnd(',');
            return movieIds;
        }




        private static List<FilmsData> GetNowShowingMovies()
        {
            string str = MovieIDsBySessionDate();

            List<FilmsData> selectedTable = objMovieObject.value.AsEnumerable()
                           .Where(i => DateTime.ParseExact(i.OpeningDate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture)
                                    && str.Contains(i.ID)).ToList();
            return selectedTable;
        }
        private static List<FilmsData> GetCommingSoonMovies()
        {
            string str = MovieIDsBySessionDate();
            List<FilmsData> selectedTable = objMovieObject.value.AsEnumerable()
                          .Where(i => DateTime.ParseExact(i.OpeningDate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture)
                           //&& !str.Contains(i.ID)
                           ).ToList();
            return selectedTable;
        }

        private static List<MovieDataModel> ConvertToMovieDataClass()
        {
            MovieDataModel oMovieDataModel = null;
            List<MovieDataModel> lstMovieDataModel = new List<MovieDataModel>();

            List<List<FilmsData>> oFinalList = new List<List<FilmsData>>();
            oFinalList.Add(GetNowShowingMovies());
            oFinalList.Add(GetCommingSoonMovies());



            string strCinemaId, strCinemaName;
            foreach (var li in oFinalList.ToList())
            {
                foreach (var item in li)
                {
                    try
                    {
                        strCinemaId = "";
                        strCinemaName = "";
                        var shortSy = item.ShortSynopsis.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        var language = shortSy.Count() > 0 ? shortSy[0].ToString() : "";
                        var subTitle = shortSy.Count() > 1 ? shortSy[1].ToString() : "";
                        var synopsis = item.Synopsis.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        string strLanguage = language.Split(':')[1].ToString().Trim();
                        string mainSynopsis = "";
                        if (synopsis.Count() >= 3)
                        {
                            string synopsisDesc = synopsis[0].ToString();
                            var director = synopsis[1].ToString();
                            var cast = synopsis[2].ToString();
                            mainSynopsis = subTitle.TrimStart(' ').TrimEnd(' ') + "\r\n" + director.TrimStart(' ').TrimEnd(' ') + "\r\r\n" + cast.TrimStart(' ').TrimEnd(' ') + " \r\nSynopsis:" + synopsisDesc.TrimStart(' ').TrimEnd(' ') + "\r";
                        }
                        else if (synopsis.Count() == 2)
                        {

                            var director = synopsis[0].ToString();
                            var cast = synopsis[1].ToString();
                            mainSynopsis = subTitle.TrimStart(' ').TrimEnd(' ') + "\r\n" + director.TrimStart(' ').TrimEnd(' ') + "\r\r\n" + cast.TrimStart(' ').TrimEnd(' ') + " \r\nSynopsis:\r";
                        }

                        GetCinemasInfo(item.ID, out strCinemaId, out strCinemaName);

                        objTotalExperiences = AssignAllExperience();


                        oMovieDataModel = new MovieDataModel();
                        oMovieDataModel.MT = DateTime.ParseExact(item.OpeningDate.Value.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture) ? "NowShowing" : "ComingSoon";
                        oMovieDataModel.MID = item.ID;
                        oMovieDataModel.CN = strCinemaName;
                        oMovieDataModel.CID = strCinemaId;
                        oMovieDataModel.MN = item.Title;
                        oMovieDataModel.ML = strLanguage;
                        oMovieDataModel.MI = item.Title.Replace(" ", "-") + ".jpg";
                        oMovieDataModel.MTR = item.TrailerUrl;
                        oMovieDataModel.SD = item.OpeningDate.Value.ToString("MM/dd/yyyy hh:mm:ss tt");
                        oMovieDataModel.DU = ConvertToHours(item.RunTime.ToString());
                        oMovieDataModel.RT = item.Rating;
                        oMovieDataModel.SP = mainSynopsis;
                        oMovieDataModel.GR = GetGenreName(item.GenreId, objDataFilmGenres.Value);
                        oMovieDataModel.MDU = item.RunTime.ToString();
                        oMovieDataModel.DD = item.OpeningDate.Value.ToString("dd/MM/yyyy 00:00:00");
                        oMovieDataModel.MSLst = ConvertToMoviesessionList(item.ID, objDataSessions.Value, item.Rating);
                        oMovieDataModel.Experiences = GetMovieExperiences();
                        oMovieDataModel.YTU = item.TrailerUrl;
                        oMovieDataModel.SC = oMovieDataModel.MSLst == null ? 0 : oMovieDataModel.MSLst.Count;

                        lstMovieDataModel.Add(oMovieDataModel);
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message;
                    }

                }
            }


            string jsonData = JsonConvert.SerializeObject(lstMovieDataModel);
            string file = String.Format("\\" + ConfigurationManager.AppSettings["MoviesSessionFileForMobile"].ToString() + "{0}.json", DateTime.Now.ToString("dd MM-yyy_hh-mm"));
            try
            {
                try
                {
                    if (File.Exists(FilePath + "\\" + ConfigurationManager.AppSettings["MoviesSessionFileForMobile"].ToString() + ".json"))
                    {
                        File.Copy(FilePath + "\\" + ConfigurationManager.AppSettings["MoviesSessionFileForMobile"].ToString() + ".json", FileArchive + file, true);
                    }
                }
                catch (Exception ex) { }
                System.IO.File.WriteAllText(FilePath + "\\" + ConfigurationManager.AppSettings["MoviesSessionFileForMobile"].ToString() + ".json", jsonData);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
            }

            return lstMovieDataModel;
        }


        private static List<MovieExperienceModel> GetMovieExperiences()
        {

            var fExp = objTotalExperiences.value.AsEnumerable()
                         .Where(i => i.IsActive == true).ToList();
            List<MovieExperienceModel> obj = new List<MovieExperienceModel>();
            foreach (var item in fExp)
            {
                MovieExperienceModel o = new MovieExperienceModel();
                o.Type = item.Type;
                o.ImageUrl = item.ImageUrl;
                obj.Add(o);
            }
            return obj;

        }


        private static void GetCinemasInfo(string iD, out string strCinemaId, out string strCinemaName)
        {
            strCinemaId = "";
            strCinemaName = "";
            var selectedTable = objDataSessions.Value.AsEnumerable()
                           .Where(i => i.ScheduledFilmId == iD).Select(s => s.CinemaId).Distinct().ToList();
            foreach (var item in selectedTable)
            {
                string cName = "";
                try
                {
                    cName = objCinemas.Value.AsEnumerable()
                           .Where(i => i.ID == item).First().Name;
                }
                catch (Exception)
                {
                    cName = "";
                }

                if (cName != "")
                {
                    strCinemaId += item + ",";
                    strCinemaName += objCinemas.Value.AsEnumerable()
                               .Where(i => i.ID == item).First().Name + ",";
                }

            }

            strCinemaId = strCinemaId.TrimEnd(',');
            strCinemaName = strCinemaName.TrimEnd(',');
        }

        private static string GetGenreName(string genreId, List<FilmGenres> filmGenreDt)
        {
            List<FilmGenres> selectedTable = filmGenreDt.AsEnumerable()
                          .Where(i => i.ID == genreId).ToList();
            return selectedTable.FirstOrDefault().Name;
        }

        private static string ConvertToHours(string totalMins)
        {
            int totalMinutes = Convert.ToInt32(totalMins);
            TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
            return string.Format("{0}hr {1}min", ts.Hours, ts.Minutes);
        }



        private static List<MoviesessionModel> ConvertToMoviesessionList(string scheduledFilmId, List<Session> sessionDt, string rating)
        {
            List<MoviesessionModel> movieSessionList = null;
            MoviesessionModel moviesession = new MoviesessionModel();
            movieSessionList = new List<MoviesessionModel>();
            List<Session> selectedTable = sessionDt.AsEnumerable()
                           .Where(i => i.ScheduledFilmId == scheduledFilmId).ToList();
            string filmName;
            selectedTable = selectedTable
                   .OrderBy(c => Convert.ToDateTime(c.Showtime).Date)
                   .ThenBy(c => Convert.ToDateTime(c.Showtime).TimeOfDay).ToList();

            foreach (var item in selectedTable)
            {

                try
                {
                    filmName = "";
                    var cinemaObj = objCinemas.Value.AsEnumerable()
                                 .Where(i => i.ID == item.CinemaId).ToList();
                    if (cinemaObj.Count > 0)
                    {
                        filmName = objCinemas.Value.AsEnumerable()
                                .Where(i => i.ID == item.CinemaId).First().Name;
                    }

                    if (item.SessionId == "39595")
                    {
                        string s = "";
                    }
                    TimeSpan start = new TimeSpan(00, 0, 0); //10 o'clock
                    TimeSpan end = new TimeSpan(06, 0, 0); //12 o'clock
                    TimeSpan now = item.Showtime.TimeOfDay;

                    DateTime Showtime = item.Showtime;
                    if ((now >= start) && (now <= end))
                    {
                        Showtime = item.Showtime.AddDays(-1);
                    }


                    moviesession = new MoviesessionModel();
                    moviesession.SC = item.ScreenName;
                    moviesession.SD = Showtime.ToString("MM/dd/yyyy HH:mm");
                    moviesession.ASD = item.Showtime.ToString("MM/dd/yyyy HH:mm");
                    moviesession.CID = item.CinemaId;
                    moviesession.CN = filmName;
                    moviesession.SID = item.SessionId;

                    string sessionAttributesNames = "";
                    bool isAgeRestricted = false;
                    bool isSessionUpgradable = false;
                    foreach (var sessionAttributeName in item.SessionAttributesNames)
                    {
                        sessionAttributesNames = sessionAttributesNames + sessionAttributeName.ToString() + ",";
                        if (sessionAttributeName.ToString().ToLower() == "upgrad")
                        {
                            isSessionUpgradable = true;
                        }
                        if (sessionAttributeName.ToString().ToLower() == "agerestri")
                        {
                            isAgeRestricted = true;
                        }
                    }
                    sessionAttributesNames = sessionAttributesNames.Trim(',');
                    moviesession.EX = CheckExperienceName(item.CinemaOperatorCode, item.CinemaId, sessionAttributesNames, "");
                    moviesession.VISTAEX = VistaEX(item.CinemaOperatorCode, item.CinemaId, sessionAttributesNames, rating);
                    moviesession.AV = CheckSeatAvailablity(item.SeatsAvailable);
                    moviesession.isAgeRestricted = isAgeRestricted;
                    moviesession.isSessionUpgradable = isSessionUpgradable;

                    string strText = string.Empty;
                    string strHeader = string.Empty;
                    bool isAfter7PM = false;
                    int iShowHour = item.Showtime.TimeOfDay.Hours;
                    if (iShowHour >= 19)
                    {
                        isAfter7PM = true;
                    }

                    GetPopUpText(moviesession.VISTAEX, isAgeRestricted, rating, out strText, out strHeader, isAfter7PM);

                    moviesession.CPPText = strText;
                    moviesession.CPPHText = strHeader;
                    movieSessionList.Add(moviesession);
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                }

            }
            return movieSessionList;
        }

        private static void GetPopUpText(string exp, bool isagerist, string rt, out string strText, out string strHeader,bool isAfter7PM)
        {
            strText = string.Empty;
            strHeader = string.Empty;
            if (rt == "15+")
            {
                strHeader = "General Movie Age Rating";
                strText = "Please confirm you are above the age of 15 before proceeding with your booking. You may be asked to present a valid ID prior to entering the cinema.";
            }
            else if (rt == "18+")
            {
                strHeader = "General Movie Age Rating";
                strText = "Please confirm you are above the age of 15 before proceeding with your booking. You may be asked to present a valid ID prior to entering the cinema.";

            }
            if (exp == "family-dine-in")
            {
                strHeader = "Dine-in Cinema Bookings";
                if (isagerist)
                {
                    strText = "This movie session in Dine-in Cinema is open to those that are 21 or above.\r\nSpecialty beverages are served in this session.";
                }
                else
                {
                    strText = "This movie session in Dine-in Cinema is open to all ages.\r\nSpecialty beverages are not served in this session.";
                }
            }
            else if (exp == "Boutique")
            {
                strHeader = "Reel Boutique at Rove Downtown";
                if (isagerist || isAfter7PM)
                {
                    strText = "This movie session in Reel Boutique is open to those that are 21 or above.\r\nSpecialty beverages are served in this session.";
                }
                else
                {
                    strText = "This movie session in Reel Boutique is open to all ages.\r\nSpecialty beverages are not served in this session.";
                }
            }
            else if (exp == "mx4d")
            {
                strHeader = "MX4D";

                strText = "Please be advised you may experience effects & movements whilst watching a movie in MX4D. If you are pregnant we advise you to choose a different experience.\r\nAll guest should be a minimum of 1m in height.";

            }
      

        }
        private static TotalExperiences AssignAllExperience()
        {
            TotalExperiences objTotal = new TotalExperiences();
            List<Experiences> li = new List<Experiences>();
            li.Add(new Experiences { Type = "Standard", ImageUrl = "/Content/images/experience-logo/standard.png", IsActive = false }); //0
            li.Add(new Experiences { Type = "Dolby Cinema", ImageUrl = "/Content/images/experience-logo/dolby.png", IsActive = false });//1
            li.Add(new Experiences { Type = "Platinum", ImageUrl = "/Content/images/experience-logo/platinum.png", IsActive = false });//2
            li.Add(new Experiences { Type = "Dine-in Cinema", ImageUrl = "/Content/images/experience-logo/dine-in.png", IsActive = false });//3
            li.Add(new Experiences { Type = "MX4D", ImageUrl = "/Content/images/experience-logo/mx4d.png", IsActive = false });//4
            li.Add(new Experiences { Type = "Premier", ImageUrl = "/Content/images/experience-logo/premier.png", IsActive = false });//5
            li.Add(new Experiences { Type = "Reel Boutique", ImageUrl = "/Content/images/experience-logo/Boutique.png", IsActive = false });//6
            li.Add(new Experiences { Type = "Premier", ImageUrl = "/Content/images/experience-logo/ScreenX.png", IsActive = false });//7
            objTotal.value = li;
            return objTotal;
        }

        //private static string CheckVistaExperienceName(string cinemaOperatorCode, string cinemaId, string sessionAttributesNames)
        //{
        //    string vistaExperienceName = "";

        //    if (cinemaId == "0001" && cinemaOperatorCode == "1000000010")
        //    {
        //        vistaExperienceName = "Reel Junior";
        //    }
        //    if (cinemaId == "0003" && cinemaOperatorCode == "3000000002")
        //    {
        //        vistaExperienceName = "Family-Dine-In";
        //    }
        //    if (cinemaId == "0002" && cinemaOperatorCode == "2000000004")
        //    {
        //        vistaExperienceName = "Premier";
        //    }
        //    if (cinemaId == "0004" && cinemaOperatorCode == "4000000002")
        //    {
        //        vistaExperienceName = "Premier";
        //    }
        //    if (cinemaId == "0006" && cinemaOperatorCode == "6000000002")
        //    {
        //        vistaExperienceName = "Reel Junior";
        //    }

        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D") && sessionAttributesNames.Contains("PLATINUM"))
        //    {
        //        vistaExperienceName = "Platinum-3D";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D") && sessionAttributesNames.Contains("DOLBY"))
        //    {
        //        vistaExperienceName = "Dolby-3D";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D"))
        //    {
        //        vistaExperienceName = "Standard-3D";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("STANDARD"))
        //    {
        //        vistaExperienceName = "Standard";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("PLATINUM"))
        //    {
        //        vistaExperienceName = "Platinum";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("MX4D"))
        //    {
        //        vistaExperienceName = "MX4D";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("ESCAPE"))
        //    {
        //        vistaExperienceName = "Barco Escape";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("DOLBY"))
        //    {
        //        vistaExperienceName = "Dolby Cinema";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("DINE-IN"))
        //    {
        //        vistaExperienceName = "Dine-in Cinema";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("Reel Btq"))
        //    {
        //        vistaExperienceName = "Boutique Package";
        //    }
        //    if (sessionAttributesNames != "" && sessionAttributesNames.Contains("ScreenX"))
        //    {
        //        vistaExperienceName = "ScreenX";
        //    }
        //    return vistaExperienceName;

        //}

        private static string VistaEX(string cinemaOperatorCode, string cinemaId, string sessionAttributesNames, string rating)
        {
            string experienceName = "";

            if (cinemaId == "0001" && cinemaOperatorCode == "1000000010")
            {
                experienceName = "Junior";
                return experienceName;
            }
            if (cinemaId == "0006" && cinemaOperatorCode == "6000000002")
            {
                experienceName = "Junior";
                return experienceName;
            }
            if (cinemaId == "0007" && cinemaOperatorCode == "7000000004")
            {
                experienceName = "Junior";
                return experienceName;
            }

            if (cinemaId == "0002" && cinemaOperatorCode == "2000000004")
            {
                experienceName = "Premier";
                objTotalExperiences.value[5].IsActive = true;
                return experienceName;
            }
            if (cinemaId == "0004" && cinemaOperatorCode == "4000000002")
            {
                experienceName = "Premier";
                objTotalExperiences.value[5].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D") && sessionAttributesNames.Contains("PLATINUM"))
            {
                experienceName = "Platinum-3D";
                objTotalExperiences.value[2].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D") && sessionAttributesNames.Contains("DOLBY"))
            {
                experienceName = "Dolby-3D";
                objTotalExperiences.value[1].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D"))
            {
                experienceName = "Standard-3D";
                objTotalExperiences.value[0].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("STANDARD"))
            {
                experienceName = "Standard";
                objTotalExperiences.value[0].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("PLATINUM"))
            {
                experienceName = "Platinum Suites";
                objTotalExperiences.value[2].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("MX4D"))
            {
                experienceName = "MX4D";
                objTotalExperiences.value[4].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("ESCAPE"))
            {
                experienceName = "Barco Escape";
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("DOLBY"))
            {
                experienceName = "Dolby Cinema";
                objTotalExperiences.value[1].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("DINE-IN"))
            {
                experienceName = "Dine-in";
                if (rating == "PG")
                {
                    experienceName = "family-dine-in";
                }

                objTotalExperiences.value[3].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("Reel Btq"))
            {
                experienceName = "Boutique";
                objTotalExperiences.value[6].IsActive = true;
                return experienceName;

            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("ScreenX"))
            {
                experienceName = "ScreenX";
                objTotalExperiences.value[7].IsActive = true;
                return experienceName;
            }
            return experienceName;
        }
        private static string CheckExperienceName(string cinemaOperatorCode, string cinemaId, string sessionAttributesNames, string rating)
        {
            string experienceName = "";

            if (cinemaId == "0001" && cinemaOperatorCode == "1000000010")
            {
                experienceName = "Reel Junior";
                return experienceName;
            }
            if (cinemaId == "0006" && cinemaOperatorCode == "6000000002")
            {
                experienceName = "Reel Junior";
                return experienceName;
            }
            if (cinemaId == "0007" && cinemaOperatorCode == "7000000004")
            {
                experienceName = "Reel Junior";
                return experienceName;
            }
            if (cinemaId == "0002" && cinemaOperatorCode == "2000000004")
            {
                experienceName = "Reel Premier";
                objTotalExperiences.value[5].IsActive = true;
                return experienceName;
            }
            if (cinemaId == "0004" && cinemaOperatorCode == "4000000002")
            {
                experienceName = "Reel Premier";
                objTotalExperiences.value[5].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D") && sessionAttributesNames.Contains("PLATINUM"))
            {
                experienceName = "Reel Platinum-3D";
                objTotalExperiences.value[2].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D") && sessionAttributesNames.Contains("DOLBY"))
            {
                experienceName = "Dolby-3D";
                objTotalExperiences.value[1].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("3D"))
            {
                experienceName = "Reel Standard-3D";
                objTotalExperiences.value[0].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("STANDARD"))
            {
                experienceName = "Reel Standard";
                objTotalExperiences.value[0].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("PLATINUM"))
            {
                experienceName = "Reel Platinum Suites";
                objTotalExperiences.value[2].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("MX4D"))
            {
                experienceName = "MX4D";
                objTotalExperiences.value[4].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("ESCAPE"))
            {
                experienceName = "Barco Escape";
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("DOLBY"))
            {
                experienceName = "Dolby Cinema";
                objTotalExperiences.value[1].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("DINE-IN"))
            {
                experienceName = "Reel Dine-in";
                if (rating == "PG")
                {
                    experienceName = "family-dine-in";
                }

                objTotalExperiences.value[3].IsActive = true;
                return experienceName;
            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("Reel Btq"))
            {
                experienceName = "Reel Boutique";
                objTotalExperiences.value[6].IsActive = true;
                return experienceName;

            }
            if (sessionAttributesNames != "" && sessionAttributesNames.Contains("ScreenX"))
            {
                experienceName = "ScreenX";
                objTotalExperiences.value[7].IsActive = true;
                return experienceName;
            }
            return experienceName;
        }

        private static string CheckSeatAvailablity(int seatsAvailable)
        {
            string status = "available";
            if (seatsAvailable > 0 && seatsAvailable <= 10)
            {
                status = "nearly-sold-out";
            }
            if (seatsAvailable > 10)
            {
                status = "available";
            }
            if (seatsAvailable == 0)
            {
                status = "sold-out";
            }
            if (seatsAvailable < 0)
            {
                status = "booked";
            }
            return status;
        }
        #endregion
    }
}