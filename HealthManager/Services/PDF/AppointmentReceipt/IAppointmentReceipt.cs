using HealthManager.Models;
using HealthManager.Models.DTO;

namespace HealthManager.Services.PDF.AppointmentReceipt
{
    public interface IAppointmentReceipt
    {
        public byte[] CreateAppointmentReceipt(AppointmentDataPDFDTO appointment);
    }
}
