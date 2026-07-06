using Acervo.Web.DTOs;
using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Carrinho
    {
        [Inject] private NavigationManager Navigation  { get; set; } = default!;
        [Inject] private SessionService    Session     { get; set; } = default!;
        [Inject] private CartService       CartSvc     { get; set; } = default!;
        [Inject] private CartItemService   CartItemSvc { get; set; } = default!;
        [Inject] private BookService       BookSvc     { get; set; } = default!;
        [Inject] private AuthorService     AuthorSvc   { get; set; } = default!;
        [Inject] private CategoryService   CatSvc      { get; set; } = default!;
        [Inject] private SellerService     SellerSvc   { get; set; } = default!;
        [Inject] private SaleService       SaleSvc     { get; set; } = default!;
        [Inject] private SaleItemService   SaleItemSvc { get; set; } = default!;
        [Inject] private ToastService      Toast       { get; set; } = default!;

        private class CartItemVm
        {
            public long    CartItemId    { get; set; }
            public long    BookId        { get; set; }
            public string  Title         { get; set; } = string.Empty;
            public string  AuthorName    { get; set; } = string.Empty;
            public string  CategoryName  { get; set; } = string.Empty;
            public string? CoverImageUrl { get; set; }
            public decimal UnitPrice     { get; set; }
            public int     Quantity      { get; set; }
            public decimal SubTotal => UnitPrice * Quantity;
        }

        private bool  IsLoading { get; set; } = true;
        private long? CartId    { get; set; }
        private List<CartItemVm> Items { get; set; } = [];

        private decimal Subtotal => Items.Sum(i => i.SubTotal);
        private decimal Total    => Subtotal;

        protected override async Task OnInitializedAsync() => await LoadCart();

        private async Task LoadCart()
        {
            IsLoading = true;

            if (Session.UserId is not long userId)
            {
                IsLoading = false;
                Toast.ShowError("Faça login para ver seu carrinho.");
                return;
            }

            try
            {
                var cart = await CartSvc.GetByUser(userId);
                CartId = cart?.Id;

                if (cart is null)
                {
                    Items = [];
                    return;
                }

                var itemsTask = CartItemSvc.GetAll();
                var booksTask = BookSvc.GetAll();
                var authTask  = AuthorSvc.GetAll();
                var catTask   = CatSvc.GetAll();

                await Task.WhenAll(itemsTask, booksTask, authTask, catTask);

                var books   = booksTask.Result.ToDictionary(b => b.Id);
                var authors = authTask.Result.ToDictionary(a => a.Id, a => a.Name);
                var cats    = catTask.Result.ToDictionary(c => c.Id, c => c.Description);

                Items = itemsTask.Result
                    .Where(ci => ci.CartId == cart.Id)
                    .Select(ci =>
                    {
                        books.TryGetValue(ci.BookId, out var book);
                        return new CartItemVm
                        {
                            CartItemId    = ci.Id,
                            BookId        = ci.BookId,
                            Title         = book?.Title ?? "—",
                            AuthorName    = book is null ? "—" : authors.GetValueOrDefault(book.AuthorId, "—"),
                            CategoryName  = book is null ? "—" : cats.GetValueOrDefault(book.CategoryId, "—"),
                            CoverImageUrl = string.IsNullOrEmpty(book?.CoverImageUrl) ? null : book!.CoverImageUrl,
                            UnitPrice     = ci.UnitPrice,
                            Quantity      = ci.Quantity
                        };
                    })
                    .ToList();
            }
            catch
            {
                Toast.ShowError("Não foi possível carregar o carrinho.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task IncreaseQty(CartItemVm item) => await UpdateQuantity(item, item.Quantity + 1);

        private async Task DecreaseQty(CartItemVm item)
        {
            if (item.Quantity <= 1) return;
            await UpdateQuantity(item, item.Quantity - 1);
        }

        private async Task UpdateQuantity(CartItemVm item, int newQuantity)
        {
            if (CartId is not long cartId) return;

            try
            {
                var ok = await CartItemSvc.Update(new UpdateCartItemDto(
                    item.CartItemId, cartId, item.BookId, item.UnitPrice, newQuantity));

                if (ok)
                    item.Quantity = newQuantity;
                else
                    Toast.ShowError("Não foi possível atualizar a quantidade.");
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }

        private async Task RemoveItem(CartItemVm item)
        {
            try
            {
                if (await CartItemSvc.Delete(item.CartItemId))
                {
                    Items.Remove(item);
                    Toast.ShowSuccess($"«{item.Title}» removido do carrinho.");
                }
                else
                {
                    Toast.ShowError("Não foi possível remover o item.");
                }
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }

        private async Task ClearCart()
        {
            if (Items.Count == 0) return;

            try
            {
                var failures = 0;
                foreach (var item in Items.ToList())
                {
                    if (await CartItemSvc.Delete(item.CartItemId))
                        Items.Remove(item);
                    else
                        failures++;
                }

                if (failures == 0)
                    Toast.ShowSuccess("Carrinho esvaziado.");
                else
                    Toast.ShowError("Alguns itens não puderam ser removidos.");
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }

        private async Task Checkout()
        {
            if (Session.UserId is not long userId || Items.Count == 0) return;

            try
            {
                var sellers = await SellerSvc.GetAll();
                var seller  = sellers.FirstOrDefault();
                if (seller is null)
                {
                    Toast.ShowError("Nenhum vendedor disponível para concluir a compra.");
                    return;
                }

                if (!await SaleSvc.Create(new CreateSaleDto(userId, seller.Id)))
                {
                    Toast.ShowError("Não foi possível criar o pedido.");
                    return;
                }

                // Recupera a venda recém-criada (a mais recente do usuário).
                var sale = (await SaleSvc.GetAll())
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefault();

                if (sale is null)
                {
                    Toast.ShowError("Pedido criado, mas não foi possível adicionar os itens.");
                    return;
                }

                foreach (var item in Items)
                    await SaleItemSvc.Create(new CreateSaleItemDto(
                        sale.Id, item.BookId, item.Quantity, item.UnitPrice));

                // Esvazia o carrinho persistido.
                foreach (var item in Items.ToList())
                    await CartItemSvc.Delete(item.CartItemId);

                Items.Clear();
                Toast.ShowSuccess("Compra finalizada! Confira em Meus Pedidos.");
                Navigation.NavigateTo("/conta?tab=pedidos");
            }
            catch
            {
                Toast.ShowError("Erro ao finalizar a compra.");
            }
        }
    }
}
