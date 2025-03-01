using MongoDB.Driver;
using UserApi.Models;
using Microsoft.Extensions.Options;

namespace UserApi.Services {
    public class UserService {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoDBSettings> settings) {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.CollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _users.Find(user => true).ToListAsync();

        public async Task<User> GetAsync(string id) =>
            await _users.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

        public async Task<User> CreateAsync(User user) {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateAsync(string id, User userIn) {
            var filter = Builders<User>.Filter.Eq(user => user.Id, id);
            var update = Builders<User>.Update
                .Set(user => user.Nombre, userIn.Nombre)
                .Set(user => user.Apellidos, userIn.Apellidos)
                .Set(user => user.Cedula, userIn.Cedula)
                .Set(user => user.CorreoElectronico, userIn.CorreoElectronico)
                .Set(user => user.Password, userIn.Password)
                .Set(user => user.FechaUltimoAcceso, userIn.FechaUltimoAcceso);

            await _users.UpdateOneAsync(filter, update);
        }

        public async Task RemoveAsync(string id) =>
            await _users.DeleteOneAsync(user => user.Id == id);

        public async Task<User> GetByEmailAsync(string email) =>
            await _users.Find<User>(user => user.CorreoElectronico == email).FirstOrDefaultAsync();
    }
}
