using Acervo.Web.DTOs;

namespace Acervo.Web.Service
{
    /// <summary>
    /// Orquestra os favoritos do usuário logado sobre a API
    /// (obter-ou-criar lista, verificar/adicionar/remover um livro).
    /// </summary>
    public class FavoritesManager(FavoritesService favorites, FavoritesItemService favoritesItems, SessionService session)
    {
        public async Task<FavoritesDto?> GetOrCreateForCurrentUser()
        {
            if (session.UserId is not long userId) return null;

            var list = await favorites.GetByUser(userId);
            if (list is not null) return list;

            if (!await favorites.Create(new CreateFavoritesDto(userId))) return null;
            return await favorites.GetByUser(userId);
        }

        public async Task<FavoritesItemDto?> FindItem(long bookId)
        {
            if (session.UserId is not long userId) return null;

            var list = await favorites.GetByUser(userId);
            if (list is null) return null;

            return (await favoritesItems.GetAll())
                .FirstOrDefault(fi => fi.FavoritesId == list.Id && fi.BookId == bookId);
        }

        public async Task<bool> Add(long bookId)
        {
            var list = await GetOrCreateForCurrentUser();
            if (list is null) return false;

            var already = (await favoritesItems.GetAll())
                .Any(fi => fi.FavoritesId == list.Id && fi.BookId == bookId);
            if (already) return true;

            return await favoritesItems.Create(new CreateFavoritesItemDto(list.Id, bookId));
        }

        public async Task<bool> Remove(long bookId)
        {
            var item = await FindItem(bookId);
            if (item is null) return false;

            return await favoritesItems.Delete(item.Id);
        }
    }
}
