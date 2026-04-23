using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibraryManagementSystem.Data;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartLibraryManagementSystem.Web.Controllers
{
    public class AiAssistantController : Controller
    {
        private readonly SmartLibraryDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public AiAssistantController(SmartLibraryDbContext context, HttpClient httpClient, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AskAi(string userMessage)
        {
            try
            {
                // 1. Lấy danh sách sách và thể loại hiện có từ Database
                var books = _context.Books.ToList();
                var bookListStr = string.Join(", ", books.Select(b => $"- {b.Title} (Thể loại: {b.Category})"));

                // 2. Tạo "mật lệnh" (System Prompt) ép AI chỉ trả lời trong phạm vi sách của thư viện
                string prompt = $"Bạn là một thủ thư AI nhiệt tình của thư viện ICTU. Thư viện hiện CHỈ CÓ những cuốn sách sau:\n{bookListStr}\n\n" +
                                $"Độc giả đang hỏi: '{userMessage}'.\n" +
                                $"Dựa vào danh sách trên, hãy gợi ý 1-2 cuốn sách phù hợp nhất. Nếu không có sách đúng chủ đề, hãy xin lỗi và giới thiệu sách khác trong danh sách. Trả lời ngắn gọn, thân thiện.";

                // 3. Đóng gói dữ liệu ĐÚNG chuẩn JSON của Google Gemini
                var requestBody = new
                {
                    contents = new[]
                    {
                        new { parts = new[] { new { text = prompt } } }
                    }
                };

                // Lấy API Key từ appsettings.json
                string? apiKey = _configuration["GeminiApiKey"]; 
                if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_GOOGLE_API_KEY_HERE")
                {
                    return Json(new { success = false, reply = "Chưa cấu hình Google API Key. Hãy thêm 'GeminiApiKey' vào file appsettings.json." });
                }

                // Sửa model thành gemini-1.5-flash (ổn định) và dùng IHttpClientFactory để tránh lỗi Socket Exhaustion sau một thời gian chạy
                string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

                var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestBody), System.Text.Encoding.UTF8, "application/json");
                
                // 4. Gửi yêu cầu sang Google bằng HttpClient được tiêm từ Factory
                var response = await _httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    
                    // 5. Bóc tách JSON để lấy đúng câu trả lời (Parse JSON)
                    using (var doc = System.Text.Json.JsonDocument.Parse(responseString))
                    {
                        var root = doc.RootElement;
                        var textResponse = root
                            .GetProperty("candidates")[0]
                            .GetProperty("content")
                            .GetProperty("parts")[0]
                            .GetProperty("text").GetString();

                        return Json(new { success = true, reply = textResponse });
                    }
                }
                else
                {
                    var errorStr = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, reply = $"Lỗi từ Google: {response.StatusCode}. Chi tiết: {errorStr}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, reply = $"Đã xảy ra lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}