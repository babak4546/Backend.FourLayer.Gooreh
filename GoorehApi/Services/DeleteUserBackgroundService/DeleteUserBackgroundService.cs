using GoorehInfrastructure.DbContextes;

namespace GoorehApi.Services.DeleteUserBackgroundService
{
    /// <summary>
    /// BackgroundService 
    /// </summary>
    public class DeleteUserBackgroundService :BackgroundService
    {
        //field provider (dependency Injection)
        private readonly IServiceProvider _provider;

        public DeleteUserBackgroundService(IServiceProvider provider)
        {
            _provider = provider;
        }
        // :BackgroundService khodesh ye loop dareh keh az vagti project ejra misheh ta vagti keh project stop misheh
        // loop darhale ejra mimoneh CancellationToken ham baraye tavagof loop hast
        //baraye kar haye da emi , tekrari ,zaman bandi shodeh va.. monaseb hast

        //override background service :method  : chizi keh to background bayad ejra besheh inja neveshteh mish 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //yeh scop jadid misazeh
                using var scoped =_provider.CreateScope();
               //chone dbContext lifeTimesh Scop hast bayad dakhel ye scop sakhteh sheh  
                var db = scoped.ServiceProvider.GetRequiredService<GoorehDbContext>();
                var limit = DateTime.Now.AddSeconds(-30);
                var remUsers = db.AppUsers.Where(s => s.IsRemoved && s.RemovedIn < limit).ToList();
                if (remUsers.Any())
                {
                    db.AppUsers.RemoveRange(remUsers);
                    await db.SaveChangesAsync();
                }
                //  har 31 sanieh cheack mikoneh
                await Task.Delay(31000, stoppingToken); 
            }
        }
    }
}
