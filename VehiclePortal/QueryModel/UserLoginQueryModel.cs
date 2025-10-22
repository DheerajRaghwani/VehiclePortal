public class UserLoginQueryModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string LoginRole { get; set; }
    public string DistrictName { get; set; }   // 🔹 Needed to map district
}
