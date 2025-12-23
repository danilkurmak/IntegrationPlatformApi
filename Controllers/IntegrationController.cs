using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using IntegrationPlatformApi.Models;


namespace IntegrationPlatformApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IntegrationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public IntegrationController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("reminders")]
        public async Task<IActionResult> GetReminders()
        {
            var reminders = await _httpClient
                .GetFromJsonAsync<List<Reminder>>(
                    "https://reminderappp-valerdolgo.amvera.io/api/Reminder"
                );

            return Ok(reminders);
        }

        [HttpPost("send-report")]
        public async Task<IActionResult> SendReport()
        {
            // получаем данные из ReminderApi
            var reminders = await _httpClient
                .GetFromJsonAsync<List<Reminder>>(
                    "https://reminderappp-valerdolgo.amvera.io/api/Reminder"
                );

            if (reminders == null || reminders.Count == 0)
            {
                return BadRequest("Нет данных напоминаний");
            }

            var reminder = reminders.First();

            // формируем объект EmailReport
            var emailReport = new EmailReport
            {
                recipient = "test@mail.ru",
                subject = reminder.title,
                body = $"{reminder.description}. Дата: {reminder.dateTime}. Приоритет: {reminder.priority}"
            };

            // отправляем данные в OtpravkaApi
            var response = await _httpClient.PostAsJsonAsync(
                "https://otpravka-api-dkurmak58.amvera.io/api/EmailReport",
                emailReport
            );

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Ошибка отправки письма");
            }

            return Ok("Письмо с отчётом успешно отправлено");
        }
    }

    
}
