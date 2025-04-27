using Supabase;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class SupaBaseContext
    {
        private readonly Client _supabase;
        private readonly ILogger<SupaBaseContext>? _logger;

        public SupaBaseContext(Client supabaseClient, ILogger<SupaBaseContext> logger)
        {
            _supabase = supabaseClient;
            _logger = logger;
        }

        public async Task<List<AppUser>> GetAllUsers() =>
            (await _supabase.From<AppUser>().Get()).Models;

        public async Task<AppUser?> GetUserById(int id) =>
            await _supabase.From<AppUser>().Where(x => x.Id == id).Single();

        public async Task<AppUser?> CreateUser(AppUser user) =>
            (await _supabase.From<AppUser>().Insert(user)).Model;

        public async Task<AppUser?> UpdateUser(AppUser user) =>
            (await _supabase.From<AppUser>()
                .Where(x => x.Id == user.Id)
                .Set(x => x.Name, user.Name)
                .Set(x => x.Age, user.Age)
                .Set(x => x.Mail, user.Mail)
                .Set(x => x.CityId, user.CityId)
                .Update()).Model;

        public async Task<bool> DeleteUser(int id)
        {
            await _supabase.From<AppUser>().Where(x => x.Id == id).Delete();
            return true;
        }

        public async Task<List<City>> GetAllCities() =>
            (await _supabase.From<City>().Get()).Models;

        public async Task<City?> GetCityById(int id) =>
            await _supabase.From<City>().Where(x => x.Id == id).Single();

        public async Task<City?> CreateCity(City city) =>
            (await _supabase.From<City>().Insert(city)).Model;

        public async Task<City?> UpdateCity(City city) =>
            (await _supabase.From<City>()
                .Where(x => x.Id == city.Id)
                .Set(x => x.Name, city.Name)
                .Update()).Model;

        public async Task<bool> DeleteCity(int id)
        {
            try
            {
                // Сначала находим всех пользователей с этим city_id
                var usersInCity = await _supabase.From<AppUser>()
                    .Where(x => x.CityId == id)
                    .Get();

                // Если есть пользователи - не удаляем город
                if (usersInCity.Models.Count > 0)
                {
                    _logger?.LogWarning($"Cannot delete city {id} - it has {usersInCity.Models.Count} associated users");
                    return false;
                }

                // Если пользователей нет - удаляем город
                await _supabase.From<City>()
                    .Where(x => x.Id == id)
                    .Delete();

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error deleting city {id}");
                return false;
            }
        }
    }
}

    