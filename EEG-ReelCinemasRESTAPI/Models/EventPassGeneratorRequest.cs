using Passbook.Generator;
using Passbook.Generator.Fields;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class EventPassGeneratorRequest : PassGeneratorRequest
    {
        public EventPassGeneratorRequest()
        {
            this.Style = PassStyle.EventTicket;
        }

        public string BookingID { get; set; }
        public string Experience { get; set; }
        public string TicketInfo { get; set; }
        public string ShowDate { get; set; }
        public string Showtime { get; set; }
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
        public string Screenname { get; set; }
        public string Rating { get; set; }
        public string SeatInfo { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Total { get; set; }
        public string Relavenantdate { get; set; }
        public override void PopulateFields()
        {
            this.RelevantDate = Convert.ToDateTime(Relavenantdate);
            this.AddPrimaryField(new StandardField("MovieName", MovieName, string.Empty));
            this.AddSecondaryField(new StandardField("CinemaName", CinemaName, string.Empty));
            this.AddAuxiliaryField(new StandardField("Showtime",ShowDate + " " + Showtime,string.Empty));
            this.AddAuxiliaryField(new StandardField("Screen", "SCREEN - " + Screenname, string.Empty));
            this.AddAuxiliaryField(new StandardField("booking-id", BookingID, string.Empty));
            this.AddAuxiliaryField(new StandardField("SeatInfo", SeatInfo, string.Empty));

            this.AddLocation(Latitude, Longitude);

            this.AddBackField(new StandardField("BOOKING ID", "BOOKING ID", BookingID));
            this.AddBackField(new StandardField("MovieName", "Movie Name", MovieName));
            this.AddBackField(new StandardField("Rating", "Rating", Rating));
            this.AddBackField(new StandardField("Showtime", "Showtime", ShowDate + " " + Showtime));
            this.AddBackField(new StandardField("CinemaName", "Location", CinemaName));
            this.AddBackField(new StandardField("Screen", "Screen", "SCREEN - " + Screenname));
            this.AddBackField(new StandardField("Experience", "Experience", Experience));
            this.AddBackField(new StandardField("Seats", "Seat(s)", TicketInfo));
            this.AddBackField(new StandardField("Total", "Total", Total));
            this.AddBackField(new StandardField("Terms and Conditions", "Terms and Conditions", ConfigurationManager.AppSettings["weburl"].ToString() + "/terms-and-condition"));

            //this.AddHeaderField(new StandardField("Showtime", Showtime, ShowDate));
            //this.RelevantDate = Convert.ToDateTime(Relavenantdate);
            //this.AddPrimaryField(new StandardField("MovieName", CinemaName, MovieName));
            //this.AddAuxiliaryField(new StandardField("Screen", "SCREEN", Screenname));
            //this.AddAuxiliaryField(new StandardField("booking-id", "BOOKING ID", BookingID));

            //this.AddBackField(new StandardField("BOOKING ID", "BOOKING ID", BookingID));
            //this.AddBackField(new StandardField("Rating", "Rating", Rating));

            //this.AddLocation(Latitude, Longitude);

            //this.AddBackField(new StandardField("Showtime", "Showtime", Showtime));
            //this.AddBackField(new StandardField("Total", "Total", Total));
            //this.AddBackField(new StandardField("Terms and Conditions", "Terms and Conditions", ConfigurationManager.AppSettings["weburl"].ToString() + "/terms-and-condition"));
        }
    }
}