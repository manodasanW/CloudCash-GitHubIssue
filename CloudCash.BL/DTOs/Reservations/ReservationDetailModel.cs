using CloudCash.BL.DTOs.TableInfo;

namespace CloudCash.BL.DTOs.Reservations
{
    public record ReservationDetailModel : ReservationListModel
    {
        public string Email { get; set; }

        public byte PeopleCount { get; set; }

        public TableInfoDetailModel SelectedTable { get; set; }
    }
}
