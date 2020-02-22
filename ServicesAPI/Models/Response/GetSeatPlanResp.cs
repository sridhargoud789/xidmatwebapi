using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response.SeatPlan
{
    public class GetSeatPlanResp
    {
        public object ErrorDescription { get; set; }
        public int ResponseCode { get; set; }
        public Seatlayoutdata SeatLayoutData { get; set; }
    }

    public class Seatlayoutdata
    {
        public Areacategory[] AreaCategories { get; set; }
        public Area[] Areas { get; set; }
        public float BoundaryLeft { get; set; }
        public float BoundaryRight { get; set; }
        public float BoundaryTop { get; set; }
        public float ScreenStart { get; set; }
        public float ScreenWidth { get; set; }
    }

    public class Areacategory
    {
        public string AreaCategoryCode { get; set; }
        public bool IsInSeatDeliveryEnabled { get; set; }
        public int SeatsAllocatedCount { get; set; }
        public int SeatsNotAllocatedCount { get; set; }
        public int SeatsToAllocate { get; set; }
        public object[] SelectedSeats { get; set; }
    }

    public class Area
    {
        public string AreaCategoryCode { get; set; }
        public int ColumnCount { get; set; }
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public bool HasSofaSeatingEnabled { get; set; }
        public float Height { get; set; }
        public bool IsAllocatedSeating { get; set; }
        public float Left { get; set; }
        public int Number { get; set; }
        public int NumberOfSeats { get; set; }
        public int RowCount { get; set; }
        public Row[] Rows { get; set; }
        public float Top { get; set; }
        public float Width { get; set; }
    }

    public class Row
    {
        public string PhysicalName { get; set; }
        public Seat[] Seats { get; set; }
    }

    public class Seat
    {
        public string Id { get; set; }
        public int OriginalStatus { get; set; }
        public Position Position { get; set; }
        public int Priority { get; set; }
        public int SeatStyle { get; set; }
        public object SeatsInGroup { get; set; }
        public int Status { get; set; }
    }

    public class Position
    {
        public int AreaNumber { get; set; }
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
    }

}