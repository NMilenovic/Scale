using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.User;
using MongoDB.Bson;
using MongoDB.Driver;
using Services;

namespace Service{
  public class LoginService
  {
    private MongoService mongoService;

    public LoginService()
    {
      mongoService = new MongoService();
    }

    public async Task<UserData> Login(UserLoginDTO ulDTO)
    {
      var usersCollection = mongoService.database.GetCollection<User>("User");
      var filter =  Builders<User>.Filter.Eq(x => x.Email,ulDTO.Email);
      var user = await usersCollection.Find(filter).ToListAsync();
      if(user.Count != 1)
        throw new Exception("User dosent exist!");
      //wrati mongoid i role  
      var ud = new UserData{
        UserId = user.First().Id.ToString(),
        Role = ((int)user.First().Role)
      };
      return ud;
    }   
  }
}